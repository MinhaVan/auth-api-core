using System.Threading.Tasks;
using Auth.Domain.ViewModels;
using Auth.Domain.ViewModels.Usuario;

namespace Auth.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        Task<TokenViewModel> Login(UsuarioLoginViewModel user);
        string Base64ToString(string base64);
        Task<TokenViewModel> RefreshToken(RefreshTokenRequest user);
    }
}