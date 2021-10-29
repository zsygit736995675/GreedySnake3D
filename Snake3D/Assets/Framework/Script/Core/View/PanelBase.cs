using UnityEngine;

public class PanelBase : ViewBase
{

    protected bool _cache = false;
    /// <summary>
    /// 缓存标识
    /// 如为false,则在关闭时destroy。
    /// </summary>
    public bool cache
    {
        get
        {
            return _cache;
        }
    }

    protected PanelName _type;
    /// <summary>
    /// 面板ID
    /// </summary>
    public PanelName type
    {
        get
        {
            return _type;
        }
    }
    /// <summary> 点击背景关闭panel </summary>
    protected bool _isClickMaskColse = true;
    /// <summary> 点击背景关闭panel </summary>
    public bool isClickMaskColse
    {
        get
        {
            return _isClickMaskColse;
        }
        set
        {
            _isClickMaskColse = value;
        }
    }

    private PanelMgr. PanelShowStyle [] PanelStyles;
    private System. Random random;

    public PanelMgr. PanelShowStyle GetShowStyle ()
    {
        PanelStyles = System. Enum. GetValues(typeof(PanelMgr. PanelShowStyle)) as PanelMgr. PanelShowStyle [];
        random = new System. Random();
        return PanelStyles [ random. Next(0, PanelStyles. Length) ];
    }


    /// <summary> 面板显示方式 </summary>
    protected PanelMgr. PanelShowStyle _showStyle = PanelMgr. PanelShowStyle. CenterScaleBigNomal;
    /// <summary>
    /// 面板显示方式
    /// </summary>
    public PanelMgr. PanelShowStyle PanelShowStyle
    {
        get
        {
            return _showStyle;
        }
    }

    protected float _alpha = 0;
    public float alpha
    {
        get
        {
            return _alpha;
        }
    }

    protected override void OnInitSkin()
    {
        base.OnInitSkin();
        skin.GetComponent<RectTransform>().sizeDelta = M_Canvas.sizeDelta;
    }

    /// <summary> 面板遮罩方式 </summary>
    protected PanelMgr. PanelMaskSytle _maskStyle = PanelMgr. PanelMaskSytle. None;
    /// <summary> 
    /// 面板遮罩方式
    /// </summary>
    public PanelMgr. PanelMaskSytle PanelMaskStyle
    {
        get
        {
            return _maskStyle;
        }
    }
    /// <summary> 面板打开时间 </summary>
    protected float _openDuration = 0.2f;
    /// <summary> 面板打开时间 </summary>
    public float OpenDuration
    {
        get
        {
            return _openDuration;
        }
    }


    protected object [] _panelArgs;
    /// <summary>
    /// 记录面板init时参数
    /// </summary>
    public object [] panelArgs
    {
        get
        {
            return _panelArgs;
        }
    }

    /// <summary>
    /// 初始化面板
    /// </summary>
    /// <param name="panelArgs">面板参数</param>
    public virtual void OnInit (params object [] panelArgs)
    {
        _panelArgs = panelArgs;
        Init();
    }

    /// <summary>
    /// 开始显示
    /// </summary>
    public virtual void OnShowing ()
    {
    }
    /// <summary>
    /// 重值数据
    /// </summary>
    /// <param name="panelArgs"></param>
    public virtual void OnResetArgs (params object [] panelArgs)
    {
        _panelArgs = panelArgs;
    }
    /// <summary>
    /// 显示面板后
    /// </summary>
    public virtual void OnShowed ()
    {

    }

    /// <summary>
    /// 发起关闭
    /// </summary>
    protected virtual void Close ()
    {
        PanelMgr. GetInstance. HidePanel(type);
    }

    /// <summary>
    /// 发起关闭
    /// </summary>
    protected virtual void Close (PanelName panel)
    {
        PanelMgr. GetInstance. HidePanel(panel);
    }

    /// <summary>
    /// 立刻关闭
    /// </summary>
    protected virtual void CloseImmediate ()
    {
        PanelMgr. GetInstance. DestroyPanel(type);
    }

    public virtual void OnHideFront ()
    {
        _cache = false;
    }

    public virtual void OnHideDone ()
    {
    }

}
/// <summary>
/// 面板名字列表（用类名来表示）
/// </summary>
public enum PanelName
{
    None = 0,
    Panel_NewRed,
    Panel_Failure,
    Panel_Pause,
    Panel_LevelList,
    Panel_GetCashChip,
    Panel_CashChip,
    Panel_Turnplate,
    Panel_GoldPig,
    Panel_RedRain,
    Panel_Grab,
    Panel_WuCaiHelp,
    Panel_WuCai,
    Panel_Phone,
    Panel_BangWeChat,
    Panel_RankHint,
    Panel_RankHengTong,
    Panel_RankTiMing,
    Panel_Pk,
    Panel_NetWork,
    Panel_Setting,
    Panel_Integral,
    Panel_Gold,
    Panel_Withdraw
}






































































































































































































































