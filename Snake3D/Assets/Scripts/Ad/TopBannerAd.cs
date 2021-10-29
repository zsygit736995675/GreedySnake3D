using System;
using System.Collections.Generic;
using AnyThinkAds.Api;
using UnityEngine;

/**
 * Banner广告
 */
public class TopBannerAd : AdWrap, ATBannerAdListener {
    private const string TAG = "TopBannerAd";

    private string adId;

    private bool isLoaded = false;

    public TopBannerAd(string adId) {
        this.adId = adId;
        LogUtil.Log("Ad", "TopBannerAd:" + adId);
        ATBannerAd.Instance.setListener(this);
    }


    public override void loadAd() {
        if (string.IsNullOrEmpty(adId)) {
            return;
        }

        GameObject gameObject=GameObject.Find("Canvas");
        RectTransform rect1 = gameObject.transform.GetComponent<RectTransform>();

        float scale = rect1.sizeDelta.x / Screen.width;
        
        int adHeight = (int)(150 / scale);
        Dictionary<string, object> jsonmap = new Dictionary<string, object>();
        //配置Banner要展示的宽度，高度，是否使用pixel为单位
        ATSize bannerSize = new ATSize(UnityEngine.Screen.width, adHeight, true);
        jsonmap.Add(ATBannerAdLoadingExtra.kATBannerAdLoadingExtraBannerAdSizeStruct, bannerSize);
        ATBannerAd.Instance.loadBannerAd(adId, jsonmap);
        LogUtil.Log(TAG, "load banner ad >>>" + adId);
        isLoaded = false;
    }

    public override void showAd() {
        if (isAdReady()) {
            ATBannerAd.Instance.showBannerAd(adId, ATBannerAdLoadingExtra.kATBannerAdShowingPisitionBottom);
        }
    }
    


    public override bool isAdReady() {
        return isLoaded;
    }

    public void onAdLoad(string unitId) {
        isLoaded = true;
        LogUtil.Log(TAG, unitId + " onAdLoad");
        onAdLoadSuccess?.Invoke(unitId);
    }

    public void onAdLoadFail(string unitId, string code, string message) {
        LogUtil.Log(TAG, unitId + " onAdLoadFail");
        onAdLoadError?.Invoke(unitId, code, message);
    }

    public void onAdImpress(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + " onAdImpress");
        onAdLoadShow?.Invoke(unitId);
        DotManager.Instance.sendILRD(callbackInfo);
    }

    public void onAdClick(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + " onAdClick");
        onAdLoadClick?.Invoke(unitId);
    }

    public void onAdAutoRefresh(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + " onAdAutoRefresh");
    }

    public void onAdAutoRefreshFail(string unitId, string code, string message) {
        LogUtil.Log(TAG, unitId + " onAdAutoRefreshFail");
    }

    public void onAdClose(string unitId) {
        LogUtil.Log(TAG, unitId + " onAdClose");
        onAdLoadClose?.Invoke(unitId);
    }

    public void onAdCloseButtonTapped(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + " onAdCloseButtonTapped");
    }
}