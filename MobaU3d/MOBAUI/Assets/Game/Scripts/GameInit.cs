using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏初始化
/// </summary>
public class GameInit : MonoBehaviour {

	// Use this for initialization
	void Start() {
        //加载登录
        UIManager.Instance.ShowUIPanel(UIDefinit.UIAccount);

     
    }
	

}
