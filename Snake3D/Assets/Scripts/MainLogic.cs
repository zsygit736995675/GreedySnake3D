using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainLogic : MonoBehaviour
{
    public static  MainLogic _Instance;
      
    public ServerType ServerType = ServerType.DEBUG;

    public bool isTest;

    private void Awake()
    {
       // PlayerPrefs.DeleteAll();
        _Instance = this;
        DotManager.Instance.autoTrack();
        DotManager.Instance.sendEvent("start", DottingType.Tga);
        DataManager. StartCount += 1;
        DotManager.Instance.userSet("user_latest_login_time", DataManager.getLoginTime());

        ConfigMgr.Ins.Init();

        NetWorkeTemp.Ins.Start();

        AdManager.Ins.Init();

        HttpClient.Ins.ServerType = ServerType;
        AudioController.Ins.PlayBgm("ambient_futuristic"); 
        OtherHttpClient.Ins.Start();

        //不能放在awake  适配 会有问题
        StartGame();



    }

    void StartGame()
    {
        ScreenManager.Ins.TcsInit();
        GameObject.Find("BG").gameObject.SetActive(false);
        SceneMgr.GetInstance.SwitchingScene(SceneName.Scene_Loading);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Gold, RedType.common);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Gold, RedType.Newcomer);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
           
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
    }

    private void OnApplicationQuit()
    {
    }
}
