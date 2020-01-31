using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moba
{
    /// <summary>
    /// 单发消息
    /// </summary>
    public class SingleSend
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="client">收响应的客户端</param>
        /// <param name="opCode">操作码</param>
        /// <param name="subCode">子操作</param>
        /// <param name="parameters">参数</param>
        public virtual void Send(MobaClient client, byte opCode, byte subCode, short retCode, string mess, params object[] parameters)
        {
            OperationResponse response = new OperationResponse();
            response.OperationCode = opCode;
            response.Parameters = new Dictionary<byte, object>();
            response[80] = subCode;
            for(int i = 0;i < parameters.Length;i++)
            {
                response[(byte)i] = parameters[i];
            }
            response.ReturnCode = retCode;
            response.DebugMessage = mess;
            client.SendOperationResponse(response, new SendParameters());
        }
    }
}
