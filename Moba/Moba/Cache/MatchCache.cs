using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moba.Room;
using System.Collections.Concurrent;

namespace Moba.Cache
{
    public class MatchCache : RoomCacheBase<MatchRoom>
    {
        /// <summary>
        /// 玩家进入匹配队列
        /// </summary>
        /// <param name="client"></param>
        /// <param name="playerId"></param>
        /// <returns>进入的房间</returns>
        /// 多人匹配playerid换成数组
        public MatchRoom StartMatch(MobaClient client, int playerId)
        {
            MatchRoom room = null;
            //存在等待的房间
            foreach (MatchRoom item in idRoomDict.Values)
            {
                if (item.IsFull)
                    continue;
                room = item;
                room.Enter(client, playerId);
                //绑定玩家和房间的映射
                playerRoomDict.TryAdd(playerId, room.Id);
                return room;
            }
            //不存在等待的房间
            //有重用的房间
            if(roomQue.Count > 0)
            {
                roomQue.TryDequeue(out room);
                //添加映射关系
                idRoomDict.TryAdd(room.Id, room);
                playerRoomDict.TryAdd(playerId, room.Id);
                //添加玩家进入房间
                room.Enter(client, playerId);
                return room;
            }
            //没有重用的房间
            else
            {
                //index为ID
                room = new MatchRoom(index, 2);
                index++;
                //添加映射关系
                idRoomDict.TryAdd(room.Id, room);
                playerRoomDict.TryAdd(playerId, room.Id);
                //添加玩家进入房间
                room.Enter(client, playerId);
                return room;
            }
        }

        /// <summary>
        /// 玩家离开匹配队列
        /// </summary>
        /// <param name="client"></param>
        /// <param name="playerId"></param>
        /// <returns>离开是否成功</returns>
        public bool StopMatch(MobaClient client, int playerId)
        {
            int roomId;
            //安全监测
            //移除玩家和房间的映射，不能移除返回false
            if (!playerRoomDict.TryRemove(playerId, out roomId))
                return false;
            MatchRoom room;
            //检测（防止多线程造成不必要的错误）
            if (!idRoomDict.TryGetValue(roomId, out room))
                return false;

            room.Leave(client, playerId);
            if(room.IsEmpty)
            {
                //移除映射关系
                idRoomDict.TryRemove(room.Id, out room);
                //移除定时任务
                if (!room.GUID.Equals(new Guid()))
                    room.timer.RemoveAction(room.GUID);

                //先把房间入重用队列
                roomQue.Enqueue(room);
            }
            return true;
        }

        /// <summary>
        /// 摧毁指定的房间
        /// </summary>
        /// <param name="room"></param>
        public void DestroyRoom(MatchRoom room)
        {
            int temp;
            //移除玩家ID和房间ID的映射
            foreach (int item in room.Team1IdList)
            {
                playerRoomDict.TryRemove(item, out temp);
            }
            foreach (int item in room.Team2IdList)
            {
                playerRoomDict.TryRemove(item, out temp);
            }
            //移除房间ID和房间的映射
            idRoomDict.TryRemove(room.Id, out room);
            //清空房间信息
            room.Team1IdList.Clear();
            room.Team2IdList.Clear();
            room.ClientList.Clear();
            //入队列
            roomQue.Enqueue(room);
        }

        /// <summary>
        /// 玩家下线
        /// </summary>
        /// <param name="client"></param>
        /// <param name="playerId"></param>
        public void Offline(MobaClient client, int playerId)
        {
            StopMatch(client, playerId);
        }
    }
}
