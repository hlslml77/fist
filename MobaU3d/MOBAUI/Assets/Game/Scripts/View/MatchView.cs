using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MatchView : MonoBehaviour
{
    /// <summary>
    /// 匹配时间
    /// </summary>
    [SerializeField]
    private Text txtTime;

    /// <summary>
    /// 是否开始计时
    /// </summary>
    private bool start = false;

    private float minute = 0;

    private float second = 0;

    public void StartMatch()
    {
        gameObject.SetActive(true);
        txtTime.text = "00:01";
        start = true;
        minute = 0;
        second = 0;
    }

    public void StopMatch()
    {
        gameObject.SetActive(false);
        start = false;
    }

    void Update()
    {
        if(start)
        {
            second += Time.deltaTime;
            if(second >= 60)
            {
                minute++;
                second = 0;
            }
            txtTime.text = minute.ToString().PadLeft(2, '0') + ":" + second.ToString("00");
            //00:00
        }
    }
}

