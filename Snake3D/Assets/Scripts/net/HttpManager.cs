using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using ThinkingAnalytics;
using UnityEditor;
using UnityEngine;

public class HttpManager : Singleton<HttpManager>
{
    public const string APPLICATION_ID = "com.cutesnake.slither.game";

#if UNITY_EDITOR || UNITY_ANDROID
    //项目关键字
    private const string BASE_KEY = "snake";
#else
    private const string BASE_KEY = "snake";

#endif

    //配置接口关键字
    private const string KEY_CONFIG = "config";

    //配置接口关键字
    private const string KEY_NUM = "num";

    private const string URL_LOGIN = "lw/auth";

    private const string URL_SYN = "lw/user/info";

    private const string URL_CASH = "lw/withdraw/cash";

    private const string URL_SCORE_WITHDRAW = "/lw/score/withdraw";

    private const string URL_SCORE_INFO = "/lw/score/info";

    private const string URL_SCORE_RANK = "/lw/score/rank";

    private const string URL_DATA = "/lw/common/data";

    private const string URL_AMOUNT = "/lw/withdraw/cash";

    private const string URL_Lottery = "lw/client/data";


    /// <summary>
    /// 获取当前用户提现档次接口
    /// </summary>
    public void GetAmount(Action<bool>action)
    {
        HttpClient.Ins.GET<Amount>(URL_AMOUNT, (success, result) =>
        {
            if (success)
            {
                Debug.Log("GetAmount------>>>>>>:" + result.rank);
                DataManager.setAmount(result.rank);
                action(true);
            }
            else
            {
                action(false);
            }
            EventMgrHelper.Ins.PushEvent(EventDef.REQUEST_AMOUNT);
        },decrypt);
    }

    /**
     * 获取排行榜接口
     */
    /*public void getScoreRank(Action callback = null)
    {
        HttpClient.Instance.POST<ScoreRankResponse>(URL_SCORE_RANK, (success, result) =>
        {
            if (success)
            {
                callback?.Invoke();

                List<RankList> score_rank = null;
                bool check = false;
                int score = 0;
                switch (ConfigManager.current_money)
                {
                    case ConfigManager.NUM_100:
                        score_rank = result.score_rank_100.rank_list;
                        break;
                    case ConfigManager.NUM_120:
                        score_rank = result.score_rank_120.rank_list;
                        break;
                    case ConfigManager.NUM_150:
                        score_rank = result.score_rank_150.rank_list;
                        break;
                    case ConfigManager.NUM_200:
                        score_rank = result.score_rank_200.rank_list;
                        break;
                }

                if (score_rank != null)
                {
                    int rank = 0;
                    foreach (RankList res in score_rank)
                    {
                        //所有排名遍历出自己(已经排好序的)
                        rank++;
                        if (res.user_id == HttpDataManager.getUser()?.user_id)
                        {
                            check = true;
                            break;
                        }
                    }

                    if (check)
                    {
                        score = score_rank[rank - 1].user_score - DataManager.getCurrentScore();
                    }
                    else
                    {
                        score = score_rank.Last().user_score - DataManager.getCurrentScore();
                    }
                }
                EventMgrHelper.Ins.PushEventEx(EventDef.REF_RANK, result, data0: score, bool0: check);

            }

            //同步成功
        }, null, getHeaders(), decrypt);
    }*/

    /**
     * 获取积分接口
     */
    public void getScore()
    {
        HttpClient.Ins.POST<ScoreResponse>(URL_SCORE_INFO, (success, result) =>
        {
            if (success)
            {
                DataManager.setCurrentScore(result.user_score);
                //用户积分埋点添加
                DotManager.Instance.sendEvent("integral_Suspension", DottingType.Tga, new Dictionary<string, object> { { "integral_Suspension", DataManager.getCurrentScore() } });
            }
            EventMgrHelper.Ins.PushEvent(EventDef.REQUEST_SCORE);
            //同步成功
        }, null, getHeaders(), decrypt);
    }


    /// <summary>
    /// 是否封榜状态
    /// </summary>
    public void IsFengBang(Action<bool> action)
    {
        GetServerData((dt) =>
        {
            if (dt.DayOfWeek == DayOfWeek.Sunday)
            {
                if (dt.Hour >= 22)
                {
                    if (dt.Hour >= 23)
                    {
                        if (dt.Minute <= 59)
                        {
                            action?.Invoke(true);
                        }
                        else 
                        {
                            action?.Invoke(false);
                        }
                    }
                    else
                    {
                        action?.Invoke(true);
                    }
                }
                else
                {
                    action?.Invoke(false);
                }
            }
            else
            {
                action?.Invoke(false);
            }
        });
    }

    /// <summary>
    /// 获取服务器时间
    /// </summary>
    public void GetServerData(Action<DateTime> action)
    {
        Debug.Log("进入GetServerData>>>"  );
        HttpClient.Ins.GET<ServerData>(URL_DATA, (b, info) =>
        {
            Debug.Log("进入GetServerData>>>"+b);
            if (b)
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                DateTime dt = startTime.AddSeconds(info.timestamp); 
                Debug.Log("进入GetServerData>>>" + info.timestamp);
                action?.Invoke(dt);
            }
        }, decrypt);
    }

    /**
     * 上报积分接口 增量
     */
    public void addScore(int score)
    {
        ScoreRequest scoreRequest = new ScoreRequest
        {
            add_score = score
        };
        HttpClient.Ins.PUT<ScoreResponse>(URL_SCORE_INFO, (success, result) =>
        {
            if (success)
            {

            }

            //同步成功
        }, AesCbcUtil.Encrypt(JsonUtility.ToJson(scoreRequest)), getHeaders(), decrypt);
    }

    /**
     * 获取提现档位接口
     */
    public void getScoreWithdraw()
    {
        HttpClient.Ins.POST<ScoreWithDraw>(URL_SCORE_WITHDRAW, (success, res) =>
        {
            if (success)
            {
                ConfigManager.withdraw_money = res.withdraw_money;
                Debug.Log("withdraw_money:" + ConfigManager.withdraw_money);
                ConfigManager.withdraw_status = res.withdraw_status;
            }
            EventMgrHelper.Ins.PushEvent(EventDef.REQUEST_SCORE_WITHDRAW);
        }, null, getHeaders(), decrypt);
    }

    /// <summary>
    /// 提现接口
    /// </summary>
    /// <param name="money"></param>
    /// <param name="res"></param>
    public void putWithDraw(double money, Action<bool> res)
    {
        WithDrawRequest request = new WithDrawRequest();
        request.wd_type = 1;
        request.coin_wd_cost = 0;
        request.amount = money;
        request.coin_wd_type = 0;
        string json = JsonUtility.ToJson(request);
        HttpClient.Ins.PUT<WithDrawResponse>(URL_CASH,(success, response) => 
        {
            //如果取不到数据 则没提过现
            if(success)
            {
                res.Invoke(true);
                return;
            }
            else
            {
                res.Invoke(false);
            }
        },
            AesCbcUtil.Encrypt(json),
            getHeaders(), decrypt);
    }


    public void getWithDrawList()
    {
        WithDrawRequest request = new WithDrawRequest();
        request.wd_type = 1;

        HttpClient.Ins.POST<List<WithDrawResponse>>(URL_CASH, (success, res) =>
            {
                if (res != null && res.Count > 0)
                {
                    for (int i = 0; i < res.Count; i++)
                    {
                        WithDrawResponse data = res[i];
                        if (data.amount == 100)
                        {
                            ConfigManager.current_money = 120;
                        }
                        else if (data.amount == 120)
                        {
                            ConfigManager.current_money = 150;
                        }
                        else if (data.amount == 150)
                        {
                            ConfigManager.current_money = 200;
                        }
                        else
                        {
                            ConfigManager.current_money = 200;
                        }
                        //有单子正在审核中
                        if (data.status == 0) 
                        {
                            int real = 100;
                            switch (data.amount)
                            {
                                case 100:
                                    real = 120;
                                    break;
                                case 120:
                                    real = 150;
                                    break;
                                case 150:
                                    real = 200;
                                    break;
                                case 200:
                                    real = 200;
                                    break;
                            }
                            ConfigManager. isShowCyht = true;
                            ConfigManager.real_Money = real;
                        }
                        
                    }
                    HttpDataManager.saveCashOut();
                }


                EventMgrHelper.Ins.PushEventEx(EventDef.REQUEST_WITHDRAW_LIST, res);
            },
            AesCbcUtil.Encrypt(JsonUtility.ToJson(request)),
            getHeaders(), decrypt
        );
    }

    //获取配置信息
    public void getNum(int index)
    {
        string num = "one";
        EventDef eventDef = EventDef.REQUEST_100;
        switch (index)
        {
            case ConfigManager.NUM_50:
                num = "one";
                eventDef = EventDef.REQUEST_100;
                break;
            case ConfigManager.NUM_100:
                num = "two";
                eventDef = EventDef.REQUEST_120;
                break;
            case ConfigManager.NUM_120:
                num = "three";
                eventDef = EventDef.REQUEST_150;
                break;
            case ConfigManager.NUM_150:
                num = "four";
                eventDef = EventDef.REQUEST_200;
                break;
        }
        string url = $"api/{BASE_KEY}/{KEY_NUM + num}?app_version_code=" + getVersionCode();
        HttpClient.Ins.GET<string>(url, (b, info) =>
        {
            if (b)
            {
                NumInfo response = JsonMapper.ToObject<NumInfo>(info);
                if (response.code == 200)
                {
                    HttpDataManager.saveNum(response, index);
                }
            }
            EventMgrHelper.Ins.PushEvent(eventDef);
        });
    }

    //获取配置信息
    public void getConfig()
    {
        string url = $"api/{BASE_KEY}/{KEY_CONFIG}?app_version_code=" + getVersionCode();
        HttpClient.Ins.GET<ConfigInfo>(url, (b, info) =>
        {
            if (b)
            {
                HttpDataManager.saveConfig(info);
            }
            EventMgrHelper.Ins.PushEvent(EventDef.REQUEST_COFING);
        });
    }

    //登录
    public void login(Action action)
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier+"snake10";
        LoginRequest loginRequest = new LoginRequest
        {
            openid = deviceId,
            country = "CN",
            platform = 0,
            device_id = deviceId,
        };
        HttpClient.Ins.POST<LoginResponse>(URL_LOGIN, (b, response) =>
        {
            if (b)
            {
                DotManager.Instance.userSet("last_login_time", DateTime.Now.ToString());
                //DotManager.Instance.Login(response.user_id.ToString());

                DotManager.Instance.sendEvent("af_login",DottingType.Af);

                HttpDataManager.saveUser(response);
                if (response.money < 0)
                {
                    response.money = 0;
                }
                DataManager.setCurrentMoney((float)response.money);
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary["account_id"] = response.user_id;
                dictionary["distinct_id"] = ThinkingAnalyticsAPI.GetDistinctId();
                dictionary["device_id"] = deviceId;
                dictionary["country"] = System.Globalization.RegionInfo.CurrentRegion;
                //StatisticManager.Instance.userSet(dictionary);
                //getIsCashOut();
                action?.Invoke();
            }
            EventMgrHelper.Ins.PushEvent(EventDef.REQUEST_LOGIN);
        },
        AesCbcUtil.Encrypt(JsonUtility.ToJson(loginRequest)),
        getHeaders(), decrypt);
    }

    public void bind(string openId, string token)
    {
        string deviceid = SystemInfo.deviceUniqueIdentifier;
        LoginRequest loginRequest = new LoginRequest
        {
            openid = openId,
            platform = 1,
            device_id = deviceid,
            token = token
        };

        Debug.Log("bind:"+JsonUtility.ToJson(loginRequest));
        HttpClient.Ins.POST<LoginResponse>(URL_LOGIN, (b, response) =>
            {
                if (b)
                {
                    HttpDataManager.saveUser(response);
                    if (response.money != 0)
                    {
                        DataManager.setCurrentMoney((float)response.money);
                    }

                    getWithDrawList();
                    EventMgrHelper.Ins.PushEvent(EventDef.BIND_WX);
                    EventMgrHelper.Ins.PushEvent(EventDef.DataUpdate);
                }
            },
            AesCbcUtil.Encrypt(JsonUtility.ToJson(loginRequest)),
            getHeaders(), decrypt);
    }

    public void LotteryData()
    {
        LotteryRequest request = new LotteryRequest();
        request.lotteryNum = DataManager.getCurrentLottery().ToString();
        request.allDebrisNum = DataManager.getCurrentDebris().ToString();
        request.redmergaNum = DataManager.getMergaRedNum().ToString();
        request.redmergalotteryNum = DataManager.getRedMergaLotteryNum().ToString();
        request.showVideoNum = DataManager.getShowVideoNum().ToString();
        request.NextRedNum = DataManager.getRedint();
        request.aliShow = DataManager.getShowAli();

        HttpClient.Ins.PUT<LotteryResponse>(URL_Lottery,(success,result)=> 
        {
            if (success)
            {
                LotteryPost();
            }
        });
    }

    public void LotteryPost()
    {
        HttpClient.Ins.POST<LotteryResponse>(URL_Lottery,
            (success, result) =>
            {
                if (success)
                {
                    try
                    {
                        DataManager.setCurrentLottery(int.Parse(result.lotteryNum));
                        DataManager.setCurrentDebris(float.Parse(result.allDebrisNum));
                        DataManager.setMergaRedNum(int.Parse(result.redmergaNum));
                        DataManager.setShowVideoNum(int.Parse(result.showVideoNum));
                        DataManager.setRedMergaLotteryNum(int.Parse(result.redmergalotteryNum));
                    }
                    catch
                    {
                    }

                    DataManager.setShowAli(result.aliShow);
                    AdManager.Ins.updateInitCustomMap();
                    // }

                    //Debug.LogError("result.NextRedNum--->" + result.NextRedNum);
                }
            });
    }

    //同步当前金钱数
    public void SynData(int opType = 1)
    {
        SynRequest request = new SynRequest
        {
            coin = "0",
            coin_today = "0",
            money = DataManager.getCurrentMoney().ToString("F2"),
            money_today = "0",
            rv_count = DataManager.getRvCount() + DataManager.getInterCount(),
            op_type = opType
        };
        LogUtil.Log("totoro>>>>同步数据的参数", JsonMapper.ToJson(request));
        HttpClient.Ins.PUT<SynResponse>(URL_SYN, (b, response) =>
            {
                // DataManager.setCurrentMoney((float)response.money);
            },
            AesCbcUtil.Encrypt(JsonUtility.ToJson(request))
            , getHeaders(), decrypt);
    }


    private Dictionary<string, string> getHeaders()
    {
        Dictionary<string, string> mHeaders = new Dictionary<string, string>();
        mHeaders["lang"] = "cn";
        mHeaders["iv"] = "5481649756531687";
        mHeaders["Pkg-Name"] = APPLICATION_ID;
        mHeaders["Content-Type"] = "application/json;charset=utf-8";
        return mHeaders;
    }

    private string decrypt(string res)
    {
        return AesCbcUtil.Decipher(res);
    }

    private int getVersionCode()
    {
        string version = Application.version;
        string[] split = version.Split('.');
        StringBuilder sb = new StringBuilder();
        foreach (string s in split)
        {
            sb.Append(s);
        }

        return Convert.ToInt32(sb.ToString());
    }
}