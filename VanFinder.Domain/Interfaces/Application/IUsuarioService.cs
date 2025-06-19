using System.Threading.Tasks;
using VanFinder.Domain.ViewModels;

namespace VanFinder.Domain.Interfaces.Application;

public interface IUsuarioService
{
    Task<UsuarioViewModel> GetByIdAsync(int id);
}