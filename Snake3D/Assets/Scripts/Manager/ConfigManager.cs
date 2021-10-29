using System;

public class ConfigManager
{
    public const int NUM_50 = 50;
    public const int NUM_100 = 100;
    public const int NUM_120 = 120;
    public const int NUM_150 = 150;

    /// <summary>
    /// 最大关卡
    /// </summary>
    public static int Max_Level = 105;

    /// <summary>
    /// 测试用是否弹广告
    /// </summary>
    public static bool isAllowAd=true;

    //推广数据
    public static AfModel afModel;

    //提现档位
    public static int current_money = NUM_50;

    public static int withdraw_money = NUM_50;
    
    //能否提现状态
    public static bool withdraw_status = false;

    /// <summary>
    // 实时的档次,审核中就会变
    /// </summary>
    public static int real_Money = 100;

    /// <summary>
    /// 是否出财运亨通
    /// </summary>
    public static bool isShowCyht=false;


    /// <summary>
    /// 邮箱
    /// </summary>
    public static string exmail = "yihuachengjin@163.com";

    /// <summary>
    /// 隐私协议 / 用户协议
    /// </summary>
    public static string ysxy = "https://d1hz5n99bypoef.cloudfront.net/yihuachengjin_ysxy.html";

    /// <summary>
    /// 服务协议
    /// </summary>
    public static string fwxy = "https://d1hz5n99bypoef.cloudfront.net/yihuachengjin_fwxy.html";
}