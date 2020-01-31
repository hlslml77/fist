using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moba.Cache
{
    public class RoomCacheBase<TRoom> where TRoom : Room.RoomBase<MobaClient>
    {
        /// <summary>
        /// 房间ID对应的房间数据
        /// </summary>
        /// 线程互斥锁防止两个玩家同时创建角色index相同，提高线程安全
        protected ConcurrentDictionary<int, TRoom> idRoomDict = new ConcurrentDictionary<int, TRoom>();

        /// <summary>
        /// 账号ID对应的玩家ID
        /// </summary>
        protected ConcurrentDictionary<int, int> playerRoomDict = new ConcurrentDictionary<int, int>();

        /// <summary>
        /// 重用的队列
        /// </summary>
        protected ConcurrentQueue<TRoom> roomQue = new ConcurrentQueue<TRoom>();

        /// <summary>
        /// 主键ID
        /// </summary>
        protected int index = 0;

    }
}
