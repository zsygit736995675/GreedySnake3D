using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;

/**
 * http请求处理
 */
public class HttpClient : SingletonObject<HttpClient> {
    private const string TAG = "HttpClient";

    private const string KEY_COOKIE = "key_cookie"; //保存cookie

    public ServerType ServerType = ServerType.DEBUG;

    private string BaseUrl 
    {
        get
        {
            switch (ServerType)
            {
                case ServerType.LOCAL:
                    return HttpConstant.XF_LOCAL;
                case ServerType.DEBUG:
                    return HttpConstant.DEBUG_URL;
                case ServerType.RELEASE:
                    return HttpConstant.RELEASE_URL;
            }
            return "";
        }
    }


    public void GET<T>(string url, Action<bool, T> callBack, Func<string, string> decryptFunc = null) where T : class {
        url = checkUrl(url);
        StartCoroutine(get(url, callBack, decryptFunc));
    }

    public void POST<T>(string url, Action<bool, T> callBack, string param = null,
        Dictionary<string, string> headers = null, Func<string, string> decryptFunc = null) where T : class {
        url = checkUrl(url);
        StartCoroutine(postAndPut("POST", url, callBack, param, headers, decryptFunc));
    }

    public void PUT<T>(string url, Action<bool, T> callBack, string param = null,
        Dictionary<string, string> headers = null, Func<string, string> decryptFunc = null) where T : class {
        url = checkUrl(url);
        StartCoroutine(postAndPut("PUT", url, callBack, param, headers, decryptFunc));
    }

    private string checkUrl(string url) {
        if (url.Contains("http")) {
            return url;
        }

        if (url.StartsWith("/")) {
            url = url.Substring(1);
        }

        if (!BaseUrl.EndsWith("/")) {
            url = $"/{url}";
        }

        return $"{BaseUrl}{url}";
    }

    //get请求方法
    private IEnumerator get<T>(string url, Action<bool, T> callBack, Func<string, string> decryptFunc = null) where T : class {

        LogUtil.Log(TAG, $"http>>get请求:{url}");
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        LogUtil.Log(TAG, $"responseCode:{request.responseCode}");
        if (request.responseCode == 200) {
            string res = decryptFunc == null  ? request.downloadHandler.text : decryptFunc.Invoke(request.downloadHandler.text);
            LogUtil.Log(TAG, $"get请求:{url}>>返回值:{res}");
            if (typeof(T).IsAssignableFrom(typeof(string))) {
                callBack?.Invoke(true, res as T);
            }
            else {
                try {
                    BaseModel<T> response = JsonMapper.ToObject<BaseModel<T>>(res);
                    if (response.code == 200) {
                        callBack?.Invoke(true, response.data);
                    }
                    else {
                        Debug.Log($"get请求:{url}>>错误:{response.code}>>message:{response.msg}");
                        LogUtil.Log(TAG, $"get请求:{url}>>错误:{response.code}>>message:{response.msg}");
                        callBack?.Invoke(false, null);
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    LogUtil.Log(TAG, $"get请求:{url}>>解析错误");
                }
            }
        }
        else {
            callBack?.Invoke(false, null);
        }
    }


    //post请求方法
    private IEnumerator postAndPut<T>(string method, string url, Action<bool, T> callBack, string param = null,
        Dictionary<string, string> headers = null, Func<string, string> decryptFunc = null) where T : class {
        LogUtil.Log(TAG, $"http>>{method}请求:{url}");
        UnityWebRequest request = new UnityWebRequest(url, method);
        if (param != null) {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(param));
        }

        //添加头
        if (headers != null) {
            foreach (string headersKey in headers.Keys) {
                request.SetRequestHeader(headersKey, headers[headersKey]);
            }
        }

        string cookie = PlayerPrefs.GetString(KEY_COOKIE, "");
        if (!string.IsNullOrEmpty(cookie)) {
            request.SetRequestHeader("Set-Cookie", cookie);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        LogUtil.Log(TAG, $"responseCode:{request.responseCode}");
        if (request.responseCode == 200) {
            string res = decryptFunc != null
                ? decryptFunc.Invoke(request.downloadHandler.text)
                : request.downloadHandler.text;
            if (request.GetResponseHeaders().ContainsKey("Set-Cookie"))
            {
                PlayerPrefs.SetString(KEY_COOKIE, request.GetResponseHeaders()["Set-Cookie"]);
            }
            LogUtil.Log(TAG, $"{method}请求:{url}>>返回值:{res}");
            if (typeof(T).IsAssignableFrom(typeof(string))) {
                callBack?.Invoke(true, res as T);
            }
            else {
                try {
                    BaseModel<T> response = JsonMapper.ToObject<BaseModel<T>>(res);
                    if (response.code == 200) {
                        callBack?.Invoke(true, response.data);
                    }
                    else {
                        LogUtil.Log(TAG, $"{method}请求:{url}>>错误:{response.code}>>message:{response.msg}");
                        callBack?.Invoke(false, null);
                    }
                }
                catch (Exception e) {
                    LogUtil.Log(TAG, $"{method}请求:{url}>>解析错误>>>去干后台");
                    Console.WriteLine(e);
                    //TODO  等待验证是否有问题
                    try {
                        T response = JsonMapper.ToObject<T>(res);
                        callBack?.Invoke(true, response);
                    }
                    catch (Exception exception) {
                        Console.WriteLine(exception);
                        LogUtil.Log(TAG, $"{method}请求:{url}>>彻底解析错误");
                        callBack?.Invoke(false, null);
                    }
                }
            }
        }
        else {
            callBack?.Invoke(false, null);
        }
    }

    
}