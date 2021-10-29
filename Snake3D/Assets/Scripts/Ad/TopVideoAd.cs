using System;
using System.Collections.Generic;
using AnyThinkAds.Api;
using UnityEngine;

public class TopVideoAd : AdWrap, ATRewardedVideoListener {
    private const string TAG = "TopVideoAd";

    private readonly string adId;

    private bool isLoading = false;

    private bool isReward = false;

    private DateTime lastLoadTime;

    public TopVideoAd(string adId) {
        this.adId = adId;
        LogUtil.Log("Ad", "TopVideoAd:" + adId);
        ATRewardedVideo.Instance.setListener(this);
        lastLoadTime = DateTime.Now;
    }

    public override void loadAd() {
        if (string.IsNullOrEmpty(adId)) {
            return;
        }

        if (isLoading && (DateTime.Now - lastLoadTime).TotalSeconds >= 300) {
            isLoading = false;
        }

        if (isLoading) {
            return;
        }

        if (isAdReady()) {
            return;
        }

        Dictionary<string, string> json = new Dictionary<string, string>();
        ATRewardedVideo.Instance.loadVideoAd(adId, json);
        LogUtil.Log(TAG, "load video ad>>>" + adId);
        isLoading = true;
        //DotManager.Instance.sendEvent(DotConstant.AD_RV_REQUEST, DottingType.Tga);
        lastLoadTime= DateTime.Now;
    }

    public override void showAd() {
        if (isAdReady()) {
            ATRewardedVideo.Instance.showAd(adId);
        }
    }


    public override bool isAdReady() {
        return ATRewardedVideo.Instance.hasAdReady(adId);
    }

    public void onRewardedVideoAdLoaded(string unitId) {
        LogUtil.Log(TAG, unitId + " onRewardedVideoAdLoaded");
        isLoading = false;
        onAdLoadSuccess?.Invoke(unitId);
    }

    public void onRewardedVideoAdLoadFail(string unitId, string code, string message) {
        LogUtil.Log(TAG, unitId + " onRewardedVideoAdLoadFail"+ code + ">>>message=" + message);
        isLoading = false;
        onAdLoadError?.Invoke(unitId, code, message);
        DotManager.Instance.sendEvent(DotConstant.AD_RV_FILL_FAILED, DottingType.Tga);
    }

    public void onRewardedVideoAdPlayStart(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + " onRewardedVideoAdPlayStart");
        isReward = false;
        onAdLoadShow?.Invoke(unitId);
        DotManager.Instance.sendEvent(DotConstant.AD_RV_SHOW,  DottingType .Af );

        DotManager.Instance.sendEvent("ad_show", DottingType.Tga ,new Dictionary<string , object> { { "ad_type", "Rewarded" },{ "Location", Ad_Location .showType} } );

        DotManager.Instance.sendEvent( "ad_count" , DottingType.Tga );
        DotManager.Instance.adAdd();
        DotManager.Instance.sendILRD(callbackInfo);
        DataManager.addRv( );
#if UNITY_ANDROID
        AndroidSend.send(DotConstant.AD_RV_SHOW);
        AndroidSend.sendWithUserId(DotConstant.AD_RV_SHOW);
#endif
    }


    public void onRewardedVideoAdPlayEnd(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + " onRewardedVideoAdPlayEnd");
    }

    public void onRewardedVideoAdPlayFail(string unitId, string code, string message) {
        LogUtil.Log(TAG, unitId + " onRewardedVideoAdPlayFail");
        DotManager.Instance.sendEvent(DotConstant.AD_RV_SHOW_FAILED);
        DotManager.Instance.sendEvent("ad_rv_show_buttonnone",DottingType.Af);

        DotManager.Instance.sendEvent("ad_show_butnone", DottingType.Tga,
        new Dictionary<string, object> { { "ad_type", "Rewarded" }, { "Location", Ad_Location.showType } });
    }

    public void onRewardedVideoAdPlayClosed(string unitId, bool isReward, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + " onRewardedVideoAdPlayClosed");
        if (isReward) {
            onAdLoadClose?.Invoke(unitId);
        }

        isReward = false;
    }

    public void onRewardedVideoAdPlayClicked(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + " onRewardedVideoAdPlayClicked");
        onAdLoadClose?.Invoke(unitId);
        DotManager.Instance.sendEvent(DotConstant.AD_RV_CLICK, DottingType.Af);
    }

    public void onReward(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + " onReward");
        isReward = true;
        onAdLoadReward?.Invoke(unitId);
    }
}