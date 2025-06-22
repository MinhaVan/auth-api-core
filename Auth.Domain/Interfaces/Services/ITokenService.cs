using System.Threading.Tasks;
using Auth.Domain.ViewModels;

namespace Auth.Domain.Interfaces.Services;

public interface ITokenService
{
    Task ConfirmarUsuarioAsync(int usuarioId);
    Task<TokenViewModel> LoginAsync(UsuarioLoginViewModel user);
    string Base64ToString(string base64);
    Task<TokenViewModel> RefreshToken(RefreshTokenRequest user);
    Task ConfirmarMotoristaAsync(int usuarioId, UsuarioLoginViewModel user);
    TokenViewModel GerarTokenAsync(int empresaId);
}