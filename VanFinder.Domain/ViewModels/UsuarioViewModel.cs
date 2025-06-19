using System;
using VanFinder.Domain.Enum;

namespace VanFinder.Domain.ViewModels;

public class UsuarioViewModel : EntityViewModel
{
    public string PrimeiroNome { get; set; } = string.Empty;
    public string UltimoNome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Contato { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public PerfilUsuarioEnum Perfil { get; set; }
}