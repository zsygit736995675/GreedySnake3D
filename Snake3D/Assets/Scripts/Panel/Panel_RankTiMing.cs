//-------------------------------------------
//作者：马超
//时间：2020-10-09 21:09
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel_RankTiMing  : PanelBase
{

    //中央文字
    Text txt_declare;
    string txt_declare_str;

    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=PanelName.Panel_RankTiMing;
        _openDuration=0.5f;
        _alpha = 0.75f;
        _showStyle =PanelMgr.PanelShowStyle.Nomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache=false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_RankTiMing");
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
            txt_declare_str = panelArgs[0].ToString();
        }
        FindObj();
        AddEvent();
    }

    /// <summary>查找物体</summary>
    private void FindObj()
    {
        //中央文字
        txt_declare = skinTrs.SeachTrs<Text>("txt_declare");
    }

    public override void OnShowing()
    {
        base.OnShowing();
        txt_declare.text = txt_declare_str;
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
                Close();
                break;
            case "Btn_Confirm":
                Close();
                break;
        }
    }
    #endregion
}
