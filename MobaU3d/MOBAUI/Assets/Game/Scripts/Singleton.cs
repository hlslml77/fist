﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }
    protected virtual void Awake()
    {
        instance = this as T;
    }
    protected virtual void OnDestory()
    {
        instance = null;
    }
	
}
