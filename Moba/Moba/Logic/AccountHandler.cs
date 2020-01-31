using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using MobaCommon.Code;
using LitJson;
using MobaCommon.Dto;
using Moba.Cache;

namespace Moba.Logic
{
    /// <summary>
    /// 账号的逻辑处理
    /// </summary>
    class AccountHandler : SingleSend, IOpHandler
    {
        //账号的缓存
        private AccountCache cache = Caches.Account;
        public void OnDisconnect(MobaClient client)
        {
            cache.Offline(client);
        }

        public void OnRequest(MobaClient client, byte subCode, OperationRequest request)
        {
            switch (subCode)
            {
                case OpAccount.Login:
                    AccountDto dto = JsonMapper.ToObject<AccountDto>(request[0].ToString());
                    onLogin(client, dto.Account, dto.Password);
                    break;
                case OpAccount.Register:
                    string acc = request[0].ToString();
                    string pwd = request[1].ToString();
                    OnRegister(client, acc, pwd);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 登录处理
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="pwd"></param>
        private void onLogin(MobaClient client, string acc, string pwd)
        {
            if(acc == null || pwd == null)
            {
                return;
            }
            //验证在线...
            if(cache.IsOnline(acc))
            {
                this.Send(client, OpCode.AccountCode, OpAccount.Login, -1, "玩家在线");
                return;
            }
            if(cache.Match(acc, pwd))
            {
                cache.Online(acc, client);
                this.Send(client, OpCode.AccountCode, OpAccount.Login, 0, "登录成功");
            }
            else
            {
                this.Send(client, OpCode.AccountCode, OpAccount.Login, -2, "账号或密码错误");

            }
        }
        /// <summary>
        /// 注册处理
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="pwd"></param>
        private void OnRegister(MobaClient client, string acc, string pwd)
        {
            MobaApplication.LogInfo("aa");
            //无效检测
            if (acc == null || pwd == null)
            {
                return;
            }
            //重复检测
            if (cache.Has(acc))
            {
                this.Send(client, OpCode.AccountCode, OpAccount.Register, -1, "账号重复");
                return;
            }
            //添加账号
            cache.Add(acc, pwd);
            this.Send(client, OpCode.AccountCode, OpAccount.Register, 0, "注册成功");
        }
    }
}
