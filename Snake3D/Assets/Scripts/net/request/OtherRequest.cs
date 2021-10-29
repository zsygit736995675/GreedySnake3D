using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class OtherRequest {
    public string account_id;
    public string user_action_set_id;
    public List<ActionRequest> actions;

    public OtherRequest() {
        account_id = "1110566019";
        user_action_set_id = "1110665970";
        actions=new List<ActionRequest>();
        ActionRequest request=new ActionRequest();
        request.action_param=new ActionParams();
        request.action_param.length_of_stay = 1;
        request.action_time=(DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        request.action_type = "START_APP";
        request.user_id=new User();
        request.user_id.hash_imei = Md5Sum(AndroidSend.getNative("imei"));
        actions.Add(request);
    }
    
    private string Md5Sum(string strToEncrypt) {
        byte[] bs = UTF8Encoding.UTF8.GetBytes(strToEncrypt);
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create();
        byte[] hashBytes = md5.ComputeHash(bs);

        string hashString = "";
        for (int i = 0;
            i < hashBytes.Length;
            i++) {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
}

[Serializable]
public class ActionRequest {
    public long action_time;
    public User user_id;
    public string action_type;
    public ActionParams action_param;
}

[Serializable]
public class User {
    public string hash_imei;
}

[Serializable]
public class ActionParams {
    public int length_of_stay;
}