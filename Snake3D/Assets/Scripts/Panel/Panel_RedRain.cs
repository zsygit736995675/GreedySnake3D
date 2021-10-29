//-------------------------------------------
//作者：马超
//时间：2020-11-10 17:04
//作用：
//-------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Numerics;

public class Panel_RedRain  : PanelBase
{

    List<Transform> redsTrs = new List<Transform>();

    Text countDown;

    Transform center;

    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=PanelName.Panel_RedRain;
        _openDuration=0.5f;
        _alpha=0.85f;
        _showStyle=PanelMgr.PanelShowStyle.Nomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache=false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_RedRain");
    }
    public override void OnInit(params object[] panelArgs)
    {
        base.OnInit(panelArgs);
        InitData();
    }
    #endregion

    #region 数据定义

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
        for (int i = 0; i < 5; i++)
        {
            redsTrs.Add(skinTrs.SeachTrs<Transform>("Image" + (i + 1)));
        }
        countDown = skinTrs.SeachTrs<Text>("countDown");

        center = skinTrs.SeachTrs<Transform>("center");
    }

    float countTimer1;
    float countTimer2;
    public override void OnShowing()
    {
        base.OnShowing();
        countTimer1 = 4; //红包雨开启倒计时
        countTimer2 = 11; //红包雨结束倒计时
        StartCoroutine(countTimer1Time());
    }

    public IEnumerator countTimer1Time()
    {
        while (true)
        {
            countTimer1 -= Time.deltaTime;
            countDown.text = ((int)countTimer1).ToString();
            if (countTimer1 <= 0)
            {
                StartCoroutine(countTimer2Time());
                center.gameObject.SetActive(false);
                InvokeRepeating("refreshHongBao",0,0.4f);
                //关闭中间部件
                yield break;
            }
            yield return 0;
        }
    }
    public IEnumerator countTimer2Time()
    {
        while (true)
        {
            countTimer2 -= Time.deltaTime;
            countDown.text = ((int)countTimer2).ToString();
            if (countTimer2 <= 0)
            {
                //弹出红包
                PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Gold,RedType.hby);
                //关闭页面
                Close();
                yield break;
            }
            yield return 0;
        }
    }

    /// <summary>
    /// 刷出红包
    /// </summary>
    public void refreshHongBao()
    {
        int range = UnityEngine.Random.Range(0, 5);
        GameObject rainRed = GameObject.Instantiate(
            Resources.Load<GameObject>("Game/rain_red"),redsTrs[range]);
        rainRed.transform.Rotate(new UnityEngine.Vector3(0,0,Random.Range(-20,21)));

        rainRed.AddComponent<Button>();
        rainRed.GetComponent<Button>().onClick.AddListener(delegate() {
            redClick(rainRed.transform);
        });

        rainRed.transform.DOLocalMoveY(-4500,6f).onComplete = () =>
        {
            Destroy(rainRed);
        };
    }

    public void redClick(Transform trans)
    {
        //生成红包点击特效
        GameObject ps = GameObject.Instantiate(Resources.Load<GameObject>("Game/HongBaoClick"), trans.position , UnityEngine.Quaternion.identity,trans.parent);
        Destroy(ps.gameObject, 3);
        Destroy(trans.gameObject);
    }

    /// <summary>添加事件</summary>
    private void AddEvent()
    {
        
    }

    /// <summary>按钮点击事件</summary>
    protected override void OnClick(Transform _target)
    {
        switch (_target.name)
        {
            case "Btn_Close":
                Close();
                break;
        }
    }
    #endregion
}
