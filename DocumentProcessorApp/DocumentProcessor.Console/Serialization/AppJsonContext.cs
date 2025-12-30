using DocumentProcessor.Console.Models;
using System.Text.Json.Serialization;

namespace DocumentProcessor.Console.Serialization
{
    [JsonSourceGenerationOptions(
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never)]
    [JsonSerializable(typeof(AddDocumentRequest))]
    internal partial class AppJsonContext : JsonSerializerContext
    {
    }
}
