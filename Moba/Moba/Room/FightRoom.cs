using MobaCommon.Config;
using MobaCommon.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moba.Room
{
    public class FightRoom : RoomBase<MobaClient>
    {
        #region 队伍1
        //英雄
        Dictionary<int, HeroModel> team1HeroModel = new Dictionary<int, HeroModel>();
        //小兵
        Dictionary<int, DogModel> team1DogModel = new Dictionary<int, DogModel>();
        //塔
        Dictionary<int, BuildModel> team1BuildModel = new Dictionary<int, BuildModel>();
        #endregion

        #region 队伍2
        //英雄
        Dictionary<int, HeroModel> team2HeroModel = new Dictionary<int, HeroModel>();
        //小兵
        Dictionary<int, DogModel> team2DogModel = new Dictionary<int, DogModel>();
        //塔
        Dictionary<int, BuildModel> team2BuildModel = new Dictionary<int, BuildModel>();
        #endregion

        /// <summary>
        /// 逃跑的客户端
        /// </summary>
        public List<MobaClient> LeaveClient = new List<MobaClient>();
        
        #region Property
        /// <summary>
        /// 是否全部进入
        /// </summary>
        public bool IsAllEnter
        {
            get { return ClientList.Count >= Count; }
        } 
        /// <summary>
        /// 是否全部退出
        /// </summary>
        public bool IsAllLeave
        {
            get { return ClientList.Count <= 0; }
        }

        public BuildModel[] Builds
        {
            get
            {
                List<BuildModel> list = new List<BuildModel>();
                //addrange一次加入多个值
                list.AddRange(team1BuildModel.Values);
                list.AddRange(team2BuildModel.Values);
                return list.ToArray();
            }
        }
        public HeroModel[] Heros
        {
            get
            {
                List<HeroModel> list = new List<HeroModel>();
                list.AddRange(team1HeroModel.Values);
                list.AddRange(team2HeroModel.Values);
                return list.ToArray();
            }
        }
        #endregion


        public FightRoom(int id, int count) : base(id, count)
        {

        }

        /// <summary>
        /// 初始化房间
        /// </summary>
        /// <param name="team1"></param>
        /// <param name="team2"></param>
        public void Init(List<SelectModel> team1, List<SelectModel> team2)
        {
            //初始化英雄数据 0--10000000000
            foreach (SelectModel item in team1)
            {
                team1HeroModel.Add(item.playerId, getHeroModel(item, 1));
            }
            foreach (SelectModel item in team2)
            {
                team2HeroModel.Add(item.playerId, getHeroModel(item, 2));
            }
            //初始化防御塔的数据
            //队伍1防御塔ID：-10
            //3个防御塔

            team1BuildModel.Add(-10, getBuildModel(-10, BuildData.Main, 1));
            team1BuildModel.Add(-11, getBuildModel(-11, BuildData.Camp, 1));
            team1BuildModel.Add(-12, getBuildModel(-12, BuildData.Turret, 1));

            //队伍2：-20
            //int type = BuildData.Main;
            //for (int id = -20; id > -23; id--, type++)
            //    team2BuildModel.Add(id, getBuildModel(id,type,2));
            // 队伍2： -20
            team2BuildModel.Add(-20, getBuildModel(-20, BuildData.Main, 2));
            team2BuildModel.Add(-21, getBuildModel(-21, BuildData.Camp, 2));
            team2BuildModel.Add(-22, getBuildModel(-22, BuildData.Turret, 2));
            //初始化小兵数据
            //spawnDog();
        }

        /// <summary>
        /// 小兵的ID
        /// </summary>
        private int dogId = -1000;
        public int DogId
        {
            get
            {
                dogId--;
                return dogId;
            }
        }
        /// <summary>
        /// 开启定时任务：30秒之后产生小兵
        /// </summary>
        private void spawnDog()
        {
            this.StartSchedule(DateTime.UtcNow.AddSeconds(30),
               delegate
               {
                   List<DogModel> dogs = new List<DogModel>();

                   for (int i = 0; i < 5; i++)
                   {
                       //产生小兵
                       DogModel dog = new DogModel();
                       dog.ModelType = ModelType.DOG;
                       //添加映射
                       team1DogModel.Add(dog.Id, dog);
                       dogs.Add(dog);

                       dog = new DogModel();
                       dog.ModelType = ModelType.DOG;
                       //添加映射
                       team2DogModel.Add(dog.Id, dog);
                       dogs.Add(dog);
                   }

                   //给 客户端发送 现在出兵了 发送的参数就是 dogs
                   //Brocast(OpCode.FightCode, OpFight.Dog, 0, "产生一波兵", null, LitJson.JsonMapper.ToJson(dogs.ToArray())); 

                   //自身调用自身，无限递归
                   spawnDog();
               });
        }

        /// <summary>
        /// 根据技能ID获取具体的 技能的数据实体
        /// </summary>
        /// <param name="skillIds"></param>
        /// <returns></returns>
        public SkillModel[] getSkillModel(int[] skillIds)
        {
            SkillModel[] skillModels = new SkillModel[skillIds.Length];
            for (int i = 0; i < skillIds.Length; i++)
            {
                //获取技能数据
                SkillDataModel data = SkillData.GetSkillData(skillIds[i]);
                //初始化的时候 就是最低级
                SkillLevelDataModel lvData = data.LvModels[0];
                skillModels[i] = new SkillModel()
                {
                    Id = data.Id,
                    Level = 0,
                    LearnLevel = lvData.LearnLv,
                    CoolDown = lvData.CoolDown,
                    Name = data.Name,
                    Description = data.Description,
                    Distance = lvData.Distance
                };
            }
            return skillModels;
        }
        /// <summary>
        /// 获取英雄数据
        /// </summary>
        /// <param name="heroId"></param>
        /// <returns></returns>
        private HeroModel getHeroModel(SelectModel model, int team)
        {
            //先从静态配置表里面 获取到英雄的数据
            HeroDataModel data = HeroData.GetHeroData(model.heroId);
            //英雄数据创建
            HeroModel hero = new HeroModel(model.playerId, data.TypeId, team, data.Hp, data.BaseAttack, data.BaseDefense, data.AttackDistance, data.Name, data.Mp, getSkillModel(data.SkillIds));
            hero.ModelType = ModelType.HERO;
            return hero;
        }

        /// <summary>
        /// 获取防御塔的数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="typeId">类型</param>
        /// <param name="team">队伍</param>
        /// <returns></returns>
        private BuildModel getBuildModel(int id, int typeId, int team)
        {
            //获取配置表里面的数据
            BuildDataModel data = BuildData.GetBuildData(typeId);

            BuildModel model = new BuildModel(id, typeId, team, data.Hp, data.Attack, data.Defense, data.AttackDistance, data.Name, data.Agressive, data.Rebirth, data.RebirthTime);

            model.ModelType = ModelType.BUILD;
            return model;
        }
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="client"></param>
        public void Enter(MobaClient client)
        {
            if(!ClientList.Contains(client))
            {
                ClientList.Add(client);
            }
        }

        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="client"></param>
        public void Leave(MobaClient client)
        {
            if (ClientList.Contains(client))
                ClientList.Remove(client);
            if (!LeaveClient.Contains(client))
                LeaveClient.Add(client);
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void Clear()
        {
            team1HeroModel.Clear();
            team1DogModel.Clear();
            team1BuildModel.Clear();
            team2HeroModel.Clear();
            team2DogModel.Clear();
            team2BuildModel.Clear();
            LeaveClient.Clear();
            ClientList.Clear();
        }
        /// <summary>
        /// 获取英雄模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HeroModel GetHeroModel(int id)
        {
            HeroModel model = null;
            if (team1HeroModel.TryGetValue(id, out model))
                return model;
            if (team2HeroModel.TryGetValue(id, out model))
                return model;
            return null;
        }

        /// <summary>
        /// 获取建筑模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BuildModel GetBuildModel(int id)
        {
            BuildModel model = null;
            if (team1BuildModel.TryGetValue(id, out model))
                return model;
            if (team2BuildModel.TryGetValue(id, out model))
                return model;
            return null;
        }

        /// <summary>
        /// 移除小兵在房间内的数据模型
        /// </summary>
        /// <param name="dog"></param>
        public void RemoveDog(DogModel dog)
        {
            if (dog.Team == 1)
                team1DogModel.Remove(dog.Id);
            else if (dog.Team == 2)
                team2DogModel.Remove(dog.Id);
        }
        /// <summary>
        /// 移除建筑在房间内的数据模型
        /// </summary>
        /// <param name="build"></param>
        public void RemoveBuild(BuildModel build)
        {
            if (build.Team == 1)
                team1DogModel.Remove(build.Id);
            else if (build.Team == 2)
                team2DogModel.Remove(build.Id);
        }
    }
}
