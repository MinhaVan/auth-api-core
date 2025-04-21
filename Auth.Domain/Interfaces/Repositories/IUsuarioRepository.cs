using System.Security.Cryptography;
using System.Threading.Tasks;
using Auth.Domain.Models;
using Auth.Domain.ViewModels;

namespace Auth.Domain.Interfaces.Repository;

public interface IUsuarioRepository : IBaseRepository<Usuario>
{
    Task<Usuario> LoginAsync(UsuarioLoginViewModel user);
    string ComputeHash(string input, SHA256CryptoServiceProvider algorithm);
}