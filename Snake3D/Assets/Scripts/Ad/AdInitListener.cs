using System;
using AnyThinkAds.Api;
using UnityEngine;

namespace Ad {
    /**
 * 广告初始化回到类
 */
    public class AdInitListener : ATSDKInitListener {
        private const string TAG = "ATSDKInitListener";

        private readonly Action callBack;

        public AdInitListener(Action callBack) {
            this.callBack = callBack;
        }


        public void initSuccess() {
            LogUtil.Log(TAG, "ad init success");
            callBack?.Invoke();
        }

        public void initFail(string message) {
            LogUtil.Log(TAG, "ad init fail>>>" + message);
        }
    }
}