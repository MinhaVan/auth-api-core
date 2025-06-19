namespace VanFinder.Domain.ViewModels;

public class VeiculoViewModel : EntityViewModel
{
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public UsuarioViewModel Usuario { get; set; } = null!;
}
