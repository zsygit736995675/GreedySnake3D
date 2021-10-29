using System;
using ThinkingAnalytics;
using UnityEngine;

/**
 * 通过android 原生发送埋点
 */
public class AndroidSend
{

    private static AndroidJavaObject mCurrentActivity;

    private static AndroidJavaObject GetAndroidJavaObject()
    {
        if (mCurrentActivity == null)
        {
            //Unity要导出的MainActivity类
            AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            //获取MainActivity的实例对象
            mCurrentActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return mCurrentActivity;
    }

    public static void send(string msg)
    {
        try
        {
            GetAndroidJavaObject().Call("sendMsg", msg);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    /**
     * 发送带有用户id 的信息
     */
    public static void sendWithUserId(string msg)
    {
        try
        {
            GetAndroidJavaObject().Call("sendMsgWithUserId", ThinkingAnalyticsAPI.GetDistinctId() ?? SystemInfo.deviceUniqueIdentifier, msg);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    //通过原生 获取信息
    public static string getNative(string key)
    {
        string NativeValue = "";
        try
        {
            NativeValue = GetAndroidJavaObject().Call<string>("getNativeValue", key);
            //DotManager.Instance.sendEvent( NativeValue , DottingType.Tga);
            //return ;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Debug.Log("getNative:"+NativeValue);
        return NativeValue;
    }


    public static void WxLogin()
    {
        try
        {
            GetAndroidJavaObject().Call("loginWx");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static void onEventRegister(string registerWay, bool issucceed)
    {
        try
        {
            GetAndroidJavaObject().Call("onEventRegister", registerWay, issucceed);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static void requestPermission()
    {
        try
        {
            GetAndroidJavaObject().Call("requestPermission");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }


    public static void showToast(string msg)
    {
        try
        {
            GetAndroidJavaObject().Call("showToast", msg);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}