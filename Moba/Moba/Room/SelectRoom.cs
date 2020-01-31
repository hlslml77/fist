using Moba.Model;
using MobaCommon.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moba.Room
{
    public class SelectRoom : RoomBase<MobaClient>
    {
        /// <summary>
        /// 队伍1玩家ID对应的选择模型
        /// </summary>
        public Dictionary<int, SelectModel> team1Dict;
        /// <summary>
        /// 队伍2玩家ID对应的选择模型
        /// </summary>
        public Dictionary<int, SelectModel> team2Dict;

        /// <summary>
        /// 进入的数量
        /// </summary>
        public int enterCount;

        /// <summary>
        /// 是否全部进入
        /// </summary>
        public bool IsAllEnter
        {
            get
            {
                return enterCount >= Count;
            }
        }
        /// <summary>
        /// 准备的数量
        /// </summary>
        public int readyCount;

        /// <summary>
        /// 是否全部准备
        /// </summary>
        public bool IsAllReady
        {
            get
            {
                return Count == ClientList.Count && readyCount == enterCount;
            }
        }
        public SelectRoom(int id, int count) : base(id, count)
        {
            team1Dict = new Dictionary<int, SelectModel>();
            team2Dict = new Dictionary<int, SelectModel>();
            enterCount = 0;
            readyCount = 0;
        }

        /// <summary>
        /// 初始化房间
        /// </summary>
        /// <param name="team1"></param>
        /// <param name="team2"></param>
        public void InitRoom(List<int> team1, List<int> team2)
        {
            //有时候这个房间是重用的 需要清空数据
            //team1Dict.Clear();
            //team2Dict.Clear();
            //再初始化房间
            PlayerModel pm;
            SelectModel sm;
            foreach (int item in team1)
            {
                pm = Cache.Caches.Player.GetModel(item);
                sm = new SelectModel(pm.Id, pm.Name);
                //添加关系
                team1Dict.Add(pm.Id, sm);
            }
            foreach (int item in team2)
            {
                pm = Cache.Caches.Player.GetModel(item);
                sm = new SelectModel(pm.Id, pm.Name);
                //添加关系
                team2Dict.Add(pm.Id, sm);
            }
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="client"></param>
        public void Enter(int playerId, MobaClient client)
        {
            if (team1Dict.ContainsKey(playerId))
            {
                team1Dict[playerId].isEnter = true;
            }
            else if (team2Dict.ContainsKey(playerId))
            {
                team2Dict[playerId].isEnter = true;
            }
            else
                return;
            //添加房间内的连接对象
            ClientList.Add(client);
            //增加进入人数
            enterCount++;
        }

        /// <summary>
        /// 选择英雄
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="heroId"></param>
        public bool Select(int playerId, int heroId)
        {
            //队友有没有选择
            if (team1Dict.ContainsKey(playerId))
            {
                foreach (var item in team1Dict.Values)
                {
                    if (item.heroId == heroId)
                        return false;
                }
                //自己可以选择了
                team1Dict[playerId].heroId = heroId;
            }
            else if (team2Dict.ContainsKey(playerId))
            {
                foreach (var item in team2Dict.Values)
                {
                    if (item.heroId == heroId)
                        return false;
                }
                //自己可以选择了
                team2Dict[playerId].heroId = heroId;
            }
            return true;

        }

        /// <summary>
        /// 确认选择
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public bool Ready(int playerId)
        {
            if(team1Dict.ContainsKey(playerId))
            {
                SelectModel model = team1Dict[playerId];
                //不选择英雄 没办法确定
                if (model.heroId == -1)
                    return false;
                model.isReady = true;
                //更新准备的人数
                readyCount++;
            }
            else if (team2Dict.ContainsKey(playerId))
            {
                SelectModel model = team2Dict[playerId];
                //不选择英雄 没办法确定
                if (model.heroId == -1)
                    return false;
                model.isReady = true;
                //更新准备的人数
                readyCount++;
            }
            return true;
        }
    }

}
