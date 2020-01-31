using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Config
{
    /// <summary>
    /// 技能表
    /// </summary>
    public class SkillData
    {
        static Dictionary<int, SkillDataModel> idSkillDict = new Dictionary<int, SkillDataModel>();

        static SkillData()
        {
            createSkill(1001, "技能一", "我天生的第一个技能。",
                new SkillLevelDataModel(1, 0, 0, 0, 0),
                new SkillLevelDataModel(3, 7, 60, 20, 10),
                new SkillLevelDataModel(5, 6, 70, 25, 10),
                new SkillLevelDataModel(9, 5, 80, 30, 10),
                new SkillLevelDataModel(-1, 4, 90, 40, 10));

            createSkill(1002, "技能二", "我天生的第二个技能。",
                new SkillLevelDataModel(1, 0, 0, 0, 0),
                new SkillLevelDataModel(3, 7, 60, 20, 10),
                new SkillLevelDataModel(5, 6, 70, 25, 10),
                new SkillLevelDataModel(9, 5, 80, 30, 10),
                new SkillLevelDataModel(-1, 4, 90, 40, 10));

            createSkill(1003, "技能三", "我天生的第三个技能。",
                new SkillLevelDataModel(1, 0, 0, 0, 0),
                new SkillLevelDataModel(3, 7, 60, 20, 10),
                new SkillLevelDataModel(5, 6, 70, 25, 10),
                new SkillLevelDataModel(9, 5, 80, 30, 10),
                new SkillLevelDataModel(-1, 4, 90, 40, 10));

            createSkill(1004, "必杀技", "不放则以，放则秒杀",
                new SkillLevelDataModel(6, 0, 0, 0, 0),
                new SkillLevelDataModel(9, 100, 100, 100, 10),
                new SkillLevelDataModel(11, 95, 110, 120, 10),
                new SkillLevelDataModel(16, 80, 150, 150, 10));
        }

        private static void createSkill(int id, string name, string des, params SkillLevelDataModel[] lvModels)
        {
            //创建数据
            SkillDataModel data = new SkillDataModel(id, name, des, lvModels);
            //保存到字典
            idSkillDict.Add(id, data);
        }

        public static SkillDataModel GetSkillData(int id)
        {
            SkillDataModel data = null;
            idSkillDict.TryGetValue(id, out data);
            return data;
        }
    }

    /// <summary>
    /// 技能的数据模型
    /// </summary>
    public class SkillDataModel
    {
        /// <summary>
        /// 技能识别码
        /// </summary>
        public int Id;
        /// <summary>
        /// 技能的名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 技能的说明
        /// </summary>
        public string Description;
        /// <summary>
        /// 技能等级的信息
        /// </summary>
        public SkillLevelDataModel[] LvModels;

        public SkillDataModel() { }

        public SkillDataModel(int id, string name, string des, SkillLevelDataModel[] lvModels)
        {
            this.Id = id;
            this.Name = name;
            this.Description = des;
            this.LvModels = lvModels;
        }
    }

    /// <summary>
    /// 技能等级的数据模型
    /// </summary>
    public class SkillLevelDataModel
    {
        /// <summary>
        /// 学习技能的等级
        /// </summary>
        public int LearnLv;
        /// <summary>
        /// 技能冷却
        /// </summary>
        public int CoolDown;
        /// <summary>
        /// 技能伤害
        /// </summary>
        public int Damage;
        /// <summary>
        /// 蓝消耗
        /// </summary>
        public int Mp;
        /// <summary>
        /// 技能的距离
        /// </summary>
        public double Distance;

        public SkillLevelDataModel() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="learnLv"></param>
        /// <param name="cd"></param>
        /// <param name="mp"></param>
        /// <param name="distance"></param>
        public SkillLevelDataModel(int learnLv, int cd, int damage, int mp, double distance)
        {
            this.LearnLv = learnLv;
            this.CoolDown = cd;
            this.Damage = damage;
            this.Mp = mp;
            this.Distance = distance;
        }

    }
}
