using System;

[Serializable]
public class WithDrawResponse {
    
    public int status;
  
    public string account;
    
    public int apply_ts;
    
    public int finished_ts;

    public double amount;
  
    public int wd_type;
    
    public int coin_wd_type;

    public int coin_wd_cost;
 
    public int user_id;
    
    public int id;
    
    public string pay_pic_url;

}