using System;

[Serializable]
public class BaseModel<T> {
    public string msg;

    public int code;

    public T data;
}


public class ServerData 
{
    public int timestamp; 
}