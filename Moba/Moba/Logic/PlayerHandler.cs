using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using MobaCommon.Code;
using Moba.Cache;
using Moba.Model;
using MobaCommon.Dto;
using LitJson;

namespace Moba.Logic
{
    public class PlayerHandler : SingleSend, IOpHandler
    {

        public Action<List<int>, List<int>> StartSelectEvent;
        
        //账号的缓存
        private AccountCache accountCache = Caches.Account;
        //角色的缓存
        private PlayerCache playerCache = Caches.Player;
        /// <summary>
        /// 匹配的缓存
        /// </summary>
        private MatchCache matchCache = Caches.Match;

        public void OnDisconnect(MobaClient client)
        {
            #region 每次下线的时候 要通知好友 显示离线状态
            PlayerModel model = playerCache.GetModel(client);
            if (model != null)
            {
                MobaClient tempClient = null;
                string[] friends = model.FriendIdList.Split(',');
                foreach (string item in friends)
                {
                    if (string.IsNullOrEmpty(item))
                        continue;
                    int id = int.Parse(item);
                    if (!playerCache.IsOnline(id))
                    {
                        continue;
                    }
                    tempClient = playerCache.GetClient(id);
                    Send(tempClient, OpCode.PlayerCode, OpPlayer.FriendOffline, 0, "此玩家下线", model.Id);

                }
            } 
            #endregion
            matchCache.Offline(client, playerCache.GetId(client));
            playerCache.Offline(client);
        }

        public void OnRequest(MobaClient client, byte subCode, OperationRequest request)
        {
            switch (subCode)
            {
                case OpPlayer.GetInfo:
                    onGetInfo(client);
                    break;
                case OpPlayer.Create:
                    string name = request[0].ToString();
                    onCreate(client, name);
                    break;
                case OpPlayer.Online:
                    onOnline(client);
                    break;
                case OpPlayer.RequestAdd:
                    string addName = request[0].ToString();
                    onAdd(client, addName);
                    break;
                case OpPlayer.ToClientAdd:
                    onToClientAdd(client, (bool)request[0], (int)request[1]);
                    break;
                case OpPlayer.StartMatch:
                    onStartMatch(client, (int)request[0]);
                    break;
                case OpPlayer.StopMatch:
                    onStopMatch(client);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 离开匹配
        /// </summary>
        /// <param name="client"></param>
        private void onStopMatch(MobaClient client)
        {
            if (matchCache.StopMatch(client, playerCache.GetId(client)))
            {
                Send(client, OpCode.PlayerCode, OpPlayer.StopMatch, 0, "离开成功");
            }

        }
        /// <summary>
        /// 开始匹配
        /// </summary>
        /// <param name="client"></param>
        /// <param name="playerId"></param>
        private void onStartMatch(MobaClient client, int playerId)
        {
            //非法操作
            if (playerCache.GetId(client) != playerId)
                return;
            //获取匹配结果
            Room.MatchRoom room = matchCache.StartMatch(client, playerId);
            Send(client, OpCode.PlayerCode, OpPlayer.StartMatch, 0, "开始匹配");
            //如果房间满了 那就开始选人
            if (room.IsFull)
            {
                //通知房间内所有人进入选人界面
                //room.Brocast(OpCode.PlayerCode, OpPlayer.MatchComplete, 1, "开始选人");
                //开始选人
                StartSelectEvent(room.Team1IdList, room.Team2IdList);
                //发起是否进入选人请求
                room.Brocast(OpCode.PlayerCode, OpPlayer.MatchComplete, 0, "是否进入选人房间(10s内)");
                //撕毁房间
                matchCache.DestroyRoom(room);
            }
        }
        /// <summary>
        /// 添加结果
        /// </summary>
        /// <param name="result"></param>
        private void onToClientAdd(MobaClient client, bool result, int requestId)
        {
            MobaClient requestClient = playerCache.GetClient(requestId);

            //同意了 那么就保存数据
            if (result == true)
            {
                int playerId = playerCache.GetId(client);

                playerCache.AddFriend(playerId, requestId);

                Send(client, OpCode.PlayerCode, OpPlayer.ToClientAdd, 1, "添加成功", JsonMapper.ToJson(toDto(playerCache.GetModel(playerId))));

                Send(requestClient, OpCode.PlayerCode, OpPlayer.ToClientAdd, 1, "添加成功", JsonMapper.ToJson(toDto(playerCache.GetModel(requestId))));

                return;
            }

            //拒绝了 就回传给原来的客户端告诉他 不同意
            Send(requestClient, OpCode.PlayerCode, OpPlayer.ToClientAdd, -1, "此玩家拒绝你的请求");
        }
        /// <summary>
        /// 添加好友的处理
        /// </summary>
        /// <param name="addName"></param>
        private void onAdd(MobaClient client, string addName)
        {
            //添加好友的数据模型
            PlayerModel toModel = playerCache.GetModel(addName);
            if(toModel == null)
            {
                Send(client, OpCode.PlayerCode, OpPlayer.RequestAdd, -1, "没有此角色");
                return;
            }

            //如果添加的是自身那么不可以
            if(playerCache.GetModel(client).Id == toModel.Id)
            {
                Send(client, OpCode.PlayerCode, OpPlayer.RequestAdd, -3, "不能添加自身");
                return;
            }

            //如果已经是好友的玩家 那么也不可以
            string[] friends = playerCache.GetModel(client).FriendIdList.Split(',');

            foreach (var item in friends)
            {
                if (string.IsNullOrEmpty(item))
                    continue;
                if (int.Parse(item) == toModel.Id)
                {
                    Send(client, OpCode.PlayerCode, OpPlayer.RequestAdd, -4, "该玩家已经是好友");
                    return;
                }
            }



            //如果能获取到数据模型 先判断他是否在线
            bool isOnline = playerCache.IsOnline(toModel.Id);
            //不在线 回传不在线
            if(!isOnline)
            {
                Send(client, OpCode.PlayerCode, OpPlayer.RequestAdd, -2, "此玩家不在线");
                return;
            }
            //在线 给模型对应的客户端 发消息：有人向他加好友 同不同意
            MobaClient toClient = playerCache.GetClient(toModel.Id);

            //当前玩家的数据模型
            PlayerModel model = playerCache.GetModel(client);
            Send(toClient, OpCode.PlayerCode, OpPlayer.ToClientAdd, 0, "是否添加好友", JsonMapper.ToJson(toDto(model)));


        }
        /// <summary>
        /// 上线的处理
        /// </summary>
        /// <param name="client"></param>
        private void onOnline(MobaClient client)
        {
            int accId = accountCache.GetId(client);
            int playerId = playerCache.GetId(accId);
            //防止重复在线
            //if (playerCache.IsOnline(client))
            //    return;
            //上线
            playerCache.Online(client, playerId);

            #region 每次上线的时候 要通知好友 显示在线状态

            PlayerModel model = playerCache.GetModel(client);
            if (model != null)
            {
                MobaClient tempClient = null;
                string[] friends = model.FriendIdList.Split(',');
                foreach (string item in friends)
                {
                    if (string.IsNullOrEmpty(item))
                        continue;
                    int id = int.Parse(item);
                    if (!playerCache.IsOnline(id))
                        continue;
                    tempClient = playerCache.GetClient(id);
                    Send(tempClient, OpCode.PlayerCode, OpPlayer.FriendOnline, 0, "此玩家上线", model.Id);
                }


            }
            #endregion

            PlayerDto dto = toDto(playerCache.GetModel(playerId));

            //发送
            Send(client, OpCode.PlayerCode, OpPlayer.Online, 0, "上线成功", JsonMapper.ToJson(dto));
        }

        private PlayerDto toDto(PlayerModel model)
        {
            PlayerDto dto = new PlayerDto()
            {
                id = model.Id,
                exp = model.Exp,
                loseCount = model.LoseCount,
                lv = model.Lv,
                name = model.Name,
                power = model.Power,
                runCount = model.RunCount,
                winCount = model.WinCount,
            };

            //赋值英雄ID列表
            string[] heros = model.HeroIdList.Split(',');
            dto.heroIds = new int[heros.Length];
            for (int i = 0; i < heros.Length; i++)
            {
                if (string.IsNullOrEmpty(heros[i]))
                    continue;
                dto.heroIds[i] = int.Parse(heros[i]);
            }
                

            //赋值好友列表
            string[] friends = model.FriendIdList.Split(',');
            dto.friends = new Friend[friends.Length];
            for (int i = 0; i < friends.Length; i++)
            {
                if (string.IsNullOrEmpty(friends[i]))
                    continue;
                int id = int.Parse(friends[i]);
                string name = playerCache.GetModel(id).Name;
                bool online = playerCache.IsOnline(id);
                dto.friends[i] = new Friend(id, name, online);
            }
            return dto;
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="client"></param>
        /// <param name="name"></param>
        private void onCreate(MobaClient client, string name)
        {
            int accId = accountCache.GetId(client);
            if (playerCache.Has(accId))
                return;

            //验证时候开始创建
            playerCache.Create(name, accId);
            Send(client, OpCode.PlayerCode, OpPlayer.Create, 0, "创建成功");

        }
        
        
        /// <summary>
        /// 获取角色信息
        /// </summary>
        private void onGetInfo(MobaClient client)
        {
            int accId = accountCache.GetId(client);
            if(accId == -1)
            {
                Send(client, OpCode.PlayerCode, OpPlayer.GetInfo, -1, "非法登录");
                return;
            }
            if (playerCache.Has(accId))
            {
                Send(client, OpCode.PlayerCode, OpPlayer.GetInfo, 0, "存在角色");
                return;
            }
            else
            {
                Send(client, OpCode.PlayerCode, OpPlayer.GetInfo, -2, "没有角色");
                return;
            }

        }
    }
}
