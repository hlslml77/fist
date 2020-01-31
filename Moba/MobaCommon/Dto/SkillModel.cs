using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Dto
{
    /// <summary>
    /// 技能模型
    /// </summary>
    public class SkillModel
    {
        /// <summary>
        /// 技能的识别码
        /// </summary>
        public int Id;
        /// <summary>
        /// 当前技能等级
        /// </summary>
        public int Level;
        /// <summary>
        /// 下一次学习的等级
        /// </summary>
        public int LearnLevel;
        /// <summary>
        /// 冷却时间
        /// </summary>
        public int CoolDown;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 说明
        /// </summary>
        public string Description;
        /// <summary>
        /// 攻击距离
        /// </summary>
        public double Distance;

        public SkillModel() { }

    }
}
