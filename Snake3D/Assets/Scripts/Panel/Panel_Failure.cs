//-------------------------------------------
//作者：马超
//时间：2020-11-23 13:51
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel_Failure  : PanelBase
{
    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=PanelName.Panel_Failure;
        _openDuration=0.5f;
        _alpha=0.5f;
        _showStyle=PanelMgr.PanelShowStyle.Nomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache=false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_Failure");
    }
    public override void OnInit(params object[] panelArgs)
    {
        base.OnInit(panelArgs);
        InitData();
    }
    #endregion

    #region 数据定义

    #endregion

    #region 逻辑
    /// <summary>初始化</summary>
    private void InitData()
    {
        if (panelArgs.Length!=0)
        {
        }
        FindObj();
        AddEvent();
    }

    /// <summary>查找物体</summary>
    private void FindObj()
    {
    }

    /// <summary>添加事件</summary>
    private void AddEvent()
    {
    }

    /// <summary>按钮点击事件</summary>
    protected override void OnClick(Transform _target)
    {
        switch (_target.name)
        {
            case "Btn_Close":
                Snake_Game.Ins.BackMain();
                Close();
                break;
            case "Btn_jx":
#if !UNITY_EDITOR
                AdManager.Ins.showRewardAd((str) =>
                {
                   ThreadManager.Ins.runOnMainThread(Resurrection);
                }, "resurrection");
#else
                Resurrection();
#endif
                break;
            case "Btn_restart":
                Close();

                Snake_Game.Ins.ReStart();
                break;
        }
    }

    void Resurrection() 
    {
        Close();
        Snake_Game.Ins.Resurrection();
    }

#endregion
}
