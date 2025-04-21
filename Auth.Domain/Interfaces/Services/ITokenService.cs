using System.Threading.Tasks;
using Auth.Domain.ViewModels;

namespace Auth.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        Task<TokenViewModel> Login(UsuarioLoginViewModel user);
        string Base64ToString(string base64);
        Task<TokenViewModel> RefreshToken(UsuarioLoginViewModel user);
    }
}