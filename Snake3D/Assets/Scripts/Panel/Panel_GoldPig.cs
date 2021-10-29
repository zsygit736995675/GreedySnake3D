//-------------------------------------------
//作者：马超
//时间：2020-11-11 09:48
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel_GoldPig  : PanelBase
{

    int count = 12; //金猪点击次数

    Transform Btn_pig;

    Transform title, effect,trans_gx;

    Image img_sp;

    ParticleSystem jinzhuNormal, jinzhuAll ;

    RewardInfo rewInfo;

    string resPathName;

    Button Btn_Close;

    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=PanelName.Panel_GoldPig;
        _openDuration=0.5f;
        _alpha=0.85f;
        _showStyle=PanelMgr.PanelShowStyle.CenterScaleBigNomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache=false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_GoldPig");
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
        count = 12;
        Btn_pig = skinTrs.SeachTrs<Transform>("Btn_pig");
        title = skinTrs.SeachTrs<Transform>("title");
        effect = skinTrs.SeachTrs<Transform>("effect");
        trans_gx = skinTrs.SeachTrs<Transform>("trans_gx");
        img_sp = skinTrs.SeachTrs<Image>("img_sp");
        Btn_Close = skinTrs.SeachTrs<Button>("Btn_Close");

        jinzhuNormal = skinTrs.SeachTrs<ParticleSystem>("jinzhuNormal");
        jinzhuAll = skinTrs.SeachTrs<ParticleSystem>("jinzhuAll");
        title.gameObject.SetActive(true);
        trans_gx.gameObject.SetActive(false);
    }

    /// <summary>添加事件</summary>
    private void AddEvent()
    {
    }


    private void Update()
    {
        effect.Rotate(Vector3.back, Time.deltaTime * 50);
    }

    void ShowClose() 
    {
        Btn_Close.gameObject.SetActive(true);
    }

    /// <summary>按钮点击事件</summary>
    protected override void OnClick(Transform _target)
    {
        switch (_target.name)
        {
            case "Btn_pig":
                count--;
               
                Btn_pig.DOPunchPosition(new Vector3(35,-35, 0),0.1f, 10, 0.1f);

                if (count <= 0)
                {
                    Btn_pig.gameObject.SetActive(false);
                    trans_gx.gameObject.SetActive(true);
                    jinzhuAll.gameObject.SetActive(true);
                    jinzhuAll.Play();
                    List<int> face = DataManager.addUserFaceValue();
                    int mianE = face[0];
                    int count = face[1];
                    resPathName = DataManager.getChipResourcesName(mianE);
                    resPathName = resPathName + "bg_" + count;
                    title.gameObject.SetActive(false);
                    //img_sp.sprite = Resources.Load<Sprite>(resPathName);
                    // img_sp.GetComponent<RectTransform>().sizeDelta = new Vector2(img_sp.sprite.textureRect.width, img_sp.sprite.textureRect.height);
                    rewInfo = RewardManager.getNextReward(DataManager.getCurrentMoney());
                    trans_gx.transform.localScale = Vector3.zero;
                    trans_gx.transform.DOScale(Vector3.one,1);
                    Btn_Close.gameObject.SetActive(false);
                    Invoke(nameof(ShowClose),2);
                }
                else 
                {
                    jinzhuNormal.gameObject.SetActive(true);
                    jinzhuNormal.Play();
                }
                break;
            case "Btn_Close":

                DataManager.Ins.jzCloseNum++;

                //当关闭到一定次数后播放插屏
                if (DataManager.Ins.jzCloseNum >= HttpDataManager.GetConfigInfo().axis[4])
                {
                    DataManager.Ins.jzCloseNum = 0;
#if !UNITY_EDITOR
    AdManager.Ins.showInterstitialAd(null, "Piggy_bank");
#endif
                }

                Close();
                break;
            case "Btn_lq":

#if !UNITY_EDITOR
                     AdManager.Ins.showRewardAd((str) =>
                    {
                         EventMgrHelper.Ins.PushEvent(EventDef.FlyNum, intdata0: 3, floatdata0: (float)rewInfo.reward, strData0: resPathName);
                          Close();
                    }, "Piggy_bank");
#else
                EventMgrHelper.Ins.PushEvent(EventDef.FlyNum, intdata0: 3, floatdata0: (float)rewInfo.reward, strData0: resPathName);
                Close();
#endif
                break;
        }
    }
    #endregion
}
