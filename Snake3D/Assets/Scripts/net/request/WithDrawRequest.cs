using System;
using System. Collections. Generic;

[Serializable]
public class WithDrawRequest
{
    public int wd_type;
    public string account;
    public double amount;
    public int coin_wd_cost;
    public int coin_wd_type;
    
}