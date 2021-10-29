using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 游戏场景
/// </summary>
public class Scene_Game : MonoBehaviour
{
    /// <summary>
    /// 布局
    /// </summary>
    GridLayoutGroup grid;
    /// <summary>
    /// 当前关卡
    /// </summary>
    int currentLevel;
    /// <summary>
    /// 当前关卡模板
    /// </summary>
    LevelModel currentMode;
    /// <summary>
    /// 格子模板
    /// </summary>
    Transform box_item;
    /// <summary>
    /// 当前格子
    /// </summary>
    List<BoxItem> boxs = new List<BoxItem>();
    /// <summary>
    /// 已经选中的格子
    /// </summary>
    List<BoxItem> seleItems = new List<BoxItem>();
    /// <summary>
    /// 上一个box 当前最前的一个
    /// </summary>
    BoxItem beforeBox;
    /// <summary>
    /// 最大关卡
    /// </summary>
    int maxLevel = 150;
    /// <summary>
    /// 是否回退中
    /// </summary>
    bool isback=false;
    /// <summary>
    /// 当前的下表
    /// </summary>
    int currentIndex;
    /// <summary>
    /// 最大提示步数
    /// </summary>
    int maxPrompt= 160 ;

    /// <summary>
    /// 鱼头鱼尾动画
    /// </summary>
    public SpineBase yutou;
    public SpineBase yuwei;
    public Transform ywpos;

    Transform hand;

    //关卡结束
    bool isLevelEnd=false;

    void Start()
    {
        grid = GetComponent<GridLayoutGroup>();

        currentLevel = DataManager.getLevelNum();

        box_item = transform.SeachTrs<Transform>("box_item");

        hand = transform.parent. SeachTrs<Transform>("hand");

        yutou = Instantiate( ResourceMgr.GetInstance.Load<GameObject>("Game/yutou",false),transform).AddComponent<SpineBase>();
        yuwei = Instantiate( ResourceMgr.GetInstance.Load<GameObject>("Game/yuwei",false),grid.transform.parent).AddComponent<SpineBase>();
        ywpos = new GameObject().transform;
        ywpos.SetParent(grid.transform);
        ywpos.localScale = Vector3.one;
        
        PlayYT();
        CreateLevel(currentLevel);
    }

    void PlayYT() 
    {
        yutou.PlayAnim("animation", true, () => {

            Invoke(nameof(PlayYT),4);

        });
    }

    void Clear()
    {
        box_item.gameObject.SetActive(false);
        for (int i = 0; i < boxs.Count; i++)
        {
            Destroy(boxs[i].gameObject);
        }
        beforeBox = null;
        seleItems.Clear();
        boxs.Clear();
    }

    /// <summary>
    /// 生成场景 
    /// </summary>
    public  void CreateLevel(int level) 
    {
        guiIndex = 2;
        handIndex = 0;
        SendDot(level);
        Clear();
        isLevelEnd = false;
        currentLevel = level;
        hand.gameObject.SetActive(false);
        DataManager.SetLevelNum(currentLevel);
      

        level = LoopLevel(level);

        TextAsset asset = ResourceMgr.GetInstance.Load<TextAsset>("Level/level"+ level, false) ;
        if (asset == null || string.IsNullOrEmpty(asset.text)) 
        {
            Debug.LogError("Load level  error : "+ "Level/level" + level);
            return;
        }

        currentMode = JsonUtility.FromJson<LevelModel>(asset.text);
        if (currentMode == null) 
        {
            Debug.LogError(" currentMode error : Level/level" + level);
            return;
        }

        SetSize();

        for (int i = 0; i < currentMode.rowTotal; i++)
        {
            for (int j = 0; j < currentMode.columnTotal; j++)
            {
                GameObject go = GameObject.Instantiate(box_item.gameObject);
                go.transform.SetParent(grid.transform);
                go.gameObject.SetActive(true);
                go.transform.localScale = Vector3.one;
                BoxItem item = go.AddComponent<BoxItem>();
                LevelItem le = GetBoxItem(i + 1, j + 1);
                item.Init(i + 1, j + 1, this, le, grid.cellSize);
                if (le != null && le.index == 1) 
                {
                    beforeBox = item;
                    beforeBox.SetAni(currentMode.items[1].row, currentMode.items[1].column);
                    seleItems.Add(item);
                }

                boxs.Add(item);
            }
        }

        if (level == 1) 
        {
            Prompt();
            NewGuide();
            Invoke(nameof(HandMove),0.5f);
        }
    }

    int handIndex = 1;
    void HandMove() 
    {
        hand.gameObject.SetActive(true);

        if (handIndex == -1 || currentLevel != 1)
        {
            hand.gameObject.SetActive(false);
            return;
        }
        handIndex++;

        BoxItem item = GetItemByPresetIndex(handIndex);
      
        if (handIndex == 1 && item == null) 
        {
            hand.gameObject.SetActive(false);
            return;
        }

        if (item == null) 
        {
            handIndex = 0;
            HandMove();
            return;
        }

        if (handIndex == 1)
        {
            hand.transform.position = new Vector3(item.transform.position.x + 0.3f, item.transform.position.y - 0.3f);
            Invoke(nameof(HandMove), 0.2f);
        }
        else 
        {
            hand.transform.DOMove(new Vector3(item.transform.position.x + 0.3f, item.transform.position.y - 0.3f), 0.5f).onComplete =()=> HandMove();
        }
    }

    int guiIndex=2;
    void  NewGuide() 
    {
        if (currentLevel != 1) return;

        BoxItem item =   GetItemByPresetIndex(guiIndex);

        if (guiIndex == 2 && item == null) return;

        if (item != null)
        {
            item.Guide(()=>{

                guiIndex++;
                NewGuide();
            });
        }
        else 
        {
            guiIndex = 2;
            NewGuide();
        }
    }


    void SendDot(int level) 
    {
        switch (level)
        {
            case 1:
                DotManager.Instance.sendEvent("level_up_1",DottingType.Af);
                break;
            case 5:
                DotManager.Instance.sendEvent("level_up_5", DottingType.Af);
                break;
            case 10:
                DotManager.Instance.sendEvent("level_up_10", DottingType.Af);
                break;
            case 20:
                DotManager.Instance.sendEvent("level_up_20", DottingType.Af);
                break;
            case 30:
                DotManager.Instance.sendEvent("level_up_30", DottingType.Af);
                break;
            case 40:
                DotManager.Instance.sendEvent("level_up_40", DottingType.Af);
                break;
            case 50:
                DotManager.Instance.sendEvent("level_up_50", DottingType.Af);
                break;
        }
    }

    /// <summary>
    /// 关卡循环
    /// </summary>
    int LoopLevel(int level) 
    {
        if (level > maxLevel)
        {
            int add = level % maxLevel;
            level = maxLevel / 2 + add;
        }

        if (level > maxLevel)
        {
            return LoopLevel(level);
        }

        return level;
    }

    /// <summary>
    /// 根据行列获取需要显示的格子
    /// </summary>
    LevelItem GetBoxItem(int row,int col) 
    {
        if (currentMode == null || currentMode.items==null || currentMode.items.Count==0) 
        {
            Debug.LogError("GetBoxItem error !");
            return null;
        }

        foreach (var item in currentMode.items)
        {
            if (item.row == row && item.column == col) 
            {
                return item;
            }
        }

        return null;
    }

    /// <summary>
    /// 添加到已选中
    /// </summary>
    public void AddSelected(BoxItem item) 
    {
        if (!isLevelEnd&&beforeBox != null && !isback)
        {
            if (!seleItems.Contains(item) && !item.isSelected)
            {
                if (IsAdjacent(beforeBox, item) )
                {
                    handIndex = -1;
                    //前进
                    seleItems.Add(item);
                    item.Selected(beforeBox);
                    beforeBox = item;
                }
                else
                {
                    //闪烁
                    Flashing();
                }
            }
            else
            {
                //回退
                if (beforeBox.row != item.row || beforeBox.column != item.column ) 
                {
                    for (int i = 0; i < seleItems.Count; i++)
                    {
                        if (seleItems[i].row == item.row && seleItems[i].column == item.column) 
                        {
                                currentIndex = i;
                                Goback();
                                return;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 提示
    /// </summary>
    public  void Prompt() 
    {
        int start = 0;
        for (int i = 0; i < seleItems.Count; i++)
        {
            LevelItem item =  GetBoxItem(seleItems[i].row, seleItems[i].column);
            if (item.index != i + 1)
            {
                currentIndex = i-1;
                start = i;
                Goback();
                break;
            }
        }

        if (start == 0 && beforeBox!=null) 
        {
            start = beforeBox.index;
        }

        int proNum = 0;
        LevelItem le = null;
        foreach (LevelItem item in currentMode.items)
        {
            BoxItem box = GetItemByPresetIndex(item.index);
            if (box != null && box.index > start)
            {
                proNum++;
                if (proNum > maxPrompt) 
                {
                    return;
                }
                if (box == null)
                {
                    box.ShowPrompt(currentMode.items[0].row, currentMode.items[0].column);
                }
                else
                {
                    box.ShowPrompt(le.row, le.column);
                }
            }
            le = item;
        }
    }

    BoxItem GetItemByPresetIndex(int  index) 
    {
        foreach (BoxItem item in boxs)
        {
            if (item.isShow && item.index == index) 
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// 退回到 index
    /// </summary>
    void Goback() 
    {
        isback = true;

        if (seleItems.Count > currentIndex+1)
        {
            int index = seleItems.Count - 2;
            if (index >= 0)
            {
                seleItems[seleItems.Count - 1].SetAni(seleItems[seleItems.Count - 2].row, seleItems[seleItems.Count - 2].column);
            }

            seleItems[seleItems.Count - 1].Canceled();
            seleItems.RemoveAt(seleItems.Count-1);
            Invoke(nameof(Goback),0.02f);
        }
        else 
        {
            if (seleItems[seleItems.Count - 1].index == 1)
            {
                seleItems[seleItems.Count - 1].SetAni(currentMode.items[1].row, currentMode.items[1].column);
            }
            else 
            {
                int index = seleItems.Count - 2;
                if (index >= 0)
                {
                    seleItems[seleItems.Count - 1].SetAni(seleItems[seleItems.Count - 2].row, seleItems[seleItems.Count - 2].column);
                }
            }
            beforeBox = seleItems[seleItems.Count - 1];
            isback = false;
        }
    }

    /// <summary>
    /// 闪烁
    /// </summary>
    public void Flashing() 
    {
        foreach (var item in boxs)
        {
            if (item.isShow && !item.isSelected && IsAdjacent(beforeBox, item)) 
            {
                item.Flashing();
            }
        }
    }

    /// <summary>
    /// 检测关卡是否结束
    /// </summary>
    public void LevelEndDetect() 
    {
        foreach (var item in boxs)
        {
            if (item.isShow && !item.isSelected) 
            {
                return;
            }
        }

        isLevelEnd = true;
        Invoke(nameof(NextLevel),0.5f);
    }

    /// <summary>
    /// 下一关
    /// </summary>
    public void NextLevel()
    {
        DotManager.Instance.userSet("grade_time", currentLevel);
        DotManager.Instance.sendEvent("level_up", DottingType.Tga, new Dictionary<string, object> { { "level_up", currentLevel } });
        //PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Gold, RedType.common,true);
        EventMgrHelper.Ins.PushEvent(EventDef.Add_Plan);
        currentLevel++; 
        //关卡结束
        CreateLevel(currentLevel);
    }

    public void NextLevelTest()
    {
        currentLevel++;
        //关卡结束
        CreateLevel(currentLevel);
    }

    /// <summary>
    /// 是否是相邻的格子
    /// </summary>
    bool IsAdjacent(BoxItem be, BoxItem cur) 
    {
        if (be.row == cur.row)
        {
            return Mathf.Abs(be.column - cur.column) == 1;
        }
        else if (be.column == cur.column)
        {
            return Mathf.Abs(be.row - cur.row) == 1;
        }
        return false;
    }

    /// <summary>
    /// 根据格子数量设置尺寸
    /// </summary>
    void SetSize() 
    {
        grid.constraintCount = currentMode.columnTotal;
        grid.cellSize = new Vector2(150, 150);

        if (currentMode.columnTotal > 5 || currentMode.rowTotal > 5)
        {
            grid.cellSize = new Vector2(120, 120);
        }

        if (currentMode.columnTotal > 7 || currentMode.rowTotal > 7)
        {
            grid.cellSize = new Vector2(100, 100);
        }

        if (currentMode.columnTotal > 9 || currentMode.rowTotal > 9)
        {
            grid.cellSize = new Vector2(80, 80);
        }
    }

}


/// <summary>
/// 格子
/// </summary>
public class BoxItem : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public int row;
    public int column;
    public int index;
    public LevelItem item;
    public bool isSelected = false;
    public bool isShow = false;
    public bool isFlashing = false;
    Color oriColor;

    public BoxItem beforeBox;
    RectTransform rect;
    Scene_Game game;
    Image allow, img_red,img_bg,img_chai;
    Vector2 size;

    Transform top, left, right, bottom;

    public void Init(int row, int column, Scene_Game game, LevelItem item ,Vector2 size)
    {
        this.row = row;
        this.column = column;
        this.game = game;
        this.item = item;
        this.size = size;

        if (item == null)
        {
            isShow = false;
        }
        else
        {
            isShow = true;
            this.index = item.index;
        }
        allow = transform.SeachTrs<Image>("img_allow");
        img_red = transform.SeachTrs<Image>("img_red");
        img_bg = transform.SeachTrs<Image>("img_bg");
        img_chai= transform.SeachTrs<Image>("img_chai");

        top = transform.SeachTrs<Transform>("top");
        left = transform.SeachTrs<Transform>("left");
        right = transform.SeachTrs<Transform>("right");
        bottom = transform.SeachTrs<Transform>("bottom");

        img_chai.gameObject.SetActive(false);
        allow.gameObject.SetActive(false);

        img_red.gameObject.SetActive(index == 1);
        isSelected = index == 1;

        img_bg.enabled = isShow;
        allow.enabled = isShow;
        img_red.enabled = isShow;
        rect = img_red.GetComponent<RectTransform>();
        rect.sizeDelta = size;

        oriColor = new Color(allow.color.r, allow.color.g, allow.color.b,1);
    }

    public void ShowChai() 
    {
        img_chai.gameObject.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0) && isShow && !isFlashing)
        {
            game.AddSelected(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0) && isShow && !isFlashing)
        {
            game.AddSelected(this);
        }
    }

    /// <summary>
    /// 闪烁
    /// </summary>
    public void Flashing()
    {
        if (!isFlashing)
        {
            isFlashing = true;
            rect.sizeDelta = size;
            rect.transform.localScale = Vector3.one;
            rect.transform.localPosition = Vector3.zero;
            rect.gameObject.SetActive(true);
            rect.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.3f).onComplete = () => {
                rect.transform.DOScale(Vector3.one, 0.3f).onComplete = () => {
                    rect.gameObject.SetActive(false);
                    isFlashing = false;
                };
            };
        }
    }

    /// <summary>
    /// 选中
    /// </summary>
    public void Selected(BoxItem beforeBox)
    {
        this.beforeBox = beforeBox;
        isSelected = true;
        img_red.gameObject.SetActive(true);
        img_bg.enabled = false;
        int offset = 50;
        if (beforeBox.row == row)
        {
            rect.sizeDelta = new Vector2(size.x + offset, size.y);
            if (beforeBox.column < column)
            {
                rect.localPosition = new Vector2(rect.localPosition.x - offset/2, rect.localPosition.y);
            }
            else 
            {
                rect.localPosition = new Vector2(rect.localPosition.x + offset/2, rect.localPosition.y);
            }
        }     
        else if (beforeBox.column == column) 
        {
            rect.sizeDelta = new Vector2(size.x , size.y + offset);
            if (beforeBox.row < row)
            {
                rect.localPosition = new Vector2(rect.localPosition.x , rect.localPosition.y + offset / 2);
            }
            else
            {
                rect.localPosition = new Vector2(rect.localPosition.x , rect.localPosition.y - offset / 2);
            }
        }

        SetAni(beforeBox.row, beforeBox.column,true);
        game.LevelEndDetect();
    }

    /// <summary>
    ///  取消选中
    /// </summary>
    public void Canceled()
    {
        img_bg.enabled = true;
        isSelected = false;
        rect.sizeDelta = size;
        img_red.gameObject.SetActive(false);
        img_red.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 显示提示箭头
    /// </summary>
    public void ShowPrompt(int befoRow,int befoCol) 
    {
        allow.gameObject.SetActive(true);
        allow.transform.eulerAngles = new Vector3(0, 0, GetAngle(befoRow, befoCol));
    }

    int GetAngle(int befoRow, int befoCol) 
    {
        if (befoRow == row)
        {
            if (befoCol < column)
            {
                return 180;//左
            }
            else
            {
                return 0;//右
            }
        }
        else if (befoCol == column)
        {
            if (befoRow < row)
            {
                return 90; //上
            }
            else
            {
                return -90;//下
            }
        }
        return 0;
    }

    /// <summary>
    /// 指引闪烁
    /// </summary>
    public void Guide(Action action) 
    {
        allow.color = new Color(oriColor.a, oriColor.g, oriColor.b, 1);
        DOTween.To(() => allow.color, p => allow.color = p, new Color(oriColor.a, oriColor.g, oriColor.b, 0), 0.25f).onComplete = () => {
            DOTween.To(() => allow.color, p => allow.color = p, new Color(oriColor.a, oriColor.g, oriColor.b, 1), 0.25f).onComplete = () => {
                DOTween.To(() => allow.color, p => allow.color = p, new Color(oriColor.a, oriColor.g, oriColor.b, 0), 0.25f).onComplete = () => {
                    DOTween.To(() => allow.color, p => allow.color = p, new Color(oriColor.a, oriColor.g, oriColor.b, 1), 0.25f).onComplete = () => {

                        action?.Invoke();

                    };
                };
            };
        };
    }

    /// <summary>
    /// 更新动画
    /// </summary>
    public void SetAni(int berow, int becol,bool isForward=false) 
    {
        int yto = 50;
        float speed = 0.1f;
        int angle =  GetAngle(berow, becol);
        if (index == 1)
        {
            switch (angle)
            {
                case 0:
                    game.ywpos.transform.SetParent(left);
                    game.ywpos.transform.localPosition = new Vector3(-20, -30);

                    game.yutou.transform.SetParent(right);
                    game.yutou.transform.localPosition = new Vector3(-yto, 0);
                    game.yutou.transform.eulerAngles = new Vector3(0, 0, 90);
                    break;
                case -90:
                    game.ywpos.transform.SetParent(top);
                    game.ywpos.transform.localPosition = new Vector3(-30, 20);

                    game.yutou.transform.SetParent(bottom);
                    game.yutou.transform.localPosition = new Vector3(0, yto);
                    game.yutou.transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
                case 90:
                    game.ywpos.transform.SetParent(bottom);
                    game.ywpos.transform.localPosition = new Vector3(30, -20);

                    game.yutou.transform.SetParent(top);
                    game.yutou.transform.localPosition = new Vector3(0, -yto);
                    game.yutou.transform.eulerAngles = new Vector3(0, 0, 180);
                    break;
                case 180:
                    game.ywpos.transform.SetParent(right);
                    game.ywpos.transform.localPosition = new Vector3(20, 30);

                    game.yutou.transform.SetParent(left);
                    game.yutou.transform.localPosition = new Vector3(yto, 0);
                    game.yutou.transform.eulerAngles = new Vector3(0, 0, -90);
                    break;
            }
            game.yuwei.transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else 
        {
            switch (angle)
            {
                case 0:
                    game.yutou.transform.SetParent(left);
                    game.yutou.transform.eulerAngles = new Vector3(0, 0, -90);

                    if (isForward) 
                    {
                        game.yutou.transform.localPosition = new Vector3(game.yutou.transform.localPosition.x,0);
                        game.yutou.transform.DOLocalMove(new Vector3(yto, 0), speed);
                    }
                    else
                        game.yutou.transform.localPosition = new Vector3(yto, 0);

                    break;
                case -90:
                    game.yutou.transform.SetParent(top);
                    game.yutou.transform.eulerAngles = new Vector3(0, 0, 180);

                    if (isForward) 
                    {
                        game.yutou.transform.localPosition = new Vector3(0, game.yutou.transform.localPosition.y);
                        game.yutou.transform.DOLocalMove(new Vector3(0, -yto), speed);
                    }
                    else
                        game.yutou.transform.localPosition = new Vector3(0, -yto);

                    break;
                case 90:
                    game.yutou.transform.SetParent(bottom);
                    game.yutou.transform.eulerAngles = new Vector3(0, 0, 0);

                    if (isForward) 
                    {
                        game.yutou.transform.localPosition = new Vector3(0, game.yutou.transform.localPosition.y);
                        game.yutou.transform.DOLocalMove(new Vector3(0, yto), speed);
                    }
                    else 
                        game.yutou.transform.localPosition = new Vector3(0, yto);

                    break;
                case 180:
                    game.yutou.transform.SetParent(right);
                    game.yutou.transform.eulerAngles = new Vector3(0, 0, 90);

                    if (isForward) 
                    {
                        game.yutou.transform.localPosition = new Vector3(game.yutou.transform.localPosition.x, 0);
                        game.yutou.transform.DOLocalMove(new Vector3(-yto, 0), speed);
                    }
                    else
                        game.yutou.transform.localPosition = new Vector3(-yto, 0);
                   
                    break;
            }
        }
        game.yuwei.transform.localScale = new Vector3(1,1,1); 
        game.yutou.transform.localScale = Vector3.one;
    }

    private void Update()
    {
        if (game.yuwei!=null&&game.ywpos!=null&& game.yuwei.transform.position != game.ywpos.position)
                game.yuwei.transform.position = game.ywpos.position;
    }


}


[Serializable]
public class LevelModel
{
    public int rowTotal;
    public int columnTotal;
    public List<LevelItem> items;
}


[Serializable]
public class LevelItem
{
    public int index;
    public int row;
    public int column;
}