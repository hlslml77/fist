using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 炮塔的敌人检测
/// </summary>
public class TurretCheck : MonoBehaviour
{
    /// <summary>
    /// 表示当前炮塔的队伍
    /// </summary>
    [SerializeField]
    private int team;
    //public void SetTeam(int team)
    //{
    //    this.team = team;
    //}
    /// <summary>
    /// 检测到敌人列表
    /// </summary>
    public List<BaseControl> conList = new List<BaseControl>();
    public void OnTriggerEnter(Collider other)
    {
        BaseControl con = other.GetComponent<BaseControl>();
        if(con && con.Model.Team != team)
        {
            conList.Add(con);
        }

    }

    public void OnTriggerExit(Collider other)
    {
        BaseControl con = other.GetComponent<BaseControl>();
        if(con && conList.Contains(con))
        {
            conList.Remove(con);
        }
    }
}
