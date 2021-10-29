using System;

[Serializable]
public class ActiveConfigModel {
    public string msg;
    public int code;
    public ActiveConfigModelData data;
}

[Serializable]
public class ActiveConfigModelData {
    public ActiveConfigModelDataModel one;
    public ActiveConfigModelDataModel two;
}

[Serializable]
public class ActiveConfigModelDataModel {
    public int amount;
    public int active_days;
    public int ad_num;
}