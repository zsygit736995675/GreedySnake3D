using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Editorpanel : MonoBehaviour
{
    Button openlevel, nextlevel, nextwave, save, openPath, openwave;

    InputField levelInput, waveInput,
        inputposx, inputposy, inputposz,
        inputscalex, inputscaley, inputscalez,
        inputanglex, inputangley, inputanglez,
        inputzzspeed, inputzzjg, inputzzjgsj,
        Inputgzsd, Inputgzjd, Inputgzsj,
        Inputysd, Inputyjs, Inputysj,
        Inputxsd, Inputxjs, Inputxsj,
        Inputsxsd, Inputsxmb, Inputsxsj,
        Inputpmsd, Inputpmmb, Inputpmsj,
        Inputcolor, Inputppathsc, Inputpathjg
        ;

    RectTransform rect;

    Text tips, txt_name,txt_level, txt_wave, centerx, Textpos, Textsca, Textang;

    Transform panel, propPanel, scene;

    Toggle pathToggle;

    /// <summary>
    /// 当前选中的道具
    /// </summary>
    public  Prop seleteProp;

    /// <summary>
    /// 道具列表
    /// </summary>
    List<Prop> propList = new List<Prop>();


    LevelModel entity;

    void Start()
    {
        openPath = transform.SeachTrs<Button>("openPath");
        openlevel = transform.SeachTrs<Button>("openlevel");
        nextlevel = transform.SeachTrs<Button>("nextlevel");
        nextwave = transform.SeachTrs<Button>("nextwave");
        openwave = transform.SeachTrs<Button>("openwave");
        save = transform.SeachTrs<Button>("save");

        levelInput = transform.SeachTrs<InputField>("level");
        waveInput = transform.SeachTrs<InputField>("wave");
        levelInput.text = "1";
        waveInput.text = "1";

        panel = transform.SeachTrs<Transform>("panel");
        tips = transform.SeachTrs<Text>("tips");

        propPanel = transform.SeachTrs<Transform>("propPanel");
     
        txt_name = transform.SeachTrs<Text>("txt_name");
        txt_level = transform.SeachTrs<Text>("txt_level");
        txt_wave = transform.SeachTrs<Text>("txt_wave");

        inputposx = transform.SeachTrs<InputField>("inputposx");
        inputposy = transform.SeachTrs<InputField>("inputposy");
        inputposz = transform.SeachTrs<InputField>("inputposz");

        Textpos = transform.SeachTrs<Text>("Textpos");
        Textsca = transform.SeachTrs<Text>("Textsca");
        Textang = transform.SeachTrs<Text>("Textang");

        inputposx.onValueChanged.AddListener((str) => {
            if (seleteProp != null) 
                seleteProp.SetPos ( new Vector3(float.Parse(str),seleteProp.transform.position.y,seleteProp.transform.position.z));
        });
        inputposy.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.SetPos(new Vector3(seleteProp.transform.position.x, float.Parse(str), seleteProp.transform.position.z));
        });
        inputposz.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.SetPos(new Vector3(seleteProp.transform.position.x, seleteProp.transform.position.y, float.Parse(str)));
        });

        inputscalex = transform.SeachTrs<InputField>("inputscalex");
        inputscaley = transform.SeachTrs<InputField>("inputscaley");
        inputscalez = transform.SeachTrs<InputField>("inputscalez");
        inputscalex.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.SetScale(new Vector3(float.Parse(str), seleteProp.transform.localScale.y, seleteProp.transform.localScale.z));
        });
        inputscaley.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.SetScale(new Vector3(seleteProp.transform.localScale.x, float.Parse(str), seleteProp.transform.localScale.z));
        });
        inputscalez.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.SetScale(new Vector3(seleteProp.transform.localScale.x, seleteProp.transform.localScale.y, float.Parse(str)));
        });

        inputanglex = transform.SeachTrs<InputField>("inputanglex");
        inputangley = transform.SeachTrs<InputField>("inputangley");
        inputanglez = transform.SeachTrs<InputField>("inputanglez");
        inputanglex.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.SetAngle( new Vector3(float.Parse(str), seleteProp.transform.eulerAngles.y, seleteProp.transform.eulerAngles.z));
        });
        inputangley.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.SetAngle( new Vector3(seleteProp.transform.eulerAngles.x, float.Parse(str), seleteProp.transform.eulerAngles.z));
        });
        inputanglez.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.SetAngle( new Vector3(seleteProp.transform.eulerAngles.x, seleteProp.transform.eulerAngles.y, float.Parse(str)));
        });

        inputzzspeed = transform.SeachTrs<InputField>("inputzzspeed");
        inputzzjg = transform.SeachTrs<InputField>("inputzzjg");
        inputzzjgsj = transform.SeachTrs<InputField>("inputzzjgsj");
        inputzzspeed.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.rotationSpeed = float.Parse(str);
        });
        inputzzjg.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.rotationinterval = float.Parse(str);
        });
        inputzzjgsj.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.rotationintervaltime = float.Parse(str);
        });
        Inputgzsd = transform.SeachTrs<InputField>("Inputgzsd");
        Inputgzjd = transform.SeachTrs<InputField>("Inputgzjd");
        Inputgzsj = transform.SeachTrs<InputField>("Inputgzsj");
        Inputgzsd.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.revolutionSpeed = float.Parse(str);
        });
        Inputgzjd.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.revolutioninterval = float.Parse(str);
        });
        Inputgzsj.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.revolutionintervaltime = float.Parse(str);
        });
        centerx = transform.SeachTrs<Text>("centerx");

        Inputysd = transform.SeachTrs<InputField>("Inputysd");
        Inputyjs = transform.SeachTrs<InputField>("Inputyjs");
        Inputysj = transform.SeachTrs<InputField>("Inputysj");
        Inputysd.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.yMoveSpeed = float.Parse(str);
        });
        Inputyjs.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.yMoveDis = float.Parse(str);
        });
        Inputysj.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.yMoveInterval = float.Parse(str);
        });

        Inputxsd = transform.SeachTrs<InputField>("Inputxsd");
        Inputxjs = transform.SeachTrs<InputField>("Inputxjs");
        Inputxsj = transform.SeachTrs<InputField>("Inputxsj");
        Inputxsd.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.xMoveSpeed = float.Parse(str);
        });
        Inputxjs.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.xMoveDis = float.Parse(str);
        });
        Inputxsj.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.xMoveInterval = float.Parse(str);
        });

        Inputsxsd = transform.SeachTrs<InputField>("Inputsxsd");
        Inputsxmb = transform.SeachTrs<InputField>("Inputsxmb");
        Inputsxsj = transform.SeachTrs<InputField>("Inputsxsj");
        Inputsxsd.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.sxSpeed = float.Parse(str);
        });
        Inputsxmb.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.sxDis = float.Parse(str);
        });
        Inputsxsj.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.sxInterval = float.Parse(str);
        });

        Inputpmsd = transform.SeachTrs<InputField>("Inputpmsd");
        Inputpmmb = transform.SeachTrs<InputField>("Inputpmmb");
        Inputpmsj = transform.SeachTrs<InputField>("Inputpmsj");
        Inputpmsd.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.pmSpeed = float.Parse(str);
        });
        Inputpmmb.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.pmDis = float.Parse(str);
        });
        Inputpmsj.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.pmInterval = float.Parse(str);
        });

        Inputcolor = transform.SeachTrs<InputField>("Inputcolor");
        Inputcolor.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.ChangeColor(int.Parse(str));
        });

        pathToggle=transform.SeachTrs<Toggle>("pathToggle");
        Inputppathsc= transform.SeachTrs<InputField>("Inputppathsc");
        Inputpathjg = transform.SeachTrs<InputField>("Inputpathjg");
        pathToggle.onValueChanged.AddListener((isOn)=> {
            if (seleteProp != null)
                seleteProp.IsMovePath = isOn;
        });
        Inputppathsc.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.pathMoveSpeed =  int.Parse(str);
        });
        Inputpathjg.onValueChanged.AddListener((str) => {
            if (seleteProp != null)
                seleteProp.pathMoveInterval = int.Parse(str);
        });

        scene = GameObject.Find("SceenObj").transform;
        openPath.onClick.AddListener(OpenPath);
        save.onClick.AddListener(onClickSave);
        nextlevel.onClick.AddListener(onClickNext);
        openlevel.onClick.AddListener(OpenScene);
        nextwave.onClick.AddListener(onClickNectWave);
        openwave.onClick.AddListener(OnClickOpenWave);
        propPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 保存
    /// </summary>
    void onClickSave()
    {
        if (string.IsNullOrEmpty(levelInput.text))
        {
            ShowTips("输入关卡");
            return;
        }
        entity.CreateSave(int.Parse(levelInput.text),  propList);
        string json = JsonConvert.SerializeObject(entity);
        File.WriteAllText(UnityEngine.Application.dataPath + "/Resources/Level/" + levelInput.text + ".json", json);
        ShowTips("保存成功！");
    }

    /// <summary>
    /// 下一关
    /// </summary>
    void onClickNext()
    {
        if (string.IsNullOrEmpty(levelInput.text))
        {
            ShowTips("输入关卡");
            return;
        }

        onClickSave();

        ClearAll();
        int level = int.Parse(levelInput.text);
        level++;

        levelInput.text = level.ToString();

        OpenScene();
    }

    /// <summary>
    /// 清空道具和小球
    /// </summary>
    void Clear()
    {
        for (int i = 0; i < propList.Count; i++)
        {
            if (!propList[i].isStatic) 
            {
                propList[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 清空所有
    /// </summary>
    void ClearAll() 
    {
        foreach (var item in propList)
        {
            Destroy(item.gameObject);
        }
        propList.Clear();
    }

    void ShowWave()
    {
        foreach (var item in propList)
        {
            if (item.wave == int.Parse(waveInput.text))
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 下一波
    /// </summary>
    void onClickNectWave() 
    {
        Clear();
        if (string.IsNullOrEmpty(waveInput.text))
        {
            ShowTips("输入波次");
            return;
        }
        waveInput.text = int.Parse(waveInput.text) + 1 +"";
        ShowWave();
    }

    void OnClickOpenWave() 
    {
        Clear();
        if (string.IsNullOrEmpty(waveInput.text))
        {
            ShowTips("输入波次");
            return;
        }
        ShowWave();
    }


    
    /// <summary>
    /// 加载场景
    /// </summary>
    void OpenScene()
    {
        if (string.IsNullOrEmpty(levelInput.text))
        {
            ShowTips("输入关卡");
            return;
        }
        ClearAll();
        waveInput.text = "1";
        if (File.Exists(UnityEngine.Application.dataPath + "/Resources/Level/" + levelInput.text + ".json"))
        {
            ShowTips("打开关卡");
            string[] RawString = File.ReadAllLines(UnityEngine.Application.dataPath + "/Resources/Level/level" + levelInput.text + ".json");  //路径
            string json="";
            foreach (var item in RawString)
            {
                json += item;
            }
            entity = JsonConvert.DeserializeObject<LevelModel>(json);

            CreateProp();
            OpenWave(1);
        }
        else 
        {
            ShowTips("新的关卡");
            entity = new LevelModel();
        }
    }

    /// <summary>
    /// 复制一个
    /// </summary>
    void Copy()
    {
        if (seleteProp == null)
        {
            ShowTips("没有选中的道具");
            return;
        }

        Transform temp = Instantiate(seleteProp.gameObject, scene).transform;
        Prop po = temp.GetComponentInChildren<Prop>();
        po.Init(seleteProp.propType, this, int.Parse(waveInput.text));
        po.gameObject.name = seleteProp.gameObject.name;
        seleteProp = po;
        propList.Add(seleteProp);
    }

    /// <summary>
    /// 生成
    /// </summary>
    void CreateProp(PropType type)
    {
        GameObject go = Resources.Load<GameObject>("pre/" + type.ToString());
        GameObject pre = Instantiate(go, scene);
        oriY ori = pre.GetComponent<oriY>();
        pre.transform.position = new Vector3(0, ori.Oriy, 0);
        seleteProp = pre.AddComponent<Prop>();
        seleteProp.Init(type, this, int.Parse(waveInput.text));
        propList.Add(seleteProp);
    }

    /// <summary>
    /// 生成障碍
    /// </summary>
    void CreateProp() 
    {
        if (entity != null && entity.propList != null) 
        {
            foreach (PropModel item in entity.propList)
            {
                GameObject go = Resources.Load<GameObject>("pre/" + ((PropType)item.propType).ToString());
                GameObject pre = Instantiate(go, scene);
                Prop prop = pre.AddComponent<Prop>();
                prop.Init((PropType)item.propType, this,item.wave);
                prop.Init(item);
                propList.Add(prop);
            }
        }
    }

    /// <summary>
    /// 生成指定波次的道具
    /// </summary>
    void OpenWave(int wave) 
    {
        if (entity != null && entity.waveList != null)
        {
            if (entity.waveList != null) 
            {          
                foreach (WaveModel item in entity.waveList)
                {
                    foreach (PropModel prop in item.propList)
                    {
                        GameObject go = Resources.Load<GameObject>("pre/" + ((PropType)prop.propType).ToString());
                        GameObject pre = Instantiate(go, scene);
                        Prop p = pre.AddComponent<Prop>();
                        p.Init((PropType)prop.propType, this,prop.wave);
                        p.Init(prop);
                        propList.Add(p);
                        p.gameObject.SetActive(prop.wave == wave);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 打开目录
    /// </summary>
    void OpenPath() 
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(Application.streamingAssetsPath);
        System.Diagnostics.Process.Start(Application.streamingAssetsPath);
    }

   
    /// <summary>
    /// panel显隐
    /// </summary>
    void ShowPanel() 
    {
        rect = panel.GetComponent<RectTransform>();
        if (rect.anchoredPosition.x == 0) 
        {
            rect.anchoredPosition = new Vector2(-500,0);
            panel.gameObject.SetActive(false);
        }
        else
        if (rect.anchoredPosition.x == -500) 
        {
            panel.gameObject.SetActive(true);
            rect.anchoredPosition = Vector2.zero;
            propPanel.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 显示错误信息
    /// </summary>
    /// <param name="txt"></param>
    public void ShowTips(string txt)
    {
        tips.text = txt;
        Invoke("CloseTips", 2);
    }

    void CloseTips()
    {
        tips.text = "";
    }

    

    void Update()
    {
        //小球 -----------------------------------------------------------
        if (Input.GetKey(KeyCode.A))
        {
            //普通
            if (Input.GetKeyDown(KeyCode.Alpha1)) 
            {
                CreateProp(PropType.qiu);
            }
            //洋葱圈
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CreateProp(PropType.qiu_2);
            }
        }

        //道具 -------------------------------------------------------------
        if (Input.GetKey(KeyCode.S))
        {
            //磁铁
            if (Input.GetKeyDown(KeyCode.Alpha1)) 
            {
                CreateProp(PropType.citie);
            }
            //暂停
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CreateProp(PropType.zanting);
            }
            //炸弹
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CreateProp(PropType.zhadan);
            }
            //炸弹飞机
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                CreateProp(PropType.feiji);
            }
            //加速
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                CreateProp(PropType.jiasu);
            }
        }
       
        //障碍-----------------------------------------------------------------
        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) 
            {
                CreateProp(PropType.Cylinder1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CreateProp(PropType.Cylinder2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CreateProp(PropType.Cylinder3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                CreateProp(PropType.Cylinder4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                CreateProp(PropType.Cylinder5);
            }
        }

        //起止点-----------------------------------------------------------------
        //起点
        if (Input.GetKey(KeyCode.F))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) 
            {
                 CreateProp(PropType.qidian);
            }
            //终点
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CreateProp(PropType.zhongdian);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CreateProp(PropType.qi);
            }
        }

        //panel显隐--------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowPanel();
        }

        //复制--------------------------------------------------------------------------
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand))&&Input.GetKeyDown(KeyCode.D))
        {
            Copy();
        }

        //清空路径--------------------------------------------------------------------------
        if (Input.GetKey(KeyCode.LeftAlt)&& Input.GetKeyDown(KeyCode.Delete))
        {
            if (seleteProp != null) 
            {
                seleteProp.ClearPath();
                ShowTips("路径已清空");
            }
        }

        //删除---------------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.Delete)) 
        {
            if (seleteProp == null) 
            {
                ShowTips("没有选中的道具");
                return;
            }
            if (propList.Contains(seleteProp)) 
            {
                propList.Remove(seleteProp);
            }
            Destroy(seleteProp.gameObject);
            seleteProp = null;
        }
    }

    /// <summary>
    /// 显示道具设置页面
    /// </summary>
    public  void ShowPropPanel()
    {
        if (seleteProp != null && !panel.gameObject.activeSelf && !Input.GetKey(KeyCode.LeftShift))
        {
            propPanel.gameObject.SetActive(true);
            txt_name.text = seleteProp.propType.ToString();
            txt_level.text = levelInput.text;
            txt_wave.text = waveInput.text;

            Textpos.text = seleteProp.transform.position.ToString();
            Textsca.text = seleteProp.transform.localScale.ToString(); 
            Textang.text = seleteProp.transform.eulerAngles.ToString();

            inputzzspeed.text = seleteProp.rotationSpeed.ToString();
            inputzzjg.text = seleteProp.rotationinterval.ToString();
            inputzzjgsj.text = seleteProp.rotationintervaltime.ToString();

            Inputgzsd.text = seleteProp.revolutionSpeed.ToString();
            Inputgzjd.text = seleteProp.revolutioninterval.ToString();
            Inputgzsj.text = seleteProp.revolutionintervaltime.ToString();

            centerx.text =  seleteProp.center.ToString();

            Inputxsd.text = seleteProp.xMoveSpeed.ToString();
            Inputxjs.text = seleteProp.xMoveDis.ToString();
            Inputxsj.text = seleteProp.xMoveInterval.ToString();

            Inputysd.text = seleteProp.yMoveSpeed.ToString();
            Inputyjs.text = seleteProp.yMoveDis.ToString();
            Inputysj.text = seleteProp.xMoveInterval.ToString();

            Inputsxsd.text = seleteProp.sxSpeed.ToString();
            Inputsxmb.text = seleteProp.sxDis.ToString();
            Inputsxsj.text = seleteProp.sxInterval.ToString();

            Inputpmsd.text = seleteProp.pmSpeed.ToString();
            Inputpmmb.text = seleteProp.pmDis.ToString();
            Inputpmsj.text = seleteProp.pmInterval.ToString();

            Inputcolor.text = seleteProp.colorInt.ToString();

            pathToggle.isOn = seleteProp.isMovePath;
            Inputppathsc.text = seleteProp.pathMoveSpeed.ToString();
            Inputpathjg.text = seleteProp.pathMoveInterval.ToString();
        }
        else 
        {
            propPanel.gameObject.SetActive(false);
        }
    }

    public void HideProPanel() 
    {
        propPanel.gameObject.SetActive(false);
    }

}
