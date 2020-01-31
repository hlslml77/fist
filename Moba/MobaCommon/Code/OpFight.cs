using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Code
{
    public class OpFight
    {
        /// <summary>
        /// 进入
        /// </summary>
        public const byte Enter = 0;

        /// <summary>
        /// 获取数据
        /// </summary>
        public const byte GetInfo = 1;

        /// <summary>
        /// 移动
        /// </summary>
        public const byte Walk = 2;

        /// <summary>
        /// 技能
        /// </summary>
        public const byte Skill = 3;

        /// <summary>
        /// 伤害
        /// </summary>
        public const byte Damage = 4;

        /// <summary>
        /// 购买
        /// </summary>
        public const byte Buy = 5;

        /// <summary>
        /// 出售
        /// </summary>
        public const byte Sale = 6;

        /// <summary>
        /// 技能升级
        /// </summary>
        public const byte SkillUp = 7;
        /// <summary>
        /// 更新模型
        /// </summary>
        public const byte UpdateModel = 8;

        /// <summary>
        /// 复活
        /// </summary>
        public const byte Resurge = 9;

        /// <summary>
        /// 游戏结束
        /// </summary>
        public const byte GameOver = 10;

        /// <summary>
        /// 出兵
        /// </summary>
        public const byte Dog = 11;
    }
}
