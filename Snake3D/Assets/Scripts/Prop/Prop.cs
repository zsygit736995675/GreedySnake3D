using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public Vector3 oriPos;

    Vector3 oriScale;

    Vector3 oriAngle;

    public int wave;

    /// <summary>
    /// 下一波是否清除
    /// </summary>
    public  bool isStatic=false;

    //路径移动
    public bool isMovePath=false;

    //自转
    float rotationOffset;
    bool isrotationRot = true;

    //公转
    float revolutionOffset;
    bool isrevolutionRot = true;
    public  Vector3 center;

    //上下移动
    float yMoveOffset;
    bool isMoveY = true;

    //左右移动
    float xMoveOffset;
    bool isMoveX = true;

    //上下缩放
    bool isSxScale = true;

    //平面缩放
    float pmOffset;
    bool isPmScale = true;

    PropModel model;

    /// <summary>
    /// 是否被吸附
    /// </summary>
    public bool isAdsorption = false;

    public void Init(PropModel model, int index = 0)
    {
        this.model = model;
        wave = model.wave;
        propType = (PropType)model.propType;
        isStatic = ((int)propType != (int)PropType.qiu && (int)propType != (int)PropType.qiu_2);
        colorInt = model.colorInt;

        SetPos(new Vector3(model.pos.X, model.oriY, model.pos.Z));

        if (model.propType == (int)PropType.qiu || model.propType == (int)PropType.qiu_2 || model.propType == (int)PropType.citie
            || model.propType == (int)PropType.feiji || model.propType == (int)PropType.jiasu || model.propType == (int)PropType.zanting
            || model.propType == (int)PropType.zhadan)
            SetScale(new Vector3(1.3f, 1.3f, 1.3f));
        else if (model.propType == (int)PropType.qi)
            SetScale(Vector3.one * 2);
        else if ( model.propType == (int)PropType.qidian)
            SetScale(new Vector3(2,1,1f));
        else if (model.propType == (int)PropType.zhongdian)
            SetScale(new Vector3(2, 2f, 1.5f));
        else
            SetScale(new Vector3(model.scale.X, model.scale.Y, model.scale.Z));

        if (model.propType != (int)PropType.qiu) 
            SetAngle( new Vector3(model.angle.X, model.angle.Y, model.angle.Z));

        center = new Vector3(model.revolutionCenter.X,model.revolutionCenter.Y,model.revolutionCenter.Z);
        IsMovePath = model.paths != null && model.pathV != null && model.paths.Count > 0;

        if (model.propType == (int)PropType.qiu||model.propType == (int)PropType.qiu_2) 
        {
            transform.DOMoveY(model.oriY, index * 0.2f).onComplete = QiuAni;
        }

        if (model.propType == (int)PropType.qi) 
        {
            SetAngle(new Vector3(330, model.angle.Y, model.angle.Z));
        }

        if (model.propType == (int)PropType.feiji) 
        {
            transform.GetChild(0).gameObject.AddComponent<FeiJI>();
        }
    }

    public void InitQi() 
    {
        propType = PropType.qi;
        SetAngle(new Vector3(330,0, 0));
        SetScale(Vector3.one * 2);
    }

    public void QiuAni() 
    {
        transform.DOMoveY(0.4f,0.5f).onComplete=()=>{

            transform.DOMoveY(model.oriY, 0.5f).onComplete = () => {

                transform.DOMoveY(model.oriY, 0.5f).onComplete = QiuAni;

            };
        };
    }

    public void SetPos(Vector3 pos)
    {
        float offZ = 0;

        if (Camera.main.orthographicSize == 6) 
        {
            if (propType == PropType.qidian) 
            {
                offZ = 1.1f;
            }

            if (propType == PropType.zhongdian||PropType.qi==propType)
            {
                offZ = -1.1f;
            }
        }
        transform.position = new Vector3(pos.x, pos.y, pos.z+offZ);
        oriPos = new Vector3(pos.x, pos.y, pos.z+offZ);
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

    /// <summary>
    /// 触发
    /// </summary>
    public void Trigger()
    {
        if (Snake_Game.Ins.isPause || !Snake_Game.Ins.isStart || Snake_Game.Ins.isvictory)
            return;

        switch (propType)
        {
            case PropType.qidian:
                break;
            case PropType.qi:
                break;
            case PropType.zhongdian:
                Snake_Game.Ins.PassLevel();
                break;
            case PropType.Cylinder1:
            case PropType.Cylinder2:
            case PropType.Cylinder3:
            case PropType.Cylinder4:
            case PropType.Cylinder5:
                Snake_Game.Ins.Failure();
                break;
            case PropType.qiu:
            case PropType.qiu_2:
                Snake_Game.Ins.AddBody(this);
                break;
            case PropType.citie:
                Snake_Game.Ins.AddBuff(BuffType.CT, this,3f);
                AudioController.Ins.PlayEffect("badge_default");
                break;
            case PropType.zanting:
                Snake_Game.Ins.AddBuff(BuffType.ZT, this, 10);
                AudioController.Ins.PlayEffect("fast_snake_lost");
                break;
            case PropType.zhadan:
                Snake_Game.Ins.Burst(this);
                break;
            case PropType.feiji:
                transform.eulerAngles = new Vector3(0, Snake_Game.Ins.snake.transform.eulerAngles.y, 0);
                transform.position = Snake_Game.Ins.snake.transform.position;
                transform.GetChild(0).transform.localEulerAngles = new Vector3(0,180,0);
                transform.GetChild(0).gameObject.GetComponent<FeiJI>().isTrigger = true ;
                break;
            case PropType.jiasu:
                Snake_Game.Ins.AddBuff(BuffType.JS, this, 10);
                AudioController.Ins.PlayEffect("fast_snake");
                break;
        }
    }

    GameObject cai;
    public void Bomb() 
    {
        AudioController.Ins.PlayEffect("bomb_explode");
        GameObject go = Resources.Load<GameObject>("Game/qiu_cai2");
        cai = Instantiate(go);
        cai.transform.position = transform.position;
        gameObject.SetActive(false);
        Invoke(nameof(DestroyMe), 1);
    }

    private void DestroyMe()
    {
        Destroy(cai);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        if ((propType == PropType.qiu || propType == PropType.qiu_2) && isAdsorption ) 
        {
            transform.position = Vector3.Lerp(transform.position, Snake_Game.Ins. snake.transform.position, (Snake_Game.Ins.snake.speed + 5) * Time.deltaTime);
        }

        if (!Snake_Game.Ins.IsZT() && propType!=PropType.qi) 
        {
            Rotation();
            RotateAround();
            Traverse();
            Verticalshift();
            EnlargeV();
            EnlargeH();
        }
    }

    /// <summary>
    /// 自转
    /// </summary>
    void Rotation() 
    {
        if (model.rotation.X != 0 && isrotationRot) 
        {
            if (model.rotation.Y == 0)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + model.rotation.X, transform.eulerAngles.z);
            }
            else 
            {
                rotationOffset += model.rotation.X;
                if (rotationOffset >= model.rotation.Y) 
                {
                    rotationOffset = 0;
                    isrotationRot = false;
                    Invoke(nameof(SetRotation),model.rotation.Z);
                }
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + model.rotation.X, transform.eulerAngles.z);
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
        if (model.revolution.X != 0&&isrevolutionRot) 
        {
            if (model.revolution.Y == 0)
            {
                Revolution();
            }
            else
            {
                revolutionOffset += model.revolution.X;
                if (revolutionOffset >= model.revolution.Y)
                {
                    revolutionOffset = 0;
                    isrevolutionRot = false;
                    Invoke(nameof(SetRotation1), model.revolution.X);
                }
                Revolution();
            }
        }
    }

     void Revolution()
    {
        //绕axis轴旋转angle角度
        Quaternion rotation = Quaternion.AngleAxis(model.revolution.X, transform.up);
        //旋转之前,以center为起点,transform.position当前物体位置为终点的向量.
        Vector3 beforeVector = transform.position - center;
        //四元数 * 向量(不能调换位置, 否则发生编译错误)
        Vector3 afterVector = rotation * beforeVector;//旋转后的向量  //向量的终点 = 向量的起点 + 向量
        transform.position = afterVector + center;
        transform.position = new Vector3(transform.position.x,model.oriY,transform.position.z);
    }

    void SetRotation1() 
    {
        isrevolutionRot = true;
    }

    //横移
    void Traverse() 
    {
        if(model.xMove.X != 0&&isMoveX)
        {
            float offset = model.xMove.X * Time.deltaTime;
            xMoveOffset += offset;

            if (Mathf.Abs(xMoveOffset) < model.xMove.Y)
            {
                transform.position = new Vector3(transform.position.x + offset, transform.position.y , transform.position.z);
            }
            else 
            {
                isMoveX = false;
                xMoveOffset = 0;
                model.xMove.X = -model.xMove.X;
                Invoke(nameof(MoveXChange), model.xMove.Z);
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
        if (model.yMove.X != 0 && isMoveY)
        {
            float offset = model.yMove.X * Time.deltaTime;
            yMoveOffset += offset;

            if (Mathf.Abs(yMoveOffset) < model.yMove.Y)
            {
                transform.position = new Vector3(transform.position.x , transform.position.y, transform.position.z + offset);
            }
            else
            {
                isMoveY = false;
                yMoveOffset = 0;
                model.yMove.X = -model.yMove.X;
                Invoke(nameof(MoveYChange), model.yMove.Z);
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
        if (model.yScale.X != 0 && model.yScale.Y!= 0 && isSxScale)
        {
            float offset = model.yScale.X * Time.deltaTime;
            if (model.yScale.X > 0)
            {
                if (transform.localScale.y < model.yScale.Y)
                {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + offset, transform.localScale.z);
                }
                else
                {
                    isSxScale = false;
                    model.yScale.X = -model.yScale.X;
                    Invoke(nameof(ChangeSx), model.yScale.Z);
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
                    model.yScale.X = -model.yScale.X;
                    Invoke(nameof(ChangeSx), model.yScale.Z);
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
        if (model.yScale.X != 0 && model.yScale.Y != 0 && isPmScale)
        {
            float offset = model.yScale.X * Time.deltaTime;
            if (model.yScale.X > 0)
            {
                if (transform.localScale.x < model.yScale.Y || transform.localScale.z < model.yScale.Y)
                {
                    transform.localScale = new Vector3(transform.localScale.x +offset, transform.localScale.y , transform.localScale.z +offset);
                }
                else
                {
                    isPmScale = false;
                    model.yScale.X = -model.yScale.X;
                    Invoke(nameof(ChangePm), model.yScale.Y);
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
                    model.yScale.X = -model.yScale.X;
                    Invoke(nameof(ChangePm), model.yScale.Z);
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
        if (isMovePath && model.paths.Count > 0 && model.pathV.X > 0) 
        {
            if (index == 0)
            {
                transform.position = new Vector3(model.paths[index].X, model.paths[index].Y, model.paths[index].Z);
                Invoke(nameof(MovePath1),model.pathV.Y);
            }
            else if (index == model.paths.Count) 
            {
                MovePath(0);
            }
            else
            {
                transform.DOMove(new Vector3(model.paths[index].X, model.paths[index].Y, model.paths[index].Z), model.pathV.X).onComplete=()=> {
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


