using System;
using System. Collections. Generic;

[Serializable]
public class RankResponse
{
    public List<RankList> rank_list;
    public  Rank_limit rank_limit;
}

public class RankList
{
    public int user_score;

    public string name;

    public int user_id;
}


public class Rank_limit
{
    //上榜人数
    public int total;
    //提现名次
    public int withdraw;
}

