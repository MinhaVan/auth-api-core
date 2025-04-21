using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Domain.Interfaces.Repository;
using Auth.Domain.Models;

namespace Auth.Domain.Interfaces.Repositories;

public interface IPermissaoRepository : IBaseRepository<Permissao>
{
    Task<List<Permissao>> ObterPermissoesPadraoPorEmpresaPerfilAsync(int empresaId, bool isMotorista);
}