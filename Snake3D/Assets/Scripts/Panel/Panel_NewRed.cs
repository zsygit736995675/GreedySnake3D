//-------------------------------------------
//作者：马超
//时间：2020-11-23 19:11
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel_NewRed  : PanelBase
{
    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=PanelName.Panel_NewRed;
        _openDuration=0.5f;
        _alpha=0.7f;
        _showStyle=PanelMgr.PanelShowStyle.CenterScaleBigNomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache=false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_NewRed");
    }
    public override void OnInit(params object[] panelArgs)
    {
        base.OnInit(panelArgs);
        InitData();
    }
    #endregion

    #region 数据定义
    Transform trs_root;
    Text txt_money;

    float Num;
    Vector3 targetPos;
    #endregion

    #region 逻辑
    /// <summary>初始化</summary>
    private void InitData()
    {
        if (panelArgs.Length!=0)
        {
            Num = (float)panelArgs[0];
            targetPos = (Vector3)panelArgs[1];
        }
        FindObj();
    }

    private void FindObj()
    {
        trs_root = skinTrs.SeachTrs<Transform>("trs_root");
        txt_money = skinTrs.SeachTrs<Text>("txt_money");
    }

    public override void OnShowing()
    {
        base.OnShowing();

        Fly();
    }

    public void Fly()
    {
        txt_money.text = "+" + Num + "元";
        trs_root.localPosition = Vector3.zero;
        trs_root.localScale = Vector3.one;
        trs_root.gameObject. SetActive(false);
        float time = 0;

        DataManager.addMoney(Num);
        DOTween.To(() => time, p => time = p, 1, 0.5f).onComplete = () =>
        {
            trs_root .gameObject.SetActive(true);
            DOTween.To(() => time, p => time = p, 2, 1.25f).onComplete = () =>
            {
                DOTween.To(() => trs_root.localScale, p => trs_root.localScale = p, Vector3.zero, 1f);
                DOTween.To(() => trs_root.position, p => trs_root.position = p, targetPos, 0.75f).onComplete = () =>
                {
                    AudioController.Ins.PlayEffect("add_coin");
                    EventMgrHelper.Ins.PushEvent(EventDef.FlyEnd);
                    Close();
                };
            };
        };
    }

    /// <summary>按钮点击事件</summary>
    protected override void OnClick(Transform _target)
    {
        switch (_target.name)
        {
            case "Btn_Close":
                Close();
                break; 
            case "Btn_Open":
           
                break;
        }
    }
    #endregion
}
