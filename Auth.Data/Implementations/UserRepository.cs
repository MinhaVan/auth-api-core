using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Auth.Data.Context;
using Auth.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Auth.Domain.Models;
using Auth.Data.Repositories;
using Auth.Domain.ViewModels;
using Auth.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Auth.Data.Implementations;

[ExcludeFromCodeCoverage]
public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    private readonly APIContext _ctx;
    public UsuarioRepository(APIContext context) : base(context)
    {
        _ctx = context;
    }

    public async Task AtualizarRefreshTokenAsync(int userId, string refreshToken, DateTime expiryTime)
    {
        var usuario = await _ctx.Usuarios.FirstOrDefaultAsync(x => x.Id == userId);
        if (usuario != null)
        {
            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenExpiryTime = expiryTime;
            await _ctx.SaveChangesAsync();
        }
    }

    public async Task<Usuario> LoginAsync(UsuarioLoginViewModel user)
    {
        var query = _ctx.Usuarios.Where(x =>
            x.EmpresaId == user.EmpresaId &&
            x.Senha.Equals(user.Senha) &&
            (x.CPF.Equals(user.CPF) || x.Email.Equals(user.Email))
        );

        if (user.IsMotorista)
        {
            query = query.Where(x => x.Perfil == PerfilEnum.Motorista);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<Usuario> BuscarPorRefreshTokenAsync(string refreshToken)
    {
        return await _ctx.Usuarios.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
    }

    public async Task<Usuario> BuscarPorCpfEmpresaAsync(string cpf, int empresaId)
    {
        return await _ctx.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CPF.Equals(cpf) && x.EmpresaId == empresaId);
    }

    public string ComputeHash(string senha)
    {
        using (var algorithm = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hashBytes = algorithm.ComputeHash(bytes);
            return Convert.ToHexString(hashBytes);
        }
    }
}
