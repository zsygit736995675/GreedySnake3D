
/// <summary>
/// 事件key
/// 2019年4月18日18:23:46
/// </summary>
public enum EventDef
{
    NONE = 0,


    BIND_WX ,//微信绑定

    DataUpdate,//数据刷新

    LoadingUpdate,//进度条刷新
    
    REQUEST_COFING ,//请求配置
    
    REQUEST_100 ,//请求数值
    
    REQUEST_120 ,//请求数值
    
    REQUEST_150 ,//请求数值
    
    REQUEST_200 ,//请求数值
    
    REQUEST_LOGIN ,//登陆
    
    REQUEST_WITHDRAW_LIST ,//提现记录
    
    REQUEST_SCORE_WITHDRAW ,//提现档位
    
    REQUEST_SCORE ,//获取积分

    REQUEST_AMOUNT, //请求当前用户档位接口

    REF_RANK ,//刷新榜单
    
    LOADING ,//loading界面

    FlyNum,//飞红包或者积分

    Evaluation,//评价

    FlyEnd,//飞红包结束

    UPdateScore,//单独刷新积分

    LevelUpdate,//关卡刷新

    CloseGrab,//关闭抢红包

    CallFlsh,//召唤

    CallQiCaiFlsh, //召唤七彩鱼流程控制

    REF_WITHDROW, //刷新提现界面

    Max,//在这前面加

    CommonFlsh,//刷鱼

    Add_Plan, //增加任务进度

    Add_Chip, //增加皮肤碎片

    Update_Main,//刷新主界面

    EnterLevel,//进入关卡

    UpdateLevelPro,//刷新关卡进度

    RemoveBuff,//删除buff

    ClearBuff,//清空buff

    ShowBuffPro,//显示buff进度

    ShowMoney,//显示金额

    NextLevel,//下一关

    Callback,//执行回调

    ShowTop,//显示顶部条

    HideTop//隐藏顶部条





}

