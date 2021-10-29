//-------------------------------------------
//作者：马超
//时间：2020-10-28 17:26
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel_WuCai : PanelBase
{
    private Transform flsh;
    private Transform wucaiyu;
    private Transform yu_glow; //七彩鱼背后特效

    private List<Text> fishsNumber = new List<Text> { };

    private Text commit;

    private Text wucainumber;

    private Text qicaiCount;

    Text txt_over;

    private List<Transform> fishs = new List<Transform>();

    private int[] newFiveFishNumber; //当前档次对应的config number数组



    //去提现
    private Transform Btn_GoToTiXian;
    private Animation Btn_commit_Ani;

    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=PanelName.Panel_WuCai;
        _openDuration=0.5f;
        _alpha=0f;
        _showStyle=PanelMgr.PanelShowStyle.Nomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache=false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_WuCai");
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
        flsh = skinTrs.SeachTrs<Transform>("flshs");
        wucaiyu = skinTrs.SeachTrs<Transform>("wucaiyu");
        yu_glow = skinTrs.SeachTrs<Transform>("yu_glow");
        Btn_GoToTiXian = skinTrs.SeachTrs<Transform>("Btn_GoToTiXian");

        Btn_commit_Ani = skinTrs.SeachTrs<Animation>("Btn_commit");

        for (int i = 0; i < 7; i++)
        {
            fishsNumber.Add(skinTrs.SeachTrs<Text>("number" + (i + 1)));
            fishs.Add(skinTrs.SeachTrs<Transform>("flsh" + (i + 1)));
        }
        commit = skinTrs.SeachTrs<Text>("commit");
        wucainumber = skinTrs.SeachTrs<Text>("wucainumber");
        qicaiCount = skinTrs.SeachTrs<Text>("qicaiCount");
        txt_over = skinTrs.SeachTrs<Text>("txt_over");
        updateUI();
    }

    /// <summary>添加事件</summary>
    private void AddEvent()
    {
        RegisterEvent(EventDef.CallFlsh);
        RegisterEvent(EventDef.CallQiCaiFlsh);
    }

    private void updateUI()
    {
        string [] userFiveFlsh = DataManager.getUserFiveFlsh().Split(',');
        //刷新当前档次要加的鱼的数量
        switch (DataManager.getAmount())
        {
            case 1:
                newFiveFishNumber = HttpDataManager.GetConfigInfo().newFiveFishNumber1;
                break;
            case 2:
                newFiveFishNumber = HttpDataManager.GetConfigInfo().newFiveFishNumber2;
                break;
            case 3:
                newFiveFishNumber = HttpDataManager.GetConfigInfo().newFiveFishNumber3;
                break;
            case 4:
                newFiveFishNumber = HttpDataManager.GetConfigInfo().newFiveFishNumber4;
                break;
        }
        //刷新鱼的数量
        for (int i = 0; i < fishsNumber.Count; i++)
        {
            fishsNumber[i].text = userFiveFlsh[i];
            fishs[i].Find("bg2").gameObject.SetActive(int.Parse(userFiveFlsh[i]) > 0);
        }
        //判断当前是合成还是召唤
        commit.text = "合  成";
        for (int i = 0; i < userFiveFlsh.Length; i++)
        {
            if(int.Parse(userFiveFlsh[i]) <= 0)
            {
                commit.text = "召  唤";
                break;
            }
        }

        //txt_over.text = "今日剩余次数："+(HttpDataManager.GetConfigInfo().carpCallTimes - DataManager.currentDayCallNum);
        //刷新五彩锦鲤的数量
        wucainumber.text = DataManager.getWuCaiFlsh().ToString();
        //刷新五彩锦鲤是否开启背景
        wucaiyu.Find("bg2").gameObject.SetActive(DataManager.getWuCaiFlsh()>0);
        //刷新七彩锦鲤计数
        qicaiCount.text = "(" + DataManager.getWuCaiFlsh() + "/" + HttpDataManager.GetConfigInfo().withDrowFishNumber[DataManager.getAmount()-1] + ")";
        //刷新去提现按钮
        Btn_GoToTiXian.gameObject.SetActive(DataManager.getWuCaiFlsh()>=HttpDataManager.GetConfigInfo().withDrowFishNumber[DataManager.getAmount() - 1]);
        //刷新七彩鱼合成按钮动画
        if (commit.text == "合  成") { Btn_commit_Ani.Play("heartbeat_btn2");}
        else { Btn_commit_Ani.Stop(); }
        //刷新七彩鱼背后特效
        yu_glow.gameObject.SetActive(DataManager.getWuCaiFlsh() > 0);
    }

    /// <summary>按钮点击事件</summary>
    protected override void OnClick(Transform _target)
    {
        base.OnClick(_target);
        switch (_target.name)
        {
            case "Btn_commit":
                if(commit.text == "合  成")
                {
                    if (!callFlshIsShow)
                    {
                        DotManager.Instance.sendEvent("colorful_carp_merge");
                        DotManager.Instance.userSet("colorful_carp", 1);
                        //合成
                        DataManager.addWuCaiFlsh();
                        callFlsh(0, new Vector3(2, 2, 2), true);
                    }
                }
                else
                {

#if !UNITY_EDITOR
                        AdManager.Ins.showRewardAd((str) =>
                        {
                            EventMgrHelper.Ins.PushEvent(EventDef.CallFlsh);

                        }, "call_red");
#else
                    if (!callFlshIsShow)
                    {
                        if (DataManager.currentDayCallNum < HttpDataManager.GetConfigInfo().carpCallTimes)
                        {
                            DataManager.currentDayCallNum++;
                            int flshNumber = DataManager.addCommonFlsh(newFiveFishNumber);
                            callFlsh(flshNumber, new Vector3(1, 1, 1), false);
                        }
                        else
                        {
                            LogicMgr.GetInstance.GetLogic<LogicTips>().AddTips(string.Format("七彩锦鲤每日限定召唤{0}次，请明日再来进行幸运召唤吧~", HttpDataManager.GetConfigInfo().carpCallTimes));
                        }
                    }
#endif
                }
                break;
            case "Btn_Help":
                PanelMgr.GetInstance.ShowPanel(PanelName.Panel_WuCaiHelp);
                break;
            case "Btn_Close":
                Close();
                break;
            case "Btn_GoToTiXian":
                Close();
                break;
          }
    }

    public override void HandleEvent(EventDef ev, LogicEvent le)
    {
        base.HandleEvent(ev, le);
        if (ev == EventDef.CallFlsh) 
        {
            if (!callFlshIsShow)
            {
                DotManager.Instance.userSet("carp", 1);
                int flshNumber = DataManager.addCommonFlsh(newFiveFishNumber);
                callFlsh(flshNumber, new Vector3(1, 1, 1), false);
            }
        }
        if(ev == EventDef.CallQiCaiFlsh)
        {
            //七彩鱼流程走完
            GameObject flsh = le.Object0 as GameObject;
            flsh.transform.Find("gongxi").gameObject.SetActive(false);
            flsh.transform.Find("get").gameObject.SetActive(false);
            DOTween.To(() => flsh.transform.position, p => flsh.transform.position = p,
                skinTrs.SeachTrs<Transform>("wucaiyu").position, 1f).
                onComplete = () =>
                {
                    Destroy(flsh);
                    callFlshIsShow = false;
                    updateUI();
                };
        }
    }

    //页面关闭
    protected override void Close()
    {
        base.Close();
        UnRegisterEvent(EventDef.CallQiCaiFlsh);
        UnRegisterEvent(EventDef.CallFlsh);
        //发送刷新提现页面ui事件
        EventMgrHelper.Ins.PushEvent(EventDef.REF_WITHDROW);
    }

    private bool callFlshIsShow;
    /// <summary>
    /// 召唤鱼 
    /// </summary>
    private void callFlsh(int flshNumber , Vector3 toScale , bool isQiCaiFlsh)
    {
        
        callFlshIsShow = true;
        GameObject flsh = GameObject.Instantiate(Resources.Load<GameObject>("Game/new_flsh"), skinTrs);
        flsh.transform.Find("flsh_img").GetComponent<Image>().sprite = Resources.Load(isQiCaiFlsh ? "Game/flsh_qicai" : "Game/flsh" + flshNumber, typeof(Sprite)) as Sprite;
        flsh.transform.Find("get").gameObject.SetActive(isQiCaiFlsh);
        //变大
        DOTween.To(() => flsh.transform.localScale, p => flsh.transform.localScale = p,
            new Vector3(2, 2, 2), 0.5f).
            onComplete = () =>
            {
                flsh.transform.DOScale(new Vector3(2, 2, 2), 0.5f).onComplete = () => {
                //位移 同时变小
                flsh.transform.DOScale(toScale, 1f);
                if (isQiCaiFlsh){ return; }
                flsh.transform.Find("gongxi").gameObject.SetActive(false);
                flsh.transform.Find("bg").gameObject.SetActive(false);
                DOTween.To(() => flsh.transform.position, p => flsh.transform.position = p,
                skinTrs.SeachTrs<Transform>(isQiCaiFlsh ? "wucaiyu" : "flsh" + flshNumber).position, 1f).
                onComplete = () =>
                    {
                        Destroy(flsh);
                        callFlshIsShow = false;
                        updateUI();
                    };
                };
            };
    }

    #endregion
}
