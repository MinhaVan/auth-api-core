using System;
using VanFinder.Domain.Enum;

namespace VanFinder.Domain.Entities;

public class RegistrarUsuarioRequest
{
    public string PrimeiroNome { get; set; } = string.Empty;
    public string UltimoNome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Contato { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string Senha { get; set; } = string.Empty;
    public PerfilUsuarioEnum Perfil { get; set; }
}