//-------------------------------------------
//作者：马超
//时间：2020-09-23 10:14
//作用：
//-------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Scene_Loading  : SceneBase
{
    Image pro_Slider;
    Transform pro_jinbi;
    float pro;

    bool isEnd = false;

    float targetPro=0;

    float loading = 40f;//Loading界面接口数

    Transform trs_bomm;

    Image img_icon;

    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=SceneName.Scene_Loading;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Scene/Scene_Loading");
    }
    public override void OnInit(params object[] sceneArgs)
    {
        base.OnInit(sceneArgs);
       
        InitData();
    }
    #endregion

    #region 数据定义

    #endregion

    #region 逻辑
    /// <summary>初始化</summary>
    private void InitData()
    {
        if (sceneArgs.Length!=0)
        {
        }
        FindObj();
        AddEvent();
    }

    private void loadingEvent()
    {
        EventMgrHelper.Ins.PushEvent(EventDef.LOADING); 
    }

    public override void HandleEvent(EventDef ev, LogicEvent le)
    {
        base.HandleEvent(ev, le);
        switch (ev)
        {
            case EventDef.LOADING:
                targetPro += 30f / loading;
                break;
            default:
                targetPro += 1f/loading;
                break;
        }
    }

    public override void OnShowing()
    {
        base.OnShowing();

        AndroidSend.requestPermission();
        img_icon.transform.localScale = Vector3.zero;

        if (PlatformManager.IsEditor) 
        {
            startLoading();
        }
    }

    /// <summary>查找物体</summary>
    private void FindObj()
    {
        img_icon = transform.SeachTrs<Image>("img_icon");
        pro_Slider = transform.SeachTrs<Image>("pro_Slider");
        pro_jinbi = transform.SeachTrs<Transform>("pro_jinbi");
        trs_bomm = transform.SeachTrs<Transform>("trs_bomm");
    }

    public void startLoading()
    {
        DOTween.To(() => img_icon.transform.localScale, x => img_icon.transform.localScale = x, Vector3.one, 0.5f).onComplete = () => {

            Invoke(nameof(loadingEvent), 0.1f);

            HttpManager.Instance.getConfig();

            HttpManager.Instance.login(() => {

                HttpManager.Instance.getNum(ConfigManager.NUM_50);
                HttpManager.Instance.getNum(ConfigManager.NUM_100);
                HttpManager.Instance.getNum(ConfigManager.NUM_120);
                HttpManager.Instance.getNum(ConfigManager.NUM_150);
                DataManager.setLoginTime();
                DataManager.setCreateTime();
                HttpManager.Instance.getWithDrawList();
                HttpManager.Instance.getScoreWithdraw();
                HttpManager.Instance.getScore();
                //刷新当前用户体现档次
                HttpManager.Instance.GetAmount(ac => { });
            });
        };
    }

    private void Update()
    {
        if (!isEnd)
        {
            if (pro < 1)
            {
                if (pro < targetPro) 
                {
                    pro += Time.deltaTime * 1;
                    pro_Slider.fillAmount = pro;
                    pro_jinbi.GetComponent<RectTransform>().anchoredPosition= new Vector3(pro_Slider.fillAmount * 878, 0, 0);
                }
            }
            else
            {
                isEnd = true;
                SceneMgr.GetInstance.SwitchingScene(SceneName.Scene_Main);
            }
        }
    }

    /// <summary>添加事件</summary>
    private void AddEvent()
    {
        RegisterEvent(EventDef.LOADING);
        RegisterEvent(EventDef.REQUEST_COFING);
        RegisterEvent(EventDef.REQUEST_100);
        RegisterEvent(EventDef.REQUEST_120);
        RegisterEvent(EventDef.REQUEST_150);
        RegisterEvent(EventDef.REQUEST_200);
        RegisterEvent(EventDef.REQUEST_LOGIN);
        RegisterEvent(EventDef.REQUEST_WITHDRAW_LIST);
        RegisterEvent(EventDef.REQUEST_SCORE_WITHDRAW);
        RegisterEvent(EventDef.REQUEST_SCORE);
        RegisterEvent(EventDef.REQUEST_AMOUNT);
    }

    /// <summary>按钮点击事件</summary>
    protected override void OnClick(Transform _target)
    {
        switch (_target.name)
        {
            case "Btn_ys":
                Application.OpenURL(SystemConfig.Get(1).ysxy);
                break;
            case "Btn_xy":
                Application.OpenURL(SystemConfig.Get(1).fwxy);
                break;
        }
    }
    #endregion
}
