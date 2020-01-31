﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //相机可以动的边界
    [SerializeField]
    private float X_MIN;
    [SerializeField]
    private float X_MAX;
    [SerializeField]
    private float Z_MIN;
    [SerializeField]
    private float Z_MAX;

    /// <summary>
    /// 相机移动速度
    /// </summary>
    [SerializeField]
    private float speed = 13f;

    /// <summary>
    /// 敏感区域,用百分比
    /// </summary>
    private float area = 0.1f;

    /// <summary>
    /// 是否是焦点
    /// </summary>
    private static bool IsFocus = true;

    //防止失去焦点还能移动镜头
    void OnApplicationFocus(bool focus)
    {
        //指定是否是焦点
        IsFocus = focus;
    }

    void Awake()
    {
        //鼠标锁定到屏幕中心
        Cursor.lockState = CursorLockMode.Confined;
    }

    void LateUpdate()
    {
        if (!IsFocus)
            return;
        //目标点
        Vector3 target = Vector3.zero;
        //限制鼠标的坐标
        Vector3 mousePos = Input.mousePosition;
        float x = Mathf.Clamp(mousePos.x, 0, Screen.width);
        float y = Mathf.Clamp(mousePos.y, 0, Screen.height);
        Vector2 pos = new Vector2(x, y);
        //检测上边
        if (y > Screen.height * (1 - area))
        {
            //到达了上边敏感的地方
            target.z = 2;

        }
        else if (y < Screen.height * area)
        {
            target.z = -2;
        }
        //检测左右
        if (x > Screen.width * (1 - area))
        {
            target.x = 2;
        }
        else if (x < Screen.width * area)
        {
            target.x = -2;
        }

        //开始移动
        //如果xy不相等，且都不为0，那么我们就要将速度同步一下
        if (target.x != 0 && target.z != 0)
        {
            //使向上移动速度和向右移动速度相同
            target = target.normalized * Mathf.Max(Mathf.Abs(target.x), Mathf.Abs(target.z));
        }

        //开始移动,因为是在update中的所以要乘以deltatime
        transform.position += target * Time.deltaTime * speed;

        //限定相机的范围
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, X_MIN, X_MAX), transform.position.y, Mathf.Clamp(transform.position.z, Z_MIN, Z_MAX));

    }

    /// <summary>
    /// 焦点到自己英雄
    /// </summary>
    public void FocusOn()
    {

        if (GameData.MyControl == null)
            return;

        Vector3 hero = GameData.MyControl.transform.position;
        transform.position = new Vector3(hero.x, transform.position.y, hero.z - 5);
    }
}
