//-------------------------------------------
//作者：马超
//时间：2020-09-25 21:47
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel_Setting  : PanelBase
{
    #region 界面加载

    Button Btn_bgy;
    Button Btn_yx;

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=PanelName.Panel_Setting;
        _openDuration=0.5f;
        _alpha=0.5f;
        _showStyle=PanelMgr.PanelShowStyle.Nomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache=false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_Setting");
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
        Btn_bgy = skinTrs.SeachTrs<Button>("Btn_bgy");
        Btn_yx = skinTrs.SeachTrs<Button>("Btn_yx");
    }

    /// <summary>添加事件</summary>
    private void AddEvent()
    {
    }

    public override void OnShowing()
    {
        base.OnShowing();

        Btn_bgy.transform.GetChild(1).gameObject.SetActive(AudioController.Ins.MusicSwitch);
        Btn_yx.transform.GetChild(1).gameObject.SetActive(AudioController.Ins.SoundSwitch);

        AdManager.Ins.showNativeAd(0,null,"setting");
    }

    protected override void Close()
    {
        base.Close();
        AdManager.Ins.removeNativeAd();
    }

    /// <summary>按钮点击事件</summary>
    protected override void OnClick(Transform _target)
    {
        switch (_target.name)
        {
            case "Btn_Close":
                Close();
                break;
            case "Btn_bgy":
                AudioController.Ins.MusicSwitch = !AudioController.Ins.MusicSwitch;
                Btn_bgy.transform.GetChild(1).gameObject.SetActive(AudioController.Ins.MusicSwitch);
                break;
            case "Btn_yx":
                AudioController.Ins.SoundSwitch = !AudioController.Ins.SoundSwitch;
                Btn_yx.transform.GetChild(1).gameObject.SetActive(AudioController.Ins.SoundSwitch);
                break;
            case "Btn_ys":
                Application.OpenURL(SystemConfig.Get(1).ysxy);
                break;
            case "Btn_xy":
                Application.OpenURL(SystemConfig.Get(1).fwxy);
                break;
            case "Btn_fk":
                string Title = Application.platform + "-" + Application.productName + "-" + Application.version + "-" +
                SystemInfo.deviceUniqueIdentifier + "-" + HttpDataManager.getUser().user_id;
                Uri uri = new Uri(string.Format("mailto:{0}?subject={1}", SystemConfig.Get(1).email, Title)); //第二个参数是邮件的标题 
                Application.OpenURL(uri.AbsoluteUri);
                break;
        }
    }
    #endregion
}
