using UnityEngine;
/**
 * 打印日志的工具类
 * 加一层判断 正式包 设置为false
 */
public class LogUtil
{
    private static bool isLog = true;

    //打印日志
    public static void Log(string tag, object msg)
    {
        if (isLog)
        {
            Debug.Log(tag + ">>>>" + msg);
        }
    }

    public static void Warn(string tag, object msg)
    {
        if (isLog)
        {
            Debug.LogWarning(tag + ">>>>" + msg);
        }
    }
    public static void Error(string tag, object msg)
    {
        if (isLog)
        {
            Debug.LogError(tag + ">>>>" + msg);
        }
    }

    public static void setLogDebug(bool log)
    {
        isLog = log;
    }
}