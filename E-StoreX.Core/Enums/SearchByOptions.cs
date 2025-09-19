using System.Text.Json.Serialization;

namespace EStoreX.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SearchByOptions
    {
        None = 0,
        Name = 1,
        Description = 2,
        Category = 3,
        Brand = 4
    }
}
