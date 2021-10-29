public class AdUnitId {
#if UNITY_ANDROID || UNITY_EDITOR
    //插屏广告id
    public const string AD_ID_INTERSTITIAL = "b5fbca70719f6e";

    //激励视频id
    public const string AD_ID_REWARD = "b5fbca6bf42f7b";

    //BANNER
    public const string AD_ID_BANNER = "b5fbca716513bd";

    //native
    public const string AD_ID_NATIVE = "b5fbca72cb96a2";

    public const string APP_ID = "a5fbca6aa95663";

    public const string APP_KEY = "dcb025aeda75aa70184c57f6d7bd4ba8";

#elif UNITY_IOS
    //插屏广告id
    public const string AD_ID_INTERSTITIAL = "b5f57394aa9aa1";

    //激励视频id
    public const string AD_ID_REWARD = "b5f573935465e9";
    
    //替补插屏广告id
    public const string AD_ID_BACK_INTERSTITIAL = "b5f573958be7d4";

    //BANNER
    public const string AD_ID_BANNER = "b5f573969cca5e";
    
    //native
    public const string AD_ID_NATIVE = "b5f115a47275c8";

    //
    public const string APP_ID = "a5f57391594652";

    public const string APP_KEY = "5937c0aa2a9d9d3176897115e55a1fe4";

#endif
    //广告测试 打开后 不现实广告 直走逻辑
    public const bool AD_DEBUG = false;
}