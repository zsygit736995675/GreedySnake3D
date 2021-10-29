using System;

[AttributeUsage(AttributeTargets.Field)]
public class JsonNameAttribute : Attribute {
    private string _name;

    public JsonNameAttribute(string name) {
        _name = name;
    }

    public string Name {
        get => _name;
        set => _name = value;
    }
}