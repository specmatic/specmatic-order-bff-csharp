using System.Text.Json.Serialization;

namespace specmatic_order_bff_csharp.models
{
    public class UuidResponse(Guid uuid): UuidRequest
    {
        [JsonPropertyName("uuid")]
        public Guid Uuid { get; init; } = uuid;
    }
}
