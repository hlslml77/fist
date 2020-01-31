using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Code
{
    /// <summary>
    /// 角色操作码
    /// </summary>
    public class OpPlayer
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        public const byte GetInfo = 0;
        /// <summary>
        /// 创建角色
        /// </summary>
        public const byte Create = 1;
        /// <summary>
        /// 玩家上线
        /// </summary>
        public const byte Online = 2;
        /// <summary>
        /// 添加好友（向服务器请求加好友）
        /// </summary>
        public const byte RequestAdd = 3;

        /// <summary>
        /// 添加好友（服务器转发给客户端 客户端决定是否加好友）
        /// </summary>
        public const byte ToClientAdd = 4;

        /// <summary>
        /// 好友上线
        /// </summary>
        public const byte FriendOnline = 5;

        /// <summary>
        /// 好友下线
        /// </summary>
        public const byte FriendOffline = 6;

        /// <summary>
        /// 开始匹配
        /// </summary>
        public const byte StartMatch = 7;

        /// <summary>
        /// 匹配完成
        /// </summary>
        public const byte MatchComplete = 8;

        /// <summary>
        /// 停止匹配
        /// </summary>
        public const byte StopMatch = 9;
    }
}
