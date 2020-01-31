using MobaCommon.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using MobaCommon.Code;
using MobaCommon.Dto;

public class UIHero : MonoBehaviour, IResourceListener {

    [SerializeField]
    private Image imgHead;
    [SerializeField]
    private Button btnHead;

    private int heroId;
    public int Id
    {
        get
        {
            return heroId;
        }
    }
    private string heroName;

    private AudioClip ac;
    private AudioClip acSelect;
    /// <summary>
    /// 初始化视图
    /// </summary>
    /// <param name="hero"></param>
    public void InitView(HeroDataModel hero)
    {
        //保存ID
        this.heroId = hero.TypeId;
        this.heroName = hero.Name;
        //加载音效文件
        ResourcesManager.Instance.Load(Paths.RES_SOUND_UI + "Select", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_SOUND_SELECT + hero.Name, typeof(AudioClip), this);
        //更新图片
        string assetName = Paths.RES_HEAD + HeroData.GetHeroData(hero.TypeId).Name;
        ResourcesManager.Instance.Load(assetName, typeof(Sprite), this);

    }

    /// <summary>
    /// 加载完成之后调用的接口
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="asset"></param>
    public void OnLoaded(string assetName, object asset)
    {
        if (assetName == Paths.RES_SOUND_SELECT + heroName)
        {
            this.ac = asset as AudioClip;
        }
        else if(Paths.RES_HEAD + heroName == assetName)
        {
            this.imgHead.sprite = asset as Sprite;
        }
        else if (assetName == Paths.RES_SOUND_UI + "Select")
            acSelect = asset as AudioClip;
    }

    /// <summary>
    /// 英雄是否可选择
    /// </summary>
    public bool Interactable
    {
        get
        {
            return btnHead.interactable;
        }
        set
        {
            btnHead.interactable = value;
        }
    }
    /// <summary>
    /// 选人
    /// </summary>
    public void OnClick()
    {
        //播放音乐
        SoundManager.Instance.PlayEffectMusic(acSelect);

        //播放音乐
        SoundManager.Instance.PlayEffectMusic(ac);
        //发起选人的请求
        PhotonManager.Instance.Request(OpCode.SelectCode, OpSelect.Select, heroId);

    }
}
