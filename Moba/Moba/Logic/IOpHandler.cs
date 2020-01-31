using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moba.Logic
{
    interface IOpHandler
    {
        /// <summary>
        /// 收到请求时的处理
        /// </summary>
        /// <param name="client">哪个客户端</param>
        /// <param name="subCode">子操作</param>
        /// <param name="request">请求</param>
        void OnRequest(MobaClient client, byte subCode, OperationRequest request);
        /// <summary>
        /// 掉线时的处理
        /// </summary>
        /// <param name="client">哪个客户端</param>
        void OnDisconnect(MobaClient client);
    }
}
