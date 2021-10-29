//-------------------------------------------
//作者：马超
//时间：2020-09-23 14:38
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading;

public class Panel_Integral : PanelBase
{
    Text txt_Integra;
    Text txt_total;
    Button Btn_receive;
    Button Btn_double;
    Button Btn_Close;

    ParticleSystem caidai;

    int closeNum = 0;

    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type = PanelName.Panel_Integral;
        _openDuration = 0.5f;
        _alpha = 0.7f;
        _showStyle = PanelMgr.PanelShowStyle.CenterScaleBigNomal;//修改打开风格
        _maskStyle = PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache = false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_Integral");
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
        FindObj();
        AddEvent();
    }

    /// <summary>查找物体</summary>
    private void FindObj()
    {
        txt_Integra = skinTrs.SeachTrs<Text>("txt_Integra");
        txt_total = skinTrs.SeachTrs<Text>("txt_total");
        Btn_receive = skinTrs.SeachTrs<Button>("Btn_receive");
        Btn_double = skinTrs.SeachTrs<Button>("Btn_double");
        Btn_Close = skinTrs.SeachTrs<Button>("Btn_Close");
        caidai = skinTrs.SeachTrs<ParticleSystem>("caidai");
    }

    int currentScore = 0;
    public override void OnShowing()
    {
        base.OnShowing();

        if (panelArgs.Length != 0)
        {
            timer = 0;
            currentScore = (int)panelArgs[0];
            txt_Integra.text = panelArgs[0].ToString();
            Btn_receive.gameObject.SetActive(false);
            Btn_double.gameObject.SetActive(true);
            Btn_Close.gameObject.SetActive(false);
            txt_total.text = "当前积分:" + DataManager.getCurrentScore();
        }

#if !UNITY_EDITOR
    AdManager.Ins.showRewardAd(null);
#endif
    }

    float timer = 0;
    private void Update()
    {
        if (!Btn_Close.gameObject.activeSelf)
        {
            if (timer < 2)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                Btn_Close.gameObject.SetActive(true);
            }
        }
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
                EventMgrHelper.Ins.PushEvent(EventDef.FlyNum, intdata0: 2, floatdata0: (float)currentScore);
                //积分票一倍领取
                DotManager.Instance.sendEvent("ad_rv_show",DottingType.Tga,
                    new Dictionary<string, object> { { "ad_rv_show", "Integral_red" } });
                closeNum++;
                Close();
                break;
            case "Btn_receive":
                EventMgrHelper.Ins.PushEvent(EventDef.FlyNum, intdata0: 2, floatdata0: (float)currentScore);
                Close();
                break;
            case "Btn_double":

#if !UNITY_EDITOR
    AdManager.Ins.showRewardAd((str) =>
                {
                    currentScore *= 2;
                    txt_Integra.text = currentScore.ToString();
                     caidai.Play();

                    Btn_receive.gameObject.SetActive(true);
                    Btn_double.gameObject.SetActive(false);
                    Btn_Close.gameObject.SetActive(false);
                    txt_total.text = "当前积分:" + DataManager.getCurrentScore();
                }, "Integral*2_red");
#else
                caidai.Play();
                currentScore *= 2;
                txt_Integra.text = currentScore.ToString();
                Btn_receive.gameObject.SetActive(true);
                Btn_double.gameObject.SetActive(false);
                Btn_Close.gameObject.SetActive(false);
                txt_total.text = "当前积分:" + DataManager.getCurrentScore();
#endif

                break;
        }
    }
    #endregion
}
