using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;

// This class is intended to be used the the AppsFlyerObject.prefab

public class AppsFlyerObjectScript : MonoBehaviour, IAppsFlyerConversionData
{

    // These fields are set from the editor so do not modify!
    //******************************//
    public string devKey;
    private string appID;
    public bool isDebug;
    public bool getConversionData;
    //******************************//

    void Start()
    {
#if UNITY_ANDROID
        appID = Application.identifier;
        AppsFlyerAndroid.setCollectIMEI(true);
        AppsFlyerAndroid.setCollectAndroidID(true);
        AppsFlyerAndroid.setCollectOaid(true);

#elif UNITY_IOS
        appID="1530792307";
#endif
        // These fields are set from the editor so do not modify!
        //******************************//
        AppsFlyer.setIsDebug(isDebug);
        AppsFlyer.initSDK(devKey, appID, getConversionData ? this : null);
        //******************************//

        AppsFlyer.startSDK();
    }

    void Update()
    {

    }
    
    private AfModel afModel
    {
        get { return ConfigManager.afModel; }
        set { ConfigManager.afModel = value; }
    }

    // Mark AppsFlyer CallBacks
    public void onConversionDataSuccess(string conversionData)
    {
        AppsFlyer.AFLog("didReceiveConversionData", conversionData);
        // add deferred deeplink logic here
        AppsFlyer.AFLog("didReceiveConversionData", conversionData);
        afModel = JsonUtility.FromJson<AfModel>(conversionData);
        //LogUtil.D("didReceiveConversionData>>>", afModel.af_status);
        LogUtil.Log("afModel.af_status>>>" , afModel.af_status.Length);
        //LogUtil.D("Organic>>>" , "Organic".Length);

        DotManager.Instance.UserSetOnce("user_media_source", afModel.media_source != null ? afModel.media_source : "未知");
        DotManager.Instance.UserSetOnce("af_status", afModel.af_status);
        DotManager.Instance.UserSetOnce("campaign", afModel.campaign);
        DotManager.Instance.UserSetOnce("campaign_id", afModel.campaign_id);
        DotManager.Instance.UserSetOnce("adset", afModel.adset);
        DotManager.Instance.UserSetOnce("adset_id", afModel.adset_id);
        DotManager.Instance.UserSetOnce("af_ad", afModel.af_ad);
        DotManager.Instance.UserSetOnce("ad_id", afModel.ad_id);
        DotManager.Instance.UserSetOnce("af_siteid", afModel.af_siteid);
        DotManager.Instance.UserSetOnce("user_media_source", afModel.user_media_source);

        if (afModel.af_status.Equals("Organic"))
        {
            if (!DataManager.IsOrganic()) return;
                         DataManager.SetOrganic(true);
        }
        else
        {
            DataManager.SetOrganic(false);
        }
       
    }

    public void onConversionDataFail(string error)
    {
        AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
    }

    public void onAppOpenAttribution(string attributionData)
    {
        AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
        Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
        // add direct deeplink logic here
    }

    public void onAppOpenAttributionFailure(string error)
    {
        AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
    }
}
