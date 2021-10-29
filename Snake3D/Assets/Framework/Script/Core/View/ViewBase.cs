using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewBase : EventHandler
{

#if NGUI
    /// <summary>
    /// 所有boxCollider
    /// </summary>
    private List<Collider> colliderList = new List<Collider>();
#endif

    /// <summary>
    /// 所有Transform
    /// </summary>
    private List<Transform> transList = new List<Transform>();

    /// <summary>
    /// 主皮肤
    /// </summary>
    private string mainSkinPath;

    /// <summary>
    /// 是否初始化
    /// </summary>
    private bool isInit;

    private GameObject _skin;
    /// <summary>
    /// 皮肤
    /// </summary>
    public GameObject skin
    {
        get
        {
            return _skin;
        }
    }

    public Transform skinTrs
    {
        get
        {
            return _skin.transform;
        }
    }

    private GameObject m_Canvas;

    public RectTransform M_Canvas
    {
        get
        {
            return m_Canvas.GetComponent<RectTransform>();
        }
    }

    protected virtual void OnClick(Transform target)
    {
        AudioController.Ins.PlayEffect("btn_click");
    }

    protected virtual void OnDown(Transform target)
    {

    }

    protected virtual void OnUp(Transform target)
    {

    }

    /// <summary>
    /// 初始化皮肤前
    /// </summary>
    protected virtual void OnInitSkinFront() { }

    /// <summary>
    /// 初始化皮肤
    /// </summary>
    protected virtual void OnInitSkin()
    {
        if (mainSkinPath != null)
        {
            _skin = LoadSrc(mainSkinPath);
        }
        else
        {
            _skin = new GameObject("Skin");
        }
        skin.transform.SetParent(transform);
        skin.transform.localEulerAngles(Vector3.zero).localScale(1);
    }

    /// <summary>
    /// 初始化前
    /// </summary>
    protected virtual void OnInitFront()
    {
        transList.Clear();
        m_Canvas = GameObject.Find("Canvas");
    }

#if NGUI
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {
        if (!isInit)
        {
            OnInitFront();
            OnInitSkinFront();
            OnInitSkin();
        }
        //Profiler.BeginSample("分析收集碰撞情况 ：");
        Collider[] triggers = this.GetComponentsInChildren<Collider>(true);
        for (int i = 0, max = triggers.Length; i < max; i++)
        {
            Collider trigger = triggers[i];
            //如果点按钮没有就是没有初始化 Init()
            if (trigger.gameObject.name.StartsWith("Btn") == true)//以"Btn"开头命名的按钮才会触发OnClick 
            {
                UIEventListener listener = UIEventListener.Get(trigger.gameObject);
                listener.onClick = Click;
            }
            colliderList.Add(trigger);
            //UIButtonScale btnScale = trigger.gameObject.GetComponent<UIButtonScale>();//设置over没有缩放
            //if (btnScale != null)
            //{
            //    btnScale.hover = Vector3.one;
            //}
        }
        isInit = true;
        //UIButtonScale btnScale = trigger.gameObject.GetComponent<UIButtonScale>();//设置over没有缩放
        //if (btnScale != null)
        //{
        //    btnScale.hover = Vector3.one;
        //}
        //Profiler.EndSample();
    }

    /// <summary>
    /// 设置该对象下所有boxcollider是否可以交互
    /// </summary>
    /// <param name="enabled"></param>
    public void SetColliderEnabled(bool enabled)
    {
        foreach (BoxCollider bc in colliderList)
        {
            bc.enabled = enabled;
        }
    }

#endif

#if !NGUI

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {
        if (!isInit)
        {
            OnInitFront();
            OnInitSkinFront();
            OnInitSkin();
        }
        Transform[] transforms = this.GetComponentsInChildren<Transform>(true);
        for (int i = 0, max = transforms.Length; i < max; i++)
        {
            Transform transform = transforms[i];
            //如果点按钮没有就是没有初始化 Init()
            if (transform.name.StartsWith("Btn_"))//以"Btn"开头命名的按钮才会触发OnClick 
            {

                if (transform.GetComponent<Button>())
                {
                    Button listener = transform.GetComponent<Button>();
                    listener.onClick.AddListener(() => { OnClick(listener.transform); });
                }
                else
                {
                    ButtonEx listener = transform.GetOrAddComponent<ButtonEx>();
                    listener.onLeftClick = (go) => { OnClick(go); };
                    listener.onDown = (go) => { OnDown(go); };
                    listener.onUp = (go) => { OnUp(go); };
                }
            }
            transList.Add(transform);
        }
        isInit = true;
    }
#endif

    /// <summary>
    /// 设置主skin
    /// </summary>
    /// <param name="path"></param>
    protected void SetMainSkinPath(string path)
    {
        mainSkinPath = path;
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected GameObject LoadSrc(string path)
    {
        return ResourceMgr.GetInstance.CreateGameObject(path, false);
    }
}
