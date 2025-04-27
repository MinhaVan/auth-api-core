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
using Auth.Domain.Interfaces.Repositories;
using Auth.Data.Extensions;
using static Auth.Domain.Constantes.Contantes;

namespace Auth.Data.Implementations;

public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    private readonly APIContext _ctx;
    private readonly IRedisRepository _redisRepository;
    public UsuarioRepository(APIContext context, IRedisRepository redisRepository) : base(context)
    {
        _ctx = context;
        _redisRepository = redisRepository;
    }

    public async Task AtualizarRefreshTokenAsync(int userId, string refreshToken, DateTime expiryTime)
    {
        var usuario = await _ctx.Usuarios.FirstOrDefaultAsync(x => x.Id == userId);
        if (usuario != null)
        {
            var chaveRefreshTokenAntigo = string.Format(Cache.ChaveRefreshToken, usuario.RefreshToken);
            var chaveRefreshTokenNovo = string.Format(Cache.ChaveRefreshToken, refreshToken);

            await _redisRepository.RemoveAsync(chaveRefreshTokenAntigo);
            await _redisRepository.SetAsync(chaveRefreshTokenNovo, refreshToken, expirationInMinutes: 60);

            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenExpiryTime = expiryTime;
            await _ctx.SaveChangesAsync();
        }
    }

    public async Task<Usuario> LoginAsync(UsuarioLoginViewModel user)
    {
        var expirationInMinutes = 60;

        var chaveLogin = string.Format(Cache.ChaveLogin, user.CPF, user.Email, user.Senha, user.EmpresaId, user.IsMotorista);
        return await _redisRepository.TryGetAsync(chaveLogin, async () =>
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
        }, expirationInMinutes);
    }

    public async Task<Usuario> BuscarPorRefreshTokenAsync(string refreshToken)
    {
        var chaveRefreshToken = string.Format(Cache.ChaveRefreshToken, refreshToken);
        var response = await _redisRepository.GetAsync<Usuario>(chaveRefreshToken);

        if (response is null)
        {
            response = await _ctx.Usuarios.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        }

        return response;
    }

    public async Task<Usuario> BuscarPorCpfEmpresaAsync(string cpf, int empresaId)
    {
        var chaveLogin = string.Format(Cache.ChaveBuscarPorCpfEmpresa, cpf, empresaId);
        return await _redisRepository.TryGetAsync(chaveLogin, async () =>
        {
            return await _ctx.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CPF.Equals(cpf) && x.EmpresaId == empresaId);
        });
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
