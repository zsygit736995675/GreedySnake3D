//-------------------------------------------
//作者：马超
//时间：2020-11-11 13:50
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel_Turnplate  : PanelBase
{

    Text explain; //按钮说明

    int turnCount = 0; //转盘次数

    int state = 0; //转盘状态

    Transform zhuanpan; //转盘

    Transform shade;//遮罩

    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=PanelName.Panel_Turnplate;
        _openDuration=0.5f;
        _alpha=0.85f;
        _showStyle=PanelMgr.PanelShowStyle.CenterScaleBigNomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache=false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_Turnplate");
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
        explain = skinTrs.SeachTrs<Text>("explain");

        zhuanpan = skinTrs.SeachTrs<Transform>("zhuanpan");

        shade = skinTrs.SeachTrs<Transform>("shade");

        turnCount = 0;
    }

    /// <summary>添加事件</summary>
    private void AddEvent()
    {

    }

    public override void OnShowing()
    {
        base.OnShowing();
        state = 0;
        turnCount = 0;
        updateUI();
        Invoke("getClose",3);
    }

    /// <summary>
    /// 出现按钮
    /// </summary>
    public void getClose()
    {
        skinTrs.SeachTrs<Transform>("Btn_Close").gameObject.SetActive(true);
    }

    //更新UI
    public void updateUI()
    {
        switch (state)
        {
            case 0:
                explain.text = "看视频免费转1次";
                break;
            case 1:
                explain.text = "转(" + turnCount + "/1)";
                if (turnCount == 0)
                {
                    explain.text = "看视频免费转2次";
                }
                break;
            case 2:
                explain.text = "转(" + turnCount + "/2)";
                if (turnCount == 0)
                {
                    explain.text = "继续游戏";
                }
                break;
        }
    }

    //转盘旋转与奖励
    private void turnplateRotate()
    {
        Debug.Log("转盘旋转与奖励");
        //打开遮罩
        shade.gameObject.SetActive(true);
        List<int> randList = new List<int>() {-1350,-1920,-920,-2180,-1748 };
        int range = UnityEngine.Random.Range(0,5);
        zhuanpan.DORotate(new Vector3(0, 0, randList[range]), UnityEngine.Random.Range(4,8)).onComplete = () =>
        {
            //关闭遮罩
            shade.gameObject.SetActive(false);
            switch (range)
            {
                case 0:
                    PanelMgr.GetInstance.ShowPanel(PanelName.Panel_GetCashChip,false);
                    break;
                case 1:
                    PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Gold, RedType.common,false);
                    break;
                case 2:
                    Debug.Log("祝君好运");
                    break;
                case 3:
                    PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Gold, RedType.super,false);
                    break;
                case 4:
                    PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Gold, RedType.super,false);
                    break;
            }
        };
    }

    /// <summary>按钮点击事件</summary>
    protected override void OnClick(Transform _target)
    {
        switch (_target.name)
        {
            case "Btn_Close":
                DataManager.Ins.zhuanpanCloseNum++;
                //当关闭到一定次数后播放插屏
                if (DataManager.Ins.zhuanpanCloseNum >= HttpDataManager.GetConfigInfo().axis[2])
                {
                    DataManager.Ins.zhuanpanCloseNum = 0;
#if !UNITY_EDITOR
    AdManager.Ins.showInterstitialAd(null, "Turn_wheel");
#endif
                }
                Close();
                break;
            case "Btn_Notarize":
                if(explain.text == "继续游戏")
                {
                    Close();
                }
                if (turnCount > 0)
                {
                    turnCount--;
                    //转转盘方法
                    turnplateRotate();
                    updateUI();
                    break;
                }
#if !UNITY_EDITOR
                     AdManager.Ins.showRewardAd((str) =>
                    {
                        if (state == 0)
                            turnCount += 1;
                        else if (state == 1)
                            turnCount += 2;
                        state++;
                        updateUI();
                    },"Turn_wheel");  
#else
                if (state == 0)
                    turnCount += 1;
                else if (state == 1)
                    turnCount += 2;
                state++;
                updateUI();
#endif
                break;
        }
    }
    #endregion
}

