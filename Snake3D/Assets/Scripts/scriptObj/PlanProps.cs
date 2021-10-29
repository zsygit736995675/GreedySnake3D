using System;

[Serializable]
public class PlanProps
{
    public int[] props;
}

public enum propType
{
    /// <summary>
    /// 空 -- 过关红包
    /// </summary>
    TYPE_EMPTY = 0,
    /// <summary>
    /// 超级红包
    /// </summary>
    TYPE_ONE = 1,
    /// <summary>
    /// 拼图
    /// </summary>
    TYPE_TWO = 2,
    /// <summary>
    /// 转盘
    /// </summary>
    TYPE_THREE = 3,
    /// <summary>
    /// 红包雨
    /// </summary>
    TYPE_FOUR = 4,
    /// <summary>
    /// 道具金猪存钱罐
    /// </summary>
    TYPE_NINE = 9
}