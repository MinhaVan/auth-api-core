using System.Threading.Tasks;
using Auth.Domain.ViewModels;

namespace Auth.Domain.Interfaces.APIs;

public interface IPessoasAPI
{
    Task<BaseResponse<MotoristaViewModel>> ObterMotoristaPorUsuarioIdAsync(int usuarioId);
}
