using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Auth.Data.Context;
using Auth.Data.Extensions;
using Auth.Data.Repositories;
using Auth.Domain.Interfaces.Repositories;
using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data.Implementations;

[ExcludeFromCodeCoverage]
public class PermissaoRepository : BaseRepository<Permissao>, IPermissaoRepository
{
    private readonly IRedisRepository _redisRepository;
    public PermissaoRepository(APIContext context, IRedisRepository redisRepository) : base(context)
    {
        _redisRepository = redisRepository;
    }

    public async Task<List<Permissao>> ObterPermissoesPadraoPorEmpresaPerfilAsync(int empresaId, bool isMotorista)
    {
        var expirationInMinutes = 60;
        var chaveLogin = $"obterPermissoesPadraoPorEmpresaPerfil:{empresaId}:{isMotorista}";
        return await _redisRepository.TryGetAsync(chaveLogin, async () =>
        {
            return await _context.Permissoes
                .Where(x => x.EmpresaId == empresaId && (isMotorista ? x.PadraoMotorista : x.PadraoPassageiros))
                .ToListAsync();
        }, expirationInMinutes);
    }
}
