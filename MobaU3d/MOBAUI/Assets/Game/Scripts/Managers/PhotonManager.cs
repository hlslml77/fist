using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Manager;
using MobaCommon.Code;

public class PhotonManager : Singleton<PhotonManager>, IPhotonPeerListener
{

    #region Receivers
    //账号
    private AccountReceiver account;
    public AccountReceiver Account
    {
        get
        {
            if (account == null)
                account = FindObjectOfType<AccountReceiver>();
            return account;
        }
    }

    //角色
    private PlayerReceiver player;
    public PlayerReceiver Player
    {
        get
        {
            if (player == null)
                player = FindObjectOfType<PlayerReceiver>();
            return player;
        }
    }

    //选人
    private SelectReceiver select;
    public SelectReceiver Select
    {
        get
        {
            if (select == null)
                select = FindObjectOfType<SelectReceiver>();
            return select;
        }
    }

    //战斗
    private FightReceiver fight;
    public FightReceiver Fight
    {
        get
        {
            if (fight == null)
            {
                //fightReceiver不继承MonoBehaviour这里会报错
                fight = FindObjectOfType<FightReceiver>();
            }
            return fight;
        }
    }
    #endregion

    #region Photon接口
    /// <summary>
    /// 代表客户端
    /// </summary>
    private PhotonPeer peer;

    /// <summary>
    /// IP地址
    /// </summary>
    private string serverAddress = "127.0.0.1:5055";

    /// <summary>
    /// 应用名字
    /// </summary>
    private string applicationName = "MOBA";

    /// <summary>
    /// 协议
    /// </summary>
    private ConnectionProtocol protocol = ConnectionProtocol.Udp;
    public void DebugReturn(DebugLevel level, string message)
    {
        
    }

    public void OnEvent(EventData eventData)
    {
        
    }
    /// <summary>
    /// 接收服务器的响应
    /// </summary>
    /// <param name="operationResponse"></param>
    public void OnOperationResponse(OperationResponse response)
    {
        Log.Debug(response.ToStringFull());
        //操作码
        byte opCode = response.OperationCode;
        //子操作
        byte subCode = (byte)response[80];
        //转接
        switch (opCode)
        {
            case OpCode.AccountCode:
                // MessageTip.Instance.Show("fdafa");
                //if (Account != null)
                    Account.OnReceive(subCode, response);
                break;
            case OpCode.PlayerCode:
                Player.OnReceive(subCode, response);
                break;
            case OpCode.SelectCode:
                Select.OnReceive(subCode, response);
                break;
            case OpCode.FightCode:
                Fight.OnReceive(subCode, response);
                break;

            default:
                break;
        }
    }
    /// <summary>
    /// 状态改变
    /// </summary>
    /// <param name="statusCode"></param>
    public void OnStatusChanged(StatusCode statusCode)
    {
        Log.Debug(statusCode);
        switch(statusCode)
        {
            case StatusCode.Connect:
                isConnect = true;
                break;
            case StatusCode.Disconnect:
                isConnect = false;
                break;
            default:
                break;
        }
    }
    #endregion
    private bool isConnect = false;

    protected override void Awake()
    {
        base.Awake();
        peer = new PhotonPeer(this, protocol);
        peer.Connect(serverAddress, applicationName);
        //设置自身物体是跨场景存在的
        //切换了场景，脚本还是存在的
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!isConnect)
        {
            peer.Connect(serverAddress, applicationName);
        }
        peer.Service();
    }

    void OnApplicationQuit()
    {
        //断开连接
        peer.Disconnect();
    }

    /// <summary>
    /// 向服务器发请求
    /// </summary>
    /// <param name="code">操作码</param>
    /// <param name="SubCode">子操作码</param>
    /// <param name="parameters">参数</param>
    public void Request(byte code, byte SubCode, params object[] parameters)
    {
        //创建参数字典
        Dictionary<byte, object> dict = new Dictionary<byte, object>();
        //指字子操作码
        dict[80] = SubCode;
        //赋值参数
        for (int i = 0; i < parameters.Length; i++)
        {
            dict[(byte)i] = parameters[i];

        }
        //发送
        peer.OpCustom(code, dict, true);
    }
}
