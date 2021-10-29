using System.Collections.Generic;
using AnyThinkAds.Api;
using UnityEngine;

public class TopNativeAd : NativeAdWrap, ATNativeAdListener
{
    private const string TAG = "TopNativeAd";

    private readonly string adId;

    private ATNativeAdView mNativeAdView;

    private bool isLoading = false;

    private int adWidth = 912;

    private int adHeight = 700;
    
    private int newAdWidth;

    private int newAdHeight;


    public TopNativeAd(string adId)
    {
        this.adId = adId;
        LogUtil.Log("Ad", "TopNativeAd:" + adId);
        ATNativeAd.Instance.setListener(this);
    }


    public override void loadAd()
    {
        if (string.IsNullOrEmpty(adId))
        {
            return;
        }

        if (isLoading)
        {
            return;
        }

        GameObject gameObject=GameObject.Find("Canvas");
        RectTransform rect1 = gameObject.transform.GetComponent<RectTransform>();

        float scale = rect1.sizeDelta.x / Screen.width;

        newAdWidth = (int)(adWidth / scale);
        newAdHeight = (int)(adHeight / scale);
        Dictionary<string, object> jsonmap = new Dictionary<string, object>();
        ATSize nativeSize = new ATSize(newAdWidth, newAdHeight, true);
#if UNITY_ANDROID
        jsonmap.Add(ATNativeAdLoadingExtra.kATNativeAdLoadingExtraNativeAdSizeStruct, nativeSize);
#elif UNITY_IOS || UNITY_IPHONE
        jsonmap.Add(ATNativeAdLoadingExtra.kATNativeAdLoadingExtraNativeAdSizeStruct, nativeSize);
#endif
        
        ATNativeAd.Instance.loadNativeAd(adId, jsonmap);
        isLoading = true;
        //DotManager.Instance.sendEvent(DotConstant.AD_NATIVE_REQUEST, DottingType.Tga);
    }


    public override void showAd()
    {
    }

    public override void showNativeAd(int height)
    {
        ATNativeAdView anyThinkNativeAdView = new ATNativeAdView(getNativeConfig(height));
        mNativeAdView = anyThinkNativeAdView;
        Debug.Log("Developer renderAdToScene--->");
        ATNativeAd.Instance.renderAdToScene(adId, anyThinkNativeAdView);
    }

    public override void removeAd()
    {
        if (mNativeAdView == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(adId))
        {
            return;
        }

        ATNativeAd.Instance.cleanAdView(adId, mNativeAdView);
        mNativeAdView = null;
    }

    private ATNativeConfig getNativeConfig(int shift)
    {
        GameObject gameObject=GameObject.Find("Canvas");
        RectTransform rect1 = gameObject.transform.GetComponent<RectTransform>();

        float scale = rect1.sizeDelta.x / Screen.width;
        
       // shift = (int)(170 / scale);
        ATNativeConfig conifg = new ATNativeConfig();
        string bgcolor = "#ffffff";
        string textcolor = "#000000";
        int rootbasex = (UnityEngine.Screen.width - newAdWidth) / 2,
            rootbasey = UnityEngine.Screen.height - newAdHeight -shift;
        
        LogUtil.Log(TAG,"GetComponent<Camera>().orthographicSize:"+adWidth);
        LogUtil.Log(TAG,"GetComponent<Camera>().orthographicSize:"+adHeight);
        LogUtil.Log(TAG,"GetComponent<Camera>().orthographicSize:"+newAdWidth);
        LogUtil.Log(TAG,"GetComponent<Camera>().orthographicSize:"+newAdHeight);
        LogUtil.Log(TAG,"adWidth:"+rootbasex);
        LogUtil.Log(TAG,"adHeight:"+rootbasey);
        
        int x = rootbasex, y = rootbasey, textsize = 15;

        conifg.parentProperty = new ATNativeItemProperty(x, y, newAdWidth, newAdHeight, bgcolor, textcolor, textsize, true);
        int width = newAdWidth, height = newAdHeight;

        int space = 0;
        bgcolor = "clearColor";

        //adlogo 
        x = 0 * 3;
        y = 0 * 3;
        width = 30 * 3;
        height = 20 * 3;
        textsize = 15;
        conifg.adLogoProperty = new ATNativeItemProperty(x, y, width, height, bgcolor, textcolor, textsize, true);

        //adicon
        x = space;
        y = (int)(0.667f * newAdHeight + space);
        width = (int)(0.33f * newAdHeight - 2 * space);
        height = width;
        textsize = 15;
        conifg.appIconProperty = new ATNativeItemProperty(x, y, width, height, bgcolor, textcolor, textsize, true);

        //ad cta 

        width = (int)(0.33f * newAdHeight - 2 * space) * 2;
        x = newAdWidth - width;
        height = (int)(0.33f * newAdHeight - 2 * space);
        y = newAdHeight - height;
        textsize = 15;
        conifg.ctaButtonProperty = new ATNativeItemProperty(x, y, width, height, "#ff21bcab", "#ffffff", textsize, true);

        //ad desc
        x = (int)(0.33f * newAdHeight);
        y = (int)(0.667f * newAdHeight + space) + (int)(0.33f * 0.33f * newAdHeight);
        width = newAdWidth - x - 2 * space - width;
        height = (int)(0.667f * 0.33f * newAdHeight);
        textsize = 10;
        conifg.descProperty = new ATNativeItemProperty(x, y, width, height, bgcolor, "#777777", textsize, true);


        //ad image
        x = space;
        y = space;
        width = newAdWidth - 2 * space;
        height = (int)(0.667f * newAdHeight) - 6;
        textsize = 15;
        conifg.mainImageProperty = new ATNativeItemProperty(x, y, width, height, bgcolor, textcolor, textsize, true);

        //ad title 
        x = (int)(0.33f * newAdHeight);
        y = (int)(0.667f * newAdHeight + space);
        width = newAdWidth - x - 2 * space - (int)(0.33f * newAdHeight - 2 * space) * 2;
        height = (int)(0.33f * 0.33f * newAdHeight);
        textsize = 12;
        conifg.titleProperty = new ATNativeItemProperty(x, y, width, height, bgcolor, textcolor, textsize, true);
        
        return conifg;
    }


    public override bool isAdReady()
    {
        return ATNativeAd.Instance.hasAdReady(adId);
    }

    public void onAdLoaded(string unitId)
    {
        LogUtil.Log(TAG, unitId + ">>onAdLoaded");
        isLoading = false;
        onAdLoadSuccess?.Invoke(unitId);
        //DotManager.Instance.sendEvent(DotConstant.AD_NATIVE_FILL, DottingType.Tga);
    }

    public void onAdLoadFail(string unitId, string code, string message)
    {
        LogUtil.Log(TAG, unitId + ">>onAdLoadFail>>code:" + code + ">>>message:" + message);
        isLoading = false;
        onAdLoadError?.Invoke(unitId, code, message);
        DotManager.Instance.sendEvent("ad_show_butnone", DottingType.Tga, new Dictionary<string, object> { { "ad_type", "Native" }, { "Location", Ad_Location.showType } });
    }

    public void onAdImpressed(string unitId, ATCallbackInfo callbackInfo)
    {
        LogUtil.Log(TAG, unitId + ">>onAdImpressed");
        onAdLoadShow?.Invoke(unitId);
      
       // DotManager.Instance.sendEvent(DotConstant.AD_NATIVE_SHOW, DottingType.Tga);
        DotManager.Instance.sendILRD(callbackInfo);
    }

    public void onAdClicked(string unitId, ATCallbackInfo callbackInfo)
    {
        LogUtil.Log(TAG, unitId + ">>onAdClicked");
        onAdLoadClick?.Invoke(unitId);
    }

    public void onAdVideoStart(string unitId)
    {
        DotManager.Instance.sendEvent("ad_show", DottingType.Tga,new Dictionary<string, object> { { "ad_type", "Native" }, { "Location", Ad_Location.showType } });
        LogUtil.Log(TAG, unitId + ">>onAdVideoStart");
    }

    public void onAdVideoEnd(string unitId)
    {
        LogUtil.Log(TAG, unitId + ">>onAdVideoEnd");
    }

    public void onAdVideoProgress(string unitId, int progress)
    {
        LogUtil.Log(TAG, unitId + ">>onAdVideoProgress");
    }

    public void onAdCloseButtonClicked(string unitId, ATCallbackInfo callbackInfo)
    {
        LogUtil.Log(TAG, unitId + ">>onAdCloseButtonClicked");
    }
}