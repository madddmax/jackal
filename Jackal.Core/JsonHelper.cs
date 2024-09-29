using Newtonsoft.Json;

namespace Jackal.Core;

public static class JsonHelper
{
    private static readonly JsonSerializerSettings TypeNameSerializer = new() { TypeNameHandling = TypeNameHandling.Objects };

    public static string SerializeWithType<T>(T obj, Formatting formatting = Formatting.None)
    {
        return JsonConvert.SerializeObject(obj, formatting, TypeNameSerializer);
    }

    public static T? DeserializeWithType<T>(string str)
    {
        return JsonConvert.DeserializeObject<T>(str, TypeNameSerializer);
    }
}