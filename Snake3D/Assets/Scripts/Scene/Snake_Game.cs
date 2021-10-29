using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

public enum BuffType 
{
    /// <summary>
    /// 磁铁
    /// </summary>
    CT,
    /// <summary>
    /// 暂停
    /// </summary>
    ZT,
    /// <summary>
    /// 加速
    /// </summary>
    JS,
        
    NONE,
}

public class Snake_Game : MonoBehaviour
{
    /// <summary>
    /// 蛇
    /// </summary>
   public  Snake snake;

    /// <summary>
    /// 当前关卡
    /// </summary>
    public  int currentLevel;
    /// <summary>
    /// 当前波次
    /// </summary>
    public   int currentWave;

    /// <summary>
    /// 当前关卡模型
    /// </summary>
    LevelModelSnake model;

    /// <summary>
    /// 障碍，道具容器
    /// </summary>
    Transform ObstacleGroup, PropGroup, SnakeGroup, Panel;
    
    /// <summary>
    /// 道具列表
    /// </summary>
    List<Prop> props=new List<Prop>();

    /// <summary>
    /// 是否开始
    /// </summary>
    public bool isStart=false;

    /// <summary>
    /// 是否暂停
    /// </summary>
    public bool isPause = false;

    public bool isvictory = false;

    /// <summary>
    /// 全部小球的数量
    /// </summary>
    public int ballTotalNum = 0;

    /// <summary>
    /// 当前吃到的小球数量
    /// </summary>
    public int triggerBallNum = 0;

    /// <summary>
    /// 当前波次小球数量
    /// </summary>
    public int waveBallNum = 0;

    /// <summary>
    /// 当前波次触发的小球数量
    /// </summary>
    public int triggerwaveBallNum = 0;

    /// <summary>
    /// 身上的buff
    /// </summary>
    public Dictionary<BuffType, bool> buffs = new Dictionary<BuffType, bool>();

    ParticleSystem sys ;

    Transform top, bottom;

    int index;

    Transform hand;

    List<Vector3> handPath = new List<Vector3>();


    static  Snake_Game ins;
    public static Snake_Game Ins { get { return ins; }}
    private void Awake()
    {
        ins = this;
    }

    void Start()
    {
        ObstacleGroup = transform.SeachTrs<Transform>("ObstacleGroup");
        PropGroup = transform.SeachTrs<Transform>("PropGroup");
        SnakeGroup= transform.SeachTrs<Transform>("SnakeGroup");
        top = transform.SeachTrs<Transform>("top");
        bottom = transform.SeachTrs<Transform>("bottom");
        Panel = transform.SeachTrs<Transform>("Panel");

        if (Camera.main.orthographicSize == 6)
        {
            top.transform.position = new Vector3(0,0,-7.7f);
            bottom.transform.position = new Vector3(0, 0, 6.2f);
            Panel.transform.localScale(7f,7f,1f);
        }
        else 
        {
            top.transform.position = new Vector3(0, 0, -6.6f);
            bottom.transform.position = new Vector3(0, 0, 5.1f);
            Panel.transform.localScale(7f, 7f, 1f);
        }
    }

    /// <summary>
    ///  打开关卡
    /// </summary>
    /// <param name="level"></param>
    public void OpenLevel(int level=-1) 
    {
        DotManager.Instance.sendEvent("Game_start", DottingType.Tga, new Dictionary<string, object> {{ "Level_ID", level }});
        DotManager.Instance.sendEvent("battle_enter", DottingType.Af, new Dictionary<string, object> { { "Level_ID", level } });

        AudioController.Ins.PlayBgm();

        ClearBuff();

        isPause = false;
        isStart = false;
        isvictory = false;
        if (level==-1)
            currentLevel = DataManager.getLevelNum();
        else
            currentLevel = level;

        if (currentLevel > ConfigManager.Max_Level)
            currentLevel = ConfigManager.Max_Level;

        EventMgrHelper.Ins.PushEvent(EventDef.LevelUpdate, currentLevel);
        EventMgrHelper.Ins.PushEvent(EventDef.ShowTop);

        if (currentLevel > DataManager.getLevelNum())
        {
            DataManager.SetLevelNum(currentLevel);
        }

        index = currentLevel / 30;
        index += 1;
        if (index > 3)
            index = 3;
        Texture tex = Resources.Load<Texture>("Prop/Img/ic_ba"+index);
        Panel.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", tex);

        currentWave = 1;
        ballTotalNum = 0;
        triggerBallNum = 0;

        SetHand(3);
        CreateScene();
        CreateSnake();
    }

    void CreateHade() 
    {
        if (hand != null) 
        {
            Destroy(hand.gameObject);
        }
        GameObject go = Resources.Load<GameObject>("Game/shou");
        hand = Instantiate(go,transform).transform;
        hand.localScale = Vector3.one*1.2f;
        hand.localPosition = new Vector3(0,1,0);
        hand.gameObject.SetActive(false);
    }

    
    void SetHand(int type)
    {
        if (type == 1)
        {
            if (currentLevel == 1) 
            {
                CreateHade();
                hand.gameObject.SetActive(true);
                if (waveBallNum == 1)
                {
                    hand.GetChild(1).GetComponent<Animation>().enabled = true;
                    hand.GetChild(0).gameObject.SetActive(true);
                    hand.transform.position = new Vector3(handPath[0].x - 0.4f, 1, handPath[0].z + 0.95f);
                }
                else
                {
                    HandMove(0);
                    hand.GetChild(0).gameObject.SetActive(false);
                    hand.GetChild(1).GetComponent<Animation>().enabled = false;
                }
            }
        }
        else if (type == 3)
        {
            if(hand!=null)
                hand.gameObject.SetActive(false);
        }
        else if (type == 2)
        {
            if (isvictory)
                return;
            Prop qi = GetPropByType(PropType.qi);
            if (qi != null)
            {
                CreateHade();
                hand.gameObject.SetActive(true);
                hand.transform.position = new Vector3(qi.transform.position.x - 0.4f, 1, qi.transform.position.z + 0.95f);
                hand.GetChild(1).GetComponent<Animation>().enabled = true;
                hand.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    void HandMove(int index) 
    {
        if (currentLevel!=1|| hand == null || !hand.gameObject.activeSelf ||handPath==null||handPath.Count<=1)
            return;

        if (index == 0) 
        {
            hand.transform.position = new Vector3(handPath[0].x - 0.4f, 1, handPath[0].z + 0.95f);
            hand.transform.DOMove(hand.transform.position, 0.5f).onComplete = () => {  HandMove(1); };
            return;
        }

        hand.transform.DOMove(new Vector3(handPath[handPath.Count-1].x - 0.4f, 1, handPath[handPath.Count - 1].z + 0.95f),1f).onComplete=()=>{
            HandMove(0);
        };
    }


    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        if (!isStart)
        {
            isStart = true;

            Prop qidian = GetPropByType(PropType.qidian);

            qidian.transform.DOMoveZ(10, 5);

            EventMgrHelper.Ins.PushEvent(EventDef.EnterLevel);
        }
    }

    /// <summary>
    /// 返回主界面
    /// </summary>
    public void BackMain() 
    {
        isPause = true;
        isStart = false;
        ClearBuff();
        AudioController.Ins.PlayBgm("ambient_futuristic");
        EventMgrHelper.Ins.PushEvent(EventDef.Update_Main);
    }

    /// <summary>
    /// 下一关
    /// </summary>
    public void NextLevel() 
    {
        DotManager.Instance.sendEvent("level_up", DottingType.Tga, new Dictionary<string, object> { { "Level_ID", currentLevel } });
        DotManager.Instance.sendEvent("game_finish", DottingType.Af, new Dictionary<string, object> { { "Level_ID", currentLevel } });
        DotManager.Instance.userSet("Game_level", currentLevel);
        currentLevel++;

        OpenLevel(currentLevel);
    }

    /// <summary>
    /// 创建场景
    /// </summary>
    void CreateScene()
    {
        TextAsset asset = Resources.Load<TextAsset>("Level/level" + currentLevel);
        if (asset == null || string.IsNullOrEmpty(asset.text))
        {
            Debug.LogError("CreateScene error : " + "Level/level" + currentLevel);
            return;
        }
        model = JsonConvert.DeserializeObject<LevelModelSnake>(asset.text);
        foreach (var item in model.waveList)
        {
            foreach (var item1 in item.propList)
            {
                if (item1.propType == (int)PropType.qiu || item1.propType == (int)PropType.qiu_2)
                {
                    ballTotalNum++;
                }
            }       
        }
        EventMgrHelper.Ins.PushEvent(EventDef.UpdateLevelPro,0, ballTotalNum);
        CreateObstacle();
        CreateWave();
    }

    /// <summary>
    /// 生成障碍
    /// </summary>
    void CreateObstacle()
    {
        ClearAll();
        if (model != null && model.propList != null)
        {
            foreach (PropModel item in model.propList)
            {
                GameObject go = Resources.Load<GameObject>("Prop/prop/" + ((PropType)item.propType).ToString());
                Prop prop = Instantiate(go, ObstacleGroup).AddComponent<Prop>();
                prop.Init(item);

                if (item.propType != (int)PropType.qi)
                {
                    int t = UnityEngine.Random.Range(1, 3);
                    Texture tex = Resources.Load<Texture>("Prop/Img/" + index + "/" + t);
                    prop.GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", tex);
                }

                props.Add(prop);
                if (item.propType == (int)PropType.zhongdian || item.propType == (int)PropType.qi) 
                {
                    prop.gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// 生成指定波次的道具
    /// </summary>
    void CreateWave()
    {
        Clear();
        handPath.Clear();
        waveBallNum = 0;
        triggerwaveBallNum = 0;

        if (model != null && model.waveList != null)
        {
            WaveModel item = GetWaveModel();
            if (item.propList != null && item.propList.Count > 0)
            {
                foreach (PropModel prop in item.propList)
                {
                    GameObject go = Resources.Load<GameObject>("Prop/prop/" + ((PropType)prop.propType).ToString());
                    Prop p = Instantiate(go, PropGroup).AddComponent<Prop>();
                    p.Init(prop, waveBallNum);
                    props.Add(p);

                    if (prop.propType == (int)PropType.qiu || prop.propType == (int)PropType.qiu_2)
                    {
                        waveBallNum++;
                        Texture tex = Resources.Load<Texture>("Prop/Img/qiu_" + index );
                        p.GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", tex);
                        handPath.Add(p.transform.position);
                    }
                }

                if (waveBallNum > 1) 
                {
                    handPath.Sort((a, b) => {
                        if (currentWave == 1)
                        {
                            if (a.z > b.z)
                                return -1;
                            else
                                return 1;
                        }
                        else
                        {
                            if (a.z > b.z)
                                return 1;
                            else
                                return -1;
                        }
                    });
                }
              
                SetHand(1);
            }
        }
    }

    WaveModel GetWaveModel()
    {
        foreach (WaveModel item in model.waveList)
        {
            if (item.wave == currentWave)
            {
                return item;
            }
        }
        currentWave++;
       return  GetWaveModel();
    }



    /// <summary>
    /// 初始化蛇
    /// </summary>
    void CreateSnake()
    {
        if (snake == null) 
        {
             snake= Instantiate(ResourceMgr.GetInstance.Load<GameObject>("Prop/she/shetou", false), SnakeGroup).AddComponent<Snake>();
        }
        Prop qidian = GetPropByType(PropType.qidian);
        snake.InitCreate(new Vector3(qidian.transform.position.x, 0, qidian.transform.position.z - 1), new Vector3(0, 180, 0));
    }

    /// <summary>
    /// 通过类型获取物体
    /// </summary>
    /// <param name="prop"></param>
    /// <returns></returns>
    Prop GetPropByType(PropType prop) 
    {
        foreach (var item in props)
        {
            if (item.propType == prop) 
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// 清空道具和小球
    /// </summary>
    void Clear()
    {
        if (props != null && props.Count > 0)
        {
            for (int i = 0; i < props.Count; i++)
            {
                if (!props[i].isStatic)
                {
                    Destroy(props[i].gameObject);
                    props.RemoveAt(i);
                }
            }
        }  
    }

    /// <summary>
    /// 清空所有
    /// </summary>
    void ClearAll()
    {
        if (props != null && props.Count > 0) 
        {
            foreach (var item in props)
            {
                Destroy(item.gameObject);
            }
            props.Clear();
        }
    }

    /// <summary>
    /// 过关
    /// </summary>
    public void PassLevel() 
    {
        if (!isvictory) 
        {
            snake.targetPos = new Vector3(snake.targetPos .x, snake.targetPos.y, snake.targetPos.z-20);

            isPause = true;
            isStart = false;
            isvictory = true;

            SetHand(3);

            Prop qi = GetPropByType(PropType.qi);
            if (qi != null)
            {
                qi.gameObject.SetActive(false);
            }

            if (sys == null)
            {
                GameObject go = Resources.Load<GameObject>("Game/zha");
                sys = Instantiate(go, transform).GetComponent<ParticleSystem>();
            }
            sys.Play();
            AudioController.Ins.PlayEffect("win");
            Invoke(nameof(ShowGold), 2);
        }
    }

    void ShowGold() 
    {
        Action action = NextLevel;
        ClearBuff();
        DotManager.Instance.sendEvent("Game_finish", DottingType.Tga, new Dictionary<string, object> { { "Level_ID", currentLevel } });
        DotManager.Instance.sendEvent("battle_success", DottingType.Af, new Dictionary<string, object> { { "Level_ID", currentLevel } });
        PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Gold, RedType.common, action);
    }

    /// <summary>
    /// 失败
    /// </summary>
    public void Failure() 
    {
        DotManager.Instance.sendEvent("Game_failed", DottingType.Tga, new Dictionary<string, object> { { "Level_ID", currentLevel } });
        DotManager.Instance.sendEvent("battle_fail", DottingType.Af, new Dictionary<string, object> { { "Level_ID", currentLevel } });
        isPause = true;
       
        snake.Death();
        Invoke(nameof(ShowFailure),1);
    }

    void ShowFailure() 
    {
        ClearBuff();
        PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Failure);
    }

    /// <summary>
    /// 复活
    /// </summary>
    public void Resurrection() 
    {
        snake.Resurrection();
        ClearBuff();
        isPause = false;
    }

    /// <summary>
    /// 重新开始
    /// </summary>
    public void ReStart() 
    {
        isPause = false;
        OpenLevel(currentLevel);
    }

    public void AddBody(Prop prop) 
    {
        triggerBallNum ++;
        triggerwaveBallNum ++;
        EventMgrHelper.Ins.PushEvent(EventDef.UpdateLevelPro,triggerBallNum, ballTotalNum);
        snake.AddBody(true);
        props.Remove(prop);
        Destroy(prop.gameObject);
        SetHand(3);

        //过关
        if (triggerBallNum >= ballTotalNum)
        {
            Prop zd = GetPropByType(PropType.zhongdian);
            Prop qi = GetPropByType(PropType.qi);
            zd.gameObject.SetActive(true);

            if (qi == null) 
            {
                GameObject go = Resources.Load<GameObject>("Prop/prop/" + (PropType.qi).ToString());
                qi = Instantiate(go, ObstacleGroup).AddComponent<Prop>();
                qi.InitQi();
                props.Add(qi);
            }

            qi.gameObject.SetActive(true);
            qi.transform.position = zd.oriPos;
            qi.transform.DOMoveZ(zd.oriPos.z + 1, 1).onComplete = () => {
                SetHand(2);
            };
            zd.transform.DOMoveZ(-8, 0);
            zd.transform.DOMoveZ(zd.oriPos.z, 1);
            EventMgrHelper.Ins.PushEvent(EventDef.HideTop);
            AudioController.Ins.PlayEffect("exit_gate_entering");
        }
        else 
        {
            //波次结束
            if (triggerwaveBallNum>=waveBallNum) 
            {
                currentWave++;
                CreateWave();
            }
        }
    }

    /// <summary>
    /// 添加buff
    /// </summary>
    public  void AddBuff(BuffType buff,Prop prop,float timer) 
    {
        if (prop != null) {
            props.Remove(prop);
            Destroy(prop.gameObject);
        }

        bool status = false;
        if (buffs.TryGetValue(buff, out status))
        {
              buffs[buff] = true;
        }
        else 
        {
            buffs.Add(buff,true);
        }

        if(buff==BuffType.CT)
             snake.SetXiTie(true);

        if (buff == BuffType.JS)
            snake.speed = snake.addSpedd;

        EventMgrHelper.Ins.PushEvent(EventDef.ShowBuffPro, (int)buff, floatdata0: timer);
    }

    /// <summary>
    /// 清除所有buff
    /// </summary>
    public void ClearBuff() 
    {
        if (snake != null) 
        {
            snake.SetXiTie(false);
            snake.speed = snake.defSpeed;
        }
      
        buffs.Clear();
        EventMgrHelper.Ins.PushEvent(EventDef.ClearBuff);
    }

    /// <summary>
    /// 去除指定buff
    /// </summary>
    public void RepelBuff(BuffType type)
    {
        if (type == BuffType.CT)
            snake.SetXiTie(false);

        if (type == BuffType.JS)
            snake.speed = snake.defSpeed;

        buffs[type] = false;
        EventMgrHelper.Ins.PushEvent(EventDef.RemoveBuff, (int)type);
    }


    /// <summary>
    /// 爆炸 
    /// </summary>
    public void Burst(Prop prop)
    {
        props.Remove(prop);
        Destroy(prop.gameObject);

        snake.RemoveBody();
    }

    /// <summary>
    /// 飞弹
    /// </summary>
    public void Missile(Prop prop)
    {
        props.Remove(prop);
        prop.Bomb();
      
        for (int i = 0; i < props.Count; i++)
        {
            Prop item =  props[i];

            if ((int)item.propType >= (int)PropType.Cylinder1 && (int)item.propType <= (int)PropType.Cylinder5 && Vector3.Distance(item.transform.position, prop.transform.position) < 2)
            {
                props.RemoveAt(i);
                item.Bomb();
            };
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            currentWave++;
            CreateWave();
        }

        CheckBuff();
    }

    void CheckBuff() 
    {
        bool ct = false;
        if (buffs.TryGetValue(BuffType.CT,out ct))
        {
            if (ct)
            {
                foreach (var item in props)
                {
                    if ((item.propType == PropType.qiu || item.propType == PropType.qiu_2) && item.wave == currentWave && Vector3.Distance(snake.transform.position, item.transform.position) < 2)
                    {
                        if(!item.isAdsorption)
                            item.isAdsorption = true;
                    }
                }
            }
        }
    }


    public bool IsZT() 
    {
        bool zt = false;
        if (buffs.TryGetValue(BuffType.ZT, out zt))
        {
            return zt;
        }
        else
        {
            return false;
        }
    }
   


}
