using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using MobaCommon.Code;
using MobaCommon.Dto;
using LitJson;
public class PlayerReceiver : MonoBehaviour, IReceiver
{
    [SerializeField]
    private MainView view;

    public void OnReceive(byte subCode, OperationResponse response)
    {
        switch (subCode)
        {
            case OpPlayer.GetInfo:
                onGetInfo(response.ReturnCode);
                break;
            case OpPlayer.Create:
                onCreate();
                break;
            case OpPlayer.Online:
                PlayerDto dto = JsonMapper.ToObject<PlayerDto>(response[0].ToString());
                onOnline(dto);
                break;
            case OpPlayer.RequestAdd:
                onRequestAdd(response.ReturnCode, response.DebugMessage);
                break;
            case OpPlayer.ToClientAdd:
                onToClientAdd(response.ReturnCode, JsonMapper.ToObject<PlayerDto>(response[0].ToString()));
                break;
            case OpPlayer.FriendOffline:
                onFriendOff((int)response[0]);
                break;
            case OpPlayer.FriendOnline:
                onFriendOn((int)response[0]);
                break;
            case OpPlayer.MatchComplete:
                onMatchComplete();
                break;
            default:
                break;
        }
    }
    private void onMatchComplete()
    {
        view.onStopMatch();
        //播放音乐
        SoundManager.Instance.PlayEffectMusic(view.acCountDown);
        MessageTip.Instance.Show("匹配成功，10S内进入房间", () =>
        {
            //先关闭主界面
            UIManager.Instance.HideUIPanel(UIDefinit.UIMain);

            //打开选人界面
            UIManager.Instance.ShowUIPanel(UIDefinit.UISelect);
            //停止特效音乐的播放
            SoundManager.Instance.StopEffectMusic();
        },10);
    }
    /// <summary>
    /// 好友下线
    /// </summary>
    /// <param name="friendId"></param>
    private void onFriendOff(int friendId)
    {
        view.UpdateFriendView(friendId, false);
    }

    /// <summary>
    /// 好友上线
    /// </summary>
    /// <param name="friendId"></param>
    private void onFriendOn(int friendId)
    {
        view.UpdateFriendView(friendId, true);
    }
    /// <summary>
    /// 客户端收到加好友请求
    /// </summary>
    /// <param name="dto"></param>
    private void onToClientAdd(int retCode, PlayerDto dto)
    {
        if(retCode == 0)
        {
            //给服务器发送同不同意
            view.ShowToClientAddPanel(dto);
        }
        else if(retCode == 1)
        {
            view.UpdateView(dto);
        }
        
    }

    /// <summary>
    /// 发起加好友请求
    /// </summary>
    private void onRequestAdd(int retCode, string mess)
    {
        MessageTip.Instance.Show(mess);
    }

    /// <summary>
    /// 上线
    /// </summary>
    private void onOnline(PlayerDto dto)
    {
        //保存数据
        GameData.Player = dto;
        //刷新视图
        view.UpdateView(dto);
    }
    /// <summary>
    /// 创建角色
    /// </summary>
    private void onCreate()
    {
        //隐藏创建面板
        view.CreatePanelActive = false;
        //创建成功后发起上线请求
        playerOnline();

    }
    private void onGetInfo(int retCode)
    {
        if (retCode == -1)
        {
            //非法登录
        }
        else if (retCode == 0)
        {
            //有角色
            //角色上线
            playerOnline();
        }
        else if (retCode == -2)
        {
            //没有角色
            //显示创建面板
            view.CreatePanelActive = true;

        }
    }

    /// <summary>
    /// 角色上线
    /// </summary>
    private void playerOnline()
    {
        PhotonManager.Instance.Request(OpCode.PlayerCode, OpPlayer.Online);

    }
}
