using MobaCommon.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 所有战斗模型的控制器基类
/// </summary>
public class BaseControl : MonoBehaviour {
    /// <summary>
    /// 此物体数据模型
    /// </summary>
	public DogModel Model { get; set; }

    /// <summary>
    /// 目标
    /// </summary>
    [SerializeField]
    public BaseControl target;

    public void Init(DogModel model, bool friend)
    {
        //保存数据
        this.Model = model;
        //设置血条颜色
        hpControl.SetColor(friend);
        //根据布尔变量来设置其层级
        string layer = friend ? "Friend" : "Enemy";
        gameObject.layer = LayerMask.NameToLayer(layer);
        
    }
    #region 动画

    [SerializeField]
    protected AnimControl animControl;

    /// <summary>
    /// 当前的动画状态
    /// </summary>
    protected AnimState state = AnimState.FREE;

    #endregion


    #region 血量
    [SerializeField]
    protected HpControl hpControl;
    /// <summary>
    /// 血量改变
    /// </summary>
    public void OnHpChange()
    {
        hpControl.SetHp((float)Model.CurrHp / Model.MaxHp);
    }
    #endregion

    #region 移动控制
    [SerializeField]
    protected NavMeshAgent agent;

    /// <summary>
    /// 是否正在移动
    /// </summary>
    protected bool IsMoving
    {
        get { return agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity != Vector3.zero || agent.pathStatus != NavMeshPathStatus.PathComplete; }
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="point">目标点</param>
    public void Move(Vector3 point)
    {
        if (state == AnimState.DEATH)
            return;
        //y不能有变化
        point.y = transform.position.y;
        //寻路
        agent.ResetPath();
        agent.SetDestination(point);
        //播放动画
        animControl.Walk();
        state = AnimState.WALK;
    }


    protected virtual void Update()
    {
        //检测寻路是否终止
        if(state == AnimState.WALK && !IsMoving)
        {
            animControl.Free();
            state = AnimState.FREE;
        }
    }
    #endregion

    #region 攻击控制

    //单机：选择人物 直接攻击 计算伤害
    //网游：选择人物 给服务器发送一个要攻击的ID，先同步攻击动画
    //动画播放到关键帧时候在服务器计算伤害，然后同步伤害到每一个客户端

    /// <summary>
    /// 请求攻击
    /// 每一个子类攻击方式都不一样所以用虚函数
    /// 子类会重写就用virtual
    /// </summary>
    public virtual void RequestAttack()
    {

    }

    /// <summary>
    /// 攻击响应
    /// </summary>
    /// <param name="target"></param>
    public virtual void AttackResponse(params Transform[] target)
    {

    }
    /// <summary>
    /// 技能响应
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="target"></param>
    /// <param name="targetPos"></param>
    public virtual void SkillResponse(int skillId, Transform target, Vector3 targetPos)
    {

    }
    #endregion


    #region 音效

    [SerializeField]
    protected AudioSource audioSource;

    /// <summary>
    /// 音效名称和音效文件的映射关系
    /// </summary>
    protected Dictionary<string, AudioClip> nameClipDict = new Dictionary<string, AudioClip>();

    protected virtual void Start()
    {
        audioSource.playOnAwake = false;
        audioSource.loop = false;

    }

    /// <summary>
    /// 播放音效 根据状态
    /// </summary>
    protected void PlayAudio(string name)
    {
        audioSource.clip = nameClipDict[name];
        audioSource.Play();
    }
    #endregion

    /// <summary>
    /// 死亡
    /// </summary>
    public virtual void DeathResponse()
    {

    }
    /// <summary>
    /// 重生
    /// </summary>
    public virtual void ResurgeResponse()
    {
        
    }
}
