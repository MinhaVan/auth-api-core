using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Auth.Domain.ViewModels.Rota;

[ExcludeFromCodeCoverage]
public class MarcadorResponse
{
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }

    [JsonPropertyName("lon")]
    public double Longitude { get; set; }
}