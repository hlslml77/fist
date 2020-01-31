using MobaCommon.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Dto.Skill
{
    /// <summary>
    /// 技能
    /// </summary>
    public interface ISkill
    {
        /// <summary>
        /// 计算伤害
        /// </summary>
        /// <param name="from">攻击者</param>
        /// <param name="to">被攻击者</param>
        /// <param name="level">技能等级</param>
        ///<returns>返回伤害的数据模型</returns>
        List<DamageModel> Damage(int skillId, int level, DogModel from, params DogModel[] to);
    }
}
