using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 贪吃蛇关卡模型
/// </summary>
public class LevelModelSnake
{
    public int level;

    public int maxWave;

    public List<PropModel> propList;

    public List<WaveModel> waveList;
}

public class WaveModel 
{
    public int wave;
    public List<PropModel> propList;
}

public class PropModel 
{
    /// <summary>
    /// 波次
    /// </summary>
    public int wave;
    /// <summary>
    /// 道具类型
    /// </summary>
    public int propType;
    /// <summary>
    /// 颜色
    /// </summary>
    public int colorInt;
    /// <summary>
    /// 模型原始Y值
    /// </summary>
    public float oriY;
    /// <summary>
    /// 位置
    /// </summary>
    public VectorM pos;
    /// <summary>
    /// 缩放
    /// </summary>
    public VectorM scale;
    /// <summary>
    /// 角度
    /// </summary>
    public VectorM angle;
    /// <summary>
    /// 自转
    /// </summary>
    public VectorM rotation;
    /// <summary>
    /// 公转
    /// </summary>
    public VectorM revolution;
    /// <summary>
    /// 公转中心点
    /// </summary>
    public VectorM revolutionCenter;
    /// <summary>
    /// y移动
    /// </summary>
    public VectorM yMove;
    /// <summary>
    /// x移动
    /// </summary>
    public VectorM xMove;
    /// <summary>
    /// y缩放
    /// </summary>
    public VectorM yScale;
    /// <summary>
    /// xz缩放
    /// </summary>
    public VectorM xzScale;
    /// <summary>
    /// 路径
    /// </summary>
    public List<VectorM> paths; 
    /// <summary>
    /// 路径移动数据
    /// </summary>
    public VectorM pathV;
    /// <summary>
    /// 非道具和小球
    /// </summary>
    public bool isStatic;
}

public class VectorM 
{
    public float X;
    public float Y;
    public float Z;

    public VectorM() { }

    public VectorM(Vector3 vec) 
    {
        X = vec.x;
        Y = vec.y;
        Z = vec.z;
    }

    public VectorM(float x,float y,float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}