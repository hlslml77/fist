using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.Scripts.Manager
{
    /// <summary>
    /// 封装输出类
    /// </summary>
    public class Log
    {
        public static Action<object> Debug = UnityEngine.Debug.Log;
        public static Action<object> Error = UnityEngine.Debug.LogError;
        public static Action<object> Warning = UnityEngine.Debug.LogWarning;

        //public static void Debug(string text) { }
        //public static void Error(string text) { }
        //public static void Warning(string text) { }

    }
}
