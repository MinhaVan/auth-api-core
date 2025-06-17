using System.Diagnostics.CodeAnalysis;
using Auth.Domain.Enums;
using Auth.Domain.ViewModels.Usuario;

namespace Auth.Domain.ViewModels;

[ExcludeFromCodeCoverage]
public class UsuarioNovoViewModel : UsuarioBaseViewModel
{
    public PerfilEnum Perfil { get; set; }
    public int? PlanoId { get; set; }
}