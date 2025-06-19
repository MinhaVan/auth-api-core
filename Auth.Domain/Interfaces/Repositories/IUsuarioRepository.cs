using System;
using System.Threading.Tasks;
using Auth.Domain.Models;
using Auth.Domain.ViewModels;

namespace Auth.Domain.Interfaces.Repository;

public interface IUsuarioRepository : IBaseRepository<Usuario>
{
    Task<Usuario> LoginAsync(UsuarioLoginViewModel user);
    Task AtualizarRefreshTokenAsync(int userId, string refreshToken, DateTime expiryTime);
    Task<Usuario> BuscarPorRefreshTokenAsync(string refreshToken);
    Task<Usuario> BuscarPorCpfEmpresaAsync(string cpf);
    string ComputeHash(string input);
}