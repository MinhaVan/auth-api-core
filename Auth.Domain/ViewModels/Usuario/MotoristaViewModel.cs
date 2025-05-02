using System;
using System.Diagnostics.CodeAnalysis;
using Auth.Domain.Enums;

namespace Auth.Domain.ViewModels;

[ExcludeFromCodeCoverage]
public class MotoristaViewModel
{
    public string CNH { get; set; }
    public DateTime Vencimento { get; set; }
    public TipoCNHEnum TipoCNH { get; set; }
    public string Foto { get; set; }
}