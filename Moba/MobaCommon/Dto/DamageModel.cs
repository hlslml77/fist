using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Dto
{
    /// <summary>
    /// 伤害的数据模型
    /// </summary>
    public class DamageModel
    {
        /// <summary>
        /// 使用者ID
        /// </summary>
        public int fromId;

        /// <summary>
        /// 被攻击者ID
        /// </summary>
        public int toId;

        /// <summary>
        /// 技能ID
        /// </summary>
        public int skillId;

        /// <summary>
        /// 伤害
        /// </summary>
        public int damage;
        /// <summary>
        /// 是否活着
        /// </summary>
        public bool isDead;

        public DamageModel()
        {

        }

        public DamageModel(int fromId, int toId, int damage, bool isDead, int skillId)
        {
            this.fromId = fromId;
            this.toId = toId;
          
            this.damage = damage;
            this.isDead = isDead;
            this.skillId = skillId;
        }
    }
}
