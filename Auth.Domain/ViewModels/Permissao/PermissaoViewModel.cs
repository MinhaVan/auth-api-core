using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Auth.Domain.ViewModels;

[ExcludeFromCodeCoverage]
public class PermissaoViewModel
{
    public int UsuarioId { get; set; }
    public List<int> PermissaoId { get; set; }
}