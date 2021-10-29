using System;

public abstract class AdWrap {
    public Action<string> onAdLoadSuccess;

    public Action<string, string, string> onAdLoadError;

    public Action<string> onAdLoadShow;

    public Action<string> onAdLoadClick;

    public Action<string> onAdLoadClose;

    public Action<string> onAdLoadReward;


    public abstract void loadAd();

    public abstract void showAd();

    

    public abstract bool isAdReady();
    
    

    
}