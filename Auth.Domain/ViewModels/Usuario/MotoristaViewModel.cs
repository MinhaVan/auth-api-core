using System;
using System.Diagnostics.CodeAnalysis;

namespace Auth.Domain.ViewModels;

[ExcludeFromCodeCoverage]
public class MotoristaViewModel : UsuarioNovoViewModel
{
    public string CNH { get; set; }
    public DateTime Vencimento { get; set; }
    public int TipoCNH { get; set; }
    public string Foto { get; set; }
    public int UsuarioId { get; set; }
}