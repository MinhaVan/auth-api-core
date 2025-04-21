using System.Text.Json.Serialization;

namespace Auth.Domain.ViewModels.Rota;

public class MarcadorResponse
{
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }

    [JsonPropertyName("lon")]
    public double Longitude { get; set; }
}