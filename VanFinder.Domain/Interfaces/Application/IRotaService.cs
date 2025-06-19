using System.Collections.Generic;
using System.Threading.Tasks;
using VanFinder.Domain.ViewModels;

namespace VanFinder.Domain.Interfaces.Application;

public interface IRotaService
{
    Task<IEnumerable<RotaViewModel>> GetByEndereco(string endereco);
    Task<RotaViewModel> CreateAsync(RotaViewModel rota);
}