using AnyThinkAds.Api;
using AppsFlyerSDK;
using System;
using System.Collections.Generic;
using ThinkingAnalytics;
using UnityEngine;

public enum DottingType
{
    None,
    Af,
    Tga,
}

public class Ad_Location
{
    public static string showType;
}
public class DotManager : Singleton<DotManager>
{
    public const string APP_ID = "f4631313eda04d888501ede36675159c";


    /// <summary>
    /// 打点事件
    /// </summary>
    /// <param name="msg">名称</param>
    /// <param name="eventValues">属性值</param>
    internal void sendEvent(string msg, DottingType type = DottingType.None, Dictionary<string, object> eventValues = null)
    {

        switch (type)
        {
            case DottingType.None:
                AppsFlyer.sendEvent(msg, eventValues.GetKeysObjToStr());
                TGAsendEvent(msg, eventValues);
                break;
            case DottingType.Af:
                AppsFlyer.sendEvent(msg, eventValues.GetKeysObjToStr());
                break;
            case DottingType.Tga:
                TGAsendEvent(msg, eventValues);
                break;
        }
    }

    /// <summary>
    /// TGA打点
    /// </summary>
    /// <param name="msg">名称</param>
    /// <param name="eventValue">属性值</param>
    private void TGAsendEvent(string msg, Dictionary<string, object> eventValue = null)
    {
        LogUtil.Log("sendEvent", msg);
        try
        {
            setSuperProperties();
            ThinkingAnalyticsAPI.Track(msg, eventValue);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// 添加公共属性
    /// </summary>
    internal static void setSuperProperties()
    {
        Dictionary<string, object> properties = new Dictionary<string, object>();
        //访客id
        properties["distinct_id"] = ThinkingAnalyticsAPI.GetDistinctId();
        //设备id
        properties["device_id"] = ThinkingAnalyticsAPI.GetDeviceId();
        //应用版本
        properties["app_version"] = Application.version;
        //系统版本
        properties["os_version"] = SystemInfo.operatingSystem;
        //国家码
        properties["country_code"] = AndroidSend.getNative("country");
        //账户
        properties["account_id"] = HttpDataManager.getUser().user_id;
        //系统
        properties["os"] = "Android";
        //渠道
        properties["media_source"] = ConfigManager.afModel?.media_source;
        //登录天数
        properties["total_logindays"] = DataManager.LoginDayNum;
        //登录次数
        properties["total_logintimes"] = DataManager.StartCount;
        //上次登录时间
        properties["latest_logintime"] = DateTime.Parse(DataManager.getLoginTime());
        //注册时间
        properties["regtime"] = DateTime.Parse(DataManager.getCreateTime());
        //余额
        properties["Cash_number"] = DataManager.getCurrentMoney();
        //激励视频次数
        properties["totalnum_adrvshow"] = DataManager.getRvCount();
        //最高等级
        properties["latest_level"] = DataManager.getLevelNum();

        //properties["channel"] = AndroidSend.getNative("channel");
        //properties["channel_id"] = PlayerPrefs.GetInt("OpenLevel", 1);
        //properties["user_active_days"] = Withdraw_task.getDays();

        /// 新加公共事件的属性
        ThinkingAnalyticsAPI.SetSuperProperties(properties);
    }

    internal void userAdd(Dictionary<string, object> properties)
    {
        ThinkingAnalyticsAPI.UserAdd(properties, APP_ID);
    }

    internal void sendCreateTime()
    {
        Dictionary<string, object> properties = new Dictionary<string, object>();
        properties["channel"] = AndroidSend.getNative("channel");
        properties["user_create_time"] = DateTime.Parse(DataManager.getCreateTime());

        ThinkingAnalyticsAPI.UserSetOnce(properties);
    }

    internal void UserSetOnce(string key, object value)
    {
        Dictionary<string, object> properties = new Dictionary<string, object>();
        properties.Add(key, value);
        properties.Add("distinct_id",ThinkingAnalyticsAPI.GetDistinctId());
        ThinkingAnalyticsAPI.UserSetOnce(properties);
    }


    internal void Login(string userId)
    {
        AppsFlyer.setCustomerUserId(userId);
        ThinkingAnalyticsAPI.Login(userId);
    }

    internal void sendLoginTime()
    {
        Dictionary<string, object> properties = new Dictionary<string, object>();
        properties["last_login"] = DateTime.Parse(DataManager.getLoginTime());
        ThinkingAnalyticsAPI.UserSet(properties);
    }

    internal void userSet(string key, object value)
    {
        Dictionary<string, object> properties = new Dictionary<string, object>();
        if (properties.ContainsKey(key))
            properties[key] = value;
        else
            properties.Add(key, value);
        ThinkingAnalyticsAPI.UserSet(properties);
    }

    /// <summary>
    /// 开启自动采集
    /// </summary>
    internal void autoTrack()
    {
        ThinkingAnalyticsAPI.EnableAutoTrack(AUTO_TRACK_EVENTS.APP_START | AUTO_TRACK_EVENTS.APP_END);
    }

    internal void adAdd()
    {
        userSet("ad_count", DataManager.getRvCount() + DataManager.getInterCount());
    }

    internal void moneyAdd(float money)
    {
        userSet("Dollar_number", money);
    }

    internal void addDebris(float debris)
    {
        Dictionary<string, object> properties = new Dictionary<string, object>();
        properties["phone_number"] = debris;
        userAdd(properties);
    }

    /// <summary>
    /// 设置ilrd回传
    /// </summary>
    /// <param name="info"></param>
    internal void sendILRD(ATCallbackInfo info)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["adunit_id"] = info.adunit_id;
        dic["adunit_name"] = "";
        dic["adunit_format"] = info.adunit_format;
        dic["currency"] = info.currency;
        dic["publisher_revenue"] = (decimal)info.publisher_revenue;
        dic["af_revenue"] = (decimal)info.publisher_revenue;
        dic["network_name"] = info.network_firm_id;
        dic["network_placement_id"] = info.network_placement_id;
        dic["adgroup_name"] = "";
        dic["adgroup_type"] = "";
        dic["adgroup_priority"] = info.adsource_index;
        dic["precision"] = info.precision;
        dic["adgroup_id"] = info.adsource_id;
        dic["id"] = info.id;
        dic["segment_id"] = info.segment_id;

        sendEvent("Topon_ilrd", DottingType.None, dic);
    }

}
