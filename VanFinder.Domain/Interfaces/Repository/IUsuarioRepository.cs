using System.Threading.Tasks;
using VanFinder.Domain.Entities;

namespace VanFinder.Domain.Interfaces.Repository;

public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    Task<Usuario?> GetByEmailOrCpfAsync(string emailOrCpf);
}