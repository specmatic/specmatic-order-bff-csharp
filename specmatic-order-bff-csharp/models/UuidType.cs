using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace specmatic_order_bff_csharp.models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UuidType
    {
        [EnumMember(Value = "Regular")]
        Regular,

        [EnumMember(Value = "Premium")]
        Premium,

        [EnumMember(Value = "Business")]
        Business,

        [EnumMember(Value = "Enterprise")]
        Enterprise
    }
}
