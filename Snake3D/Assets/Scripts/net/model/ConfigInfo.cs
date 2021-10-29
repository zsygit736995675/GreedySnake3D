using System;
using System.Collections.Generic;

[Serializable]
public class ConfigInfo {


    /// <summary>
    /// 红包弹插屏关闭次数
    /// </summary>
    public int hongbao_dialog_close_show_inter { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int cid { get; set; }




    ///---------------------------------------------------------------------以下废弃
    /// <summary>
    /// 红包1 2 3间隔时间
    /// </summary>
    public int [] red_duration { get; set; }

    /// <summary>
    /// 判断是否开启版本控制
    /// </summary>
    public string app_code { get; set; }
    /// <summary>
    /// 判断是否开启渠道控制
    /// </summary>
    public string app_channel { get; set; }
    /// <summary>
    /// c控制关闭开红包界面X次数弹插屏
    /// </summary>
    public int closenum { get; set; }

    /// <summary>
    /// 关闭分红页面x此弹插屏
    /// </summary>
    public int bonus_close { get; set; }

    public int red_produce { get; set; }

    /// <summary>
    /// 要出现第五条鱼的数量
    /// </summary>
    public int[] newFiveFishNumber1 { get; set; }
    public int[] newFiveFishNumber2 { get; set; }
    public int[] newFiveFishNumber3 { get; set; }
    public int[] newFiveFishNumber4 { get; set; }

    /// <summary>
    /// 玩家可提现需要的五彩锦鲤的数量
    /// </summary>
    public int [] withDrowFishNumber = { 5 , 8 };

    /// <summary>
    /// 悬浮红包刷新时间区间值
    /// </summary>
    public int[] reddisappear { get; set; }

    /// <summary>
    /// 每天可以召唤锦鲤的数量
    /// </summary>
    public int carpCallTimes { get; set; }

    /// <summary>
    /// 任务道具数量
    /// </summary>
    public List<PlanProps> planPropNumber = new List<PlanProps>();

    /// <summary>
    /// 面额配置 (集碎片功能)
    /// </summary>
    public List<FaceValue> FaceValueCro = new List<FaceValue>();

    /// <summary>
    /// 轴事件关闭X次弹插屏
    ///1超级红包 2现金拼图 3转盘 4红包雨 5存钱罐
    /// </summary>
    public int[] axis { get; set; }
}


///// <summary>
///// c控制关闭开红包界面X次数弹插屏
///// </summary>
//public int closenum;
///// <summary>
///// b祈福功能单次奖励的金币区间值
///// </summary>
//public int [ ] blessinggoldcoin;
///// <summary>
///// g游戏关卡胜利后RV下一关奖励的金币数量
///// </summary>
//public int gamevictory;
///// <summary>
///// g游戏关卡胜利后X次下一关弹出插屏
///// </summary>
//public int gamevictorycp;
///// <summary>
///// g关卡失败后，体力值-1界面点击OK选项第X次时弹出的插屏广告;
///// </summary>
//public int gamefailure;
///// <summary>
///// r悬浮红包消失时间
///// </summary>
//public int reddisappear;
///// <summary>
///// 按合成次数打开红包
///// </summary>
//public int OpenRedIndex;
///// <summary>
///// 判断是否开启渠道控制
///// </summary>
//public string app_channel;
///// <summary>
///// 判断是否开启版本控制
///// </summary>
//public string app_version;