using System.Collections.Generic;
using System.Linq;

public static class DictionaryEx
{
    public static Dictionary<string, string> GetKeysObjToStr(this Dictionary<string, object> eventValues)
    {
        return eventValues==null ? new Dictionary<string, string>() : eventValues.ToDictionary(k => k.Key, k => k.Value.ToString());
    }
}
