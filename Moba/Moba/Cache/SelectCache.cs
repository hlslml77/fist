using Moba.Room;
using MobaCommon.Code;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moba.Cache
{
    public class SelectCache : RoomCacheBase<SelectRoom>
    {
        //角色的缓存
        private PlayerCache playerCache = Caches.Player;
        /// <summary>
        /// 玩家下线
        /// </summary>
        /// <param name="client"></param>
        public void Offline(MobaClient client, int playerId)
        {
            int roomId = -1;
            if (!playerRoomDict.TryGetValue(playerId, out roomId))
                return;
            SelectRoom room = null;
            if (!idRoomDict.TryGetValue(roomId, out room))
                return;

            //移除退出的客户端
            room.ClientList.Remove(client);
            //给剩余的客户端发一个消息：有人退出 房间解散 回到主界面
            room.Brocast(OpCode.SelectCode, OpSelect.Destroy, 0, "有人退出", null);

            //销毁房间
            Destroy(roomId);

        }
        /// <summary>
        /// 开始选人，创建选人房间
        /// </summary>
        /// <param name="team1"></param>
        /// <param name="team2"></param>
        /// <param name="clientList"></param>
        public void CreateRoom(List<int> team1, List<int> team2)
        {
            SelectRoom room = null;
            //取不出来重用房间
            if (!roomQue.TryDequeue(out room))
            {
                room = new SelectRoom(index, team1.Count + team2.Count);
                index++;
            }
            //能取出来
            room.InitRoom(team1, team2);
            //绑定玩家ID和房间ID
            foreach (int item in team1)
            {
                playerRoomDict.TryAdd(item, room.Id);
            }
            foreach (int item in team2)
            {
                playerRoomDict.TryAdd(item, room.Id);
            }
            //绑定房间ID和房间
            idRoomDict.TryAdd(room.Id, room);

            //创建成功了
            //开启一个定时任务，通知玩家 10S之内进入房间 否则房间自动销毁
            room.StartSchedule(DateTime.UtcNow.AddSeconds(10),
                () =>
                {
                    //销毁房间的回调
                    if (!room.IsAllEnter)
                    {
                        SingleSend ss = new SingleSend();
                        foreach (int item in playerRoomDict.Keys)
                        {
                            MobaClient c = playerCache.GetClient(item);
                            ss.Send(c, OpCode.SelectCode, OpSelect.Destroy, 0, "有人未进入 解散当前选人");

                        }

                        room.Brocast(OpCode.SelectCode, OpSelect.Destroy, 0, "有人未进入 解散当前选人");
                    }
                    Destroy(room.Id);
                });
        }

        /// <summary>
        /// 销毁房间
        /// </summary>
        /// <param name="roomId"></param>
        public void Destroy(int roomId)
        {
            SelectRoom room = null;
            if (!idRoomDict.TryRemove(roomId, out room))
                return;

            //移除玩家ID和房间ID的关系
            foreach (int item in room.team1Dict.Keys)
            {
                playerRoomDict.TryRemove(item, out roomId);
            }
            foreach (int item in room.team2Dict.Keys)
            {
                playerRoomDict.TryRemove(item, out roomId);
            }
            //清空房间内的数据
            room.team1Dict.Clear();
            room.team2Dict.Clear();
            room.ClientList.Clear();
            room.enterCount = 0;
            room.readyCount = 0;
            //入队列
            roomQue.Enqueue(room);
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public SelectRoom Enter(int playerId, MobaClient client)
        {
            int roomId = -1;
            if (!playerRoomDict.TryGetValue(playerId, out roomId))
                return null;
            SelectRoom room = null;
            if (!idRoomDict.TryGetValue(roomId, out room))
                return null;
            room.Enter(playerId, client);
            return room;
        }
        /// <summary>
        /// 选择英雄
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="heroId"></param>
        /// <returns></returns>
        public SelectRoom Select(int playerId, int heroId)
        {
            int roomId = -1;
            if (!playerRoomDict.TryGetValue(playerId, out roomId))
                return null;
            SelectRoom room = null;
            if (!idRoomDict.TryGetValue(roomId, out room))
                return null;

            if (room.Select(playerId, heroId))
            {
                return room;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 确认选择
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="heroId"></param>
        /// <returns></returns>
        public SelectRoom Ready(int playerId)
        {
            int roomId = -1;
            if (!playerRoomDict.TryGetValue(playerId, out roomId))
                return null;
            SelectRoom room = null;
            if (!idRoomDict.TryGetValue(roomId, out room))
                return null;

            return room.Ready(playerId) ? room : null;
        }

        /// <summary>
        /// 获取连接对象所在的房间
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public SelectRoom GetRoom(int playerId)
        {
            int roomId = -1;
            if (!playerRoomDict.TryGetValue(playerId, out roomId))
                return null;
            SelectRoom room = null;
            if (!idRoomDict.TryGetValue(roomId, out room))
                return null;
            return room;
        }
    }
}
