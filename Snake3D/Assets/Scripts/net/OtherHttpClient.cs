using System;
using System.Collections;
using System.Text;
using LitJson;
using UnityEngine.Networking;

public class OtherHttpClient : SingletonObject<OtherHttpClient> {
    public void Start() {
        
    }

    public void sendCL() {
        long span = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        LogUtil.Log("time", span);
        StartCoroutine(postUrl(
            "https://api.e.qq.com/v1.1/user_actions/add?access_token=ffe83676140f9e850cd9d2820c2ad865&timestamp=" +
            span + "&nonce=" + span.ToString(), JsonMapper.ToJson(new OtherRequest())));
    }


    public void getToken(string code) {
        string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
            SystemConfig.Get(1).wxId,SystemConfig.Get(1).wxSecret,code); 
        LogUtil.Log("getToken", "code>>>>" +code);
        StartCoroutine(getUrl<WxModel>(url, (success, result) =>
        {
            LogUtil.Log("getToken", "getUrl>>>>" + result.errmsg);
            if (success) {
                HttpManager.Instance.bind(result.openid, result.access_token);
            }
            else {
            }
        }));
    }


    private IEnumerator postUrl(string url, string parameter = null) {
        UnityWebRequest request = null;
        request = new UnityWebRequest(url, "POST");
        if (parameter != null) {
            byte[] body = Encoding.UTF8.GetBytes(AesCbcUtil.Encrypt(parameter));
            request.uploadHandler = new UploadHandlerRaw(body);
            LogUtil.Log("other", parameter);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.responseCode == 200) {
            string res = request.downloadHandler.text;
            LogUtil.Log("http", "返回数据>>>>" + res);
        }
        else {
        }
    }


    private IEnumerator getUrl<T>(string url, Action<bool, T> callBack) where T : class {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        if (request.responseCode == 200) {
            string res = request.downloadHandler.text;
            LogUtil.Log("http", "返回数据>>>>" + res);
            callBack.Invoke(true, JsonMapper.ToObject<T>(res));
        }
        else {
            LogUtil.Log("http", "网络错误>>>>" + request.responseCode);
            callBack?.Invoke(false, null);
        }
    }
}