using System.Diagnostics.CodeAnalysis;

namespace Auth.Domain.Models;

[ExcludeFromCodeCoverage]
public class UsuarioPermissao : Entity
{
    public int UsuarioId { get; set; }
    public int PermissaoId { get; set; }
    // 
    public virtual Usuario Usuario { get; set; }
    public virtual Permissao Permissao { get; set; }
}