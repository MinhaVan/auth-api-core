using VanFinder.Domain.Enum;

namespace VanFinder.Domain.Entities;

public class Rota : Entity
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Horario { get; set; } = string.Empty;
    public int DiaSemana { get; set; }
    public TipoRotaEnum TipoRota { get; set; }
    public int VeiculoId { get; set; }
    public Veiculo Veiculo { get; set; } = null!;
    public int EnderecoId { get; set; }
    public Endereco Endereco { get; set; } = null!;
}