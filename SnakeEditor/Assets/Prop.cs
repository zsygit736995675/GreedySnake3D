using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum PropType 
{
    /// <summary>
    /// 起始点
    /// </summary>
    qidian,
    /// <summary>
    /// 旗子
    /// </summary>
    qi,
    /// <summary>
    /// 结束点
    /// </summary>
    zhongdian,
    /// <summary>
    /// 障碍
    /// </summary>
    Cylinder1,
    Cylinder2,
    Cylinder3,
    Cylinder4,
    Cylinder5,

    /// <summary>
    /// 小球
    /// </summary>
    qiu,
    qiu_2,
    /// <summary>
    /// 道具
    /// </summary>
    citie,
    zanting,
    zhadan,
    feiji,
    jiasu,
   
}

[ExecuteInEditMode]
public class Prop : MonoBehaviour
{
    public int colorInt;

    public PropType propType;

    Editorpanel editor;

    Vector3 oriPos;

    Vector3 oriScale;

    Vector3 oriAngle;

    List<Vector3> paths = new List<Vector3>();

    oriY ori;

    public int wave;

    public  bool isStatic=false;

    //路径移动
    public bool isMovePath=false;
    public float pathMoveSpeed;
    public float pathMoveInterval;

    //自转
    public float rotationSpeed;
    public float rotationinterval;
    public float rotationintervaltime;
    float rotationOffset;
    bool isrotationRot = true;

    //公转
    public float revolutionSpeed;
    public float revolutioninterval;
    public float revolutionintervaltime;
    float revolutionOffset;
    bool isrevolutionRot = true;
    public  Vector3 center;

    //上下移动
    public float yMoveSpeed;
    public float yMoveDis;
    public float yMoveInterval;
    float yMoveOffset;
    bool isMoveY = true;

    //左右移动
    public float xMoveSpeed;
    public float xMoveDis;
    public float xMoveInterval;
    float xMoveOffset;
    bool isMoveX = true;

    //上下缩放
    public float sxSpeed;
    public float sxDis;
    public float sxInterval;
    bool isSxScale = true;

    //平面缩放
    public float pmSpeed;
    public float pmDis;
    public float pmInterval;
    float pmOffset;
    bool isPmScale = true;


    public void Init(PropType type, Editorpanel editor,int wave) 
    {
        propType = type;
        this.editor = editor;
        this.wave = wave;
        oriScale = Vector3.one;
        isStatic = ((int)propType >= (int)PropType.qidian  && (int)propType <= (int)PropType.Cylinder5);
        ori = GetComponent<oriY>();
    }

    public void Init(PropModel model)
    {
        propType = (PropType)model.propType;
        colorInt = model.colorInt;
        wave = model.wave;

        SetPos( new Vector3(model.pos.X, model.oriY, model.pos.Z));
        SetScale( new Vector3(model.scale.X,model.scale.Y,model.scale.Z));
        transform.eulerAngles = new Vector3(model.angle.X, model.angle.Y, model.angle.Z);

        if (model.rotation != null) 
        {
            rotationSpeed = model.rotation.X;
            rotationinterval = model.rotation.Y;
            rotationintervaltime = model.rotation.Z;
        }

        if (model.revolution!=null) 
        {
            revolutionSpeed = model.revolution.X;
            revolutioninterval = model.revolution.Y;
            revolutionintervaltime = model.revolution.Z;
        }
      
        if(model.revolutionCenter!=null)
              center = new Vector3(model.revolutionCenter.X,model.revolutionCenter.Y,model.revolutionCenter.Z);

        if (model.yMove != null) 
        {
            yMoveSpeed = model.yMove.X;
            yMoveDis = model.yMove.Y;
            yMoveInterval = model.yMove.Z;
        }

        if (model.xMove != null) 
        {
            xMoveSpeed = model.xMove.X;
            xMoveDis = model.xMove.Y;
            xMoveInterval = model.xMove.Z;
        }

        if (model.yScale != null) 
        {
            sxSpeed = model.yScale.X;
            sxDis = model.yScale.Y;
            sxInterval = model.yScale.Z;
        }

        if (model.xzScale != null) 
        {
            pmSpeed = model.xzScale.X;
            pmDis = model.xzScale.Y;
            pmInterval = model.xzScale.Z;
        }

        if (model.paths != null) 
        {
            foreach (VectorM item in model.paths)
            {
                paths.Add(new Vector3(item.X,item.Y,item.Z));
            }

            if (model.pathV != null) 
            {
                pathMoveSpeed = model.pathV.X;
                pathMoveInterval = model.pathV.Y;
                IsMovePath = true;
            }
        }
    }

    public PropModel GetModel() 
    {
        PropModel model = new PropModel();
        model.propType = (int)propType;
        model.colorInt = colorInt;
        model.oriY = ori.Oriy;
        model.pos = new VectorM(oriPos);
        model.scale = new VectorM(oriScale);
        model.angle = new VectorM(oriAngle);
        model.rotation = new VectorM(rotationSpeed, rotationinterval, rotationintervaltime);
        model.revolution = new VectorM(revolutionSpeed, revolutioninterval, revolutionintervaltime);
        model.revolutionCenter = new VectorM(center);
        model.yMove = new VectorM(yMoveSpeed, yMoveDis, yMoveInterval);
        model.xMove = new VectorM(xMoveSpeed, xMoveDis, xMoveInterval);
        model.yScale = new VectorM(sxSpeed, sxDis, sxInterval);
        model.xzScale = new VectorM(pmSpeed, pmDis, pmInterval);
        model.isStatic = isStatic;
        model.wave = wave;
         if (paths.Count > 0)
        {
            model.paths = new List<VectorM>();
            foreach (Vector3 item in paths)
            {
                model.paths.Add(new VectorM(item));
            }
            model.pathV = new VectorM(pathMoveSpeed, pathMoveInterval, 0);
        }
        else 
        {
            model.paths = new List<VectorM>();
            model.pathV = new VectorM(0, 0, 0);
        }
        return model;
    }
    

    public void ClearPath() 
    {
        paths.Clear();
    }

    //偏移值
    Vector3 m_Offset;
    //当前物体对应的屏幕坐标
    Vector3 m_TargetScreenVec;
    private IEnumerator  OnMouseDown()
    {
        if (!IsPointerOverUIObject()) 
        {

            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (editor.seleteProp != null)
                {
                    editor.seleteProp.center = transform.position;
                    editor.ShowPropPanel();
                }
                else
                {
                    editor.ShowTips("没有选中的道具");
                }
            }
            else if (Input.GetKey(KeyCode.LeftAlt))
            {
                paths.Add(transform.position);
            }
            else
            {
                editor.seleteProp = this;
                //当前物体对应的屏幕坐标
                m_TargetScreenVec = Camera.main.WorldToScreenPoint(transform.position);
                //偏移值=物体的世界坐标，减去转化之后的鼠标世界坐标（z轴的值为物体屏幕坐标的z值）
                m_Offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_TargetScreenVec.z));
                //当鼠标左键点击
                while (Input.GetMouseButton(0))
                {
                    //当前坐标等于转化鼠标为世界坐标（z轴的值为物体屏幕坐标的z值）+ 偏移量
                    transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_TargetScreenVec.z)) + m_Offset;
                    SetPos(new Vector3(transform.position.x, ori.Oriy, transform.position.z));
                    editor.ShowPropPanel();
                    //等待固定更新
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }

    public  bool IsPointerOverUIObject()
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

    public void SetPos(Vector3 pos) 
    {
        transform.position = new Vector3(pos.x, pos.y, pos.z);
        oriPos = new Vector3(pos.x, pos.y, pos.z);
    }

    public void SetScale(Vector3 pos)
    {
        transform.localScale = new Vector3(pos.x, pos.y, pos.z);
        oriScale = new Vector3(pos.x, pos.y, pos.z);
    }

    public void SetAngle(Vector3 pos)
    {
        transform.eulerAngles = new Vector3(pos.x, pos.y, pos.z);
        oriAngle = new Vector3(pos.x, pos.y, pos.z);
    }

    public bool IsMovePath 
    {
        set {
            if (isMovePath != value) 
            {
                isMovePath = value;

                if (isMovePath)
                {
                    MovePath(0);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
        RotateAround();
        Traverse();
        Verticalshift();
        EnlargeV();
        EnlargeH();
    }

    /// <summary>
    /// 自转
    /// </summary>
    void Rotation() 
    {
        if (rotationSpeed != 0 && isrotationRot) 
        {
            if (rotationinterval == 0)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + rotationSpeed, transform.eulerAngles.z);
            }
            else 
            {
                rotationOffset += rotationSpeed;
                if (rotationOffset >= rotationinterval) 
                {
                    rotationOffset = 0;
                    isrotationRot = false;
                    Invoke(nameof(SetRotation),rotationintervaltime);
                }
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + rotationSpeed, transform.eulerAngles.z);
            } 
        }
    }

    void SetRotation() 
    {
        isrotationRot = true;
    }
    
    /// <summary>
    /// 公转
    /// </summary>
    private void RotateAround()
    {
        if (revolutionSpeed != 0&&isrevolutionRot) 
        {
            if (revolutioninterval == 0)
            {
                Revolution();
            }
            else
            {
                revolutionOffset += revolutionSpeed;
                if (revolutionOffset >= revolutioninterval)
                {
                    revolutionOffset = 0;
                    isrevolutionRot = false;
                    Invoke(nameof(SetRotation1), revolutionintervaltime);
                }
                Revolution();
            }
        }
    }

     void Revolution()
    {
        //绕axis轴旋转angle角度
        Quaternion rotation = Quaternion.AngleAxis(revolutionSpeed, transform.up);
        //旋转之前,以center为起点,transform.position当前物体位置为终点的向量.
        Vector3 beforeVector = transform.position - center;
        //四元数 * 向量(不能调换位置, 否则发生编译错误)
        Vector3 afterVector = rotation * beforeVector;//旋转后的向量  //向量的终点 = 向量的起点 + 向量
        transform.position = afterVector + center;
        transform.position = new Vector3(transform.position.x,ori.Oriy,transform.position.z);
    }

    void SetRotation1() 
    {
        isrevolutionRot = true;
    }

    //横移
    void Traverse() 
    {
        if(xMoveSpeed != 0&&isMoveX)
        {
            float offset = xMoveSpeed * Time.deltaTime;
            xMoveOffset += offset;

            if (Mathf.Abs(xMoveOffset) < xMoveDis)
            {
                transform.position = new Vector3(transform.position.x + offset, transform.position.y , transform.position.z);
            }
            else 
            {
                isMoveX = false;
                xMoveOffset = 0;
                xMoveSpeed = -xMoveSpeed;
                Invoke(nameof(MoveXChange), xMoveInterval);
            }
        }
    }

    void MoveXChange() 
    {
        isMoveX = true;
    }

    //竖向移动
    void Verticalshift() 
    {
        if (yMoveSpeed != 0 && isMoveY)
        {
            float offset = yMoveSpeed * Time.deltaTime;
            yMoveOffset += offset;

            if (Mathf.Abs(yMoveOffset) < yMoveDis)
            {
                transform.position = new Vector3(transform.position.x , transform.position.y, transform.position.z + offset);
            }
            else
            {
                isMoveY = false;
                yMoveOffset = 0;
                yMoveSpeed = -yMoveSpeed;
                Invoke(nameof(MoveYChange), yMoveInterval);
            }
        }
    }

    void MoveYChange()
    {
        isMoveY = true;
    }

    //放大上下
    void EnlargeV()
    {
        if (sxSpeed != 0 && sxDis!=0 && isSxScale)
        {
            float offset = sxSpeed * Time.deltaTime;
            if (sxSpeed > 0)
            {
                if (transform.localScale.y < sxDis)
                {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + offset, transform.localScale.z);
                }
                else
                {
                    isSxScale = false;
                    sxSpeed = -sxSpeed;
                    Invoke(nameof(ChangeSx), sxInterval);
                }
            }
            else 
            {
                if (transform.localScale.y > oriScale.y)
                {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + offset, transform.localScale.z);
                }
                else
                {
                    isSxScale = false;
                    sxSpeed = -sxSpeed;
                    Invoke(nameof(ChangeSx), sxInterval);
                }
            }      
        }
    }

    void ChangeSx() 
    {
        isSxScale = true;
    }

    //平面放大
    void EnlargeH()
    {
        if (pmSpeed != 0 && pmDis != 0 && isPmScale)
        {
            float offset = pmSpeed * Time.deltaTime;
            if (pmSpeed > 0)
            {
                if (transform.localScale.x < pmDis || transform.localScale.z < pmDis)
                {
                    transform.localScale = new Vector3(transform.localScale.x +offset, transform.localScale.y , transform.localScale.z +offset);
                }
                else
                {
                    isPmScale = false;
                    pmSpeed = -pmSpeed;
                    Invoke(nameof(ChangePm), pmInterval);
                }
            }
            else
            {
                if (transform.localScale.x > oriScale.x || transform.localScale.z > oriScale.z)
                {
                    transform.localScale = new Vector3(transform.localScale.x + offset, transform.localScale.y, transform.localScale.z + offset);
                }
                else
                {
                    isPmScale = false;
                    pmSpeed = -pmSpeed;
                    Invoke(nameof(ChangePm), pmInterval);
                }
            }
        }
    }
    void ChangePm()
    {
        isPmScale = true;
    }

    /// <summary>
    /// 切换颜色
    /// </summary>
    public void ChangeColor(int color) 
    {
        if (color != colorInt && !Input.GetMouseButton(0)) 
        {
            colorInt = color;
            gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("mat/prop"+colorInt);
        }
    }

    /// <summary>
    /// 沿路径移动
    /// </summary>
    void MovePath(int index) 
    {
        if (isMovePath && paths.Count > 0 && pathMoveSpeed>0) 
        {
            if (index == 0)
            {
                transform.position = paths[index];
                Invoke(nameof(MovePath1),pathMoveInterval);
            }
            else if (index == paths.Count) 
            {
                MovePath(0);
            }
            else
            {
                transform.DOMove(paths[index], pathMoveSpeed).onComplete=()=> {
                    index++;
                    MovePath(index);
                };
            }
        }
    }

    void MovePath1() 
    {
        MovePath(1);
    }
}


