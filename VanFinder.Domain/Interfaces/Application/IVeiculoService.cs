using System.Collections.Generic;
using System.Threading.Tasks;
using VanFinder.Domain.ViewModels;

namespace VanFinder.Domain.Interfaces.Application;

public interface IVeiculoService
{
    Task<IEnumerable<VeiculoViewModel>> GetByUsuarioIdAsync(int usuarioId);
    Task<VeiculoViewModel> CreateAsync(VeiculoViewModel veiculo);
}