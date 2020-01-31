using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Dto
{
    /// <summary>
    /// 英雄模型
    /// </summary>
    public class HeroModel : DogModel
    {
        /// <summary>
        /// 当前蓝量
        /// </summary>
        public int CurrMp { get; set; }
        /// <summary>
        /// 最大蓝量
        /// </summary>
        public int MaxMp { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 经验
        /// </summary>
        public int Exp { get; set; }
        /// <summary>
        /// 金钱
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// 装备列表
        /// </summary>
        public int[] Equipments { get; set; }
        /// <summary>
        /// 技能列表
        /// </summary>
        public SkillModel[] Skills { get; set; }
        /// <summary>
        /// 技能点数
        /// </summary>
        public int Points { get; set; }
        /// <summary>
        /// 击杀个数
        /// </summary>
        public int Kill { get; set; }
        /// <summary>
        /// 死亡次数
        /// </summary>
        public int Dead { get; set; }
        public HeroModel()
        {

        }

        public HeroModel(int id, int typeId, int team, int maxHp, int attack, int defense, double attackDistance, string name, int maxMp, SkillModel[] skills) : base(id, typeId, team, maxHp, attack, defense, attackDistance, name)
        {
            this.MaxMp = maxMp;
            this.CurrMp = maxMp;
            this.Level = 1;
            this.Exp = 0;
            this.Money = 500;
            this.Equipments = new int[6] { -1,-1,-1, -1, -1, -1 };
            this.Skills = skills;
            this.Points = 1;
            this.Kill = 0;
            this.Dead = 0;
        }
    }
}
