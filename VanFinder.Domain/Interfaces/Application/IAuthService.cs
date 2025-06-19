using System.Threading.Tasks;
using VanFinder.Domain.Entities;
using VanFinder.Domain.ViewModels;

namespace VanFinder.Domain.Interfaces.Application;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);
    Task RegisterAsync(RegistrarUsuarioRequest usuario);
}