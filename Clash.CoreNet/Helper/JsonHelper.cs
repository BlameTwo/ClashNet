using System.Text.Json;

namespace Clash.CoreNet.Helper;

public static class JsonHelper
{
    public static JsonSerializerOptions  GetJsonSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            WriteIndented = true
        };
        return options;
    }
}
