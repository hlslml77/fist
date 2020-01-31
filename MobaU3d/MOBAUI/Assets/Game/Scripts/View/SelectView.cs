using LitJson;
using MobaCommon.Code;
using MobaCommon.Config;
using MobaCommon.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SelectView : UIBase, IResourceListener
{
    [SerializeField]
    private UIPlayer[] team1;
    [SerializeField]
    private UIPlayer[] team2;

    private AudioClip acClick;
    private AudioClip acReady;

    #region UIBase
    public override void Init()
    {
        //不需要加载，直接调用资源
        acClick = ResourcesManager.Instance.GetAsset(Paths.RES_SOUND_UI + "Click") as AudioClip;

        //需要加载
        ResourcesManager.Instance.Load(Paths.RES_SOUND_UI + "Ready", typeof(AudioClip), this);

    }

    public void OnLoaded(string assetName, object asset)
    {

        if (assetName == Paths.RES_SOUND_UI + "Ready")
            acReady = asset as AudioClip;

    }

    public override void OnShow()
    {
        //先要调用base里面的方法
        base.OnShow();
        //发起进入的请求
        PhotonManager.Instance.Request(OpCode.SelectCode, OpSelect.Enter);
        //初始化玩家拥有英雄的列表,在此调用防止一有玩家加入就多次加载英雄头像
        this.InitSelectHeroPanel(GameData.Player.heroIds);
        //清空聊天框
        txtContent.text = string.Empty;
    }
    public override void OnDestroy()
    {

    }

    public override string UIName()
    {
        return UIDefinit.UISelect;
    }

    #endregion

   

    [Header("英雄选择预设")]
    [SerializeField]
    private GameObject UIHero;
    [SerializeField]
    private Transform heroParent;

    /// <summary>
    /// 已经加载的英雄选择头像
    /// </summary>
    private Dictionary<int, UIHero> idHeroDict = new Dictionary<int, UIHero>();
    /// <summary>
    /// 初始化选择英雄面板
    /// </summary>
    /// <param name="heroIds"></param>
    public void InitSelectHeroPanel(int[] heroIds)
    {
        GameObject go;
        foreach (var id in heroIds)
        {
            if (idHeroDict.ContainsKey(id))
                continue;
            //实例化预设体
            go = Instantiate(UIHero);
            UIHero hero = go.GetComponent<UIHero>();
            hero.InitView(HeroData.GetHeroData(id));
            go.transform.SetParent(heroParent);
            //缩放设为1防止它消失
            go.transform.localScale = Vector3.one;
            idHeroDict.Add(id, hero);

        }
    }
    [SerializeField]
    private Button btnReady;
    /// <summary>
    /// 确认选择
    /// </summary>
    public void ReadySelect()
    {
        //播放音乐
        SoundManager.Instance.PlayEffectMusic(acReady);
        //禁用按钮
        btnReady.interactable = false;
        //给服务器发送请求
        PhotonManager.Instance.Request(OpCode.SelectCode, OpSelect.Ready);

       
    }

    /// <summary>
    /// 聊天
    /// </summary>
    [Header("聊天模块")]
    [SerializeField]
    private Text txtContent;
    [SerializeField]
    private InputField inTalk;
    [SerializeField]
    private Scrollbar bar;

    public void OnSendClick()
    {
        //播放音乐
        SoundManager.Instance.PlayEffectMusic(acClick);

        string text = inTalk.text;
        if (string.IsNullOrEmpty(text))
        {
            return;
        }
        //给服务器发送聊天请求
        PhotonManager.Instance.Request(OpCode.SelectCode, OpSelect.Talk, text);
        //清除上次输入
        inTalk.text = string.Empty;
    }

    /// <summary>
    /// 追加聊天内容
    /// </summary>
    /// <param name="text"></param>
    public void TalkAppend(string text)
    {
        //添加一行记录
        txtContent.text += "\n" + text;
        //每次聊天都显示最后一行
        bar.value = 0;
    }

    /// <summary>
    /// 更新视图显示
    /// </summary>
    /// <param name="myTeam"></param>
    /// <param name="team1"></param>
    /// <param name="team2"></param>
    public void UpdateView(int myTeam, SelectModel[] team1, SelectModel[] team2)
    {
        List<int> selectedHero = new List<int>();
        if (myTeam == 1)
        {
            for (int i = 0; i < team1.Length; i++)
                this.team1[i].UpdateView(team1[i]);
            for (int i = 0; i < team2.Length; i++)
                this.team2[i].UpdateView(team2[i]);
            //禁用 一个队伍已经选择的英雄头像的点击属性
            foreach (SelectModel item in team1)
            {
                if (item.heroId != -1)
                    selectedHero.Add(item.heroId);
            }
        }
        else if (myTeam == 2)
        {
            for (int i = 0; i < team2.Length; i++)
                this.team1[i].UpdateView(team2[i]);
            for (int i = 0; i < team1.Length; i++)
                this.team2[i].UpdateView(team1[i]);
            //禁用 一个队伍已经选择的英雄头像的点击属性
            foreach (SelectModel item in team2)
            {
                if (item.heroId != -1)
                    selectedHero.Add(item.heroId);
            }
        }
        //禁用英雄
        foreach (UIHero item in idHeroDict.Values)
        {
            //如果当前这个英雄已经被选择了 或者 玩家已准备
            if (selectedHero.Contains(item.Id) || btnReady.interactable == false)
                item.Interactable = false;
            else
                item.Interactable = true;
        }
    }
}