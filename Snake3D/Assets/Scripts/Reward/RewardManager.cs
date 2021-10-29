using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

/**
 * 奖励管理
 */
public class RewardManager
{
    private static List<RewardInfo> mCurrentList;

    public static int current_money = 100;

    private static string readFile()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("File/number");
        LogUtil.Log("number", textAsset.text);
        return textAsset.text;
    }

   static NumInfo numInfo;
    public  static List<RewardInfo> getAllReward()
    {
        if (numInfo == null)
        {
            var res = readFile();
            numInfo = JsonMapper.ToObject<NumInfo>(res);
        }
        return numInfo.num;
    }

    public static string  FormatRew(float rew) 
    {
        return rew.ToString("f2");
    }

    public static int GetCurrentMoney() 
    {
        if (ConfigManager.isShowCyht)
        {
            return ConfigManager.real_Money;
        }
        else 
        {
            return DataManager. getAmount();
        }
    }

    /**
     * 获取下一次的奖励值
     */
    public static RewardInfo getNextReward(float currentMoney)
    {
        if (mCurrentList == null)
        {
            mCurrentList = getAllReward();
        }

        RewardInfo reward = null;
        foreach (var info in mCurrentList)
        {
            if (!(info.money > (decimal) currentMoney)) continue;
            reward = info;
            break;
        }

        return reward ?? (reward = new RewardInfo {reward = (decimal) 0.01f});
    }
}