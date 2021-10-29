using System;
using System.Collections.Generic;
using AnyThinkAds.Api;

public class TopInterstitialAd : AdWrap, ATInterstitialAdListener {
    private const string TAG = "TopInterstitialAd";

    private readonly string adId;

    private bool isLoading = false;


    private DateTime lastLoadTime;

    public TopInterstitialAd(string adId) {
        this.adId = adId;
        LogUtil.Log("Ad","TopInterstitialAd:"+adId);

        WrapATInterstitialAd.Instance.setListener(this,adId);
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

        Dictionary<string, string> ps = new Dictionary<string, string>();
        WrapATInterstitialAd.Instance.loadInterstitialAd(adId, ps);
        LogUtil.Log(TAG, "load>>>" + adId);
        isLoading = true;
        //DotManager.Instance.sendEvent(DotConstant.AD_INTER_REQUEST, DottingType.Tga);
        lastLoadTime= DateTime.Now;
    }


    public override void showAd() {
        if (isAdReady()) {
            WrapATInterstitialAd.Instance.showInterstitialAd(adId);
        }
    }


    public override bool isAdReady() {
        return WrapATInterstitialAd.Instance.hasInterstitialAdReady(adId);
    }

    public void onInterstitialAdLoad(string unitId) {
        LogUtil.Log(TAG, unitId + "onInterstitialAdLoad");
        isLoading = false;
        onAdLoadSuccess?.Invoke(unitId);
        //DotManager.Instance.sendEvent(DotConstant.AD_INTER_FILL, DottingType.Tga);
    }

    public void onInterstitialAdLoadFail(string unitId, string code, string message) {
        LogUtil.Log(TAG, unitId + "onInterstitialAdLoadFail>>>code==" + code + ">>>message=" + message);
        isLoading = false;
        onAdLoadError?.Invoke(unitId, code, message);
        DotManager.Instance.sendEvent(DotConstant.AD_INTER_FILL_FAILED, DottingType.Tga);

        DotManager.Instance.sendEvent("ad_show_butnone", DottingType.Tga,
        new Dictionary<string, object> { { "ad_type", "Interstitial" }, { "Location", Ad_Location.showType } });
    }

    public void onInterstitialAdShow(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + "onInterstitialAdShow");
        onAdLoadShow?.Invoke(unitId);

        DotManager.Instance.sendEvent("ad_show", DottingType.Tga,
           new Dictionary<string, object> { { "ad_type", "Interstitial" }, { "Location", Ad_Location.showType } });

        DotManager.Instance.sendEvent( "ad_count" , DottingType.Tga );
        DotManager.Instance.sendEvent("ad_inter_show", DottingType.Af);
        DotManager.Instance.sendILRD(callbackInfo);
        DataManager.addInter( );

#if UNITY_ANDROID
        //AndroidSend.send("ad_inter_show");
#endif
    }

    public void onInterstitialAdFailedToShow(string unitId) {
        LogUtil.Log(TAG, unitId + "onInterstitialAdFailedToShow");
        DotManager.Instance.sendEvent(DotConstant .AD_INTER_SHOW_FAILED);
    }

    public void onInterstitialAdClose(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + "  onInterstitialAdClose");
        onAdLoadClose?.Invoke(unitId);
    }

    public void onInterstitialAdClick(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + "onInterstitialAdClick");
        onAdLoadClick?.Invoke(unitId);
        DotManager.Instance.sendEvent(DotConstant.AD_INTER_CLICK, DottingType.Af);
    }

    public void onInterstitialAdStartPlayingVideo(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + "onInterstitialAdStartPlayingVideo");
    }

    public void onInterstitialAdEndPlayingVideo(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + "onInterstitialAdEndPlayingVideo");
    }

    public void onInterstitialAdFailedToPlayVideo(string unitId, string code, string message) {
        LogUtil.Log(TAG, unitId + "onInterstitialAdFailedToPlayVideo");
    }
}