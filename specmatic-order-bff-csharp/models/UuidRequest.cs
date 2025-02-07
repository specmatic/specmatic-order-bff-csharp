using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace specmatic_order_bff_csharp.models
{
    public class UuidRequest(string name, string type, string? email, UuidType UuidType)
    {
        [JsonPropertyName("firstName")]
        [Required]
        public string FirstName { get; init; } = name;

        [JsonPropertyName("lastName")]
        [Required]
        public string LastName { get; init; } = type;

        [JsonPropertyName("email")]
        public string? Email { get; init; } = email;

        [JsonPropertyName("uuidType")]
        public UuidType UuidType { get; init; } = UuidType;

        public UuidRequest() : this("John", "Doe", "John@mail.com", UuidType.Regular) {}
    }
}
