using System.Collections.Generic;
using System.Threading.Tasks;
using VanFinder.Domain.Entities;

namespace VanFinder.Domain.Interfaces.Repository;

public interface IVeiculoRepository : IGenericRepository<Veiculo>
{
    Task<IEnumerable<Veiculo>> GetByUsuarioIdAsync(int usuarioId);
}