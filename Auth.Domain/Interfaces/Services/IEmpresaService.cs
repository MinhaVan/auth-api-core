using System.Threading.Tasks;
using Auth.Domain.ViewModels;

namespace Auth.Domain.Interfaces.Services;

public interface IEmpresaService
{
    Task<EmpresaViewModel> ObterPorIdAsync(int id);
    Task<EmpresaViewModel> CriarAsync(EmpresaAdicionarViewModel empresa);
    Task AtualizarAsync(int empresaId, EmpresaAdicionarViewModel empresa);
}