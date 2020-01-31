using ExitGames.Threading;
using Moba.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moba.Cache
{
    /// <summary>
    /// 账号的缓存层
    /// </summary>
    class AccountCache
    {

        #region 数据
        /// <summary>
        /// 账号和模型的映射
        /// </summary>
        private SynchronizedDictionary<string, AccountModel> accModelDict = new SynchronizedDictionary<string, AccountModel>();
        /// <summary>
        /// 判断账号密码是否存在正确
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool Match(string acc, string pwd)
        {
            //if (!accModelDict.ContainsKey(acc))
            //    return false;
            //return accModelDict[acc].Password == pwd;

            //如果内存里面有 直接判断
            if (accModelDict.ContainsKey(acc))
                return accModelDict[acc].Password == pwd;

            AccountModel model = new AccountModel();
            //如果没有 那就真的没有
            if (!model.Exists(acc))
                return false;
            //如果数据库里面存在
            model.GetModel(acc);
            //添加到内存里
            accModelDict.TryAdd(model.Account, model);
            //再进行判断
            return model.Password == pwd;
        }
       // private int id = 0;

        /// <summary>
        /// 添加账号信息
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool Add(string acc, string pwd)
        {
            //重复检测
            if (Has(acc))
            {
                return false;
            }
            //添加
            AccountModel model = new AccountModel();
            model.Account = acc;
            model.Password = pwd;

            //id++;
            //添加到数据库
            model.Add();
            model.GetModel(acc);
            //添加到内存
            accModelDict[acc] = model;
            return true;
        }
        /// <summary>
        /// 是否有账号
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public bool Has(string acc)
        {
            if (accModelDict.ContainsKey(acc))
                return true;
            //return accModelDict.ContainsKey(acc);

            AccountModel model = new AccountModel();
            //如果没有 那就真的没有
           if (!model.Exists(acc))
                return false;
            //如果数据库里面存在
            model.GetModel(acc);
            //添加到内存里
            accModelDict.TryAdd(model.Account, model);
            //再进行判断
            return true;
        }
        #endregion

        #region 在线玩家
        private SynchronizedDictionary<MobaClient, string> clientAccDict = new SynchronizedDictionary<MobaClient, string>();

        //双向映射
        private SynchronizedDictionary<string, MobaClient> accClientDict = new SynchronizedDictionary<string, MobaClient>();

        /// <summary>
        /// 玩家是否在线
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public bool IsOnline(string acc)
        {
            return accClientDict.ContainsKey(acc);

            //return clientAccDict.ContainsValue(acc);
        }
        /// <summary>
        /// 添加在线关系
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="client"></param>
        public bool Online(string acc, MobaClient client)
        {
            if(IsOnline(acc))
            {
                return false;
            }

            clientAccDict[client] = acc;
            accClientDict[acc] = client;
            return true;
        }
        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="client"></param>
        public void Offline(MobaClient client)
        {
            if (!clientAccDict.ContainsKey(client))
                return;

            string acc = clientAccDict[client];

            if (accClientDict.ContainsKey(acc))
                accClientDict.Remove(acc);

            if(clientAccDict.ContainsKey(client))
                clientAccDict.Remove(client);
        }
        #endregion

        /// <summary>
        /// 根据连接对象获取账号ID
        /// </summary>
        /// <param name="client"></param>
        /// <returns>有就返回，没有就返回-1</returns>
        public int GetId(MobaClient client)
        {
            if (!clientAccDict.ContainsKey(client))
                return -1;
            string account = clientAccDict[client];
            if (!accModelDict.ContainsKey(account))
                return -1;
            return accModelDict[account].Id;
        }
    }
}
