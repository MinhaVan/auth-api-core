using System;
using VanFinder.Domain.Enum;

namespace VanFinder.Domain.Entities;

public class Usuario : Entity
{
    public string PrimeiroNome { get; set; } = string.Empty;
    public string UltimoNome { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Contato { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public PerfilUsuarioEnum Perfil { get; set; }
}