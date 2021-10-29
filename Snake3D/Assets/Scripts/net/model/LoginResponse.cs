using System;
using System.Collections.Generic;

/**
 * {"msg": "ok", "code": 200, "data": {"package_name": "com.rich.lemon.gold.winner.match.cards.dail", "create_version": 0, "pk_id": 6, "money": 0.0, "create_time": 1590546934206, "be_invited": 0, "share_monty_total": 0.0, "user_id": 3493761, "extra": [], "invite_code": "FBJ5M", "m2s": 0, "money_today": 0.0, "update_version": 0, "login_time": 1590546934206, "status": 1, "rv_count": 0, "share_wd_ok": 0, "adid": "", "coin_today": 0, "cash_shared": 0, "ratio": 5.0, "coin": 0, "device_id": "255B2AC6-A75C-57B6-9436-DD50FADC2E4E", "name": "", "country": "US", "share_wd_total": 0, "share_invite_num": 0, "avatar_url": "", "token_for_business": "255B2AC6-A75C-57B6-9436-DD50FADC2E4E", "be_invited_code": ""}}
 */
[Serializable]
public class LoginResponse {
    public string package_name;
    public int create_version;
    public int pk_id;
    public double money;
    public long create_time;
    public int be_invited;
    public double share_monty_total;
    public int user_id;
    public List<string> extra;
    public string invite_code;
    public int m2s;
    public double money_today;
    public int update_version;
    public long login_time;
    public int status;
    public int rv_count;
    public string adid;
    public int coin_today;
    public int cash_shared;
    public double ratio;
    public int coin;
    public string device_id;
    public string name;
    public string country;
    public int share_wd_total;
    public int share_invite_num;
    public string avatar_url;
    public string token_for_business;
    public string be_invited_code;
    public int is_bind_wechat;
    public int wd_mode;
}