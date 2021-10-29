using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlatformType 
{
    Android,
    Ios,
    Editor,
    Win
}

public class PlatformManager 
{

    public static PlatformType Type 
    {
        get {
#if UNITY_ANDROID && !UNITY_EDITOR
		return PlatformType.Android;
#elif UNITY_IOS && !UNITY_EDITOR
		return PlatformType.Ios;
#elif UNITY_STANDALONE_WIN
        return PlatformType.Win;
#else
         return PlatformType.Editor;
#endif
        }
    }

    public static bool IsEditor
    {
        get 
        {
#if !UNITY_EDITOR
            return false;
#else
            return true;
#endif
        }
    }







}
