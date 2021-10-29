1.将framework导入项目 在场景中创建Canvas 

2.点击LG_Tool > 一键添加文件夹  自动生成项目分类目录
	//Atlas    --->资源图片 文件夹
	//Scripts  --->项目脚本 文件夹
	//Scenes   --->项目场景 文件夹
	//Model    --->模型资源 文件夹
	//Plugins  --->导入插件 文件夹
	//Editor   --->编辑器用 文件夹
	//Prefabs  --->缓存预设 文件夹
	//Resources--->加载资源 文件夹
	//Thread   --->三方插件 文件夹

3.在场景中创建需要加载的UI预设体 。格式为（Panel_ 或Scene_）+ name

4.做完物体后，选中物体后 点击菜单栏中LG_Tool > 一键制作预制物  自动生成需要加载的预设体及对应脚本

5.打开脚本如图，分三块区域
①界面加载中 都是继承自父类的方法， 现在自动创建完后基本不需要修改
②数据定义中 定义该面板所需变量，物体，等等
③逻辑中     各种实现功能的方法，逻辑  panelArgs 及sceneArgs 是调动这个面板时传的参数数组。取时按数组下标去取

注意：
_cache 关闭时 物体关闭时为销毁 打开时 物体关闭时为隐藏

如果为 Panel ，界面加载中 
		_alpha =0~1; 遮罩的透明度
        _showStyle=PanelMgr.PanelShowStyle.Nomal;//修改打开风格
        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式

6.调用打开对应物体。
①打开一个Panel ，PanelMgr.GetInstance.ShowPanel(名称,参数[]);没有参数则不用传
②打开一个Scene ，SceneMgr.GetInstance.SwitchingScene(名称，参数[]);没有参数则不用传
③ 如需提示窗 ,   LogicMgr.GetInstance.GetLogic<LogicTips>().AddTips("添加一条提示");


具体使用时，可删除Resources 和Scripts 下Panel_Gold及Scene_Main参考