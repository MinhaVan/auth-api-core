using System.Threading.Tasks;
using Auth.Domain.ViewModels;

namespace Auth.Domain.Interfaces.APIs;

public interface IPessoasAPI
{
    Task<BaseResponse<object>> AdicionarMotoristaAsync(MotoristaViewModel motorista);
    Task<BaseResponse<MotoristaViewModel>> ObterMotoristaPorIdAsync(int motoristaId);
    Task<BaseResponse<MotoristaViewModel>> ObterMotoristaPorUsuarioIdAsync(int usuarioId);
}
