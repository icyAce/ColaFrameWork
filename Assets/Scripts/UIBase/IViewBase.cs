﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI的接口
/// </summary>
public interface IViewBase
{
    
}

/// <summary>
/// UI基类
/// </summary>
public abstract class UIBase : IViewBase
{
    /// <summary>
    /// 当前的UI界面GameObject
    /// </summary>
    public GameObject Panel { get; protected set; }

    /// <summary>
    /// 当前的UI界面GameObject对应的唯一资源ID
    /// </summary>
    public int ResId { get; protected set; }

    /// <summary>
    /// UI界面的名字
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// UI界面的深度
    /// </summary>
    public int Depth { get; set; }

    /// <summary>
    /// UI界面的层级
    /// </summary>
    public int Layer { get; set; }
}