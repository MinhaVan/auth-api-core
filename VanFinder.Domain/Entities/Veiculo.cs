namespace VanFinder.Domain.Entities;

public class Veiculo : Entity
{
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
}