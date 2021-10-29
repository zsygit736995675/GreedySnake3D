using System;

using LitJson;

using UnityEngine;

/**
 * 网络请求数据的管理类
 */
public class HttpDataManager
{
    private const string KEY_CONFIG = "key_config";
    private const string KEY_NUM = "key_num";
    private const string KEY_USER = "key_user";
    private const string KEY_ACTIVE_CONFIG = "key_active_config";
    private const string KEY_CASH_OUT = "key_cash_out";
    private const string KEY_RED_BAG_COUNT = "key_red_bag_count";

    private const string KEY_ACTIVITY_DAYS = "key_activity_days";


    public static void saveActivityDays(int days)
    {
        PlayerPrefs.SetInt(KEY_ACTIVITY_DAYS, days);
    }

    public static int getActivityDays()
    {
        return PlayerPrefs.GetInt(KEY_ACTIVITY_DAYS, 0);
    }

    public static void setRedCount(int count)
    {
        PlayerPrefs.SetInt(KEY_RED_BAG_COUNT, count);
    }

    public static int getRedCount()
    {
        return PlayerPrefs.GetInt(KEY_RED_BAG_COUNT, -1);
    }

    public static void saveCashOut()
    {
        PlayerPrefs.SetInt(KEY_CASH_OUT, 1);
    }

    public static bool isCashOut()
    {
        return PlayerPrefs.GetInt(KEY_CASH_OUT, 0) == 1;
    }


    public static void saveActiveConfig(ActiveConfigModel model)
    {
        string res = JsonMapper.ToJson(model);
        PlayerPrefs.SetString(KEY_ACTIVE_CONFIG, res);
    }

    public static ActiveConfigModel GetActiveConfigModel()
    {
        string res = PlayerPrefs.GetString(KEY_ACTIVE_CONFIG, "");
        if (string.IsNullOrEmpty(res))
        {
            ActiveConfigModel model = new ActiveConfigModel();
            ActiveConfigModelDataModel modelDataModel = new ActiveConfigModelDataModel
            {
                amount = 100,
                active_days = 30,
                ad_num = 40
            };
            ActiveConfigModelDataModel modelDataModelTwo = new ActiveConfigModelDataModel
            {
                amount = 200,
                active_days = 30,
                ad_num = 40
            };
            ActiveConfigModelData modelData = new ActiveConfigModelData { one = modelDataModel, two = modelDataModelTwo };
            model.data = modelData;
            return model;
        }

        return JsonMapper.ToObject<ActiveConfigModel>(res);
    }

//保存num
    public static void saveNum(NumInfo info,int index)
    {
        string res = JsonMapper.ToJson(info);
        PlayerPrefs.SetString(KEY_NUM+"_"+index, res);
    }
    
    public static NumInfo getNum(int index)
    {
        string res = PlayerPrefs.GetString(KEY_NUM+"_"+index);
        if (string.IsNullOrEmpty(res))
        {
            return null;
        }

        return JsonMapper.ToObject<NumInfo>(res);
    }
    
    //保存config
    public static void saveConfig(ConfigInfo info)
    {
        if (info != null) 
        {
            string res = JsonMapper.ToJson(info);
            Debug.Log("KEY_CONFIG:" + res);
            PlayerPrefs.SetString(KEY_CONFIG, res);
        }
    }

    //获取配置数据
    public static ConfigInfo GetConfigInfo()
    {
        string res = PlayerPrefs.GetString(KEY_CONFIG);

        if (string.IsNullOrEmpty(res))
        {
            ConfigInfo info = new ConfigInfo();
            return info;
        }

        return JsonMapper.ToObject<ConfigInfo>(res);
    }

    public static void saveUser(LoginResponse response)
    {

        string res = JsonMapper.ToJson(response);
        PlayerPrefs.SetString(KEY_USER, res);
    }

    public static LoginResponse getUser()
    {
        string res = PlayerPrefs.GetString(KEY_USER);
        if (String.IsNullOrEmpty(res))
        {
            return new LoginResponse()
            {
                money = 0.01
            };
        }
        else
        {
            return JsonMapper.ToObject<LoginResponse>(res);
        }
    }
}