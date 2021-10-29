using System;

[Serializable]
public class WxModel {
    public int errcode;
    public string errmsg;
    public string access_token;
    public string refresh_token;
    public string openid;
    public string scope;
    public string unionid;
    public int expires_in;

}