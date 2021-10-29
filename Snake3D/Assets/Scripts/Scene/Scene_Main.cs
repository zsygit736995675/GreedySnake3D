using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Scene_Main : SceneBase
{

    Text Red_Money;

    DateInfo info;
    DataItem currentItem;
    float timer;

    Image yun1, yun2, bg;

    Transform trs_ys1, trs_ys2, trs_ys3, Btn_cash_chip, bottom, middle, Btn_pause, trs_tx, trs_jb, trs_buff, test,Top;

    Text txt_level, txt_pro, txt_Gamelevel;

    InputField input, InputFieldqc;

    Toggle adToggle;

    /// <summary>
    /// 广播内容
    /// </summary>
    Text Pk_Name;

    /// <summary>
    /// 广播名字列表
    /// </summary>
    public static List<string> nameList = new List<string>();

    /// <summary>
    /// 广播背景
    /// </summary>
    RectTransform Pk_Bg;

    ParticleSystem hao;
    ParticleSystem zhenbang;
    ParticleSystem wanmei;

    #region 界面加载
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Scene/Scene_Main");

    }
    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type = SceneName.Scene_Main;
    }
    public override void OnInit(params object[] sceneArgs)
    {
        base.OnInit(sceneArgs);
        InitData();
    }
    public override void OnHiding()
    {
        base.OnHiding();
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

    private void FindObj()
    {
        Red_Money = skinTrs.SeachTrs<Text>("Red_Money");
        Pk_Name = skinTrs.SeachTrs<Text>("Pk_Name");
        Pk_Bg = skinTrs.SeachTrs<RectTransform>("Pk_Bg");
        trs_ys1 = skinTrs.SeachTrs<Transform>("trs_ys1");
        trs_ys2 = skinTrs.SeachTrs<Transform>("trs_ys2");
        trs_ys3 = skinTrs.SeachTrs<Transform>("trs_ys3");
        Btn_cash_chip = skinTrs.SeachTrs<Transform>("Btn_cash_chip");
        txt_level = skinTrs.SeachTrs<Text>("txt_level");
        input = skinTrs.SeachTrs<InputField>("InputField");
        InputFieldqc = skinTrs.SeachTrs<InputField>("InputFieldqc");
        bottom = skinTrs.SeachTrs<Transform>("bottom");
        middle = skinTrs.SeachTrs<Transform>("middle");
        bg = skinTrs.SeachTrs<Image>("bg");
        Btn_pause = skinTrs.SeachTrs<Transform>("Btn_pause");
        txt_pro = skinTrs.SeachTrs<Text>("txt_pro");
        trs_tx = skinTrs.SeachTrs<Transform>("trs_tx");
        trs_jb = skinTrs.SeachTrs<Transform>("trs_jb");
        trs_buff = skinTrs.SeachTrs<Transform>("trs_buff");

        test = skinTrs.SeachTrs<Transform>("test");
        adToggle = skinTrs.SeachTrs<Toggle>("adToggle");
        txt_Gamelevel = skinTrs.SeachTrs<Text>("txt_Gamelevel");

        Top = skinTrs.SeachTrs<Transform>("Top");

        GameObject sceen = Resources.Load<GameObject>("Game/SceenObj");
        Instantiate(sceen).gameObject.AddComponent<Snake_Game>();


        adToggle.onValueChanged.AddListener(AdToggle);
        adToggle.isOn = true;
    }

    void AdToggle(bool ison)
    {
        ConfigManager.isAllowAd = ison;
    }

    void FlyYun()
    {
        int oft = UnityEngine.Random.Range(5, 10);
        Invoke(nameof(FlyYun), oft);

        int dir = UnityEngine.Random.Range(0, 2);
        int y = UnityEngine.Random.Range(40, 190);
        int type = UnityEngine.Random.Range(0, 2);

        int x = 0;

        if (dir == 0)
        {
            x = -800;
        }
        else
        {
            x = 800;
        }

        GameObject go = null;
        if (type == 0)
        {
            go = GameObject.Instantiate(yun1, yun1.transform.parent).gameObject;
        }
        else
        {
            go = GameObject.Instantiate(yun2, yun2.transform.parent).gameObject;
        }
        go.transform.localScale = Vector3.one;
        go.SetActive(true);
        go.transform.localPosition = new Vector3(x, y, 0);
        int speed = UnityEngine.Random.Range(15, 20);
        go.transform.DOLocalMoveX(-x, speed).onComplete = () => {
            Destroy(go);
        };
    }


    void UpdateData()
    {
        if (currentItem != null && currentItem.timer > 0)
        {
            int minute = currentItem.timer / 60;
            int second = currentItem.timer % 60;

            string timerstr = "";
            if (minute < 10)
            {
                timerstr = "0" + minute + ":";
            }
            else
            {
                timerstr = minute + ":";
            }

            if (second < 10)
            {
                timerstr += "0" + second;
            }
            else
            {
                timerstr += second;
            }
        }
    }

    IEnumerator WWWReadFile(Action action)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        string PathName = Application.dataPath + "/StreamingAssets/name.txt";
#elif UNITY_ANDROID
         string PathName = "jar:file://" + Application.dataPath + "!/assets/name.txt";
#elif UNITY_IPHONE
         string PathName = Application.dataPath + "/Raw/name.txt";
#else
        return "";
#endif

        WWW www = new WWW(PathName);

        yield return www;
        if (www.isDone && string.IsNullOrEmpty(www.error))
        {
            string[] strs = www.text.Split('\n');
            foreach (var item in strs)
            {
                nameList.Add(item);
            }
        }

        action?.Invoke();
    }

    public override void OnShowing()
    {
        base.OnShowing();

        Invoke(nameof(CheckRed), 1);

        currentMoney = DataManager.getCurrentMoney();
        currentScore = DataManager.getCurrentScore();
        Red_Money.text = DataManager.getCurrentMoney().ToString();

        UpdateMain();

    }

    void CheckRed()
    {
        if (DataManager.getCurrentMoney() == 0f)
        {
            PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Gold, RedType.Newcomer);
        }
        else
        {
            if (DataManager.getDailyRedTime())
            {
                // PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Gold, RedType.daily);
            }
        }
    }

    /// <summary>
    /// 创建飞翔红包 递归刷出
    /// </summary>
    public void instanceFly_Red()
    {
        int destroyTime = 17;
        GameObject fly_red = GameObject.Instantiate(Resources.Load<GameObject>("new_fly_red/fly_red"), transform.parent);
        //飞翔红包存活时间
        Destroy(fly_red, destroyTime);
        Invoke("instanceFly_Red", destroyTime + UnityEngine.Random.Range(HttpDataManager.GetConfigInfo().reddisappear[0], HttpDataManager.GetConfigInfo().reddisappear[1]));
    }

    void TimingRed()
    {
        //if (currentItem == null || currentItem.isreceive)
        //{
        //    currentItem = GetItem();
        //}

        //if (currentItem != null)
        //{
        //    //if (!RedTime.gameObject.activeSelf)
        //    //{
        //    //    RedTime.gameObject.SetActive(true);
        //    //}

        //    if (currentItem.timer > 0)
        //    {
        //        timer += Time.deltaTime;
        //        if (timer >= 1)
        //        {
        //            timer = 0;
        //            currentItem.timer -= 1;
        //            UpdateData();
        //            if (ani.enabled)
        //                ani.enabled = false;
        //            Btn_red.transform.eulerAngles = Vector3.zero;
        //            DataManager.SetCurrentDayOnlineRed(info);
        //        }
        //    }
        //    else
        //    {
        //        ani.enabled = true;
        //        RedTime.text = "可领取";
        //    }
        //}
        //else
        //{
        //    ani.enabled = false;
        //    Btn_red.transform.eulerAngles = Vector3.zero;
        //    //领取完毕
        //    RedTime.gameObject.SetActive(false);
        //}
    }

    DataItem GetItem()
    {
        foreach (DataItem item in info.data)
        {
            if (!item.isreceive)
            {
                return item;
            }
        }

        info.isEnd = true;
        DataManager.SetCurrentDayOnlineRed(info);
        return null;
    }

    private void Update()
    {
        if (info != null && !info.isEnd)
        {
            TimingRed();
        }

        //主界面金额同步
        if (Red_Money.text != currentMoney.ToString())
        {
            Red_Money.text = currentMoney.ToString("f2");
        }
    }

    private float pkMoveTimer;
    /// <summary>
    /// pk 广播 移动
    /// </summary>
    private void pkMove()
    {
        float speed = 100; //广播移动速度
        int xFirst = 1100; //移动起始点
        Pk_Bg.localPosition = Pk_Bg.localPosition + Vector3.left * Time.deltaTime * speed;
        if (Pk_Bg.localPosition.x + xFirst <= 1)
        {
            int range = UnityEngine.Random.Range(0, 2);
            switch (UnityEngine.Random.Range(0, 3))
            {
                case 0:
                    {
                        //广播名字赋值
                        Pk_Name.text = "恭喜用户 <color=white>" + nameList[UnityEngine.Random.Range(0, nameList.Count - 1)] + "</color> 成功提现" + (range == 0 ? "50" : "100") + "元";
                        Pk_Bg.localPosition = new Vector3(xFirst, Pk_Bg.localPosition.y);
                        break;
                    }
                case 1:
                    {
                        //微信广播名字赋值
                        string wxName = "wx" + UnityEngine.Random.Range(100000, 9999999);
                        Pk_Name.text = "恭喜用户 <color=white>" + wxName + "</color> 成功提现" + (range == 0 ? "50" : "100") + "元";
                        Pk_Bg.localPosition = new Vector3(xFirst, Pk_Bg.localPosition.y);
                        break;
                    }
                case 2:
                    {
                        //碎片广播名字复制
                        string wxName = "wx" + UnityEngine.Random.Range(100000, 9999999);
                        int[] ints = new int[] { 1, 5, 10, 20, 50, 100 };
                        Pk_Name.text = "恭喜用户 <color=white>" + wxName + "</color> 成功现金碎片合成" + ints[UnityEngine.Random.Range(0, 6)] + "元提现成功!";
                        Pk_Bg.localPosition = new Vector3(xFirst, Pk_Bg.localPosition.y);
                        break;
                    }
                case 3:
                    {
                        //提示类广播赋值
                        switch (UnityEngine.Random.Range(0, 4))
                        {
                            case 0:
                                {
                                    Pk_Name.text = "每周现金大奖，只需积分上榜!";
                                    break;
                                }
                            case 1:
                                {
                                    Pk_Name.text = "观看广告可获得大额积分奖励!";
                                    break;
                                }
                            case 2:
                                {
                                    Pk_Name.text = "排行榜每周刷新一次，周周有现金!";
                                    break;
                                }
                            case 3:
                                {
                                    Pk_Name.text = "每周可提现，谁都有机会!";
                                    break;
                                }
                        }
                        Pk_Bg.localPosition = new Vector3(xFirst, Pk_Bg.localPosition.y);
                        break;
                    }
            }
        }
    }

    private void AddEvent()
    {
        RegisterEvent(EventDef.DataUpdate);
        RegisterEvent(EventDef.FlyNum);
        RegisterEvent(EventDef.Evaluation);
        RegisterEvent(EventDef.FlyEnd);
        RegisterEvent(EventDef.UPdateScore);
        RegisterEvent(EventDef.LevelUpdate);
        RegisterEvent(EventDef.CloseGrab);
        RegisterEvent(EventDef.Add_Plan);
        RegisterEvent(EventDef.Add_Chip);
        RegisterEvent(EventDef.Update_Main);
        RegisterEvent(EventDef.EnterLevel);
        RegisterEvent(EventDef.UpdateLevelPro);
        RegisterEvent(EventDef.ShowBuffPro);
        RegisterEvent(EventDef.ShowMoney);
        RegisterEvent(EventDef.NextLevel);
        RegisterEvent(EventDef.Callback);
        RegisterEvent(EventDef.RemoveBuff);
        RegisterEvent(EventDef.ClearBuff);
        RegisterEvent(EventDef.ShowTop);
        RegisterEvent(EventDef.HideTop);
    }

    float currentMoney;
    int currentScore;
    public override void HandleEvent(EventDef ev, LogicEvent le)
    {
        base.HandleEvent(ev, le);
        switch (ev)
        {
            case EventDef.UPdateScore:
                int tempScore = DataManager.getCurrentScore();
                DOTween.To(() => currentScore, p => currentScore = p, tempScore, 0.5f);
                break;
            case EventDef.FlyEnd:

                float tempMoney = DataManager.getCurrentMoney();
                tempScore = DataManager.getCurrentScore();

                DOTween.To(() => currentMoney, p => currentMoney = p, tempMoney, 0.5f).onUpdate=()=> {

                    Red_Money.text = currentMoney.ToString("f2");
                };
                DOTween.To(() => currentScore, p => currentScore = p, tempScore, 0.5f);

                break;
            case EventDef.DataUpdate:
                UpdateData();
                break;
            case EventDef.FlyNum:
                PanelMgr.GetInstance.ShowPanel(PanelName.Panel_NewRed, le.FloatParam0, Red_Money.transform.position);
                break;
            case EventDef.LevelUpdate:
                txt_level.text = le.IntParam0 + "关";
                txt_Gamelevel.text = "第" + le.IntParam0 + "关";
                break;
            case EventDef.Update_Main:
                UpdateMain();
                break;
            case EventDef.EnterLevel:
                EnterLevel();
                break;
            case EventDef.UpdateLevelPro:
                txt_pro.text = le.IntParam0 + "/" + le.IntParam1;
                break;
            case EventDef.RemoveBuff:
                RemoveBuff((BuffType)le.IntParam0);
                break;
            case EventDef.ClearBuff:
                ClearBuff();
                break;
            case EventDef.ShowBuffPro:
                AddBuff((BuffType)le.IntParam0,le.FloatParam0);
                break;
            case EventDef.ShowMoney:
                trs_tx.gameObject.SetActive(true);
                break;
            case EventDef.NextLevel:
                Snake_Game.Ins.NextLevel();
                break;
            case EventDef.Callback:
                Action callback = (Action)le.Object0;
                ThreadManager.Ins.runOnMainThread(() =>
                {
                    callback?.Invoke();
                });
                break;
            case EventDef.ShowTop:
                Top.gameObject.SetActive(true);
                break;
            case EventDef.HideTop:
                Top.gameObject.SetActive(false);
                break;
        }
    }

    List<BuffUI> buffs = new List<BuffUI>();
    void AddBuff (BuffType type,float timer)
    {
        BuffUI ui=null;
        foreach (var item in buffs)
        {
            if (item.type == type||!item.gameObject.activeSelf)
            {
                ui = item;
                break;
            }
        }

        if (ui == null) 
        {
            GameObject go = Resources.Load<GameObject>("Game/trs_buff");
            GameObject buffui = Instantiate(go, trs_buff);
            ui = buffui.AddComponent<BuffUI>();
            ui.Init();
            buffs.Add(ui);
        }

        ui.ShowBuff(type,timer);
    }

    void ClearBuff() 
    {
        foreach (var item in buffs)
        {
                item.Clear();
        }
    }

    void RemoveBuff(BuffType type)
    {
        foreach (var item in buffs)
        {
            if (item.type == type) 
            {
                item.Clear();
            }
        }
    }

    /// <summary>
    /// 用户积分 和 最后一次登录时间 埋点
    /// </summary>
    private void AddTGAUser()
    {
        DotManager.Instance.userSet("integral_Suspension_number", DataManager.getCurrentScore());
        DotManager.Instance.sendLoginTime();
    }

    void UpdateMain() 
    {
        txt_Gamelevel.gameObject.SetActive(false);
        test.gameObject.SetActive(MainLogic._Instance.isTest);
        //trs_jb.gameObject.SetActive(true);
        trs_tx.gameObject.SetActive(true);
        bottom.gameObject.SetActive(true);
        middle.gameObject.SetActive(true);
        bg.gameObject.SetActive(true);
        Btn_pause.gameObject.SetActive(false);
        txt_pro.gameObject.SetActive(false);
        txt_level.text = string.Format("第{0}关", DataManager.getLevelNum());
    }

    void EnterLevel() 
    {
        txt_Gamelevel.gameObject.SetActive(true);
        test.gameObject.SetActive(false);
        trs_jb.gameObject.SetActive(false);
       // trs_tx.gameObject.SetActive(false);
        bottom.gameObject.SetActive(false);
        middle.gameObject.SetActive(false);
        bg.gameObject.SetActive(false);
        Btn_pause.gameObject.SetActive(true);
        txt_pro.gameObject.SetActive(true);
    }

    void OnLineRed() 
    {
        if (currentItem != null)
        {
            if (currentItem.timer <= 0)
            {
                currentItem.isreceive = true;
                //在线时长红包埋点
                DotManager.Instance.sendEvent("online_time_red", DottingType.Tga, new Dictionary<string, object> { { "online_time_red", "online_red_" + (currentItem.num + 1) } });
                PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Gold, RedType.online, currentItem.num);
            }
            else
            {
                LogicMgr.GetInstance.GetLogic<LogicTips>().AddTips("时间还没到哦！！！");
            }
        }
        else
        {
            LogicMgr.GetInstance.GetLogic<LogicTips>().AddTips("今天的红包已经全部领完了！！！");
        }
    }

    /// <summary>按钮点击事件</summary>
    protected override void OnClick(Transform target)
    {
        base.OnClick(target);
        switch (target.name)
        {
            case "Btn_Withdrow":
                DotManager.Instance.sendEvent("cashpage_show", DottingType.Tga);
                PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Withdraw);
                Snake_Game.Ins.isPause = true;
                break;
            case "Btn_Setting":
                PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Setting);
                break;
            case "Btn_red":
                OnLineRed();
                break;
            case "Btn_Enter":
                Snake_Game.Ins.OpenLevel();
                EnterLevel();
                break;
            case "Btn_pause":
                PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Pause);
                Snake_Game.Ins.isPause = true;
                break;
            case "Btn_Level":
                PanelMgr.GetInstance.ShowPanel(PanelName.Panel_LevelList);
                break;
            case "Btn_test":
                Snake_Game.Ins.OpenLevel(int.Parse( input.text));
                EnterLevel();
                break;
            case "Btn_test_qc":
                if (!string.IsNullOrEmpty(InputFieldqc.text))
                {
                    PlayerPrefs.SetInt("key_wucai_flsh_number", int.Parse(InputFieldqc.text)) ;
                }
                break;

            case "Btn_cash_chip":
                PanelMgr.GetInstance.ShowPanel(PanelName.Panel_CashChip);
                break;
            case "Btn_prize_pool":
                PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Grab);
                break;
        }
    }

    #endregion

}


public class DataItem
{
    public int num { get; set; }
    public int timer { get; set; }
    public bool isreceive { get; set; }
    public int nearTime { get; set; }
}

public class DateInfo
{
    public string date { get; set; }
    public bool isEnd { get; set; }
    public List<DataItem> data { get; set; }
}