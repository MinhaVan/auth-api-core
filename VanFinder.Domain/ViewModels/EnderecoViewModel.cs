namespace VanFinder.Domain.ViewModels;

public class EnderecoViewModel : EntityViewModel
{
    public string Logradouro { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string CEP { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}