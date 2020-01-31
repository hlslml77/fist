using MobaCommon.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MobaCommon.Config;
using System;

public class UIPlayer : MonoBehaviour, IResourceListener {

    private Text txtName;
    private Image imgBg;
    private Text txtState;
    private Image imgHead;

    //非拖拽方式获取控件
    void Start()
    {
        txtName = transform.Find("txtName").GetComponent<Text>();
        imgBg = transform.Find("imgBG").GetComponent<Image>();
        txtState = transform.Find("Text").GetComponent<Text>();
        imgHead = transform.Find("imgHead").GetComponent<Image>();

    }

    public void UpdateView(SelectModel model)
    {
        txtName.text = model.playerName;

        //判断玩家是否进入了
        if(!model.isEnter)
        {
            //什么时候用到就什么时候加载
              ResourcesManager.Instance.Load(Paths.RES_HEAD + "no-Connect", typeof(Sprite), this);
                return;
               
        }
        else
        {
            //进入之后
            ResourcesManager.Instance.Load(Paths.RES_HEAD + "no-Select", typeof(Sprite), this);

        }
        //进入之后
        if (model.heroId != -1)
        {
            string assetName = Paths.RES_HEAD + HeroData.GetHeroData(model.heroId).Name;
            ResourcesManager.Instance.Load(assetName, typeof(Sprite), this);

        }

        else
        {
            //有重复加载会返回字典里的
            ResourcesManager.Instance.Load(Paths.RES_HEAD + "no-Select", typeof(Sprite), this);

        }

        //判断是否准备
        if (model.isReady)
        {
            imgBg.color = Color.green;
            txtState.text = "已经选择";
        }
        else
        {
            imgBg.color = Color.white;
            txtState.text = "正在选择..";
        }
    }

    public void OnLoaded(string assetName, object asset)
    {
        Sprite s = asset as Sprite;
        imgHead.sprite = s;
    }
}
