using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Auth.Data.Context;
using Auth.Data.Repositories;
using Auth.Domain.Interfaces.Repositories;
using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data.Implementations;

[ExcludeFromCodeCoverage]
public class PermissaoRepository : BaseRepository<Permissao>, IPermissaoRepository
{
    public PermissaoRepository(APIContext context) : base(context)
    {
    }

    public async Task<List<Permissao>> ObterPermissoesPadraoPorEmpresaPerfilAsync(int empresaId, bool isMotorista)
    {
        return await _context.Permissoes
            .Where(x => x.EmpresaId == empresaId && (isMotorista ? x.PadraoMotorista : x.PadraoPassageiros))
            .ToListAsync();
    }
}
