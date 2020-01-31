using ExitGames.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace Moba.Room
{
    /// <summary>
    /// 房间的一个基类
    /// </summary>
    public class RoomBase<TClient> where TClient : ClientPeer
    {
        /// <summary>
        /// 房间Id
        /// </summary>
        public int Id;

        /// <summary>
        /// 连接对象的集合
        /// </summary>
        public List<TClient> ClientList;

        /// <summary>
        /// 房间的容纳量
        /// </summary>
        public int Count;

        /// <summary>
        /// 定时器
        /// </summary>
        public Timer timer;

        /// <summary>
        /// 定时任务的ID
        /// </summary>
        public Guid GUID;

        public RoomBase(int id, int count)
        {
            this.Id = id;
            this.Count = count;
            ClientList = new List<TClient>();
            GUID = new Guid();
            timer = new Timer();
            timer.Start();
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected bool EnterRoom(TClient client)
        {
            if (ClientList.Contains(client))
            {
                return false;
            }
            ClientList.Add(client);
            return true;
        }

        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected bool LeaveRoom(TClient client)
        {
            if (!ClientList.Contains(client))
            {
                return false;
            }
            ClientList.Remove(client);
            return true;
        }

        /// <summary>
        /// 开启定时任务
        /// </summary>
        /// <param name="utcTime"></param>
        /// <param name="callback"></param>
        public void StartSchedule(DateTime utcTime, Action callback)
        {
            this.GUID = timer.AddAction(utcTime, callback);
        }

        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="client">收响应的客户端</param>
        /// <param name="opCode">操作码</param>
        /// <param name="subCode">子操作</param>
        /// <param name="parameters">参数</param>
        public void Brocast(byte opCode, byte subCode, short retCode, string mess, TClient exClient = null, params object[] parameters)
        {
            OperationResponse response = new OperationResponse();
            response.OperationCode = opCode;
            response.Parameters = new Dictionary<byte, object>();
            response[80] = subCode;
            for (int i = 0; i < parameters.Length; i++)
            {
                response[(byte)i] = parameters[i];
            }
            response.ReturnCode = retCode;
            response.DebugMessage = mess;

            foreach (TClient client in ClientList)
            {
                if (client == exClient)
                    continue;
                client.SendOperationResponse(response, new SendParameters());
            }

        }
    }
}
