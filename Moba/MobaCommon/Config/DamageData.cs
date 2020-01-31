using MobaCommon.Dto.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Config
{
    /// <summary>
    /// 伤害的数据
    /// </summary>
    public class DamageData
    {
        /// <summary>
        /// 技能ID和技能的映射关系
        /// </summary>
        static Dictionary<int, ISkill> idSkillDict = new Dictionary<int, ISkill>();

        static DamageData()
        {
            //添加普通攻击的映射
            idSkillDict.Add(1, new AttackSkill());
        }

        /// <summary>
        /// 根据ID获取对应的技能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ISkill GetSkill(int id)
        {
            if(!idSkillDict.ContainsKey(id))
            {
                return null;
            }

            return idSkillDict[id];
        }
    }
}
