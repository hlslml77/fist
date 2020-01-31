using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Config
{

    /// <summary>
    /// 建筑的数据
    /// </summary>
    public class BuildData
    {
        //基地
        public const int Main = 1;
        //兵营
        public const int Camp = 2;
        //炮塔
        public const int Turret = 3;

        /// <summary>
        /// 类型和模型的映射
        /// </summary>
        static Dictionary<int, BuildDataModel> idBuildDict = new Dictionary<int, BuildDataModel>();

        static BuildData()
        {
            createBuild(Main, 5000, -1, 100, -1, "主基地", false, false, -1);
            createBuild(Camp, 3000, -1, 100, -1, "兵营", false, true, 300);
            createBuild(Turret, 5000, 200, 20, 15, "炮塔", true, false, -1);

        }

        private static void createBuild(int typeId, int hp, int attack, int defense, double attackDistance, string name, bool agressive, bool rebirth, int rebirthTime)
        {
            BuildDataModel model = new BuildDataModel(typeId, hp, attack, defense, attackDistance, name, agressive, rebirth, rebirthTime);
            idBuildDict.Add(model.TypeId, model);
        }

        /// <summary>
        /// 根据类型获取数据
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public static BuildDataModel GetBuildData(int typeId)
        {
            BuildDataModel model = null;
            idBuildDict.TryGetValue(typeId, out model);
            return model;
        }
    }

    /// <summary>
    /// 建筑的数据模型
    /// </summary>
    public class BuildDataModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 血量
        /// </summary>
        public int Hp { get; set; }
        /// <summary>
        /// 攻击
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
        /// <summary>
        /// 是否攻击
        /// </summary>
        public bool Agressive { get; set; }
        /// <summary>
        /// 是否重生
        /// </summary>
        public bool Rebirth { get; set; }
        /// <summary>
        /// 重生时间
        /// </summary>
        public int RebirthTime { get; set; }

        public BuildDataModel(int typeId, int hp, int attack, int defense, double attackDistance, string name, bool agressive, bool rebirth, int rebirthTime)
        {
            this.TypeId = typeId;
            this.Hp = hp;
            this.Attack = attack;
            this.Defense = defense;
            this.AttackDistance = attackDistance;
            this.Name = name;
            this.Agressive = agressive;
            this.Rebirth = rebirth;
            this.RebirthTime = rebirthTime;
        }
    }

}
