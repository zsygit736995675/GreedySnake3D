//-------------------------------------------
//作者：马超
//时间：2020-09-14 10:36
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine;

public enum RedType
{
    /// <summary>
    /// 新人红包
    /// </summary>
    Newcomer,
    /// <summary>
    /// 每日红包
    /// </summary>
    daily,
    /// <summary>
    /// 在线红包
    /// </summary>
    online,
    /// <summary>
    /// 普通红包
    /// </summary>
    common,
    /// <summary>
    /// 奖金池红包
    /// </summary>
    bonuspool,
    /// <summary>
    /// 悬浮红包
    /// </summary>
    OpenCommon,
    /// <summary>
    /// 超级红包
    /// </summary>
    super,
    /// <summary>
    /// 红包雨
    /// </summary>
    hby,
        
}



public class Panel_Gold : PanelBase
{

    RedType currentType = RedType.common;

    bool isOpen = false;

    Image qie1, qie2, qie3, unbg, openbg, fhbg, fhopenbg, img_rew, img_new;

    Button Btn_open, Btn_save, Btn_income, Btn_carry, Btn_Close, Btn_fh,Btn_fh_open;

    Text txt_gx, txt_xr, txt_daily, txt_gxget, txt_getnum, txt_need, txt_save, txt_rewNum,
        txt_Remarks, txt_over, txt_overNum, txt_carry, txt_timerStr, txt_open, txt_fh_v,
        txt_fhopen_v,txt_fhopen_tv, txt_superje, txt_super_ye;

    Transform bg, con, txt_fhopen, txt_fh, caidai1, super, super_open, super_close, jibi;

    RewardInfo rewInfo;

    Action callback;

    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type = PanelName.Panel_Gold;
        _openDuration = 0.5f;
        _alpha = 0.7f;
        _showStyle = PanelMgr.PanelShowStyle.CenterScaleBigNomal;//修改打开风格
        _maskStyle = PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache = false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_Gold");
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
        qie1 = skinTrs.SeachTrs<Image>("qie1");
        qie2 = skinTrs.SeachTrs<Image>("qie2");
        qie3 = skinTrs.SeachTrs<Image>("qie3");
        unbg = skinTrs.SeachTrs<Image>("unbg");
        openbg = skinTrs.SeachTrs<Image>("openbg");

        bg = skinTrs.SeachTrs<Transform>("bg");
        Btn_open = skinTrs.SeachTrs<Button>("Btn_open");
        Btn_save = skinTrs.SeachTrs<Button>("Btn_save");
        Btn_income = skinTrs.SeachTrs<Button>("Btn_income");
        Btn_carry = skinTrs.SeachTrs<Button>("Btn_carry");
        Btn_Close = skinTrs.SeachTrs<Button>("Btn_Close");

        txt_gx = skinTrs.SeachTrs<Text>("txt_gx");
        txt_xr = skinTrs.SeachTrs<Text>("txt_xr");
        txt_daily = skinTrs.SeachTrs<Text>("txt_daily");
        txt_gxget = skinTrs.SeachTrs<Text>("txt_gxget");
        txt_getnum = skinTrs.SeachTrs<Text>("txt_getnum");
        txt_need = skinTrs.SeachTrs<Text>("txt_need");
        txt_save = skinTrs.SeachTrs<Text>("txt_save");
        txt_rewNum = skinTrs.SeachTrs<Text>("txt_rewNum");
        txt_Remarks = skinTrs.SeachTrs<Text>("txt_Remarks");
        txt_over = skinTrs.SeachTrs<Text>("txt_over");
        txt_overNum = skinTrs.SeachTrs<Text>("txt_overNum");
        txt_carry = skinTrs.SeachTrs<Text>("txt_carry");
        txt_timerStr = skinTrs.SeachTrs<Text>("txt_timerStr");
        txt_open = skinTrs.SeachTrs<Text>("txt_open");

        txt_fh = skinTrs.SeachTrs<Transform>("txt_fh");
        txt_fh_v = skinTrs.SeachTrs<Text>("txt_fh_v");
        Btn_fh = skinTrs.SeachTrs<Button>("Btn_fh");

        txt_fhopen = skinTrs.SeachTrs<Transform>("txt_fhopen");
        txt_fhopen_v = skinTrs.SeachTrs<Text>("txt_fhopen_v");
        txt_fhopen_tv = skinTrs.SeachTrs<Text>("txt_fhopen_tv");
        Btn_fh_open = skinTrs.SeachTrs<Button>("Btn_fh_open");
   
        fhbg = skinTrs.SeachTrs<Image>("fhbg");
        fhopenbg = skinTrs.SeachTrs<Image>("fhopenbg");
        caidai1 = skinTrs.SeachTrs<Transform>("caidai1");
        con = skinTrs.SeachTrs<Transform>("con");

        super= skinTrs.SeachTrs<Transform>("super");
        super_close = skinTrs.SeachTrs<Transform>("super_close");
        super_open = skinTrs.SeachTrs<Transform>("super_open");
        txt_superje = skinTrs.SeachTrs<Text>("txt_superje");
        txt_super_ye = skinTrs.SeachTrs<Text>("txt_super_ye");

        jibi = skinTrs.SeachTrs<Transform>("jibi");

        img_rew = skinTrs.SeachTrs<Image>("img_rew");
        img_new = skinTrs.SeachTrs<Image>("img_new");
    }

    /// <summary>添加事件</summary>
    private void AddEvent()
    {

    }

    int currentNum = 0;

    /// <summary>
    /// 转盘来的不显示广告
    /// </summary>
    bool isShowRv = true;

    public override void OnShowing()
    {
        base.OnShowing();

        if (panelArgs.Length != 0)
        {
            isOpen = false;
            currentType = (RedType)panelArgs[0];

            if (panelArgs.Length > 1) 
            {
                callback = (Action)panelArgs[1];
            }

            //获取下一次红包数值
            rewInfo = RewardManager.getNextReward(DataManager.getCurrentMoney());

            DotManager.Instance.sendEvent("red_cash_dialog_click_get", DottingType.Tga, new Dictionary<string, object> { { "red_packet_ID", rewInfo.id }, { "red_packet_value", rewInfo.reward } } );

            AudioController.Ins.PlayEffect("red");
            UPdateUI();
        }
    }

    void ShowClose() 
    {
        if (currentType != RedType.Newcomer && currentType != RedType.online && currentType != RedType.daily) 
        {
            Btn_Close.gameObject.SetActive(true);
        }
    }


    void UPdateUI()
    {
        Btn_Close.gameObject.SetActive(false);
        Invoke(nameof(ShowClose), 2);

        foreach (Transform item in bg)
        {
            item.gameObject.SetActive(false);
        }

        foreach (Transform item in con)
        {
            item.gameObject.SetActive(false);
        }

        switch (currentType)
        {
            case RedType.Newcomer:
                if (isOpen)
                {
                   setActive(Btn_carry.gameObject, txt_rewNum.gameObject, txt_over.gameObject, txt_need.gameObject, openbg.gameObject);

                    txt_rewNum.text = RewardManager.FormatRew((float)rewInfo.reward);
                    float offset = ConfigManager.current_money - ((float)rewInfo.reward + DataManager.getCurrentMoney());

                    if (offset <= 0)
                    {
                        offset = 0;
                        txt_carry.text = "继续赚红包";
                        txt_need.text = "获得七彩锦鲤即可提现";
                    }
                    else
                    {
                        txt_need.text = "再赚取" + RewardManager.FormatRew(offset) + "元可提现";
                        txt_carry.text = "继续赚红包";
                    }

                    txt_overNum.text = RewardManager.FormatRew((float)rewInfo.reward + DataManager.getCurrentMoney()) + "元";
                }
                else
                {
                    DotManager.Instance.sendEvent("gift_box_first_show", DottingType.Tga);
                    setActive(Btn_open.gameObject, unbg.gameObject,img_new.gameObject);
                }

                break;
            case RedType.daily:
                if (isOpen)
                {
                    setActive(qie3.gameObject, openbg.gameObject, txt_rewNum.gameObject, Btn_income.gameObject, txt_Remarks.gameObject);
                    txt_rewNum.text = RewardManager.FormatRew((float)rewInfo.reward);
                    //每日登陆红包埋点
                    DotManager.Instance.sendEvent("everyday_start_red", DottingType.Tga);
                }
                else
                {
                    setActive(Btn_open.gameObject, txt_open.gameObject, txt_daily.gameObject, qie2.gameObject, unbg.gameObject);
                }
                break;
            case RedType.online:
                setActive(txt_rewNum.gameObject, txt_timerStr.gameObject, Btn_income.gameObject, txt_Remarks.gameObject, qie3.gameObject, openbg.gameObject);
                txt_rewNum.text = RewardManager.FormatRew((float)rewInfo.reward);
                break;
            case RedType.common:
                if (isOpen)
                {
                    setActive(Btn_carry.gameObject, txt_rewNum.gameObject, txt_over.gameObject, txt_need.gameObject, openbg.gameObject);
                    txt_rewNum.text = RewardManager.FormatRew((float)rewInfo.reward);
                    float offset = ConfigManager.current_money - ((float)rewInfo.reward + DataManager.getCurrentMoney());

                    if (offset <= 0)
                    {
                        offset = 0;
                        txt_carry.text = "继续赚红包";
                        txt_need.text = "获得七彩锦鲤即可提现";
                    }
                    else
                    {
                        txt_need.text = "再赚取" + RewardManager.FormatRew(offset) + "元可提现";
                        txt_carry.text = "继续赚红包";
                    }

                    txt_overNum.text = RewardManager.FormatRew((float)rewInfo.reward + DataManager.getCurrentMoney()) + "元";
                }
                else
                {
                    DotManager.Instance.sendEvent("gift_box_dialog_show", DottingType.Tga);
                    DotManager.Instance.sendEvent("red_cash_dialog_show", DottingType.Af);

                    setActive(Btn_open.gameObject, unbg.gameObject,img_rew.gameObject);
                }
                break;
            case RedType.super:
                if (isOpen)
                {
                    jibi.gameObject.SetActive(false);
                    super_close.gameObject.SetActive(false);
                    setActive(super.gameObject, super_open.gameObject);
                    txt_superje.text = RewardManager.FormatRew((float)rewInfo.reward) + "元";
                    txt_super_ye.text= "当前余额:"+ DataManager.getCurrentMoney()+"元";
                }
                else
                {
                    super_open.gameObject.SetActive(false);
                    setActive(super.gameObject, super_close.gameObject);
                }
                break;
            case RedType.OpenCommon:
            case RedType.hby:
                isOpen = true;
                if (isOpen)
                {
                    setActive(Btn_carry.gameObject, txt_rewNum.gameObject, txt_over.gameObject, txt_need.gameObject, openbg.gameObject);
                    txt_rewNum.text = RewardManager.FormatRew((float)rewInfo.reward);
                    float offset = ConfigManager.current_money - ((float)rewInfo.reward + DataManager.getCurrentMoney());

                    if (offset <= 0)
                    {
                        offset = 0;
                        txt_carry.text = "继续赚红包";
                        txt_need.text = "获得七彩锦鲤即可提现";
                    }
                    else
                    {
                        txt_need.text = "再赚取" + RewardManager.FormatRew(offset) + "元可提现";
                        txt_carry.text = "继续赚红包";
                    }

                    txt_overNum.text = RewardManager.FormatRew((float)rewInfo.reward + DataManager.getCurrentMoney()) + "元";
                }
                else
                {
                    //普通红包展示次数埋点
                    DotManager.Instance.sendEvent("red_cash_dialog_show", DottingType.Tga);
                    setActive(Btn_open.gameObject, unbg.gameObject, txt_gx.gameObject);
                }
                break;
            case RedType.bonuspool:
                if (isOpen)
                {
                    caidai1.gameObject.GetComponent<ParticleSystem>().Play();

                    setActive(txt_fhopen.gameObject, fhopenbg.gameObject);
                    txt_fhopen_v.text = RewardManager.FormatRew((float)rewInfo.reward);
                }
                else
                {
                    setActive(txt_fh.gameObject, unbg.gameObject, fhbg.gameObject);
                    string v = DataManager.getPoolNumber().Insert(3,".");
                    txt_fh_v.text = v;
                }
                break;
        }
    }

    void setActive(params GameObject[] trs)
    {
        foreach (GameObject item in trs)
        {
            item.SetActive(true);
        }
    }

    /// <summary>按钮点击事件</summary>
    protected override void OnClick(Transform _target)
    {
        switch (_target.name)
        {
            case "Btn_Close":

                Close();

                if (currentType == RedType.common) 
                {
                    DotManager.Instance.sendEvent("gift_box_dialog_giveup", DottingType.Tga);
                    DotManager.Instance.sendEvent("red_cash_dialog_click_giveup", DottingType.Af);
                    EventMgrHelper.Ins.PushEventEx(EventDef.Callback, object0: callback);
                    if (!PlatformManager.IsEditor)
                    {
                        //当关闭到一定次数后依然播放rv
                        DataManager.Ins.closeNum++;
                        if (DataManager.Ins.closeNum >= HttpDataManager.GetConfigInfo().hongbao_dialog_close_show_inter)
                        {
                            DataManager.Ins.closeNum = 0;
                            AdManager.Ins.showInterstitialAd(null, "Pass");
                        }
                    }
                }
                break;
            case "Btn_open":

                Action action = () => {
                    Close();
                    EventMgrHelper.Ins.PushEvent(EventDef.FlyNum, intdata0: 1, floatdata0: (float)rewInfo.reward);
                    callback?.Invoke();
                    if (currentType == RedType.Newcomer) 
                    {
                        DotManager.Instance.sendEvent("gift_box_first_click_open", DottingType.Tga);
                    }
                    if (currentType == RedType.common)
                    {
                        DotManager.Instance.sendEvent("red_cash_dialog_click_open", DottingType.Af);
                        DotManager.Instance.sendEvent("gift_box_dialog_click_open", DottingType.Tga);
                        DotManager.Instance.sendEvent("gift_box_get", DottingType.Tga);
                    }
                };

                if (currentType == RedType.Newcomer)
                {
                    EventMgrHelper.Ins.PushEventEx(EventDef.Callback,action);
                }

                if (currentType == RedType.common)
                {
                    if (PlatformManager.IsEditor)
                    {
                        EventMgrHelper.Ins.PushEventEx(EventDef.Callback, action);
                    }
                    else 
                    {
                        AdManager.Ins.showRewardAd((str) =>
                        {
                            EventMgrHelper.Ins.PushEventEx(EventDef.Callback, action);
                        }, "Pass");
                    }
                }
                break;

            case "Btn_income":

                if (currentType == RedType.online || currentType == RedType.daily)
                {
#if !UNITY_EDITOR
                    AdManager.Ins.showInterstitialAd((str) =>
                    {
                        EventMgrHelper.Ins.PushEvent(EventDef.FlyNum, 1, 0, "", "", true, true, true, (float)rewInfo.reward);
                        
                    }, currentType == RedType.online? "online_red" + currentNum : "Everyday_red");
#else
                    EventMgrHelper.Ins.PushEvent(EventDef.FlyNum, intdata0: 1, floatdata0: (float)rewInfo.reward);
#endif
                    Close();
                }

                if (currentType == RedType.common)
                {
                    EventMgrHelper.Ins.PushEvent(EventDef.FlyNum, intdata0: 1, floatdata0: (float)rewInfo.reward);
                    Close();
                }
                break;
            case "Btn_carry":

                //加红包次数
                DataManager.addMergaRedNum(1);
                if (currentType == RedType.common || currentType == RedType.OpenCommon)
                {
                    EventMgrHelper.Ins.PushEvent(EventDef.FlyNum, intdata0: 1, floatdata0: (float)rewInfo.reward);

                    if (currentType == RedType.common)
                    {
                        DotManager.Instance.sendEvent(DotConstant.RED_CASH_DIALOG_CLICK_OPEN);
                    }
                    else if (currentType == RedType.OpenCommon)
                    {
                        DotManager.Instance.sendEvent("red_cash_suspension_open", DottingType.Tga);
                    }
                }

                if (currentType == RedType.hby) 
                {
#if !UNITY_EDITOR
                    AdManager.Ins.showRewardAd((str) =>
                    {
                        EventMgrHelper.Ins.PushEvent(EventDef.FlyNum, intdata0: 1, floatdata0: (float)rewInfo.reward);
                    }, "rain_red");
#else
                    EventMgrHelper.Ins.PushEvent(EventDef.FlyNum, intdata0: 1, floatdata0: (float)rewInfo.reward);
#endif
                }

                if (currentType == RedType.Newcomer)
                {
                    DotManager.Instance.sendEvent(DotConstant.RED_CASH_FIRST_CLICK_OPEN, DottingType.Tga);
                    EventMgrHelper.Ins.PushEvent(EventDef.FlyNum, intdata0: 1, floatdata0: (float)rewInfo.reward);
                }

                Close();

                break;
            case "Btn_fh":
                if (currentType == RedType.bonuspool)
                {
#if !UNITY_EDITOR

                    AdManager.Ins.showRewardAd((str) =>
                    {
                        isOpen = true;
                        UPdateUI();
                    },"Dividends_red");
#else
                    isOpen = true;
                    UPdateUI();
#endif
                }
                break;
            case "Btn_fh_open":
                EventMgrHelper.Ins.PushEvent(EventDef.FlyNum, intdata0: 1, floatdata0: (float)rewInfo.reward);
                Close();
                break;
            case "Btn_chai":
#if !UNITY_EDITOR
                 if (isShowRv)
                 {
                    AdManager.Ins.showRewardAd((str) =>
                    {
                        isOpen = true;
                        UPdateUI();
                    }, "super_red");
                 }else
                 {
                        isOpen = true;
                        UPdateUI();
                 }
#else
                isOpen = true;
                UPdateUI();
#endif
                break;
            case "Btn_superlq":
                EventMgrHelper.Ins.PushEvent(EventDef.FlyNum, intdata0: 1, floatdata0: (float)rewInfo.reward);
                Close();
                break;
        }
    }


    #endregion
}
