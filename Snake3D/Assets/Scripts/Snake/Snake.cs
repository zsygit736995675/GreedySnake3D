using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Snake : MonoBehaviour
{
    /// <summary>
    /// 身体
    /// </summary>
    List<SnakeBody> bodys = new List<SnakeBody>();

    /// <summary>
    /// 移动速度
    /// </summary>
    public float speed=3f;

    /// <summary>
    /// 默认速度
    /// </summary>
    public float defSpeed = 3;

    /// <summary>
    /// 加速速度
    /// </summary>
    public float addSpedd = 6;

    /// <summary>
    /// 转弯速度
    /// </summary>
    public float angleSpeed=9;

    /// <summary>
    /// 第一节偏移量
    /// </summary>
    float firstOffset = 0.3f;
    /// <summary>
    /// 其它节偏移量
    /// </summary>
    float otherOffset = 0.25f;

    /// <summary>
    /// 目标位置
    /// </summary>
   public Vector3 targetPos;

    /// <summary>
    /// 时否滑动
    /// </summary>
    bool isSlide=false;

    SnakeEntity entity;

    Vector3 oriPos;
    Vector3 oriAng;

    Transform tongkong1, tongkong2 ,huan ,xitie_01, dingzi;


    public bool isDeath = false;

    LineRenderer line;

    bool isInit = false;
    /// <summary>
    /// 特效列表
    /// </summary>
    List<ParticleSystem> pars = new List<ParticleSystem>();

    public void InitCreate(Vector3 pos,Vector3 ang) 
    {
        oriPos = pos;
        oriAng = ang;
        isDeath = false;
        transform.position = pos;
        transform.eulerAngles = ang;
        speed = defSpeed;
        if (!isInit) 
        {
            isInit = true;
            tongkong1 = transform.SeachTrs<Transform>("tongkong1");
            tongkong2 = transform.SeachTrs<Transform>("tongkong2");
            huan=transform.SeachTrs<Transform>("yuan1383");
            GameObject go = Resources.Load<GameObject>("Game/dingzi");
            dingzi = Instantiate(go).transform;
            line = GetComponent<LineRenderer>();
         
        }
        line.enabled = false;
        dingzi.gameObject.SetActive(false);
        huan.gameObject.SetActive(false);

        SetFace("she");

        foreach (var item in bodys)
        {
            Destroy(item.gameObject);
        }

        bodys.Clear();

        int count = 2;
        if (Snake_Game.Ins.currentLevel > 1) 
        {
            count = UnityEngine.Random.Range(3,9);
        }
        for (int i = 0; i < count; i++)
        {
            AddBody();
        }
    }

    public void SetXiTie(bool isA)
    {
        if (!isA) 
        {
            if(xitie_01!=null)
                 Destroy(xitie_01.gameObject);
            return;
        }

        if (xitie_01 != null) 
        {
            Destroy(xitie_01.gameObject);
        }
        GameObject go = Resources.Load<GameObject>("Game/xitie_01");
        xitie_01 = Instantiate(go, transform.GetChild(0)).transform;
        xitie_01.gameObject.SetActive(true);
        xitie_01.localScale = Vector3.one*3;
        xitie_01.localPosition = new Vector3(0f,0f,0.2f);
        xitie_01.GetComponent<ParticleSystem>().Play();
    }

    public void Resurrection() 
    {
        speed = defSpeed;
        line.enabled = false;
        dingzi.gameObject.SetActive(false);
        huan.gameObject.SetActive(false);
        isDeath = false;
        transform.position = oriPos;
        transform.eulerAngles = oriAng;
        SetFace("she");

        targetPos = oriPos;
        for (int i = 0; i < bodys.Count; i++)
        {
            if (i == 0)
            {
                bodys[i].transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + firstOffset);
                bodys[i].offset = firstOffset;
            }
            else
            {
                Vector3 after = bodys[i - 1].transform.position;
                bodys[i].transform.position = new Vector3(after.x, after.y, after.z + otherOffset);
                bodys[i].offset = otherOffset;
            }
        }
        
    }

    private void Start()
    {
        entity = transform.GetChild(0).gameObject.AddComponent<SnakeEntity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !Snake_Game.Ins.isvictory) 
        {
            if (IsPointerOverUIObject()) 
            {
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                dingzi.gameObject.SetActive(false);
                targetPos = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
                UpdateLine();
                dingzi.transform.position = new Vector3(targetPos.x,0.25f,targetPos.z);
                Snake_Game.Ins.StartGame();
                isSlide = false;
            }
        }

        if (Input.GetMouseButtonUp(0) && !Snake_Game.Ins.isvictory && !Snake_Game.Ins.isPause && Snake_Game.Ins.isStart) 
        {
            dingzi.gameObject.SetActive(true);
        }

        if (pars.Count > 0) 
        {
            foreach (var item in pars)
            {
                if (item.gameObject.activeSelf && !item.isPlaying) 
                {
                    item.gameObject.SetActive(false);
                }
            }
        }

        Move();

        EndMove();
    }

    void UpdateLine() 
    {
        line.enabled = true;
        line.SetVertexCount(2);
        line.SetPosition(0, new Vector3(transform.position.x,0.2f, transform.position.z)) ;
        line.SetPosition(1,new Vector3(targetPos.x,0.2f,targetPos.z));
    }

    ParticleSystem fsys;
    public void Death() 
    {
        isDeath = true;
        AudioController.Ins.PlayEffect("bomb_explode");
        SetFace("siwang");

        line.enabled = false;
        dingzi.gameObject.SetActive(false);
        huan.gameObject.SetActive(false);

        if (fsys == null)
        {
            GameObject go = Resources.Load<GameObject>("Game/qiu_bai");
            fsys = Instantiate(go, transform).GetComponent<ParticleSystem>();
        }
        fsys.transform.localPosition = new Vector3(0, 0.3f, 0f);
        fsys.transform.localScale = Vector3.one * 0.3f;
        fsys.Play();

        foreach (var item in bodys)
        {
            item.Death();
        }
    }

    ParticleSystem GetEffect() 
    {
        ParticleSystem sys = null;
        if (pars.Count > 0) 
        {
            foreach (var item in pars)
            {
                if (!item.gameObject.activeSelf)
                {
                    sys = item;
                    break;
                }
            }
        }
        if (sys == null)
        {
            GameObject go = Resources.Load<GameObject>("Game/qiu_hong");
            GameObject qiu = Instantiate(go, transform);
            sys = qiu.GetComponent<ParticleSystem>();
            pars.Add(sys);
        }
        sys.gameObject.SetActive(true);
        sys.transform.localPosition = new Vector3(0,0.2f,0f);
        sys.transform.localScale = Vector3.one * 0.5f;
        sys.Play();
     
        return sys;
    }


    public void AddBody(bool isAni=false)
    {
        Action action = () => {

            SnakeBody body = Instantiate(ResourceMgr.GetInstance.Load<GameObject>("Prop/she/she-shenti", false), transform.parent).AddComponent<SnakeBody>();
            if (bodys.Count == 0)
            {
                body.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + firstOffset);
                body.offset = firstOffset;
                body.target = transform;
            }
            else
            {
                Vector3 offset = Vector3.zero ;
                if (bodys.Count == 1) 
                {
                    offset = bodys[0].transform.position - transform.position;
                }
                else 
                {
                    offset = bodys[bodys.Count - 1].transform.position - bodys[bodys.Count - 2].transform.position;
                }

                Vector3 after = bodys[bodys.Count - 1].transform.position;
                body.transform.position = after+ offset;
                body.offset = otherOffset;
                body.target = bodys[bodys.Count - 1].transform;
            }

            if (isAni) 
            {
                body.PlayAdd();
            }
            bodys.Add(body);
        };

        if (isAni)
        {
            AudioController.Ins.PlayEffect("bomb");

            eatCount += 10;

            Eat();

            GetEffect();

            transform.DOScale(new Vector3(2.5f, 11, 2.5f), 0.2f).onComplete = () => {
                transform.DOScale(new Vector3(2f, 7, 2f), 0.2f);
            };
            
            if (bodys.Count > 0)
            {
                for (int i = 0; i < bodys.Count; i++)
                {
                    if (i == bodys.Count - 1)
                    {
                        bodys[i].Enlarge(i, () => {

                            action?.Invoke();

                        });
                    }
                    else
                    {
                        bodys[i].Enlarge(i);
                    }
                }
            }
            else 
            {
                action?.Invoke();
            }
        }
        else 
        {
            action?.Invoke();
        }
    }

    /// <summary>
    /// 吃
    /// </summary>
    int eatCount = 0;
    void Eat() 
    {
        if (Snake_Game.Ins.isPause||isDeath) 
        {
            return;
        }

        eatCount--;
        if (eatCount <= 0) 
        {
            SetFace("she");
            return;
        }

        if (eatCount % 2 == 0)
        {
            SetFace("she_bizui");
        }
        else 
        {
            SetFace("she_zhangzui");
        }
        Invoke(nameof(Eat), 0.1f);
    }


    /// <summary>
    /// 设置表情
    /// </summary>
    void SetFace(string nameStr) 
    {
        if (isDeath && !nameStr.Equals("siwang"))
            return;

        if (nameStr.Equals("siwang") || nameStr.Equals("she_yun"))
        {
            tongkong1.gameObject.SetActive(false);
            tongkong2.gameObject.SetActive(false);
        }
        else 
        {
            tongkong1.gameObject.SetActive(true);
            tongkong2.gameObject.SetActive(true);
        }

        Texture tex = Resources.Load<Texture>("Prop/Img/"+ nameStr);
        transform.GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", tex);
    }

    /// <summary>
    /// 删除身体
    /// </summary>
    public void RemoveBody()
    {
        if (bodys.Count >= 3)
        {
            for (int i = 0; i < bodys.Count; i++)
            {
                if (i <= bodys.Count - 4)
                {
                    bodys[i].Enlarge(i);
                    if (bodys.Count - 4 == i) 
                    {
                        bodys[i].Enlarge(i,()=> {

                            for (int j = 0; j < 3; j++)
                            {
                                SnakeBody body = bodys[bodys.Count - 1];
                                bodys.RemoveAt(bodys.Count - 1);
                                body.Burst(j);
                            }
                        });
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < bodys.Count; i++)
            {
                SnakeBody body = bodys[i];
                bodys.RemoveAt(i);
                body.Burst(i);
            }
        }
    }

    /// <summary>
    /// 撞到自己的身体
    /// </summary>
    public void HitBody(SnakeBody body) 
    {
        int index = bodys.IndexOf(body);
        if (index > 2) 
        {
            Snake_Game.Ins.Failure();
        }
    }

    private void Move()
    {
        if (Snake_Game.Ins.isStart && !Snake_Game.Ins.isPause )
        {
            float curSpeed=2;
            if (!isSlide)
            {
                if (Vector3.Distance(transform.position, targetPos) > 0.5f)
                {
                    UpdateLine();
                    curSpeed = speed;
                    float tempAngle = Vector3.Angle(transform.forward, targetPos - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), angleSpeed * Time.deltaTime);
                }
                else
                {
                    line.enabled = false;
                    dingzi.gameObject.SetActive(false);
                    huan.gameObject.SetActive(true);
                    Invoke(nameof(HideHuan),1);
                    isSlide = true;
                }
            }
            else 
            {
                curSpeed = speed / 10;
            }

            Vector3 offset = transform.forward * curSpeed * Time.deltaTime;
            Vector3 headPos = transform.position;
            transform.position = new Vector3(headPos.x + offset.x, 0, headPos.z + offset.z);

            if (bodys.Count > 0)
            {
                for (int i = bodys.Count - 2; i >= 0; i--)
                {
                    bodys[i + 1].Move(bodys[i].transform.position, curSpeed);
                }
                bodys[0].Move(headPos, curSpeed);
            }
        }
    }

    void HideHuan() 
    {
        huan.gameObject.SetActive(false);
    }

    private void EndMove()
    {
        if (Snake_Game.Ins.isvictory) 
        {
            speed = addSpedd;
            line.enabled = false;
            dingzi.gameObject.SetActive(false);
            if (Vector3.Distance(transform.position, targetPos) > 0.2f)
            {
                float tempAngle = Vector3.Angle(transform.forward, targetPos - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), angleSpeed * Time.deltaTime);
                Vector3 offset = transform.forward * speed * Time.deltaTime;
                Vector3 headPos = transform.position;
                transform.position = new Vector3(headPos.x + offset.x, 0, headPos.z + offset.z);

                if (bodys.Count > 0)
                {
                    for (int i = bodys.Count - 2; i >= 0; i--)
                    {
                        bodys[i + 1].Move(bodys[i].transform.position, speed);
                    }
                    bodys[0].Move(headPos, speed);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Snake_Game.Ins.isvictory || Snake_Game.Ins.isPause)
            return;

        if (collision.gameObject.tag.Equals("wall") || collision.gameObject.tag.Equals("obstacle"))
        {
            Vector3 offset = new Vector3();
            foreach (var item in collision.contacts)
            {
                if (item.point.x < transform.position.x)
                {
                    offset.x = 0.5f;
                }
                else
                {
                    offset.x = -0.5f;
                }

                if (item.point.z < transform.position.z)
                {
                    offset.z = 0.5f;
                }
                else
                {
                    offset.z = -0.5f;
                }
                break;
            }

            SetFace("she_yun");
            targetPos = transform.position + offset;
            transform.DOMove(transform.position + offset, 0.5f).onComplete = () => {
                SetFace("she");
            };
        }
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
        eventDataCurrentPosition.position = Input.GetTouch(0).position;
#else
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
#endif

        List<RaycastResult> results = new List<RaycastResult>();
        if (EventSystem.current)
        {
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        }

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }

}
