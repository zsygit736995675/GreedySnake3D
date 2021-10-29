using System;
using System.Collections.Generic;
using Ad;
using AnyThinkAds.Api;
using UnityEngine;

public class AdManager : SingletonObject<AdManager>
{
    private const string TAG = "AdManager";

    private AdWrap mInterstitialAd;
    private AdWrap mRewardAd;
    private AdWrap mBannerAd;
    private NativeAdWrap mNativeAd;


    public void Init() 
    {
        Invoke(nameof(initAd), 2f);
    }

    private void initAd()
    {
        /*if (AndroidSend.getNative("is_channel").Equals("1")) {
            LogUtil.D("totoro>>>>", "is_channel");
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["channel"] = AndroidSend.getNative("channel");
            StatisticUtil.sendEventWithLevel(AndroidSend.getNative("channel"));
            ATSDKAPI.initCustomMap(dictionary);
        }*/
        cacheAd();
    }

    private void cacheAd()
    {
        mInterstitialAd = new TopInterstitialAd(SystemConfig.Get(1).Interstit);
        mInterstitialAd.loadAd();
        mRewardAd = new TopVideoAd(SystemConfig.Get(1).rv);
        mRewardAd.loadAd();
        //mBannerAd = new TopBannerAd(SystemConfig.Get(1).banner) { onAdLoadSuccess = callBackBannerLoadSuccess };
        //mBannerAd.loadAd();
        mNativeAd = new TopNativeAd(SystemConfig.Get(1).Native);
        mNativeAd.loadAd();
    }

    /// <summary>
    /// ��ҳ���
    /// </summary>
    /// <param name="close"></param>
    /// <param name="showType"></param>
    public void showInterstitialAd(Action<string> close, string showType = "")
    {
        Ad_Location.showType = showType;
        //DotManager.Instance.sendEvent(DotConstant.AD_INTER_READY_SHOW, DottingType.Tga,new Dictionary<string, object> { { "inter_location", showType } });
        DotManager.Instance.sendEvent(DotConstant.AD_INTER_READY_SHOW, DottingType.Tga);

        if (mInterstitialAd?.isAdReady() ?? false)
        {
            mInterstitialAd.onAdLoadClose = close;
            mInterstitialAd.onAdLoadClose += callBackInterstitialClose;
            mInterstitialAd?.showAd();
        }
        else
        {
            loadInterstitialAd();
        }
    }

    public void updateInitCustomMap()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        if (AndroidSend.getNative("is_channel").Equals("1"))
        {
            LogUtil.Log("totoro>>>>", "is_channel");

            dictionary["channel"] = AndroidSend.getNative("channel");
        }

        LogUtil.Log("totoro>>>>", "loginDay-createDay:" + DataManager.getShowAli());
        dictionary["day"] = DataManager.getShowAli() + "";

        ATSDKAPI.initCustomMap(dictionary);
    }

    private void loadInterstitialAd()
    {
        if (mInterstitialAd == null)
        {
            mInterstitialAd = new TopInterstitialAd(SystemConfig.Get(1).Interstit);
        }

        mInterstitialAd.loadAd();
    }

    /// <summary>
    /// ������Ƶ
    /// </summary>
    /// <param name="close"></param>
    /// <param name="showType"></param>
    public void showRewardAd(Action<string> close, string showType = "")
    {
        if (!ConfigManager.isAllowAd) 
        {
            close?.Invoke("");
            return;
        }
        Ad_Location.showType = showType;
        if (mRewardAd?.isAdReady() ?? false)
        {
            mRewardAd.onAdLoadClose = close;
            mRewardAd.onAdLoadClose += callBackRewardClose;
            mRewardAd?.showAd();
        }
        else
        {
            loadRewardAd();
        }
    }

    public bool hasRewardAd()
    {
        bool has = mRewardAd?.isAdReady() ?? false;
        if (has)
        {
            return has;
        }

        if (!has)
        {
            loadRewardAd();
        }

        return has;
    }

    public bool hasBannerAd()
    {
        bool has = mBannerAd?.isAdReady() ?? false;
        return has;
    }

    public bool hasInterAd()
    {
        bool has = mInterstitialAd?.isAdReady() ?? false;
        if (!has)
        {
            loadInterstitialAd();
        }

        return has;
    }

    private void loadRewardAd()
    {
        if (mRewardAd == null)
        {
            mRewardAd = new TopVideoAd(SystemConfig.Get(1).rv);
        }
        mRewardAd.loadAd();
    }


    private void callBackBannerLoadSuccess(string adId)
    {
        mBannerAd?.showAd();
    }

    public void showNativeAd(int height, Action<string> click = null, string showType = "")
    {
        Ad_Location.showType = showType;
        if (mNativeAd?.isAdReady() ?? false)
        {
            removeNativeAd();
            if (click != null)
            {
                mNativeAd.onAdLoadClick = click;
            }

            mNativeAd.showNativeAd(height);
        }
        else
        {
            loadNativeAd();
        }
    }

    private void loadNativeAd()
    {
        if (mNativeAd == null)
        {
            mNativeAd = new TopNativeAd(SystemConfig.Get(1).Native);
        }

        mNativeAd.loadAd();
    }

    public void removeNativeAd()
    {
        mNativeAd?.removeAd();
        if (mNativeAd != null)
        {
            mNativeAd.onAdLoadClick = null;
        }

        loadNativeAd();
    }

    private void callBackInterstitialClose(string adId)
    {
        LogUtil.Log(TAG, "callBackInterstitialClose");
        mInterstitialAd.onAdLoadClose = null;
        mInterstitialAd.onAdLoadSuccess = null;
        mInterstitialAd.onAdLoadError = null;
        mInterstitialAd.onAdLoadClick = null;
        mInterstitialAd.onAdLoadShow = null;
        loadInterstitialAd();
    }

    private void callBackRewardClose(string adId)
    {
        LogUtil.Log(TAG, "callBackRewardClose");
        mRewardAd.onAdLoadClose = null;
        mRewardAd.onAdLoadSuccess = null;
        mRewardAd.onAdLoadError = null;
        mRewardAd.onAdLoadClick = null;
        mRewardAd.onAdLoadShow = null;
        loadRewardAd();
    }
}