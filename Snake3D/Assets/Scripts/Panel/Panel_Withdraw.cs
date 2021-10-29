//-------------------------------------------
//作者：马超
//时间：2020-09-22 20:19
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;



public class Panel_Withdraw : PanelBase
{
    Text txt_Balance;

    private GameObject Tip;
    Toggle tog1;
    Toggle tog2;
    Toggle tog3;
    Toggle tog4;
    Toggle tog_protocol;
    ToggleGroup group;
    Toggle[] toggles;

    Text txt_name;
    Text txt_wtx;
    Transform with_Item;

    Text txt_id;
    Text txt_tixianName;

    Transform withdraw;
    Transform withdrawList;

    Transform qicaiExplain;

    Transform centerBottom;

    Transform onLoading;
    Transform centerData;

    //滚动背景
    bool pkIsShow;
    RectTransform Pk_Bg;
    Text Pk_Name;

    ScoreRankResponse totalRank;//所有排行榜
    List<RankList> score_rank;//当前排行榜
    Rank_limit currentLimit;//当前limit
    int currentRank = ConfigManager.NUM_100;//当前页面
    int myRankNumber; //自己在排行榜的排名

    RankList myRank;//自己在排行榜中的数据

    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=PanelName.Panel_Withdraw;
        _openDuration=0.5f;
        _alpha=0f;
        _showStyle=PanelMgr.PanelShowStyle.Nomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache=false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_Withdraw");
    }
    public override void OnInit(params object[] panelArgs)
    {
        base.OnInit(panelArgs);
        InitData();
    }
    #endregion

    #region 数据定义
    private float money = 50f;
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
        withdraw =skinTrs.SeachTrs<Transform>("withdraw");
        withdrawList = skinTrs.SeachTrs<Transform>("withdrawList");
        withdrawList.gameObject.SetActive(false);
        txt_wtx= skinTrs.SeachTrs<Text>("txt_wtx");
        with_Item = skinTrs.SeachTrs<Transform>("with_Item");

        txt_id = skinTrs.SeachTrs<Text>("txt_id");
        txt_tixianName = skinTrs.SeachTrs<Text>("txt_weixinName");

        Pk_Bg = skinTrs.SeachTrs<RectTransform>("Pk_Bg");
        Pk_Name = skinTrs.SeachTrs<Text>("Pk_Name");

        Tip = skinTrs.SeachTrs<Transform>( "money_tip" ).gameObject;
        txt_Balance = skinTrs.SeachTrs<Text>("txt_Balance");
        tog1 = skinTrs.SeachTrs<Toggle>("tog1");
        tog2 = skinTrs.SeachTrs<Toggle>("tog2");
        tog3 = skinTrs.SeachTrs<Toggle>("tog3");
        tog4 = skinTrs.SeachTrs<Toggle>("tog4");
        tog_protocol = skinTrs.SeachTrs<Toggle>("txt_Balance");

        group = skinTrs.SeachTrs<Transform>( "center_top" ).gameObject.GetComponent<ToggleGroup>( );
        qicaiExplain = skinTrs.SeachTrs<Transform>("qicaiExplain");
        centerBottom = skinTrs.SeachTrs<Transform>("centerBottom");
        onLoading = skinTrs.SeachTrs<Transform>("onLoading");
        centerData = skinTrs.SeachTrs<Transform>("centerData");

        txt_name = skinTrs.SeachTrs<Text>("txt_name");

        toggles = group.GetComponentsInChildren<Toggle>( );
        //按钮至灰
        updateUI();
    }

    public void updateUI()
    {
        //按钮控制刷新
        choiceToggleAndMoney(toggles);

      
    }

    /// <summary>
    /// 按钮至灰 不可点击 
    /// </summary>
    /// <param name="toggles"></param>
    private void choiceToggleAndMoney(Toggle[] toggles)
    {
        //数据正在加载
        //onLoading.gameObject.SetActive(true);
        //centerData.gameObject.SetActive(false);
        //刷新服务器体现档次
        // HttpManager.Instance.GetAmount(b => {
        //获取提现档次
        //int userMoney = DataManager.getAmount() - 1;
        //金额是否达到当前档次来展示锦鲤合成
        // bool qicaiIsShow = false;
        //switch (userMoney)
        //{
        //    case 0:
        //        qicaiIsShow = DataManager.getCurrentMoney() >= ConfigManager.NUM_50;
        //        viewCenterControl(qicaiIsShow, ConfigManager.NUM_50.ToString(), userMoney, new Vector3(0, 0, 0), new Vector3(0, 52, 0), new Vector3(0, -850, 0));
        //        break;
        //    case 1:
        //        qicaiIsShow = DataManager.getCurrentMoney() >= ConfigManager.NUM_100;
        //        viewCenterControl(qicaiIsShow, ConfigManager.NUM_100.ToString(), userMoney, new Vector3(0, 180, 0), new Vector3(0, 52, 0), new Vector3(0, -850, 0));
        //        break;
        //    case 2:
        //        qicaiIsShow = DataManager.getCurrentMoney() >= ConfigManager.NUM_120;
        //        viewCenterControl(qicaiIsShow, ConfigManager.NUM_120.ToString(), userMoney, new Vector3(0, 0, 0), new Vector3(0, -190, 0), new Vector3(0, -1076, 0));
        //        break;
        //    case 3:
        //        qicaiIsShow = DataManager.getCurrentMoney() >= ConfigManager.NUM_150;
        //        viewCenterControl(qicaiIsShow, ConfigManager.NUM_150.ToString(), userMoney, new Vector3(0, 180, 0), new Vector3(0, -190, 0), new Vector3(0, -1076, 0));
        //        break;
        //}
        //for (int i = 0; i < toggles.Length; i++)
        //{
        //    if (userMoney == i)
        //    {
        //        toggles[i].isOn = true;
        //        toggles[i].transform.SeachTrs<Image>("Background").color = Color.white;
        //        toggles[i].transform.SeachTrs<Image>("Checkmark").enabled = true;
        //        //更改已选中togg里的字体颜色
        //        toggles[i].transform.SeachTrs<Text>("Label").color = Color.white;
        //        toggles[i].transform.SeachTrs<Outline>("Label").enabled = true;
        //    }
        //    else
        //    {
        //        toggles[i].enabled = false;
        //        toggles[i].transform.GetChild(2).gameObject.SetActive(false);
        //        toggles[i].transform.SeachTrs<Image>("Background").color = Color.gray;
        //        toggles[i].transform.SeachTrs<Image>("Checkmark").enabled = false;
        //    }
        //}
        //数据加载完毕
        toggles[0].enabled = true;
        onLoading.gameObject.SetActive(false);
            centerData.gameObject.SetActive(true);
       // });
    }

    /// <summary>
    /// 锦鲤中部提示面板 适配,功能,位移,文案解决
    /// </summary>
    private void viewCenterControl(bool isShow , string money , int amount , Vector3 bgRoV3 , Vector3 qicaiExplainV3 , Vector3 centerBottomV3)
    {
        qicaiExplain.gameObject.SetActive(isShow);
        qicaiExplain.Find("title").GetComponent<Text>().text = money + "元立即提现说明";
        qicaiExplain.Find("center").GetComponent<Text>().text = "1.当获得" + HttpDataManager.GetConfigInfo().withDrowFishNumber[amount] + "条锦鲤后可立即提现\n"
        + "2.领取到的锦鲤必合成七彩锦鲤,快去领取吧"/*\n" + string .Format("3.七彩锦鲤每日限定召唤{0}次，因此不要错过每天的幸运召唤哟~",HttpDataManager.GetConfigInfo().carpCallTimes)*/;
        qicaiExplain.Find("Btn_WuCai").Find("lingqu").GetComponent<Text>().text = "领取七彩锦鲤(" + DataManager.getWuCaiFlsh() + "/" + HttpDataManager.GetConfigInfo().withDrowFishNumber[amount] + ")";
        qicaiExplain.Find("bg").Rotate(bgRoV3);
        qicaiExplain.localPosition = qicaiExplainV3;
        centerBottom.localPosition = centerBottomV3;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        //提现名称
        txt_tixianName.text = "立即提现";

        //广播展示
        pkIsShow = true;

        //刷新榜单
        //HttpManager.Instance.getScoreRank();
        
        txt_Balance.text = DataManager.getCurrentMoney().ToString()+"元";

        withdrawList.gameObject.SetActive(false);

        UpdateName();

        //HttpManager.Instance.getWithDrawList();

        txt_id.text= "ID:"+HttpDataManager.getUser()?.user_id.ToString();

        AdManager.Ins.showNativeAd(0,null,"withdraw");
    }
    
    void UpdateName() 
    {
        if (HttpDataManager.getUser().is_bind_wechat != 0)
        {
            txt_name.gameObject.SetActive(true);
            txt_name.text = HttpDataManager.getUser().name;
        }
        else 
        {
            txt_name.gameObject.SetActive(false);
        }
    }

    /// <summary>添加事件</summary>
    private void AddEvent()
    {
        RegisterEvent(EventDef.REQUEST_WITHDRAW_LIST);
        RegisterEvent(EventDef.REF_WITHDROW);
        RegisterEvent(EventDef.BIND_WX);
    }

    private void Update()
    {
        //广播展示
        //pkShow();
    }

    private int pkNameCount;
    /// <summary>
    /// 广播展示
    /// </summary>
    public void pkShow()
    {
        if (pkIsShow)
        {
            if (Pk_Bg.sizeDelta.y <= 118)
            {
                Pk_Bg.sizeDelta = Pk_Bg.sizeDelta + new Vector2(0, 90 * Time.deltaTime);
            }
            else
            {
                //已经展开
                if(Pk_Name.transform.localPosition.x == 1500)
                {
                    switch (pkNameCount)
                    {
                        case 0:
                            {
                                Pk_Name.text = "游戏得红包！游戏得积分！每周排行榜！冲榜得现金!"; 
                                pkNameCount++;
                                break;
                            }
                        case 1:
                            {
                                Pk_Name.text = "当红包余额满足后,只需积分进入排行榜提现榜单即可立即提现!";
                                pkNameCount++;
                                break;
                            }
                        case 2:
                            {
                                Pk_Name.text = "每周日22点,系统对封榜时获得提现资格的玩家发放现金奖励!";
                                pkNameCount = 0;
                                break;
                            }
                    }
                }
                Pk_Name.transform.localPosition = Pk_Name.transform.localPosition + new Vector3(-210f * Time.deltaTime, Pk_Name.transform.localPosition.y);
                if(Pk_Name.transform.localPosition.x <= -1500)
                {
                    pkIsShow = false;
                    //重新推送
                    Pk_Name.transform.localPosition = new Vector3(1500, Pk_Name.transform.localPosition.y);
                }
            }
        }
        else
        {
            Pk_Bg.sizeDelta = Pk_Bg.sizeDelta - new Vector2(0, 90 * Time.deltaTime);
            if(Pk_Bg.sizeDelta.y <= -50)
            {
                pkIsShow = true;
            }
        }
    }

    public override void HandleEvent(EventDef ev, LogicEvent le)
    {
        base.HandleEvent(ev, le);

        if (ev == EventDef.REQUEST_WITHDRAW_LIST) 
        {
            if (le.Object0 != null) 
            {
                List<WithDrawResponse> res = (List<WithDrawResponse>)le.Object0;
                with_Item.gameObject.SetActive(false);
                if (res != null && res.Count > 0)
                {
                    txt_wtx.gameObject.SetActive(false);
                    foreach (WithDrawResponse item in res)
                    {
                        RecordItem record = GameObject.Instantiate(with_Item.gameObject).AddComponent<RecordItem>();
                        record.transform.SetParent(with_Item.parent);
                        record.Init();
                        record.UPdateUI(item);
                    }
                }
                else 
                {
                    txt_wtx.gameObject.SetActive(true);
                }
            } 
        }
        //刷新ui
        if (ev == EventDef.REF_WITHDROW)
        {
            updateUI();
        }

        if (ev == EventDef.BIND_WX) 
        {
            UpdateName();
        }
    }

    protected override void Close()
    {
        base.Close();

        UnRegisterEvent(EventDef.REF_WITHDROW);
        UnRegisterEvent(EventDef.BIND_WX);
        UnRegisterEvent(EventDef.REQUEST_WITHDRAW_LIST);
        AdManager.Ins.removeNativeAd();
        if (Snake_Game.Ins.isPause)
            Snake_Game.Ins.isPause = false;
    }

    /// <summary>按钮点击事件</summary>
    protected override void OnClick(Transform _target)
    {
        base.OnClick(_target);
        switch (_target.name)
        {
            case "Btn_Close":
                if (withdrawList.gameObject.activeSelf)
                {
                    withdraw.gameObject.SetActive(true);
                    withdrawList.gameObject.SetActive(false);
                }
                else 
                {

                    Close();
                    UnRegisterEvent(EventDef.REQUEST_WITHDRAW_LIST);
                }
                break;
            case "Btn_recording":
                withdraw.gameObject.SetActive(false);
                withdrawList.gameObject.SetActive(true);

                break;
            case "Btn_xy":
            case "Btn_xy1":
                Application.OpenURL(SystemConfig.Get(1).fwxy);
                break;
            case "Btn_ysxy":
                Application.OpenURL(SystemConfig.Get(1).ysxy); 
                break;
            case "Btn_yhxy":
                Application.OpenURL(SystemConfig.Get(1).fwxy);
                break;
            case "Btn_youxiang":
                string Title = Application.platform + "-" + Application.productName + "-" + Application.version + "-" +
                     SystemInfo.deviceUniqueIdentifier + "-" + HttpDataManager.getUser().user_id;
                Uri uri = new Uri(string.Format("mailto:{0}?subject={1}", ConfigManager.exmail, Title)); //第二个参数是邮件的标题 
                Application.OpenURL(uri.AbsoluteUri);
                break;
            case "Btn_WuCai":
                DotManager.Instance.sendEvent("withdrawal_call_carp");
                //PlayerPrefs.SetString("key_user_five_flsh", "0,0,0,0,0,0,0"); //普通鱼测试初始化
                //PlayerPrefs.SetInt("key_wucai_flsh_number", 5); //七彩鱼测试初始化
                //PlayerPrefs.SetInt("key_common_flsh_number", 0);
                //PlayerPrefs.SetInt("key_seven_flsh_count", 0);
                PanelMgr.GetInstance.ShowPanel(PanelName.Panel_WuCai);
                break;
            case "Btn_Withdraw":
                DotManager.Instance.sendEvent("cashpage_apply", DottingType.Tga);
                LogicMgr.GetInstance.GetLogic<LogicTips>().AddTips("余额不足");
                return;
                //提现 (拉取当前提现档位) 
                choseToggesMoney(toggles);
                Debug.Log("当前选择的档位:" + money);
                //1.如果未绑定微信
                if (HttpDataManager.getUser().is_bind_wechat == 0)
                {
                    showTip("请先绑定微信后再申请立即提现！");
#if UNITY_EDITOR
                    Debug.Log("绑定微信");
                    return;
#elif UNITY_IOS
                    WeChatSDK.LoginWechat();
                    return;
#elif UNITY_ANDROID
                    AndroidSend.WxLogin();
                    return;
#endif
                }
                //2.钱够不够
                if (DataManager.getCurrentMoney() < money)
                {
                    showTip("余额不足，请继续闯关赚取红包！");
                    return;
                }
                //3.锦鲤够不够 改为四个档 
                int qicaiCount = 0;
                switch (money)
                {
                    case ConfigManager.NUM_50:
                        qicaiCount = HttpDataManager.GetConfigInfo().withDrowFishNumber[0];
                        break;
                    case ConfigManager.NUM_100:
                        qicaiCount = HttpDataManager.GetConfigInfo().withDrowFishNumber[1];
                        break;
                    case ConfigManager.NUM_120:
                        qicaiCount = HttpDataManager.GetConfigInfo().withDrowFishNumber[2];
                        break;
                    case ConfigManager.NUM_150:
                        qicaiCount = HttpDataManager.GetConfigInfo().withDrowFishNumber[3];
                        break;
                }
                if (DataManager.getWuCaiFlsh() < qicaiCount)
                {
                    showTip("立即提现需得到"+qicaiCount+"条七彩锦鲤的祝福！");
                    return;
                }
                //最终提现
                HttpManager.Instance.putWithDraw(money,action =>
                {
                    if (action)
                    {
                        showTip("已提交申请,请耐心等待审核~");
                        // - 钱
                        txt_Balance.text = (DataManager.getCurrentMoney() - money).ToString()+"元";
                        DataManager.addMoney(-money);
                        // - 七彩锦鲤
                        PlayerPrefs.SetInt("key_wucai_flsh_number", 0);
                        // - 普通鱼
                        PlayerPrefs.SetString("key_user_five_flsh", "0,0,0,0,0,0,0");
                        //updateUI
                        updateUI();
                    } 
                    else
                        showTip("申请失败");
                });
                break;
        }
    }

    //当前选择的哪个档次
    public void choseToggesMoney(Toggle[] toggles)
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                switch (i)
                {
                    case 0:
                        money = 50;
                        break;
                    case 1:
                        money = 100;
                        break;
                    case 2:
                        money = 120;
                        break;
                    case 3:
                        money = 150;
                        break;
                }
            }
        }
    }
    
    public void showTip(string msg)
    {
        if ( !Tip.activeSelf )
        {
            Tip.transform.GetChild( 0 ).GetComponent<Text>( ).text = msg;
            Tip.SetActive( true );
            Invoke( nameof( hideTip ) , 2f );
        }
    }

    private void hideTip()
    {
        Tip.SetActive( false );
    }
    
    
    #endregion


}


/// <summary>
/// 记录条目
/// </summary>
class RecordItem : MonoBehaviour
{
    Text txt_cg;
    Text txt_time;
    Text txt_count;

    public void Init()
    {
        txt_cg = transform.SeachTrs<Text>("txt_cg");
        txt_time = transform.SeachTrs<Text>("txt_time");
        txt_count = transform.SeachTrs<Text>("txt_count");
    }

    public void UPdateUI(WithDrawResponse rank)
    {
        gameObject.SetActive(true);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        //txt_cg.text = rank.rank.ToString();
        //txt_time.text = rank..ToString();
        //txt_count.text = rank.user_score.ToString();
    }

    
}