using System.Diagnostics.CodeAnalysis;
using Auth.Domain.Enums;

namespace Auth.Domain.ViewModels;

[ExcludeFromCodeCoverage]
public class UsuarioNovoViewModel
{
    public string CPF { get; set; }
    public string Contato { get; set; }
    public string Email { get; set; }
    public string PrimeiroNome { get; set; }
    public string UltimoNome { get; set; }
    public PerfilEnum Perfil { get; set; }
    public int? PlanoId { get; set; }
    public int EmpresaId { get; set; }
    public string Senha { get; set; }
    public bool IsMotorista { get; set; }
}