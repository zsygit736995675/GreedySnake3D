using System.Collections.Generic;
using AnyThinkAds.Api;

public class WrapATInterstitialAd : ATInterstitialAdListener {
    public static WrapATInterstitialAd Instance { get; } = new WrapATInterstitialAd();

    private readonly Dictionary<string, ATInterstitialAdListener>
        mListeners = new Dictionary<string, ATInterstitialAdListener>();

    private WrapATInterstitialAd() {
        ATInterstitialAd.Instance.setListener(this);
    }

    public void loadInterstitialAd(string unitId, Dictionary<string, string> pairs) {
        ATInterstitialAd.Instance.loadInterstitialAd(unitId, pairs);
    }

    public void setListener(ATInterstitialAdListener listener, string unitId) {
        if (!mListeners.ContainsKey(unitId)) {
            mListeners[unitId] = listener;
        }
    }

    public bool hasInterstitialAdReady(string unitId) {
        return ATInterstitialAd.Instance.hasInterstitialAdReady(unitId);
    }

    public void showInterstitialAd(string unitId) {
        ATInterstitialAd.Instance.showInterstitialAd(unitId);
    }

    public void showInterstitialAd(string unitId, Dictionary<string, string> pairs) {
        ATInterstitialAd.Instance.showInterstitialAd(unitId, pairs);
    }

    public void onInterstitialAdLoad(string unitId) {
        mListeners[unitId]?.onInterstitialAdLoad(unitId);
    }

    public void onInterstitialAdLoadFail(string unitId, string code, string message) {
        mListeners[unitId]?.onInterstitialAdLoadFail(unitId, code, message);
    }

    public void onInterstitialAdShow(string unitId, ATCallbackInfo callbackInfo) {
        mListeners[unitId]?.onInterstitialAdShow(unitId, callbackInfo);
    }

    public void onInterstitialAdFailedToShow(string unitId) {
        mListeners[unitId]?.onInterstitialAdFailedToShow(unitId);
    }

    public void onInterstitialAdClose(string unitId, ATCallbackInfo callbackInfo) {
        mListeners[unitId]?.onInterstitialAdClose(unitId, callbackInfo);
    }

    public void onInterstitialAdClick(string unitId, ATCallbackInfo callbackInfo) {
        mListeners[unitId]?.onInterstitialAdClick(unitId, callbackInfo);
    }

    public void onInterstitialAdStartPlayingVideo(string unitId, ATCallbackInfo callbackInfo) {
        mListeners[unitId]?.onInterstitialAdStartPlayingVideo(unitId, callbackInfo);
    }

    public void onInterstitialAdEndPlayingVideo(string unitId, ATCallbackInfo callbackInfo) {
        mListeners[unitId]?.onInterstitialAdEndPlayingVideo(unitId, callbackInfo);
    }

    public void onInterstitialAdFailedToPlayVideo(string unitId, string code, string message) {
        mListeners[unitId]?.onInterstitialAdFailedToPlayVideo(unitId, code, message);
    }
}