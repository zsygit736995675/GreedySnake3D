//-------------------------------------------
//作者：马超
//时间：2020-11-20 17:40
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel_Pause  : PanelBase
{
    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=PanelName.Panel_Pause;
        _openDuration=0.5f;
        _alpha=0.7f;
        _showStyle=PanelMgr.PanelShowStyle.Nomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache=false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_Pause");
    }
    public override void OnInit(params object[] panelArgs)
    {
        base.OnInit(panelArgs);
        InitData();
    }
    #endregion

    #region 数据定义
    Text txt_level;

    Transform Btn_up, Btn_next;
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
        txt_level = skinTrs.SeachTrs<Text>("txt_level");
        txt_level.text  = string .Format( "第{0}关",Snake_Game.Ins.currentLevel);

        Btn_up = skinTrs.SeachTrs<Transform>("Btn_up");
        Btn_next = skinTrs.SeachTrs<Transform>("Btn_next");


        Btn_up.GetChild(0).gameObject.SetActive(Snake_Game.Ins.currentLevel <= 1);
        Btn_up.GetComponent<Button>().enabled = Snake_Game.Ins.currentLevel > 1;
        Btn_next.GetChild(0).gameObject.SetActive(Snake_Game.Ins.currentLevel >= DataManager.getLevelNum()||Snake_Game.Ins.currentLevel>=ConfigManager.Max_Level);
        Btn_next.GetComponent<Button>().enabled = Snake_Game.Ins.currentLevel < DataManager.getLevelNum() && Snake_Game.Ins.currentLevel < ConfigManager.Max_Level;

        if (Btn_up.GetComponent<Button>().enabled)
        {
            Btn_up.GetComponentInChildren<Outline>().effectColor = new Color(0.03529412f, 0.682353f, 0.2901961f,1);
        }
        else 
        {
            Btn_up.GetComponentInChildren<Outline>().effectColor = new Color(0.5882353f, 0.5882353f, 0.5882353f,1);
        }

        if (Btn_next.GetComponent<Button>().enabled)
        {
            Btn_next.GetComponentInChildren<Outline>().effectColor = new Color(0.03529412f, 0.682353f, 0.2901961f,1);
        }
        else
        {
            Btn_next.GetComponentInChildren<Outline>().effectColor = new Color(0.5882353f, 0.5882353f, 0.5882353f, 1);
        }
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
            case "Btn_jx":
                Snake_Game.Ins.isPause = false;
                Close();
                break;
                      
            case "Btn_restart":
                Close();

                Snake_Game.Ins.OpenLevel(Snake_Game.Ins.currentLevel);

                break;
            case "Btn_level":

                Close();
                PanelMgr.GetInstance.ShowPanel(PanelName.Panel_LevelList);

                break;
            case "Btn_up":
                Snake_Game.Ins.OpenLevel(Snake_Game.Ins.currentLevel - 1);
                Close();
                break;
            case "Btn_next":
                Snake_Game.Ins.OpenLevel(Snake_Game.Ins.currentLevel + 1);
                Close();
             
                break;
            case "Btn_back":

                Snake_Game.Ins.BackMain();
                Close();

                break;
                
        }
    }
    #endregion
}
