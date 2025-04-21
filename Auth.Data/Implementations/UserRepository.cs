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

    public async Task<Usuario> LoginAsync(UsuarioLoginViewModel user)
    {
        var expirationInMinutes = 60;
        var chaveLogin = $"login:{user.CPF}:{user.Email}:{user.Senha}:{user.EmpresaId}:{user.IsMotorista}";
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

    public async Task<Usuario> BuscarPorCpfEmpresaAsync(string cpf, int empresaId)
    {
        var chaveLogin = $"login:cpf:{cpf}:{empresaId}";
        return await _redisRepository.TryGetAsync(chaveLogin, async () =>
        {
            return await _ctx.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CPF.Equals(cpf) && x.EmpresaId == empresaId);
        });
    }

    public string ComputeHash(string input, SHA256CryptoServiceProvider algorithm)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
        return BitConverter.ToString(hashedBytes);
    }
}
