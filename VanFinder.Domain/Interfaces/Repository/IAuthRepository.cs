using System.Threading.Tasks;
using VanFinder.Domain.Entities;

namespace VanFinder.Domain.Interfaces.Repository;

public interface IAuthRepository
{
    Task<Usuario> GetUsuarioByEmailOrCpfAsync(string emailCpf);
}