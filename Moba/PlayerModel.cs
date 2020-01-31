using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moba.Model
{
    /// <summary>
    /// 玩家数据模型
    /// </summary>
    public class PlayerModel
    {
        private int id;
        private string name;
        private int lv;
        private int exp;
        private int power;
        private int winCount;
        private int loseCount;
        private int runCount;
        private List<int> heroIdList;
        private List<int> friendIdList;
        private int accountId;

        #region Property
        /// <summary>
        /// 主键
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
        /// <summary>
        /// 等级
        /// </summary>
        public int Lv
        {
            get
            {
                return lv;
            }

            set
            {
                lv = value;
            }
        }
        /// <summary>
        /// 经验
        /// </summary>
        public int Exp
        {
            get
            {
                return exp;
            }

            set
            {
                exp = value;
            }
        }
        /// <summary>
        /// 战斗力
        /// </summary>
        public int Power
        {
            get
            {
                return power;
            }

            set
            {
                power = value;
            }
        }
        /// <summary>
        /// 胜利场次
        /// </summary>
        public int WinCount
        {
            get
            {
                return winCount;
            }

            set
            {
                winCount = value;
            }
        }
        /// <summary>
        /// 失败场次
        /// </summary>
        public int LoseCount
        {
            get
            {
                return loseCount;
            }

            set
            {
                loseCount = value;
            }
        }
        /// <summary>
        /// 逃跑场次
        /// </summary>
        public int RunCount
        {
            get
            {
                return runCount;
            }

            set
            {
                runCount = value;
            }
        }
        /// <summary>
        /// 英雄ID列表
        /// </summary>
        public List<int> HeroIdList
        {
            get
            {
                return heroIdList;
            }

            set
            {
                heroIdList = value;
            }
        }
        /// <summary>
        /// 好友ID列表
        /// </summary>
        public List<int> FriendIdList
        {
            get
            {
                return friendIdList;
            }

            set
            {
                friendIdList = value;
            }
        }

        #endregion

        public PlayerModel()
        {

        }

        public PlayerModel(int id, string name, int accId)
        {
            this.Id = id;
            this.Name = name;
            this.accountId = accId;

            Lv = 1;
            Exp = 0;
            Power = 2000;
            WinCount = 0;
            LoseCount = 0;
            RunCount = 0;
            HeroIdList = new List<int> { 1,2};
            FriendIdList = new List<int>();

        }
    }
}
