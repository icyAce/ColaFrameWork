﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Setting_UIView UI界面
/// </summary>
public class Setting_UIView : UIBase
{

    #region 变量

    #endregion

    #region 构造器

    public Setting_UIView(int resId, UILevel uiLevel) : base(resId, uiLevel)
    {
        
    }

    #endregion
    /// <summary>
    /// 打开一个UI界面
    /// </summary>
    public override void Open()
    {
        base.Open();
    }

    /// <summary>
    /// 关闭一个UI界面
    /// </summary>
    public override void Close()
    {
        base.Close();
    }

    /// <summary>
    /// UI界面显隐的时候会调用该方法
    /// </summary>
    /// <param name="isShow"></param>
    public override void OnShow(bool isShow)
    {
        base.OnShow(isShow);
    }

    /// <summary>
    /// 创建UI界面
    /// </summary>
    public override void Create()
    {
        base.Create();
    }

    /// <summary>
    /// UI结束后中会调用该方法
    /// </summary>
    public override void OnCreate()
    {
        base.OnCreate();
    }

    /// <summary>
    /// 销毁一个UI界面
    /// </summary>
    public override void Destroy()
    {
        base.Destroy();
    }

    /// <summary>
    /// 销毁UI界面后调用该方法
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
