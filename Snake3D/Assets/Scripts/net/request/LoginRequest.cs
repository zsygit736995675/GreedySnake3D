using System;

[Serializable]
public class LoginRequest
{


    public int platform;
    public string openid;
    public string device_id;
    public string token;
    public string pl_name;
    public string pl_avatar_url;
    public string adid;
    public string country;
    public string token_for_business;
    public string master_pk_name;
    public bool is_new;


    public override string ToString ()
    {
        return base. ToString();
    }
}