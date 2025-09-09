using System.Text.Json.Serialization;

namespace EStoreX.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DiscountType
    {
        Global = 0,
        Product = 1,
        Category = 2,
        Brand = 3
    }
}
