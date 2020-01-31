using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;
using UnityEngine;
using MobaCommon.Code;
using MobaCommon.Dto;
using LitJson;
using UnityEngine.SceneManagement;

public class SelectReceiver : MonoBehaviour, IReceiver
{
    [SerializeField]
    private SelectView view;

    SelectModel[] team1;

    SelectModel[] team2;

    int myTeam;

    public void OnReceive(byte subCode, OperationResponse response)
    {
        switch (subCode)
        {
            case OpSelect.GetInfo:
                //先保存数据
                team1 = JsonMapper.ToObject<SelectModel[]>(response[0].ToString());
                team2 = JsonMapper.ToObject<SelectModel[]>(response[1].ToString());
                GetTeam(team1, team2);
                onUpdateView();
                break;
            case OpSelect.Enter:
                int pId = (int)response[0];
                onEnter(pId);
                break;
            case OpSelect.Destroy:
                //关闭选人界面
                UIManager.Instance.HideUIPanel(UIDefinit.UISelect);
                //打开主界面
                UIManager.Instance.ShowUIPanel(UIDefinit.UIMain);
                break;
            case OpSelect.Select:
                if (response.ReturnCode == 0)
                    onSelect((int)response[0], (int)response[1]);
                break;
            case OpSelect.Ready:
                onReady((int)response[0]);
                break;
            case OpSelect.Talk:
                string resultTalk = response[0].ToString();
                onTalk(resultTalk);
                break;
            case OpSelect.StartFight:
                //跳转场景
                SceneManager.LoadScene("Fight");
                break;
            default:
                break;
        }
    }

    #region 帮助方法

    private void onTalk(string text)
    {
        view.TalkAppend(text);
    }

    /// <summary>
    /// 确认选择
    /// </summary>
    /// <param name="v"></param>
    private void onReady(int playerId)
    {
        foreach (SelectModel item in team1)
        {
            if (item.playerId == playerId)
            {
                item.isReady = true;
                
                onUpdateView();
                return;
            }
        }

        foreach (SelectModel item in team2)
        {
            if (item.playerId == playerId)
            {
                item.isReady = true;
                onUpdateView();
                return;
            }
        }
    }

    /// <summary>
    /// 英雄选择
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="heroId"></param>
    private void onSelect(int playerId, int heroId)
    {
        foreach (SelectModel item in team1)
        {
            if (item.playerId == playerId)
            {
                item.heroId = heroId;
                onUpdateView();
                return;
            }
        }
        foreach (SelectModel item in team2)
        {
            if (item.playerId == playerId)
            {
                item.heroId = heroId;
                onUpdateView();
                return;
            }
        }
    }
    /// <summary>
    /// 有其他玩家进入
    /// </summary>
    /// <param name="playerId"></param>
    private void onEnter(int playerId)
    {
        foreach (SelectModel item in team1)
        {
            if (item.playerId == playerId)
            {
                item.isEnter = true;
                onUpdateView();
                return;
            }
        }

        foreach (SelectModel item in team2)
        {
            if (item.playerId == playerId)
            {
                item.isEnter = true;
                onUpdateView();
                return;
            }
        }
    }

    /// <summary>
    /// 获取房间数据
    /// </summary>
    /// <param name="team1"></param>
    /// <param name="team2"></param>
    private void onUpdateView()
    {
        //更新显示
        view.UpdateView(myTeam, team1, team2);
    }

    /// <summary>
    /// 获取队伍
    /// </summary>
    /// <param name="team1"></param>
    /// <param name="team2"></param>
    /// <returns></returns>
    private void GetTeam(SelectModel[] team1, SelectModel[] team2)
    {
        int playerId = GameData.Player.id;
        for (int i = 0; i < team1.Length; i++)
        {
            if (team1[i].playerId == playerId)
                this.myTeam = 1;
        }
        for (int i = 0; i < team2.Length; i++)
        {
            if (team2[i].playerId == playerId)
                this.myTeam = 2;
        }
    } 


    #endregion
}

