//-------------------------------------------
//作者：马超
//时间：2020-11-20 16:38
//作用：
//-------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel_LevelList  : PanelBase
{
    #region 界面加载

    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type=PanelName.Panel_LevelList;
        _openDuration=0.5f;
        _alpha=0f;
        _showStyle=PanelMgr.PanelShowStyle.Nomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式
        _cache=false;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Panel/Panel_LevelList");
    }
    public override void OnInit(params object[] panelArgs)
    {
        base.OnInit(panelArgs);
        InitData();
    }
    #endregion

    #region 数据定义
    Transform item, Content;
    List<LevelListItem> items=new List<LevelListItem>();
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

    protected override void Close()
    {
        base.Close();

        Snake_Game.Ins.isPause = false;
    }

    /// <summary>查找物体</summary>
    private void FindObj()
    {
        item = skinTrs.SeachTrs<Transform>("item");
        Content = skinTrs.SeachTrs<Transform>("Content");
    }

    /// <summary>添加事件</summary>
    private void AddEvent()
    {
    }

    public override void OnShowing()
    {
        base.OnShowing();

        item.gameObject.SetActive(false);
        for (int i = 1; i <= ConfigManager.Max_Level; i++)
        {
            GameObject go = Instantiate<GameObject>(item.gameObject, Content);
            go.transform.localScale = Vector3.one;
            LevelListItem level = go.AddComponent<LevelListItem>();
            level.Init(i);
        }
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

class LevelListItem :MonoBehaviour
{
    Image img_lock, img_by, img_unlock, img_rot;

    Text txt_level;

    Button Btn_enter;

    Transform xings;

    int level;

    public void Init(int level) 
    {
        this.level = level;
        gameObject.SetActive(true);
        img_lock = transform.SeachTrs<Image>("img_lock");
        img_by = transform.SeachTrs<Image>("img_by");
        img_unlock = transform.SeachTrs<Image>("img_unlock");
        txt_level = transform.SeachTrs<Text>("txt_level");
        Btn_enter = transform.SeachTrs<Button>("Btn_enter");
        xings = transform.SeachTrs<Transform>("xings");
        img_rot = transform.SeachTrs<Image>("img_rot");

        int currentl = DataManager.getLevelNum();

        img_lock.gameObject.SetActive(level>currentl);
        img_by.gameObject.SetActive(level < currentl);
        img_unlock.gameObject.SetActive(level==currentl);
        img_rot.gameObject.SetActive(level == currentl);
        txt_level.text = level.ToString();
        xings.gameObject.SetActive(level < currentl);

        Btn_enter.onClick.AddListener(()=> {

            if (level <= currentl) 
            {
                Snake_Game.Ins.OpenLevel(level);
                EventMgrHelper.Ins.PushEvent(EventDef.EnterLevel,level);
                PanelMgr.GetInstance.HidePanel(PanelName.Panel_LevelList);
            }
        });
    }

    private void Update()
    {
        if (img_rot.gameObject.activeSelf) 
        {
            img_rot.transform.Rotate(Vector3.back,50* Time.deltaTime);
        }
    }

}
