using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : SingletonObject<ScreenManager>
{
    [Header("游戏的有效内容宽度")]
    public float vaildWidth = 5;


    public void Init() 
    {
        float aspectRatio = Screen.width * 1f / Screen.height;
        float orthographicSize = vaildWidth / aspectRatio / 2;
        Camera.main.orthographicSize = orthographicSize;
    }

    public void TcsInit() 
    {
        float aspectRatio = Screen.width * 1f / Screen.height;
        if (aspectRatio > 0.5f)
        {
            Camera.main.orthographicSize = 5;
        }
        else 
        {
            Camera.main.orthographicSize = 6;
        }
    }

}
