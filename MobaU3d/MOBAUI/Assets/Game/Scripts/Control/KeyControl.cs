using MobaCommon.Code;
using MobaCommon.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyControl : MonoBehaviour {
    /// <summary>
    /// E技能
    /// </summary>
    [SerializeField]
    private KeyCode Skill_E = KeyCode.E;

    [SerializeField]
    private UISkill uiSkill_E;

	void Update () {
        #region 当鼠标右键点击
        //当鼠标右键点击
        if (Input.GetMouseButton(1))
        {
            Vector2 mouse = Input.mousePosition;
            //转成三维射线
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            //不能只检测一个
            //RaycastHit hit;
            //判断有没有投射到
            //if (!Physics.Raycast(ray, out hit))
            //    return;
            //如果点到了地面 那就移动
            RaycastHit[] his = Physics.RaycastAll(ray);
            for (int i = his.Length - 1; i >= 0; i--)
            {
                RaycastHit hit = his[i];
                //如果点到了敌方单位 那就攻击
                if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
                {
                    attack(hit.collider.gameObject);
                    //如果攻击到了地方，就不往下继续判断了 因为不需要行走了
                    break;
                }
                //如果点到了地面 那就移动
                else if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))
                {
                    Move(hit.point);
                }
            }
        }

        #endregion

        #region 空格
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //焦距到自己的英雄
            Camera.main.GetComponent<CameraControl>().FocusOn();
        }
        #endregion

        #region 技能释放
        if(Input.GetKeyDown(Skill_E) && uiSkill_E.CanUse)
        {
            Vector2 mouse = Input.mousePosition;
            //转成三维射线
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //释放技能
                skill(3, hit.point);
            }

        }
        #endregion
    }

    /// <summary>
    /// 攻击
    /// </summary>
    /// <param name="target">目标</param>
    private void attack(GameObject target)
    {
        //获取相对距离
        float distance = Vector3.Distance(GameData.MyControl.transform.position, target.transform.position);
        //超过攻击范围 走到最近的一个点 然后攻击,或者对方血量已经为0就不攻击
        if (distance > GameData.MyControl.Model.Attack || target.GetComponent<BaseControl>().Model.CurrHp <= 0)
        {
            return;
        }
        //在攻击范围内 直接攻击
        else
        {
            //获取目标的Id
            int targetId = target.GetComponent<BaseControl>().Model.Id;
            //int myId = GameData.MyControl.Model.Id;
            //向服务器发起请求 参数: 1.技能的ID 2.攻击者ID，3.目标ID
            int attackId = GameData.MyControl.Model.Id;
            PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Skill, 1, attackId, targetId, -1f, -1f, -1f);

        }
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="point"></param>
    private void Move(Vector3 point)
    {
        //显示一个特效
        GameObject go = PoolManager.Instance.GetObject("ClickMove");
        go.transform.position = point + Vector3.up;
        //给服务器发一个请求 参数：点的坐标
        //服务器不支持vector3格式的数据
        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Walk, point.x, point.y, point.z);
        
    }

    private void skill(int index, Vector3 targetPos)
    {
        HeroModel myHero = (HeroModel)GameData.MyControl.Model;
        int skillId = myHero.Skills[index - 1].Id;

        int attackId = myHero.Id;
        //向服务器发起请求 参数：1、技能的ID 2、攻击者ID， 3、目标ID 4、目标点的坐标
        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Skill, skillId, attackId, -1, targetPos.x, targetPos.y, targetPos.z);

    }
}
