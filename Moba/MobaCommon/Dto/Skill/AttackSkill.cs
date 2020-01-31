using MobaCommon.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Dto.Skill
{
    /// <summary>
    /// 普通攻击
    /// </summary>
    public class AttackSkill : ISkill
    {
        public List<DamageModel> Damage(int skillId, int level, DogModel from, params DogModel[] to)
        {
            List<DamageModel> list = new List<DamageModel>();
            //攻击者的攻击力
            int attack = from.Attack;
            foreach (var item in to)
            {
                //被攻击者的防御力
                int defense = item.Defense;
                //计算伤害
                int damage = attack - defense;
                //掉血
                item.CurrHp -= damage;
                //保证血限大于0
                if (item.CurrHp <= 0)
                {
                    item.CurrHp = 0;
                }
                //添加到列表
                list.Add(new DamageModel(from.Id, item.Id, damage, item.CurrHp == 0, skillId));
            }

            return list;
        }

    
    }
}
