using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Auth.Domain.Enums;
using Auth.Domain.Models;

namespace Auth.Domain.ViewModels;

[ExcludeFromCodeCoverage]
public class UsuarioViewModel
{
    public int Id { get; set; }
    public string CPF { get; set; }
    public string Contato { get; set; }
    public string Email { get; set; }
    public string PrimeiroNome { get; set; }
    public string UltimoNome { get; set; }
    public PerfilEnum Perfil { get; set; }
    public int PlanoId { get; set; }
    public bool UsuarioValidado { get; set; }
    public int? EnderecoPrincipalId { get; set; }
    public string Senha { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public int EmpresaId { get; set; }

    public virtual IEnumerable<EnderecoViewModel> Enderecos { get; set; } = new List<EnderecoViewModel>();
    public virtual EnderecoViewModel EnderecoPrincipal { get; set; } = new EnderecoViewModel();
    public virtual MotoristaViewModel Motorista { get; set; }
}
