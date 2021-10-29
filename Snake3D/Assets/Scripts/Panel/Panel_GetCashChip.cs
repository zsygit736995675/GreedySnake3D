//-------------------------------------------
//作者：马超
//时间：2020-11-11 16:38
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel_GetCashChip : PanelBase
{
    int mianE;
    int count;

    Transform chip;

    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type = PanelName.Panel_GetCashChip;
        _openDuration = 0.5f;
        _alpha = 0.8f;
        _showStyle = PanelMgr.PanelShowStyle.Nomal;//修改打开风格
        _maskStyle = PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache = false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_GetCashChip");
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
        if (panelArgs.Length != 0)
        {
        }
        FindObj();
        AddEvent();
    }

    bool isShowRv = true;

    public override void OnShowing()
    {
        base.OnShowing();
        if (panelArgs.Length > 0)
        {
            isShowRv = (bool)panelArgs[0];
        }


        Invoke("getClose", 3);
    }

    /// <summary>查找物体</summary>
    private void FindObj()
    {
        chip = skinTrs.SeachTrs<Transform>("chip");
    }

    /// <summary>
    /// 出现按钮
    /// </summary>
    public void getClose() {
        skinTrs.SeachTrs<Transform>("Btn_Close").gameObject.SetActive(true);
    }

    /// <summary>添加事件</summary>
    private void AddEvent()
    {

    }

    float chaPingNewTimer = 3;
    /// <summary>按钮点击事件</summary>
    protected override void OnClick(Transform _target)
    {
        switch (_target.name)
        {
            case "Btn_Close":
                DataManager.Ins.cashChipNum++;
                //当关闭到一定次数后播放插屏
                if (DataManager.Ins.cashChipNum >= HttpDataManager.GetConfigInfo().axis[1])
                {
                    DataManager.Ins.cashChipNum = 0;

#if !UNITY_EDITOR
 if(isShowRv)
    AdManager.Ins.showInterstitialAd(null, "cash_fragment");
#endif
                }
                Close();
                break;
            case "Btn_get":
                //观看视频获得碎片
#if !UNITY_EDITOR
   AdManager.Ins.showRewardAd((str) =>
                {
                    //获得碎片
                    getAndPushChip();
                    Close();
                },"cash_fragment");
               
#else
                getAndPushChip();
                Close();
#endif
                break;
        }
    }

    public void getAndPushChip()
    {
        //获得碎片
        List<int> listInt = DataManager.addUserFaceValue();
        mianE = listInt[0];
        count = listInt[1];
        string resPathName = DataManager.getChipResourcesName(mianE);
        EventMgrHelper.Ins.PushEventEx(EventDef.Add_Chip, data0: mianE, data1: count);
    }

    #endregion
}
