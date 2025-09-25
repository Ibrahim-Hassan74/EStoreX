using System.Text.Json.Serialization;

namespace EStoreX.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ExternalLoginProvider
    {
        Google,
        GitHub
    }
}
