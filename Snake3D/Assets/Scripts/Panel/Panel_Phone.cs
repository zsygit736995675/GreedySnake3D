using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Phone : PanelBase
{
    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type = PanelName.Panel_Phone;
        _openDuration = 0.5f;
        _showStyle = PanelMgr.PanelShowStyle.Nomal;
        _maskStyle = PanelMgr.PanelMaskSytle.None;
        _cache = false;
    }

    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_Phone");
    }

    public override void OnInit(params object[] sceneArgs)
    {
        base.OnInit(sceneArgs);
        skinTrs.GetComponent<RectTransform>().sizeDelta = M_Canvas.sizeDelta;
        InitData();
    }

    #endregion

    #region 数据定义

    private List<Toggle> m_AllToggle;
    private Transform mPlay;

    private string[] AllPrzie = new string[8]
        {"手机碎片*1", "手机碎片*2", "手机碎片*0.5", "现金红包", "手机碎片*1", "谢谢参与", "现金红包", "手机碎片*0.2"};

    private int m_Num = 0;
    private int frequency = 0;

    private bool isOpen;

    //观看视频任务
    private Transform m_Lock;

    //合成红包任务
    private Transform m_Read;

    //碎片数量
    private Transform m_Exchange_Text;

    private Slider ExchangeSlider;

    //剩余次数
    private Text Play_Nub;

    //获奖界面
    private Transform Prize_Image;
    private Text Congratulations_Text;
    private Text Stillbad_Text;
    private Transform Tomorrow_Text;
    private bool Play_Open = true;

    private int m_RedNum;

    //测试用输入框
    private InputField inputField;
    private InputField inputField1;
    private Transform Lockmtmp;


    /// <summary>
    /// money<99.8&&x<3   3--7  4--3 5--6  6--5 7--4    
    /// </summary>
    //private int[] GetRange0 = new int[] { 5, 0, 30, 50, 5, 5, 0, 5 };

    // private int[] GetRange0 = new int[] { 5, 0, 30, 5, 50, 0, 5, 5 };
    private int[] GetRange0 = new int[] { 20, 0, 30, 10, 20, 0, 10, 10 };

    /// <summary>
    /// money<99.8&&3<=x<7.5
    /// </summary>
    // private int[] GetRange1 = new int[] { 20, 0, 30, 10, 10, 10, 0, 20 };
    private int[] GetRange1 = new int[] { 5, 0, 30, 5, 5, 0, 5, 50 };

    /// <summary>
    /// <money<99.8&&x>7.5
    /// </summary>
    //private int[] GetRange2 = new int[] { 0, 0, 0, 0, 33, 33, 34, 0 };
    private int[] GetRange2 = new int[] { 0, 0, 0, 33, 0, 34, 33, 0 };

    /// <summary>
    /// money>=99.8&&x<7.5
    /// </summary>
    //private int[] GetRange3 = new int[] { 5, 0, 30, 50, 0, 0, 10, 5 };
    private int[] GetRange3 = new int[] { 5, 0, 30, 0, 5, 10, 0, 50 };

    /// <summary>
    /// money>=99.8&&x>7.5
    /// </summary>
    //private int[] GetRange4 = new int[] { 0, 0, 0, 0, 0, 0, 100, 0 };
    private int[] GetRange4 = new int[] { 0, 0, 0, 0, 0, 100, 0, 0 };

    #endregion

    #region 逻辑

    /// <summary>初始化</summary>
    private void InitData()
    {
        FindObj();
        InitPrize();

        Refresh();

        //if (Config.Instance.m_First)
        //{
        //    isOpen = Config.Instance.m_First;
        //    mPlay.Find("Play_Text").GetComponent<Text>().text = "剩余抽奖次数:1";
        //    //mPlay.GetComponent<Image>()
        //}
    }

    /// <summary>查找物体</summary>
    private void FindObj()
    {
        m_AllToggle = new List<Toggle>(skinTrs.SeachTrs<Transform>("Prize").GetComponentsInChildren<Toggle>());
        //mPlay = skinTrs.SeachTrs<Transform>("Btn_Play");
        m_Lock = skinTrs.SeachTrs<Transform>("Lock_Image");
        m_Read = skinTrs.SeachTrs<Transform>("Synthesis_Image");
        m_Exchange_Text = skinTrs.SeachTrs<Transform>("Exchange_Text");
        ExchangeSlider = skinTrs.SeachTrs<Slider>("ExchangeSlider");
        Play_Nub = skinTrs.SeachTrs<Text>("Play_Nub");
        Prize_Image = skinTrs.SeachTrs<Transform>("Prize_Image");
        Congratulations_Text = skinTrs.SeachTrs<Text>("Congratulations_Text");
        Stillbad_Text = skinTrs.SeachTrs<Text>("Stillbad_Text");
        Tomorrow_Text = skinTrs.SeachTrs<Transform>("Tomorrow_Text");
        Lockmtmp = skinTrs.SeachTrs<Transform>("Lockmtmp");

        inputField = skinTrs.SeachTrs<InputField>("InputField");
        inputField1 = skinTrs.SeachTrs<InputField>("InputField (1)");
    }

    /// <summary>Update</summary>
    private void Update()
    {
    }

    private void InitPrize()
    {
        for (int i = 0; i < m_AllToggle.Count; i++)
        {
            m_AllToggle[i].transform.Find("Text_Prize").GetComponent<Text>().text = AllPrzie[i];
        }

        Refresh();
        InitPhone();
        Play_Nub.text = DataManager.getCurrentLottery() + "次";
        Prize_Image.gameObject.SetActive(false);
    }

    private void InitPhone()
    {
        m_Exchange_Text.GetComponent<Text>().text = "华为P40 手机碎片" + DataManager.getCurrentDebris() + "/" + "10";
        ExchangeSlider.value = DataManager.getCurrentDebris() * 10;
    }

    IEnumerator PlayLuck(GameObject obj, int num, int c, float time = 0.15f)
    {
        //Debug.LogError("抽奖中"+ m_Num+"--->"+ frequency);


        yield return new WaitForSeconds(time);
        obj.GetComponent<Toggle>().isOn = true;
        if (m_Num == num && frequency == c)
        {
            EndPrzie(m_Num);
            frequency = 0;
            yield return 0;
        }
        else
        {
            m_Num++;
            if (m_Num > 7)
            {
                m_Num = 0;
                frequency++;
            }

            StartCoroutine(PlayLuck(m_AllToggle[m_Num].gameObject, num, c));
        }
    }

    private void StartPrzie()
    {
        int End = GetLotteryResult();
        Debug.LogError("End" + End);
        isOpen = false;
        int c = UnityEngine.Random.Range(2, 4);
        Play_Open = false;
        StartCoroutine(PlayLuck(m_AllToggle[m_Num].gameObject, End, c));
        //Debug.LogError("开始"); 
        //}
    }

    /// <summary>
    /// 抽奖结束，发奖品
    /// </summary>
    private void EndPrzie(int i)
    {
        switch (i)
        {
            case 0:

                DataManager.addDebris(+1);
                CreatPrzie(1, true);
                break;
            case 1:
                DataManager.addDebris(+2);
                CreatPrzie(2, true);
                break;
            case 2:
                DataManager.addDebris(+0.5f);
                CreatPrzie(0.5f, true);
                break;
            case 3:
                CreatPrzie(0.01f, false);
                break;
            case 4:
                DataManager.addDebris(+1);
                CreatPrzie(1, true);
                break;
            case 5:

                break;
            case 6:
                CreatPrzie(0.01f, false);

                break;
            case 7:
                DataManager.addDebris(+0.2f);
                CreatPrzie(0.2f, true);
                break;
        }
        HttpManager.Instance.LotteryData();
        InitPhone();
        Play_Open = true;
    }

    //发奖界面
    private void CreatPrzie(float num, bool where)
    {
        Prize_Image.gameObject.SetActive(true);
        if (where)
        {
            skinTrs.SeachTrs<Image>("Prize_Bg_Image").sprite = Resources.Load<Sprite>("pop_p30");
            Congratulations_Text.text = "获得华为P40手机碎片" + num;
            Stillbad_Text.text = "还差" + (10 - DataManager.getCurrentDebris()) + "个碎片即可兑换手机";
        }
        else
        {
            skinTrs.SeachTrs<Image>("Prize_Bg_Image").sprite = Resources.Load<Sprite>("pop_honbao");
            Congratulations_Text.text = "获得" + num + "元";
            Stillbad_Text.text = "还差" + (100 - DataManager.getCurrentMoney()).ToString("F2") + "元即可提现";
        }
    }

    private void AddLotteryNum()
    {
        DataManager.addLottery(1);
    }

    /// <summary>
    /// 获取结果
    /// </summary>
    /// <returns>0=碎片1,1=碎片2，2=碎片0.5,3=现金红包0.01,4=碎片1， 5=谢谢参与,6=现金红包0.01,7=碎片0.2</returns>
    private int GetLotteryResult()
    {
        //正式包用
        float tempMoney = DataManager.getCurrentMoney();
        float tempDebris = DataManager.getCurrentDebris();


        if (tempMoney < 99.8f) //money<99.8
        {
            if (tempDebris < 3) //x<3
            {
                return getResult(GetRange0);
            }
            else if (tempDebris >= 3 && tempDebris <= 7.5f) //3<=x<7.5f
            {
                return getResult(GetRange1);
            }
            else //x>7.5
            {
                return getResult(GetRange2);
            }
        }
        else //money>=99.8
        {
            if (tempDebris <= 7.5f) //x<=7.5f
            {
                return getResult(GetRange3);
            }
            else //x>7.5f
            {
                return getResult(GetRange4);
            }
        }
    }


    /// <summary>
    /// 获取随机结果
    /// </summary>
    /// <param name="rate">概率</param>
    /// <param name="total">总概率</param>
    /// <returns></returns>
    internal int getResult(int[] rate, int total = 100)
    {
        int r = UnityEngine.Random.Range(1, total + 1);
        int t = 0;
        for (int i = 0; i < rate.Length; i++)
        {
            t += rate[i];
            if (r <= t) return i;
        }

        return 5;
    }

    /// <summary>
    /// 刷新任务进度
    /// </summary>
    private void Refresh()
    {
        //Debug.LogError("合并红包后获得的机会" + DataManager.getRedMergaLotteryNum() + "------合并红包的数量" +
        //               DataManager.getMergaRedNum());

        if (DataManager.getShowVideoNum() >= 3)
        {
            skinTrs.SeachTrs<Text>("Text_Lock").text = "已完成";
            //ic_zp_lq
            skinTrs.SeachTrs<Image>("Btn_Lock").sprite = Resources.Load<Sprite>("ic_zp_wwc");
        }
        else
        {
            skinTrs.SeachTrs<Text>("Text_Lock").text = "可领取";
            skinTrs.SeachTrs<Image>("Btn_Lock").sprite = Resources.Load<Sprite>("ic_zp_lq");
        }

        //if (DataManager.getMergaRedNum() <30 && DataManager.getMergaRedNum()%10!=0&& DataManager.getRedMergaLotteryNum() <= 0)
        //{
        //    skinTrs.SeachTrs<Text>("Text_Synthesis").text = "未完成";
        //    skinTrs.SeachTrs<Image>("Btn_Synthesis").sprite = Resources.Load<Sprite>("ic_zp_wwc");
        //    m_Read.Find("Synthesis_Text").GetComponent<Text>().text = "合成红包十次(" + DataManager.getRedMergaLotteryNum() + "/3)";

        //}
        //else if (DataManager.getMergaRedNum() >= 30&&DataManager.getRedMergaLotteryNum() <= 0)
        //{
        //    skinTrs.SeachTrs<Text>("Text_Synthesis").text = "以完成";
        //    skinTrs.SeachTrs<Image>("Btn_Synthesis").sprite = Resources.Load<Sprite>("ic_zp_wwc");
        //    m_Read.Find("Synthesis_Text").GetComponent<Text>().text = "合成红包十次(" + 3 + "/3)";
        //}

        //else if(DataManager.getMergaRedNum() / 10 <= 3 && DataManager.getRedMergaLotteryNum() > 0)
        //{
        //    skinTrs.SeachTrs<Image>("Btn_Synthesis").sprite = Resources.Load<Sprite>("ic_zp_lq");
        //    skinTrs.SeachTrs<Text>("Text_Synthesis").text = "可领取";
        //    m_Read.Find("Synthesis_Text").GetComponent<Text>().text = "合成红包十次(" + (DataManager.getRedMergaLotteryNum()) + "/3)";
        //}
        if (DataManager.getMergaRedNum() < 30 && DataManager.getRedMergaLotteryNum() < 1)
        {
            //未完成
            skinTrs.SeachTrs<Text>("Text_Synthesis").text = "未完成";
            skinTrs.SeachTrs<Image>("Btn_Synthesis").sprite = Resources.Load<Sprite>("ic_zp_wwc");
            m_Read.Find("Synthesis_Text").GetComponent<Text>().text =
                "打开10个红包(" + DataManager.getMergaRedNum() / 10 + "/3)";
        }
        else if (DataManager.getMergaRedNum() >= 30 && DataManager.getRedMergaLotteryNum() <= 0)
        {
            skinTrs.SeachTrs<Text>("Text_Synthesis").text = "已完成";
            skinTrs.SeachTrs<Image>("Btn_Synthesis").sprite = Resources.Load<Sprite>("ic_zp_wwc");
            m_Read.Find("Synthesis_Text").GetComponent<Text>().text = "打开10个红包(" + 3 + "/3)";
        }
        else
        {
            //可领取
            skinTrs.SeachTrs<Image>("Btn_Synthesis").sprite = Resources.Load<Sprite>("ic_zp_lq");
            skinTrs.SeachTrs<Text>("Text_Synthesis").text = "可领取";
            m_Read.Find("Synthesis_Text").GetComponent<Text>().text =
                "打开10个红包(" + (DataManager.getMergaRedNum() / 10) + "/3)";
        }


        if (DataManager.getCurrentLottery() <= 0 && DataManager.getShowVideoNum() >= 3 &&
            DataManager.getMergaRedNum() >= 30)
        {
            Tomorrow_Text.gameObject.SetActive(true);
            skinTrs.SeachTrs<Transform>("Null_Image").gameObject.SetActive(false);
            skinTrs.SeachTrs<Transform>("Play_Text").gameObject.SetActive(false);
            Lockmtmp.gameObject.SetActive(false);
        }
        else if (DataManager.getShowVideoNum() < 3 && DataManager.getCurrentLottery() <= 0)
        {
            Lockmtmp.gameObject.SetActive(true);
            skinTrs.SeachTrs<Transform>("Null_Image").gameObject.SetActive(false);
            skinTrs.SeachTrs<Transform>("Play_Text").gameObject.SetActive(false);
            Tomorrow_Text.gameObject.SetActive(false);
        }
        else
        {
            skinTrs.SeachTrs<Transform>("Null_Image").gameObject.SetActive(true);
            skinTrs.SeachTrs<Transform>("Play_Text").gameObject.SetActive(true);
            Lockmtmp.gameObject.SetActive(false);
            Tomorrow_Text.gameObject.SetActive(false);
            Play_Nub.text = DataManager.getCurrentLottery() + "次";
        }

        m_Lock.Find("Lock_Text").GetComponent<Text>().text = "观看视频(" + DataManager.getShowVideoNum() + "/3)";
    }

    /// <summary>按钮点击事件</summary>
    protected override void OnClick(Transform _target)
    {
        switch (_target.name)
        {
            case "Btn_Close":
                Close();
                break;
            case "Btn_Play":
                if (DataManager.getShowVideoNum() >= 3 && DataManager.getCurrentLottery() <= 0 &&
                    DataManager.getMergaRedNum() < 30 && GameObject.Find("Tips").transform.childCount < 1)
                {
                    LogicMgr.GetInstance.GetLogic<LogicTips>().AddTips("打开红包抽取手机碎片");
                }

                if (DataManager.getCurrentLottery() > 0 && Play_Open)
                {
                    //2048中的埋点
                    //StatisticUtil.sendEvent("phone_click_inside");
                    DataManager.addLottery(-1);
                    Refresh();
                    StartPrzie();
                }
                else if (Play_Open && DataManager.getShowVideoNum() < 3)
                {
#if UNITY_EDITOR
                    DataManager.addShowVideoNum(1);
                    StartPrzie();
                    Refresh();

#else
                     AdManager.Ins.showRewardAd(AD =>
                    {
                        //ThreadManager.Instance.runOnMainThread(() =>
                        //{
                        //    DataManager.addShowVideoNum(1);
                        //    StartPrzie();
                        //    Refresh();
                        //});
                       
                    });
#endif
                }

                if (DataManager.getShowVideoNum() >= 3 && DataManager.getCurrentLottery() <= 0 &&
                    DataManager.getMergaRedNum() < 30 && GameObject.Find("Tips").transform.childCount < 1)
                {
                    LogicMgr.GetInstance.GetLogic<LogicTips>().AddTips("合成红包抽取手机碎片");
                }

                //Debug.LogError("点击抽奖" + DataManager.getCurrentLottery());
                // StartPrzie();
                break;
            case "Btn_Shippingaddress":

                if (GameObject.Find("Tips").transform.childCount < 1)
                {
                    LogicMgr.GetInstance.GetLogic<LogicTips>().AddTips("集齐10片碎片即可赢取P40手机");
                }

                //填写收货地址
                break;
            case "Btn_Lock":
                //看视频任务
                //if(AdManager.getShowVideoNum()>3)
                if (DataManager.getShowVideoNum() < 3)
                {
#if UNITY_EDITOR
                    DataManager.addLottery(1);
                    DataManager.addShowVideoNum(1);
                    Refresh();
                    HttpManager.Instance.LotteryData();
#else
                    // AdManager.Ins.showRewardAd(AD =>
                    //{
                    //    ThreadManager.Instance.runOnMainThread(() =>
                    //    {
                    //        //DataManager.add(1);
                    //        DataManager.addLottery(1);
                    //        DataManager.addShowVideoNum(1);
                    //        // getCurrentLottery
                    //        Refresh();
                    //        HttpManager.Instance.LotteryData();
                    //        LogicMgr.GetInstance.GetLogic<LogicTips>().AddTips("已增加一次抽奖机会");
                            
                    //    });
                        
                    //});
#endif
                }

                break;
            case "Btn_Synthesis":
                //if (DataManager.getMergaRedNum() >= 0 && DataManager.getMergaRedNum() % 10 == 0)
                //{

                //    DataManager.addRedMergaLotteryNum(1);
                //}

                if (DataManager.getMergaRedNum() / 10 <= 3 && DataManager.getRedMergaLotteryNum() > 0)
                {
                    DataManager.addRedMergaLotteryNum(-1);
                    DataManager.addLottery(1);
                    LogicMgr.GetInstance.GetLogic<LogicTips>().AddTips("已增加一次抽奖机会");
                    Refresh();
                }
                else
                {
                    if (GameObject.Find("Tips").transform.childCount < 1)
                    {
                        LogicMgr.GetInstance.GetLogic<LogicTips>().AddTips("合成红包抽取手机碎片");
                    }
                }

                //if (20 / 10 <= 3 && 20 % 10 == 0)
                //{
                //    DataManager.addLottery(1);
                //    Refresh();
                //}

                //合成红包任务
                break;
            case "Btn_Prize_Close":
                Prize_Image.gameObject.SetActive(false);
                break;
            case "Btn_Prize_Continue":
                Prize_Image.gameObject.SetActive(false);
                break;
            case "Btn_30":
                DataManager.addMergaRedNum(30);
                //DataManager.addRedMergaLotteryNum(3);
                Refresh();
                break;
            case "Btn_QK":
                DataManager.setCurrentLottery(0);
                DataManager.setCurrentDebris(0);
                DataManager.setMergaRedNum(0);
                DataManager.setShowVideoNum(0);
                break;
        }
    }

    #endregion
}