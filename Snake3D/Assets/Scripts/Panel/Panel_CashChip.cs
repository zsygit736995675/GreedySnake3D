//-------------------------------------------
//作者：马超
//时间：2020-11-11 16:24
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel_CashChip  : PanelBase
{

    Transform center;

    int showNumber; //展示面额编号

    ToggleGroup toggles;
    Toggle[] tgs;

    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=PanelName.Panel_CashChip;
        _openDuration=0.5f;
        _alpha=0.8f;
        _showStyle=PanelMgr.PanelShowStyle.Nomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache=false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_CashChip");
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

    public override void OnShowing()
    {
        base.OnShowing();
        showNumber = 1; //默认展示一元
        //红点控制
        PlayerPrefs.SetInt("key_chip_hongdian_is_on", 1);
        updateUI();
    }

    public void updateUI()
    {
        //清子
        DataManager.clearChildrens(center);
        //刷新面值碎片 showNumber (当前面值)
        List<int> listInt= DataManager.getUserMoneyFaceValue(showNumber);
        string resPathName = DataManager.getChipResourcesName(showNumber); //当前面值图片路径
        GameObject type = null;
        switch (showNumber)
        {
            case 1:
                type = GameObject.Instantiate(Resources.Load<GameObject>("Game/pingtu/type1"), center);
                break;
            case 5:
                type = GameObject.Instantiate(Resources.Load<GameObject>("Game/pingtu/type1"), center);
                break;
            case 10:
                type = GameObject.Instantiate(Resources.Load<GameObject>("Game/pingtu/type2"), center);
                break;
            case 20:
                type = GameObject.Instantiate(Resources.Load<GameObject>("Game/pingtu/type2"), center);
                break;
            case 50:
                type = GameObject.Instantiate(Resources.Load<GameObject>("Game/pingtu/type3"), center);
                break;
            case 100:
                type = GameObject.Instantiate(Resources.Load<GameObject>("Game/pingtu/type3"), center);
                break;
        }
        type.transform.SeachTrs<Text>("title_text").text = ""+ showNumber + "元";
        //刷出碎片
        for (int i = 0; i < listInt.Count; i++)
        {
            type.transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>(resPathName + "bg_" + i);
            //type.transform.GetChild(i).Find("bg").GetComponent<Image>().sprite = Resources.Load<Sprite>(resPathName + "bg_" + i);
            if (listInt[i] <= 0)
            {
                DataManager.ImageGray(type.transform.GetChild(i).GetComponent<Image>(),true);
            }
        }
    }

    /// <summary>查找物体</summary>
    private void FindObj()
    {
        center = skinTrs.SeachTrs<Transform>("center");

        toggles = skinTrs.SeachTrs<ToggleGroup>("MoneyGrop");

        tgs = toggles.GetComponentsInChildren<Toggle>();

        for (int i = 0; i < tgs.Length; i++)
        {
            tgs[i].onValueChanged.AddListener((bool value) => {
                onChangesToggle();
            });
        }
    }

    /// <summary>
    /// toggle选择
    /// </summary>
    /// <param name="number"></param>
    private void onChangesToggle()
    {
        int number = 0;
        for (int i = 0; i < tgs.Length; i++)
        {
            if (tgs[i].isOn == true)
            {
                number = i;
                tgs[i].transform.SeachTrs<Text>("money_text").fontSize = 44;
                tgs[i].transform.SeachTrs<Transform>("money_img").DOScale(new Vector3(1.1f,1.1f,1.1f),0.5f);
            }
            else
            {
                tgs[i].transform.SeachTrs<Text>("money_text").fontSize = 40;
                tgs[i].transform.SeachTrs<Transform>("money_img").localScale = Vector3.one;
            }
        }
        switch (number)
        {
            case 0:
                showNumber = 1;
                break;
            case 1:
                showNumber = 5;
                break;
            case 2:
                showNumber = 10;
                break;
            case 3:
                showNumber = 20;
                break;
            case 4:
                showNumber = 50;
                break;
            case 5:
                showNumber = 100;
                break;
        }
        updateUI();
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
