using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScene : MonoBehaviour
{

    

    public void startLoading()
    {
        //加载游戏场景
        AsyncOperation ao = SceneManager.LoadSceneAsync("Loading");
        ao.allowSceneActivation = true;
    }

   
}
