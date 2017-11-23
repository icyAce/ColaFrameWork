﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 资源管理器
/// </summary>
public class ResourceMgr
{
    private static ResourceMgr instance;
    private ResourceLoader resourceLoader;

    public static ResourceMgr GetInstance()
    {
        if (null == instance)
        {
            instance = new ResourceMgr();
        }
        return instance;
    }

    private ResourceMgr()
    {
        GameObject resourceMgrObj = new GameObject();
        GameObject.DontDestroyOnLoad(resourceMgrObj);

        resourceLoader = resourceMgrObj.AddComponent<ResourceLoader>();
    }


    public void LoadText(string path, string fileName, Action<string, string> callback)
    {
        resourceLoader.LoadAsync<TextAsset>(path,(obj,name)=>
        {
            TextAsset textAsset = obj as TextAsset;
            if(null!=callback)
            callback(fileName, textAsset.text);
        });
    }

    public void LoadText(string path, string fileName, Action<string, byte[]> callback)
    {
#if UNITY_ANDROID && !UNITY_EDITOR

#endif
        var bytes = File.ReadAllBytes(path);
        if (null!=callback)
            callback(fileName, bytes);
    }
}
