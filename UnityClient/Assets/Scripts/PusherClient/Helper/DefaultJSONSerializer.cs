public class DefaultJSONSerializer : IJsonSerializer
{
    public T Deserialize<T>(string data)
    {
        return (T)MiniJSON.Json.Deserialize(data);
    }

    public object Deserialize(string data)
    {
        return MiniJSON.Json.Deserialize(data);
    }

    public string Serialize(object data)
    {
        return MiniJSON.Json.Serialize(data);
    }
}