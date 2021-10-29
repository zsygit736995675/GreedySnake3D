//-------------------------------------------
//作者：马超
//时间：2020-11-03 17:31
//作用：
//-------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel_Grab  : PanelBase
{
    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=PanelName.Panel_Grab;
        _openDuration=0.5f;
        _alpha=0f;
        _showStyle=PanelMgr.PanelShowStyle.Nomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache=false;
    }

    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_Grab");
    }
    public override void OnInit(params object[] panelArgs)
    {
        base.OnInit(panelArgs);
        InitData();
    }
    #endregion

    #region 数据定义
    Text txt_second1;
    Text txt_second2;
   
    Image img_hb;
    Image trs_hbItem;

    bool isStart = false;

    float totalTimer = 5;
    float timeleft=0;
    float timing = 0;

    Vector3 oripos;
    #endregion

    #region 逻辑
    /// <summary>初始化</summary>
    private void InitData()
    {
        if (panelArgs.Length!=0)
        {
        }
        FindObj();
        AddEvent();
    }

    /// <summary>查找物体</summary>
    private void FindObj()
    {
        txt_second1 = skinTrs.SeachTrs<Text>("txt_second1");
        txt_second2 = skinTrs.SeachTrs<Text>("txt_second2");
        img_hb =skinTrs.SeachTrs<Image>("img_hb");
        trs_hbItem = skinTrs.SeachTrs<Image>("trs_hbItem");
        trs_hbItem.gameObject.SetActive(false);
        oripos = new Vector3(0, img_hb.transform. localPosition.y, 0);
    }

    /// <summary>添加事件</summary>
    private void AddEvent()
    {
    }

    public override void OnShowing()
    {
        base.OnShowing();

        isStart = false;
        timeleft = totalTimer;
        timing = 0;
        UpdateUI();
    }

    void UpdateUI() 
    {
        txt_second1.text = ((int)(timeleft / 10)).ToString();
        txt_second2.text = ((int)(timeleft % 10)).ToString();
    }

    void Click() 
    {
        if (!isStart)
            isStart = true;

        img_hb.transform.localPosition = new Vector3(0, oripos.y, 0);
        img_hb.transform.DOLocalJump(oripos, 50,1,0.1f).onComplete=()=> {

            img_hb.transform.localPosition = new Vector3(0, oripos.y, 0);
        };

        Transform fly = Instantiate(trs_hbItem.gameObject,trs_hbItem.transform.parent).transform;
        fly.localScale = Vector3.one;
        fly.localPosition = new Vector3(0,img_hb.transform.localPosition.y+100,0);
        fly.gameObject.SetActive(true);
        float x = UnityEngine.Random.Range(-200 ,201);
        float y = UnityEngine.Random.Range(200,300);

        fly.transform.DOScale(new Vector3(1.5f, 1.5f), 0.5f);
        fly.transform.DOLocalMove(new Vector3(x,y),0.5f).onComplete=()=>{
            fly.GetComponent<Image>().DOColor(new Color(1,1,1,0),0.5f).onComplete =()=> {
                Destroy(fly.gameObject);
            };
        };
    }

    private void Update()
    {
        if (isStart)
        {
            if (timeleft > 0)
            {
                timing += Time.deltaTime;
                if (timing >= 1) 
                {
                    timeleft--;
                    timing = 0;
                    UpdateUI();
                }
            }
            else 
            {
                isStart = false;
                EndGame();
            }
        }
    }

    void EndGame() 
    {
        Close();

        if(timeleft<=0)
            PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Gold,RedType.bonuspool);
    }

    protected override void Close()
    {
        base.Close();
        EventMgrHelper.Ins.PushEvent(EventDef.CloseGrab);
    }

    /// <summary>按钮点击事件</summary>
    protected override void OnClick(Transform _target)
    {
        switch (_target.name)
        {
            case "Btn_Close":
                Close();
                break;
            case "Btn_click":
                Click();
                break;
        }
    }
    #endregion
}
