//-------------------------------------------
//作者：马超
//时间：2020-10-12 22:02
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel_BangWeChat  : PanelBase
{
    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=PanelName.Panel_BangWeChat;
        _openDuration=0.5f;
        _alpha=0f;
        _showStyle=PanelMgr.PanelShowStyle.Nomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache=false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_BangWeChat");
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
                Close();
                break;
            case "Btn_Sure":
                {
                    //如果未绑定微信
                    if (HttpDataManager.getUser().is_bind_wechat == 0)
                    {
#if UNITY_EDITOR
                        Debug.Log("去绑定微信");
#elif UNITY_IOS
                    WeChatSDK.LoginWechat();
#elif UNITY_ANDROID
                    AndroidSend.WxLogin();
#endif
                    }
                    Close();
                    break;
                }
        }
    }
    #endregion
}
