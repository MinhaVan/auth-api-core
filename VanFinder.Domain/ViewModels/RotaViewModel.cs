using VanFinder.Domain.Enum;

namespace VanFinder.Domain.ViewModels;

public class RotaViewModel : EntityViewModel
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Horario { get; set; } = string.Empty;
    public int DiaSemana { get; set; }
    public TipoRotaEnum TipoRota { get; set; }
    public int VeiculoId { get; set; }
    public VeiculoViewModel Veiculo { get; set; } = null!;
    public int EnderecoId { get; set; }
    public EnderecoViewModel Endereco { get; set; } = null!;
}