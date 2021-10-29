using System.Collections.Generic;
using AnyThinkAds.Api;

public class TopNativeBannerAd : AdWrap, ATNativeBannerAdListener {
    private const string TAG = "TopNativeBannerAd";

    private readonly string adId;

    private bool isReady = false;

    public TopNativeBannerAd(string adId) {
        this.adId = adId;
        ATNativeBannerAd.Instance.setListener(this);
    }

    public override void loadAd() {
        
        if (string.IsNullOrEmpty(adId)) {
            return;
        }

        if (isAdReady()) {
            return;
        }
        isReady = false;
        Dictionary<string, string> jsonmap = new Dictionary<string, string>();
        ATNativeBannerAd.Instance.loadAd(adId, null);
        LogUtil.Log(TAG, ">>load");
    }

    public override void showAd() {
        ATRect arpuRect = new ATRect(0, 100, 414, 200);
        ATNativeBannerAd.Instance.showAd(adId, arpuRect,
            new Dictionary<string, string>
            {
                {ATNativeBannerAdShowingExtra.kATNativeBannerAdShowingExtraBackgroundColor, "#FFFFFF"},
                {ATNativeBannerAdShowingExtra.kATNativeBannerAdShowingExtraTitleColor, "#FF0000"}
            });
    }
    

    public override bool isAdReady() {
        return isReady;
    }

    public void onAdLoaded(string unitId) {
        LogUtil.Log(TAG, unitId + ">>onAdLoaded");
        isReady = true;
        onAdLoadSuccess?.Invoke(unitId);
    }

    public void onAdLoadFail(string unitId, string code, string message) {
        LogUtil.Log(TAG, unitId + ">>onAdLoadFail>>code:" + code + ">>>message:" + message);
        onAdLoadError?.Invoke(unitId, code, message);
    }

    public void onAdImpressed(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + ">>onAdImpressed");
        onAdLoadShow?.Invoke(unitId);
    }

    public void onAdClicked(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + ">>onAdClicked");
        onAdLoadClick?.Invoke(unitId);
    }

    public void onAdAutoRefresh(string unitId, ATCallbackInfo callbackInfo) {
        LogUtil.Log(TAG, unitId + ">>onAdAutoRefresh");
    }

    public void onAdAutoRefreshFailure(string unitId, string code, string message) {
        LogUtil.Log(TAG, unitId + ">>onAdAutoRefreshFailure");
    }

    public void onAdCloseButtonClicked(string unitId) {
        LogUtil.Log(TAG, unitId + ">>onAdCloseButtonClicked");
    }
}