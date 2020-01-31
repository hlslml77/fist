using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Dto
{
    public enum ModelType
    {
        DOG,BUILD,HERO
    }

    /// <summary>
    /// 小兵的模型
    /// </summary>
    public class DogModel
    {
        /// <summary>
        /// 身份
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 类型（近战兵，远战兵等）
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 模型类型
        /// </summary>
        public ModelType ModelType { get; set; }
        /// <summary>
        /// 队伍
        /// </summary>
        public int Team { get; set; }
        /// <summary>
        /// 当前血量
        /// </summary>
        public int CurrHp { get; set; }
        /// <summary>
        /// 最大血量
        /// </summary>
        public int MaxHp { get; set; }
        /// <summary>
        /// 攻击力
        /// </summary>
        public int Attack { get; set; }
        /// <summary>
        /// 防御力
        /// </summary>
        public int Defense { get; set; }
        /// <summary>
        /// 攻击距离
        /// </summary>
        public double AttackDistance { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        public DogModel()
        {

        }

        public DogModel(int id, int typeId, int team, int maxHp, int attack, int defense, double attackDistance, string name)
        {
            this.Id = id;
            this.TypeId = typeId;
            this.Team = team;
            this.MaxHp = maxHp;
            this.CurrHp = maxHp;
            this.Attack = attack;
            this.Defense = defense;
            this.AttackDistance = attackDistance;
            this.Name = name;
        }

    }
}
