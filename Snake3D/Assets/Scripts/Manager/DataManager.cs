﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using AppsFlyerSDK;
using UnityEngine.UI;

using LitJson;
using TMPro;
using UnityEngine;

public class DataManager:SingletonObject<DataManager>
{
    private static string BaseTag = "_snake";

    private static string KEY_MONEY = "key_current_money"+ BaseTag;

    private static string KEY_LOTTERY = "key_current_lottery" + BaseTag;

    private static string KEY_DEBRIS = "key_current_Debris" + BaseTag;

    private static string KEY_MERGAREDNUM = "key_Merga_RedNum" + BaseTag;

    private static string KEY_MERGALOTTERYREDNUM = "key_Merga_Lottery_RedNum" + BaseTag;

    private static string KEY_SHOWVIDEO = "KEY_ShowVideo" + BaseTag;

    private static string KEY_RED_BAG_GIVE_UP = "key_red_bag_give_up" + BaseTag;

    private static string KEY_MAX_SCORE = "key_max_score" + BaseTag; //最高积分
    private static string KEY_MAX_LEVEL = "key_max_level" + BaseTag; //最高等级

    private static string KEY_SCORE = "key_score" + BaseTag; //当前积分

    private static string KEY_Date = "key_date" + BaseTag; //当天在线红包数据

    private static string KEY_CREATE_TIME = "key_create_time" + BaseTag;

    private static string KEY_LOGIN_TIME = "key_login_time" + BaseTag;

    private static string KEY_RV_COUNT = "key_rv_count" + BaseTag;

    private static string KEY_COMPOSITE_COUNT = "key_composite_count" + BaseTag;

    private static string KEY_LEVEL_NUN = "key_level_nun" + BaseTag;

    private static string KEY_INTER_COUNT = "key_inter_count" + BaseTag;

    private static string KEY_TWO_OPEN = "KEY_TWO_OPEN" + BaseTag;

    private static string KEY_NAT = "KEY_NAT" + BaseTag;

    private static string KEY_HAS_CASH = "key_has_cash" + BaseTag;

    private static string KEY_ACTIVITY_DAY = "key_activity_day" + BaseTag;

    private static string KEY_DAILY_RED_TIME = "key_daily_red_time" + BaseTag;

    private static string KEY_BGM = "key_bgm" + BaseTag;//背景音开关

    private static string KEY_SOUND = "key_sound" + BaseTag;//音效开关

    private static string AMOUNT = "amount" + BaseTag;//当前用户当前提现档次

    private static string KEY_RED_INT = "key_red_int" + BaseTag;

    private static string KEY_SHOW_ALI = "key_show_ali" + BaseTag;

    private static string KEY_USER_FIVE_FLSH = "key_user_five_flsh" + BaseTag; //当前用户所拥有的所有鱼

    private static string KEY_COMMON_FLSH_NUMBER = "key_common_flsh_number" + BaseTag; //当前用户刷到了第几条鱼

    private static string KEY_WUCAI_FLSH_NUMBER = "key_wucai_flsh_number" + BaseTag; //当前用户刷到的五彩鱼的数量

    private static string KEY_SEVEN_FLSH_COUNT1 = "key_seven_flsh_count1" + BaseTag; //第七条鱼刷出的数组坐标 (档位一)
    private static string KEY_SEVEN_FLSH_COUNT2 = "key_seven_flsh_count2" + BaseTag; //第七条鱼刷出的数组坐标 (档位二)
    private static string KEY_SEVEN_FLSH_COUNT3 = "key_seven_flsh_count3" + BaseTag; //第七条鱼刷出的数组坐标 (档位三)
    private static string KEY_SEVEN_FLSH_COUNT4 = "key_seven_flsh_count4" + BaseTag; //第七条鱼刷出的数组坐标 (档位四)

    private static string KEY_PRIZE_POOL_REFRESH_TIME = "key_prize_pool_refresh_time" + BaseTag; //分红奖池刷新时间记录

    private static string KEY_POOL_NUMBER = "key_pool_number" + BaseTag; //分红奖池number

    private static string KEY_START_COUNT = "key_start_count" + BaseTag;//进游戏的次数

    private static string KEY_LOGIN_DAY_NUM = "key_login_day_num" + BaseTag;//登录天数

    private static string KEY_CURRENT_CALL_NUM = "key_currentday_callnum" + BaseTag;//当天召唤次数

    private static string KEY_PLAN_PROPS = "key_plan_props" + BaseTag;//进度条任务道具

    private static string KEY_PLAN_COUNT = "key_plan_count" + BaseTag; //当前走到的任务进度下标

    private static string KEY_PLAN_lIST_COUNT = "key_plan_list_count" + BaseTag; //当前走到的任务进度表下标

    private static string KEY_USER_1_MONEY_FACEVALUE = "key_user_1_money_facevalue" + BaseTag;   //当前用户指定的钱碎片
    private static string KEY_USER_5_MONEY_FACEVALUE = "key_user_5_money_facevalue" + BaseTag;
    private static string KEY_USER_10_MONEY_FACEVALUE = "key_user_10_money_facevalue" + BaseTag;
    private static string KEY_USER_20_MONEY_FACEVALUE = "key_user_20_money_facevalue" + BaseTag;
    private static string KEY_USER_50_MONEY_FACEVALUE = "key_user_50_money_facevalue" + BaseTag;
    private static string KEY_USER_100_MONEY_FACEVALUE = "key_user_100_money_facevalue" + BaseTag;

    private static string KEY_CHIP_HONGDIAN_IS_ON = "key_chip_hongdian_is_on" + BaseTag; //红点是否打开
    /// <summary>
    /// 关闭次数
    /// </summary>
    public int closeNum = 0;

    /// <summary>
    /// 超级红包关闭次数
    /// </summary>
    public int supercloseNum = 0;

    /// <summary>
    ///  红包雨红包页关闭次数
    /// </summary>
    public int hbycloseNum = 0;

    /// <summary>
    /// 分红红包关闭次数
    /// </summary>
    public int fhCloseNum = 0;
    /// <summary>
    /// 金猪关闭次数
    /// </summary>
    public int jzCloseNum = 0;

    /// <summary>
    /// 获取碎片关闭次数
    /// </summary>
    public int cashChipNum = 0;

    /// <summary>
    /// 获取碎片关闭次数
    /// </summary>
    public int zhuanpanCloseNum = 0;

    public static   bool  Bgm_Set 
    {
        get 
        {
           return bool.Parse(  PlayerPrefs.GetString(KEY_BGM,"true"));
        }

        set 
        {
            PlayerPrefs.SetString(KEY_BGM, value.ToString());
        }
    }

    public static bool Sound_Set
    {
        get
        {
            return bool.Parse(PlayerPrefs.GetString(KEY_SOUND, "true"));
        }

        set
        {
            PlayerPrefs.SetString(KEY_SOUND, value.ToString());
        }
    }

    /// <summary>
    /// 获取当前的红点
    /// </summary>
    /// <returns></returns>
    public static int getChipHongDianIsOn()
    {
        return PlayerPrefs.GetInt(KEY_CHIP_HONGDIAN_IS_ON,1);
    }

    //增加用户的随机面额碎片
    public static List<int> addUserFaceValue()
    {
        //红点控制
        PlayerPrefs.SetInt(KEY_CHIP_HONGDIAN_IS_ON, 0);
        ConfigInfo config = HttpDataManager.GetConfigInfo();
        //1.随机面额
        int range1 = UnityEngine.Random.Range(0,config.FaceValueCro.Count);
        int mianE = config.FaceValueCro[range1].cost; //面额
        //如果是第一次必得最大面值碎片
        if (PlayerPrefs.GetInt("firstAddUserFaceValue", 0) == 0)
        {
            PlayerPrefs.SetInt("firstAddUserFaceValue", 1);
            range1 = 5;
            mianE = config.FaceValueCro[config.FaceValueCro.Count-1].cost;
        }
        //2.随机碎片
        int range2 = 0;
        while (true)
        {
            range2 = UnityEngine.Random.Range(0,config.FaceValueCro[range1].chipNumber);
            if(range2 != (config.FaceValueCro[range1].unGetNumber - 1))
            {
                break;
            }
        }
        //拿到当前面值的所有碎片
        List<int> chips = getUserMoneyFaceValue(config.FaceValueCro[range1].cost);
        //增加碎片
        chips[range2]++;
        //重新封装
        string setMoneyChip = listIntToString(chips);
        switch (mianE)
        {
            case 1:
                PlayerPrefs.SetString(KEY_USER_1_MONEY_FACEVALUE, setMoneyChip);
                break;
            case 5:
                PlayerPrefs.SetString(KEY_USER_5_MONEY_FACEVALUE, setMoneyChip);
                break;
            case 10:
                PlayerPrefs.SetString(KEY_USER_10_MONEY_FACEVALUE, setMoneyChip);
                break;
            case 20:
                PlayerPrefs.SetString(KEY_USER_20_MONEY_FACEVALUE, setMoneyChip);
                break;
            case 50:
                PlayerPrefs.SetString(KEY_USER_50_MONEY_FACEVALUE, setMoneyChip);
                break;
            case 100:
                PlayerPrefs.SetString(KEY_USER_100_MONEY_FACEVALUE, setMoneyChip);
                break;
        }
        DotManager.Instance.userSet("user_money_fragment",1);//现金碎片数量打点
        List<int> reList = new List<int>();
        reList.Add(mianE);
        reList.Add(range2);
        return reList;
    }
    
    //获取当前用户指定的钱碎片
    public static List<int> getUserMoneyFaceValue(int money)
    {
        string moneyStr = "";
        switch (money)
        {
            case 1:
                moneyStr = PlayerPrefs.GetString(KEY_USER_1_MONEY_FACEVALUE, "0,0,0,0,0,0");
                break;
            case 5:
                moneyStr = PlayerPrefs.GetString(KEY_USER_5_MONEY_FACEVALUE, "0,0,0,0,0,0");
                break;
            case 10:
                moneyStr = PlayerPrefs.GetString(KEY_USER_10_MONEY_FACEVALUE, "0,0,0,0,0,0,0,0");
                break;
            case 20:
                moneyStr = PlayerPrefs.GetString(KEY_USER_20_MONEY_FACEVALUE, "0,0,0,0,0,0,0,0");
                break;
            case 50:
                moneyStr = PlayerPrefs.GetString(KEY_USER_50_MONEY_FACEVALUE, "0,0,0,0,0,0,0,0,0");
                break;
            case 100:
                moneyStr = PlayerPrefs.GetString(KEY_USER_100_MONEY_FACEVALUE, "0,0,0,0,0,0,0,0,0");
                break;
        }
        return stringToIntListForDouHao(moneyStr);
    }

    /// <summary>
    /// 增加任务表下标
    /// </summary>
    /// <returns></returns>
    public static void addPlanListCount()
    {
        if (getPlanListCount() >= HttpDataManager.GetConfigInfo().planPropNumber.Count-1)
        {
            PlayerPrefs.SetInt(KEY_PLAN_lIST_COUNT, 0);
        }
        else
        {
            PlayerPrefs.SetInt(KEY_PLAN_lIST_COUNT, (getPlanListCount() + 1));
        }
    }

    /// <summary>
    /// 获取当前走到的任务进度下标
    /// </summary>
    /// <returns></returns>
    public static int getPlanListCount()
    {
        return PlayerPrefs.GetInt(KEY_PLAN_lIST_COUNT,-1);
    }

    /// <summary>
    /// 增加任务下标
    /// </summary>
    /// <returns></returns>
    public static bool addPlanCount()
    {
        if (getPlanCount() >= 8)
        {
            PlayerPrefs.SetInt(KEY_PLAN_COUNT, 0);
            //重新分配任务
            refreshPlanProgress();
            return true;
        }
        else
        {
            PlayerPrefs.SetInt(KEY_PLAN_COUNT, (getPlanCount()+1));
            return false;
        }
    }

    /// <summary>
    /// 获取当前走到的任务进度下标
    /// </summary>
    /// <returns></returns>
    public static int getPlanCount()
    {
        return PlayerPrefs.GetInt(KEY_PLAN_COUNT, 0);
    }
    /// <summary>
    /// 获取当前所有的任务道具
    /// </summary>
    public static List<int> getPlanProgres()
    {
        List<int> props = new List<int>();
        if(string.IsNullOrEmpty(PlayerPrefs.GetString(KEY_PLAN_PROPS, "")))
        {
            refreshPlanProgress();
        }
        string[] strs = PlayerPrefs.GetString(KEY_PLAN_PROPS).Split(',');
        foreach (var item in strs)
        {
            props.Add(int.Parse(item));
        }
        return props;
    }

    /// <summary>
    /// 刷新任务进度
    /// </summary>
    public static void refreshPlanProgress()
    {
        //增加任务表下标
        addPlanListCount();
        //分配任务
        List<int> props = new List<int>();
        //出现道具数量
        int [] prop = HttpDataManager.GetConfigInfo().planPropNumber[getPlanListCount()].props;
        for (int i = 0; i < 8; i++)
        {
            props.Add(prop[i]);
        }
        props.Add(9);
        string keyPlanProps = "";
        for (int i = 0; i < props.Count; i++)
        {
            if (i >= (props.Count - 1))
                keyPlanProps += props[i];
            else
                keyPlanProps += props[i] + ",";
        }
        PlayerPrefs.SetString(KEY_PLAN_PROPS, keyPlanProps);
    }

    /// <summary>
    /// 启动次数
    /// </summary>
    public static int StartCount 
    {
        get { return PlayerPrefs.GetInt(KEY_START_COUNT,1); }

        set {
            int count = StartCount + value;
            PlayerPrefs.SetInt(KEY_START_COUNT, count);
            if (count == 3 || count == 5) 
            {
                DotManager.Instance.sendEvent("sessions_"+count, DottingType.Af);
            }
            DotManager.Instance.userSet("latest_total_logintimes", count);
            LoginDayNum = 1;
        }
    }

    /// <summary>
    /// 获取登录天数
    /// </summary>
    public static int LoginDayNum 
    {
        get 
        {
            string str = PlayerPrefs.GetString(KEY_LOGIN_DAY_NUM, "");
            if (string.IsNullOrEmpty(str)) 
            {
                LoginDayNum = 1;
                return 1;
            }
            string[] days = str.Split(',');

            return days.Length;
        }

        set
        {
            string str = PlayerPrefs.GetString(KEY_LOGIN_DAY_NUM, "");
            if (string.IsNullOrEmpty(str))
            {
                PlayerPrefs.SetString(KEY_LOGIN_DAY_NUM, System.DateTime.Now.ToString("yyyy-MM-dd"));
                return;
            }

            if (!str.Contains(System.DateTime.Now.ToString("yyyy-MM-dd"))) 
            {
                str += "," + System.DateTime.Now.ToString("yyyy-MM-dd");
                PlayerPrefs.SetString(KEY_LOGIN_DAY_NUM,str);

                string[] days = str.Split(',');
                if (days.Length == 2 || days.Length == 3 || days.Length == 7 || days.Length == 15 || days.Length == 30)
                {
                    DotManager.Instance.sendEvent("day" + days.Length + "_open", DottingType.Af);
                }
                DotManager.Instance.userSet("latest_total_logindays", days.Length);
            }
        }
    }


    public static string getPoolNumber()
    {
        return PlayerPrefs.GetString(KEY_POOL_NUMBER,"00000");
    }

    /// <summary>
    /// 记录分红奖池时间
    /// </summary>
    /// <returns></returns>
    public static void setPrizePoolTime(string time)
    {
        PlayerPrefs.SetString(KEY_PRIZE_POOL_REFRESH_TIME, time);
        int hour = DateTime.Now.Hour;
        int poolFloat = UnityEngine.Random.Range(10, 100);
        if (hour >= 0 && hour < 9)
        {
            int poolInt = UnityEngine.Random.Range(100,300);
            PlayerPrefs.SetString(KEY_POOL_NUMBER, poolInt + ""+poolFloat);
        }
        else if(hour >= 9 && hour < 12)
        {
            int poolInt = UnityEngine.Random.Range(300,600);
            PlayerPrefs.SetString(KEY_POOL_NUMBER, poolInt+""+ poolFloat);
        }
        else if (hour >= 12 && hour < 24)
        {
            int poolInt = UnityEngine.Random.Range(700,1000);
            PlayerPrefs.SetString(KEY_POOL_NUMBER, poolInt + "" + poolFloat);
        }
    }


    /// <summary>
    /// 获取分红奖池记录时间
    /// </summary>
    /// <returns></returns>
    public static string getPrizePoolTime()
    {
        return PlayerPrefs.GetString(KEY_PRIZE_POOL_REFRESH_TIME, 1 + "," + 1);
    }

    /// <summary>
    /// 当天召唤次数
    /// </summary>
    public static int currentDayCallNum 
    {
        get 
        {
            return PlayerPrefs.GetInt(KEY_CURRENT_CALL_NUM +":"+ System.DateTime.Now.ToString("yyyy-MM-dd"),0);
        }
        set 
        {
            PlayerPrefs.SetInt(KEY_CURRENT_CALL_NUM + ":" + System.DateTime.Now.ToString("yyyy-MM-dd"),value);
        }
    }

    /// <summary>
    /// 增加当前档次第七条鱼增加数组坐标
    /// </summary>
    public static void addSevenFlshCount()
    {
        switch (getAmount())
        {
            case 1:
                if (getSevenFlshCount() >= HttpDataManager.GetConfigInfo().newFiveFishNumber1.Count() - 1)
                {
                    PlayerPrefs.SetInt(KEY_SEVEN_FLSH_COUNT1, 0);
                }
                else
                {
                    PlayerPrefs.SetInt(KEY_SEVEN_FLSH_COUNT1, getSevenFlshCount() + 1);
                }
                break;
            case 2:
                if (getSevenFlshCount() >= HttpDataManager.GetConfigInfo().newFiveFishNumber2.Count() - 1)
                {
                    PlayerPrefs.SetInt(KEY_SEVEN_FLSH_COUNT2, 0);
                }
                else
                {
                    PlayerPrefs.SetInt(KEY_SEVEN_FLSH_COUNT2, getSevenFlshCount() + 1);
                }
                break;
            case 3:
                if (getSevenFlshCount() >= HttpDataManager.GetConfigInfo().newFiveFishNumber3.Count() - 1)
                {
                    PlayerPrefs.SetInt(KEY_SEVEN_FLSH_COUNT3, 0);
                }
                else
                {
                    PlayerPrefs.SetInt(KEY_SEVEN_FLSH_COUNT3, getSevenFlshCount() + 1);
                }
                break;
            case 4:
                if (getSevenFlshCount() >= HttpDataManager.GetConfigInfo().newFiveFishNumber4.Count() - 1)
                {
                    PlayerPrefs.SetInt(KEY_SEVEN_FLSH_COUNT4, 0);
                }
                else
                {
                    PlayerPrefs.SetInt(KEY_SEVEN_FLSH_COUNT4, getSevenFlshCount() + 1);
                }
                break;
        }
        
    }
    /// <summary>
    /// 获取第七条鱼刷出的数组坐标
    /// </summary>
    public static int getSevenFlshCount()
    {
        switch (getAmount())
        {
            case 1:
                return PlayerPrefs.GetInt(KEY_SEVEN_FLSH_COUNT1, 0);
            case 2:
                return PlayerPrefs.GetInt(KEY_SEVEN_FLSH_COUNT2, 0);
            case 3:
                return PlayerPrefs.GetInt(KEY_SEVEN_FLSH_COUNT3, 0);
            case 4:
                return PlayerPrefs.GetInt(KEY_SEVEN_FLSH_COUNT4, 0);
        }
        return PlayerPrefs.GetInt(KEY_SEVEN_FLSH_COUNT1, 0);
    }

    /// <summary>
    /// 减少五彩锦鲤
    /// </summary>
    public static void subWuCaiFlsh(int subCount)
    {
        PlayerPrefs.SetInt(KEY_WUCAI_FLSH_NUMBER, getWuCaiFlsh() - subCount);
    }


    /// <summary>
    /// 合成增加一条五彩锦鲤
    /// </summary>
    public static void addWuCaiFlsh()
    {
        string [] flshs = getUserFiveFlsh().Split(',');
        //每条普通鱼-1
        for (int i = 0; i < flshs.Length; i++)
        {
            flshs[i] = (int.Parse(flshs[i]) - 1).ToString();
        }
        string str = spellFlshNumber(flshs);
        //保存用户的每条鱼
        PlayerPrefs.SetString(KEY_USER_FIVE_FLSH, str);
        //保存五彩锦鲤
        PlayerPrefs.SetInt(KEY_WUCAI_FLSH_NUMBER,getWuCaiFlsh()+1);
    }
    /// <summary>
    /// 获取当前拥有多少条五彩锦鲤
    /// </summary>
    public static int getWuCaiFlsh()
    {
        return PlayerPrefs.GetInt(KEY_WUCAI_FLSH_NUMBER,0);
    }
    /// <summary>
    /// 增加普通鱼
    /// </summary>
    public static int addCommonFlsh(int [] newFiveFishNumber)
    {
        if (getCommonFlsh() < newFiveFishNumber[getSevenFlshCount()]) {
            int range = UnityEngine.Random.Range(0, 6);
            switch (range)
            {
                case 0:
                    splitSaveFlshNumber(0);
                    break;
                case 1:
                    splitSaveFlshNumber(1);
                    break;
                case 2:
                    splitSaveFlshNumber(2);
                    break;
                case 3:
                    splitSaveFlshNumber(3);
                    break;
                case 4:
                    splitSaveFlshNumber(4);
                    break;
                case 5:
                    splitSaveFlshNumber(5);
                    break;
            }
            PlayerPrefs.SetInt(KEY_COMMON_FLSH_NUMBER, getCommonFlsh() + 1);
            Debug.Log("增加普通鱼  次数 : "+ getCommonFlsh());
            return range + 1;
        }
        else
        {
            splitSaveFlshNumber(6);
            //刷出第七条鱼 归零
            PlayerPrefs.SetInt(KEY_COMMON_FLSH_NUMBER, 0);
            //增加当前档次第七条鱼增加数组坐标
            addSevenFlshCount();
            Debug.Log("刷出第七条鱼");
            return 7;
        }
    }
    public static void splitSaveFlshNumber(int count)
    {
        string[] flshNumber = getUserFiveFlsh().Split(',');
        int flsh = int.Parse(flshNumber[count]);
        flsh++;
        flshNumber[count] = flsh.ToString();
        string str = spellFlshNumber(flshNumber);
        PlayerPrefs.SetString(KEY_USER_FIVE_FLSH, str);
    }
    public static string spellFlshNumber(string [] flshNumber)
    {
        string str = "";
        for (int i = 0; i < flshNumber.Length; i++)
        {
            if (i == flshNumber.Length - 1)
            {
                str += flshNumber[i];
                break;
            }
            str += flshNumber[i] + ",";
        }
        return str;
    }
    /// <summary>
    /// 获取当前刷到第多少条普通鱼
    /// </summary>
    /// <returns></returns>
    public static int getCommonFlsh()
    {
        return PlayerPrefs.GetInt(KEY_COMMON_FLSH_NUMBER, 0);
    }

    public static string getUserFiveFlsh()
    {
        return PlayerPrefs.GetString(KEY_USER_FIVE_FLSH, "0,0,0,0,0,0,0");
    }
        

    /// <summary>
    /// 获取当前用户提现档次
    /// </summary>
    public static int getAmount()
    {
        return PlayerPrefs.GetInt(AMOUNT,1);
    }

    /// <summary>
    /// 设置当前用户提现档次
    /// </summary>
    /// <returns></returns>
    public static void setAmount(int amount)
    {
        PlayerPrefs.SetInt(AMOUNT,amount);
    }

    public static void saveActivityDay()
    {
        PlayerPrefs.SetString(KEY_ACTIVITY_DAY, DateTime.Now.ToString());
    }

    public static DateTime getActivityDay()
    {
        return DateTime.Parse(PlayerPrefs.GetString(KEY_ACTIVITY_DAY, DateTime.Now.ToString()));
    }

    public static void setHasCash()
    {
        PlayerPrefs.SetInt(KEY_HAS_CASH, 1);
    }

    public static bool isHasCash()
    {
        return PlayerPrefs.GetInt(KEY_HAS_CASH, 0) == 1;
    }

    public static int getGravityNum
    {
        get { return PlayerPrefs.GetInt("GravityNum", 3); }
    }

    public static void setGravityNum(bool isDefult = false)
    {
        PlayerPrefs.SetInt("GravityNum", isDefult ? 3 : PlayerPrefs.GetInt("GravityNum", 3) - 1);
    }

    public static int getUniversalNum
    {
        get { return PlayerPrefs.GetInt("UniversalNum", 3); }
    }

    public static void setUniversalNum(bool isDefult = false)
    {
        PlayerPrefs.SetInt("UniversalNum", isDefult ? 3 : PlayerPrefs.GetInt("UniversalNum", 3) - 1);
    }

    //当前金钱数
    public static float getCurrentMoney()
    {
        return float.Parse(PlayerPrefs.GetFloat(KEY_MONEY, 0f).ToString("F2"));
    }

    //设置金钱数
    public static void setCurrentMoney(float money)
    {
        PlayerPrefs.SetFloat(KEY_MONEY, money);

        EventMgrHelper.Ins.PushEvent(EventDef.DataUpdate);
    }

    //当前积分
    public static int getCurrentScore()
    {
        return PlayerPrefs.GetInt(KEY_SCORE, 0);
    }

    //设置积分
    public static void setCurrentScore(int score)
    {
        PlayerPrefs.SetInt(KEY_SCORE, score);
        EventMgrHelper.Ins.PushEvent(EventDef.DataUpdate);
    }

    public static void addScore1(int score)
    {
        addScoreNum += score;
        PlayerPrefs.SetInt(KEY_SCORE, getCurrentScore() + score);
        DotManager.Instance.userSet("integral_Suspension_number", 1);
    }

        static int addScoreNum = 0;
    //添加积分
    public static void addScore(int score)
    {
        addScoreNum += score;
        PlayerPrefs.SetInt(KEY_SCORE, getCurrentScore() + score);
        EventMgrHelper.Ins.PushEvent(EventDef.DataUpdate);
        //用户积分打点
        DotManager.Instance.userSet("integral_Suspension_number",1);
    }

    /// <summary>
    /// 同步积分
    /// </summary>
    public static void SynScore() 
    {
        HttpManager.Instance.addScore(addScoreNum);
        addScoreNum = 0;
    }

    public static int getRedint()
    {
        // Debug.LogError("----->" + PlayerPrefs.GetInt(KEY_RED_INT,0));
        return PlayerPrefs.GetInt(KEY_RED_INT);
    }
    public static void setRedint(int n)
    {
        PlayerPrefs.SetInt(KEY_RED_INT, n);
    }

    /// <summary>
    /// 合并红包后获得机会
    /// </summary>
    /// <returns></returns>
    public static int getRedMergaLotteryNum()
    {
        return PlayerPrefs.GetInt(KEY_MERGALOTTERYREDNUM, 0);
    }

    public static void setRedMergaLotteryNum(int num)
    {
        PlayerPrefs.SetInt(KEY_MERGALOTTERYREDNUM, num);
    }

    public static void addRedMergaLotteryNum(int num)
    {
        int m = getRedMergaLotteryNum() + num;
        PlayerPrefs.SetInt(KEY_MERGALOTTERYREDNUM, m);

    }

    public static void addMoney(float money)
    {
        float m = float.Parse((getCurrentMoney() + money).ToString("F2"));
       // m = 50;
        setCurrentMoney(m);

        //MoneyManager.sendChange( getCurrentMoney( ) );
        DotManager.Instance.userSet("money_number", getCurrentMoney());
        DotManager.Instance.sendEvent("money_number",DottingType.Tga, new Dictionary<string, object> { { "money_number", m } });
        DotManager.Instance.sendEvent("coin_number", DottingType.Af, new Dictionary<string, object> { { "money_number", m } });

        bool tb = money > 0 && HttpDataManager.isCashOut();
        if (m > 0)
        {
            HttpManager.Instance.SynData(tb ? 1 : 0);
        }

        if (m > 100 && !HttpDataManager.isCashOut())
        {
            DateTime last = DataManager.getActivityDay();
            GregorianCalendar gc = new GregorianCalendar();
            int lastDayOfYear = gc.GetDayOfYear(last);
            int currentDay = gc.GetDayOfYear(DateTime.Now);
            if (currentDay - lastDayOfYear <= 1)
            {
                DotManager.Instance.userSet("User_first_signin_date", last);
            }
            else
            {
                DotManager.Instance.userSet("User_first_signin_date", DateTime.Now);
            }
        }
    }

    public static DateInfo GetCurrentDayOnlineRed()
    {
       // PlayerPrefs.SetString(KEY_Date, "");
        string json = PlayerPrefs.GetString(KEY_Date, "");
        DateTime NowTime = DateTime.Now.ToLocalTime();
        string date = NowTime.ToString("yyyy-MM-dd");
        LogUtil.Log(json, "GetCurrentDayOnlineRed");
        if (!string.IsNullOrEmpty(json))
        {
            DateInfo info = JsonMapper.ToObject<DateInfo>(json);
            if (info.date == date)
            {
                return info;
            }
            else 
            {
                return CreateDateInfo(date);
            }
        }
        else 
        {
            return CreateDateInfo(date);
        }
    }

    static DateInfo CreateDateInfo(string da) 
    {
        DateInfo info = new DateInfo();
        info.date = da;
        info.isEnd = false;
        info.data = new List<DataItem>();
        ConfigInfo config = HttpDataManager.GetConfigInfo();
        if (config != null) 
        {
            Debug.Log("dateInof: !=null");
            for (int i = 0; i < config.red_duration.Length; i++)
            {
                DataItem item = new DataItem();
                item.num = i;
                item.isreceive = false;
                item.timer = config.red_duration[i] * 60;
                item.nearTime = config.red_duration[i] * 60;
                info.data.Add(item);
            }
        }
        
        SetCurrentDayOnlineRed(info);
        return info;
    }

    public static void SetCurrentDayOnlineRed(DateInfo date) 
    {
        string json = JsonMapper.ToJson(date);
        PlayerPrefs.SetString(KEY_Date, json);
    }

    /// <summary>
    /// 获取抽奖机会数
    /// </summary>
    /// <returns></returns>
    public static int getCurrentLottery()
    {
        return PlayerPrefs.GetInt(KEY_LOTTERY, 1);
    }

    public static void setCurrentLottery(int lotterynum)
    {
        PlayerPrefs.SetInt(KEY_LOTTERY, lotterynum);
    }

    public static void addLottery(int lotterynum)
    {
        int m = getCurrentLottery() + lotterynum;
        PlayerPrefs.SetInt(KEY_LOTTERY, m);
        ///HttpClient.Instance.LotteryData();
    }

    /// <summary>
    /// 获取手机碎片数
    /// </summary>
    /// <returns></returns>
    public static float getCurrentDebris()
    {
        return float.Parse(PlayerPrefs.GetFloat(KEY_DEBRIS, 0f).ToString("F1"));
    }

    public static void setCurrentDebris(float debris)
    {
        PlayerPrefs.SetFloat(KEY_DEBRIS, debris);
    }

    public static void addDebris(float debris)
    {
        float m = float.Parse((getCurrentDebris() + debris).ToString("F1"));
        PlayerPrefs.SetFloat(KEY_DEBRIS, m);
        //HttpClient.Instance.LotteryData();
        DotManager.Instance.addDebris(debris);
    }

    /// <summary>
    /// 获取今日红包数 (隔日刷新)
    /// </summary>
    /// <returns></returns>
    public static int getMergaRedNum()
    {
        return PlayerPrefs.GetInt(KEY_MERGAREDNUM, 1);
    }

    public static void setMergaRedNum(int num)
    {
        PlayerPrefs.SetInt(KEY_MERGAREDNUM, num);
    }

    public static void addMergaRedNum(int num)
    {
        int m = getMergaRedNum() + num;
        PlayerPrefs.SetInt(KEY_MERGAREDNUM, m);
        //HttpClient.Instance.LotteryData();
    }

    /// <summary>
    /// 获取今日观看视频数
    /// </summary>
    /// <returns></returns>
    public static int getShowVideoNum()
    {
        return PlayerPrefs.GetInt(KEY_SHOWVIDEO, 0);
    }

    public static void setShowVideoNum(int num)
    {
        PlayerPrefs.SetInt(KEY_SHOWVIDEO, num);
    }

    public static void addShowVideoNum(int num)
    {
        int m = getShowVideoNum() + num;
        PlayerPrefs.SetInt(KEY_SHOWVIDEO, m);
        //HttpClient.Instance.LotteryData();
    }

    public static void resetGiveUpNum()
    {
        PlayerPrefs.SetInt(KEY_RED_BAG_GIVE_UP, 0);
        PlayerPrefs.SetInt(KEY_LEVEL_NUN, 0);
    }

    public static void addGiveUpNum()
    {
        PlayerPrefs.SetInt(KEY_RED_BAG_GIVE_UP, getGiveUpNum() + 1);
    }

    public static int getGiveUpNum()
    {
        return PlayerPrefs.GetInt(KEY_RED_BAG_GIVE_UP, 0);
    }

    public static void addLevelNum()
    {
        PlayerPrefs.SetInt(KEY_LEVEL_NUN, getLevelNum() + 1);
    }

    public static void SetLevelNum(int value)
    {
        PlayerPrefs.SetInt(KEY_LEVEL_NUN, value);
    }

    public static int getShowAli()
    {
        return PlayerPrefs.GetInt(KEY_SHOW_ALI, 0);
    }

    public static void setShowAli(int show)
    {
        PlayerPrefs.SetInt(KEY_SHOW_ALI, show);
    }

    public static int getLevelNum()
    {
        int level = PlayerPrefs.GetInt(KEY_LEVEL_NUN, 1);
        if (level < 1)
            level = 1;
        return level;
    }


    public static void setCreateTime()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(KEY_CREATE_TIME)))
        {
            PlayerPrefs.SetString(KEY_CREATE_TIME, DateTime.Now.ToString());

            DotManager.Instance.sendEvent("Reg",DottingType.Tga);
            DotManager.Instance.sendEvent("first_login", DottingType.Af);
            DotManager.Instance.UserSetOnce("user_os", "Android");
            DotManager.Instance.UserSetOnce("user_channel", "toutiao");
            DotManager.Instance.UserSetOnce("uer_IMEI", AndroidSend.getNative("imei"));

            DotManager.Instance.sendCreateTime();
        }
    }

    public static string getCreateTime()
    {
        return PlayerPrefs.GetString(KEY_CREATE_TIME, DateTime.Now.ToString());
    }

    /// <summary>
    /// 获取当天的 每日红包是否领取
    /// </summary>
    public static bool getDailyRedTime() 
    {
        DateTime NowTime = DateTime.Now.ToLocalTime();
        string date = NowTime.ToString("yyyy-MM-dd");
        //首次登录不领取
        if (isFirstLogin)
        {
            PlayerPrefs.SetString(KEY_DAILY_RED_TIME, date);
            return false;
        }
        else 
        {
            //第一天登录也不领取因为有新人红包
            string createTime = getCreateTime();
            DateTime dateTime = DateTime.Parse(createTime);
            string current = dateTime.ToString("yyyy-MM-dd");

            if (current.Equals(date))
            {
                return false;
            }
            else 
            {
                string lastDate = PlayerPrefs.GetString(KEY_DAILY_RED_TIME);

                if (lastDate.Equals(date))
                {
                    return false;
                }
                else
                {
                    PlayerPrefs.SetString(KEY_DAILY_RED_TIME, date);
                    return true;
                }
            } 
        }
    }

    public static void setLoginTime()
    {
        PlayerPrefs.SetString(KEY_LOGIN_TIME, DateTime.Now.ToString());
        //Debug.LogError("KEY_LOGIN_TIME>>" + PlayerPrefs.GetString(KEY_LOGIN_TIME));
        //如果当前登录时间比注册时间多一天 就上传
        //LogUtil.D("ttt", ((TimeSpan) (DateTime.Parse(getLoginTime()) - DateTime.Parse(getCreateTime()))).Days);
        if (PlayerPrefs.GetInt(KEY_TWO_OPEN, 0) == 1)
        {
            return;
        }

        try
        {
            GregorianCalendar gc = new GregorianCalendar();
            int loginDay = gc.GetDayOfYear(DateTime.Parse(getLoginTime()));
            int createDay = gc.GetDayOfYear(DateTime.Parse(getCreateTime()));
            if (loginDay - createDay == 1)
            {
                DotManager.Instance.sendEvent("2day_login",DottingType.Af);
                LogUtil.Error("Day2Open", "Day2Open");
                PlayerPrefs.SetInt(KEY_TWO_OPEN, 1);
            }

            if (loginDay - createDay >= 1)
            {
                setMergaRedNum(0);
                setShowVideoNum(0);
                setRedMergaLotteryNum(0);
            }
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// 首次登陆
    /// </summary>
    public static bool isFirstLogin
    {
        get
        {
            TimeSpan gc = DateTime.Parse(getLoginTime()).Subtract(DateTime.Parse(getCreateTime()));
#if UNITY_EDITOR
            return true;
#else
            return gc.TotalSeconds <= 1;
#endif
        }
    }

    public static string getLoginTime()
    {
        return PlayerPrefs.GetString(KEY_LOGIN_TIME, DateTime.Now.ToString());
    }

    public static void addRv()
    {
        PlayerPrefs.SetInt(KEY_RV_COUNT, getRvCount() + 1);
        if (getRvCount() == 10 || getRvCount() == 5 || getRvCount() == 20  || getRvCount() == 30)
        {
            DotManager.Instance.sendEvent("adRVshow_over" + getRvCount(), DottingType.Af);
        }
       
    }

    public static int getRvCount()
    {
        return PlayerPrefs.GetInt(KEY_RV_COUNT, 0);
    }

    public static void addInter()
    {
        PlayerPrefs.SetInt(KEY_INTER_COUNT, getInterCount() + 1);
        //if ( getInterCount( ) == 10 || getInterCount( ) == 20 || getInterCount( ) == 30 )
        //{
        //    DotManager.Instance.sendEvent( "adFSVshow_over_" + getInterCount( ) );
        //}
    }

    public static int getInterCount()
    {
        return PlayerPrefs.GetInt(KEY_INTER_COUNT, 0);
    }


    public static void SetOrganic(bool isNat)
    {
        PlayerPrefs.SetInt(KEY_NAT, isNat ? 1 : 0);
    }

    /// <summary>
    /// false 显示广告  true不显示广告
    /// </summary>
    /// <returns></returns>
    public static bool IsOrganic()
    {
        //#if UNITY_EDITOR
        //        return false;
        //#endif
        //return PlayerPrefs.GetInt(KEY_NAT, 1) == 1;
        return isOrganic;
    }


    private static bool isOrganic
    {
        get
        {
            // if (HttpDataManager.GetConfigInfo().app_channel == Config.Instance.channelType.ToString()
            //     && HttpDataManager.GetConfigInfo().app_version == Application.version)
            //     return true;
            return false;
        }
    }




    public static int getCompositeBall()
    {
        return PlayerPrefs.GetInt(KEY_COMPOSITE_COUNT, 0);
    }

    /// <summary>
    /// string for ',' To return List Int
    /// </summary>
    /// <param name="listInt"></param>
    /// <returns></returns>
    public static List<int> stringToIntListForDouHao(string str)
    {
        string[] strs = str.Split(',');
        List<int> reList = new List<int>(); 
        for (int i = 0; i < strs.Length; i++)
        {
            reList.Add(int.Parse(strs[i]));
        }
        return reList;
    }

    /// <summary>
    /// 清子
    /// </summary>
    /// <param name="transform"></param>
    public static void clearChildrens(Transform transform)
    {
        int childCount = transform.childCount;
        if (childCount > 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// ListInt To String On 1,1,1
    /// </summary>
    /// <param name="listInt"></param>
    /// <returns></returns>
    public static string listIntToString(List<int> listInt)
    {
        string str = "";
        for (int i = 0; i < listInt.Count; i++)
        {
            if (i >= listInt.Count - 1)
            {
                str += listInt[i];
                break;
            }
            str += listInt[i] + ",";
        }
        return str;
    }

    /// <summary>
    /// 通过面额来确定Resources路径
    /// </summary>
    /// <param name="money"></param>
    /// <returns></returns>
    public static string getChipResourcesName(int money)
    {
        string str = "";
        switch (money)
        {
            case 1:
                str = "Game/yiyuan/";
                break;
            case 5:
                str = "Game/wuyuan/";
                break;
            case 10:
                str = "Game/shiyuan/";
                break;
            case 20:
                str = "Game/ershiyuan/";
                break;
            case 50:
                str = "Game/wushiyuan/";
                break;
            case 100:
                str = "Game/yibaiyuan/";
                break;
        }
        return str;
    }

    /// <summary>
    /// 切换shader
    /// </summary>
    static Material material = null;
    public static void ImageGray(Image image, bool isgray)
    {
        //"UISprites/DefaultGray"
        if (isgray)
        {
            if (material == null)
            {
                material = Resources.Load<Material>("Material/Gray");
            }

            if (material == null)
            {
                Debug.LogError("ImageGray material null");
                return;
            }

            image.material = material;
            image.SetMaterialDirty();
        }
        else
        {
            image.material = null;
        }
    }
}