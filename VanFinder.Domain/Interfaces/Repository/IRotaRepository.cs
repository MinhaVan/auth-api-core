using System.Collections.Generic;
using System.Threading.Tasks;
using VanFinder.Domain.Entities;

namespace VanFinder.Domain.Interfaces.Repository;

public interface IRotaRepository : IGenericRepository<Rota>
{
    Task<IEnumerable<Rota>> GetByEnderecoAsync(string bairro);
}