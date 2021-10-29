using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGameScene : MonoBehaviour
{
    private void Start()
    {
        //加载游戏场景
        StartCoroutine(loadingGame());
    }

    IEnumerator loadingGame()
    {
        AsyncOperation ao= SceneManager.LoadSceneAsync("Game");
        ao.allowSceneActivation = true;
        yield return null;
    }
}
