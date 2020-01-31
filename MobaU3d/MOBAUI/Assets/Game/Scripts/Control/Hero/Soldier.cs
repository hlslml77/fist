using MobaCommon.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using MobaCommon.Config;
using MobaCommon.Dto;

/// <summary>
/// 战士 英雄的控制脚本
/// </summary>
public class Soldier : BaseControl, IResourceListener
{

    protected override void Start()
    {
        ResourcesManager.Instance.Load(Paths.RES_SOUND_FIGHT + "SoldierAttack", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_SOUND_FIGHT + "SoldierDeath", typeof(AudioClip), this);
    }

    public void OnLoaded(string assetName, object asset)
    {
        switch (assetName)
        {
            case Paths.RES_SOUND_FIGHT + "SoldierAttack":
                nameClipDict.Add("Attack", asset as AudioClip);
                break;
            case Paths.RES_SOUND_FIGHT + "SoldierDeath":
                nameClipDict.Add("Death", asset as AudioClip);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 动画播放完毕 计算伤害 
    /// </summary>
    public override void RequestAttack()
    {
        //如果是自身发起的攻击 那么return
        if (this != GameData.MyControl)
            return;
        //播放声音
       // PlayAudio("Attack");
        //获取目标的ID
        int targetId = target.GetComponent<BaseControl>().Model.Id;
        int myId = GameData.MyControl.Model.Id;
       
        //发起一个计算伤害的请求：参数1、技能ID，2、目标ID
        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Damage, myId, 1, new int[] { targetId });

    }

    /// <summary>
    /// 同步攻击动画 （不计算伤害，就是显示动画的）
    /// </summary>
    /// <param name="target"></param>
    public override void AttackResponse(params Transform[] target)
    {
        if (state == AnimState.DEATH)
            return;

        //保存目标
        this.target = target[0].GetComponent<BaseControl>();
        //一定要面向目标
        transform.LookAt(target[0]);
        //播放动画，什么时候攻击不知道，所以不需要在这里播放动画
        //animControl.Attack();
        //设置状态
        state = AnimState.ATTACK;

    }


    protected override void Update()
    {
        base.Update();

        //如果有目标 ，并且是攻击状态 我们就攻击
        if (target != null && state == AnimState.ATTACK)
        {
            //先检测攻击范围
            float distance = Vector3.Distance(transform.position, target.transform.position);
            //超过攻击范围 走到最近的一个点 然后攻击
            if (distance > Model.AttackDistance)
            {
                //不能用basecontrol里的move应该那里状态设置为了walk就不能攻击了
                //向目标走动
                agent.ResetPath();
                agent.SetDestination(target.transform.position);
                //播放动画
                animControl.Walk();
            }
            else//在攻击范围内 直接攻击
            {
                //停止寻路
                agent.isStopped = true;
                //一定要面向目标
                transform.LookAt(target.transform);
                //播放动画
                animControl.Attack();
                //改变状态
                state = AnimState.FREE;
            }
        }

    }

    public override void SkillResponse(int skillId, Transform target, Vector3 targetPos)
    {
        switch (skillId)
        {
            case 1001:
                break;
            case 1002:
                break;
            case 1003:
                targetPos.y = transform.position.y;
                //面向点
                transform.LookAt(targetPos);
                //加载技能的攻击特效
                GameObject go = PoolManager.Instance.GetObject("E");
                //初始化脚本
                go.GetComponent<LineSkill>().Init(
                    transform,
                    (float)((HeroModel)this.Model).Skills[2].Distance,
                    5f,
                    skillId,
                    this.Model.Id,
                    this == GameData.MyControl);
                break;
            case 1004:
                break;
            default:
                break;
        }
    }
    public override void DeathResponse()
    {
        //停止寻路
        //agent.Stop();
        agent.isStopped = true;
        //播放动画
        animControl.Death();
        //改变状态
        state = AnimState.DEATH;
        //播放声音
       // this.PlayAudio("Death");
    }
}
