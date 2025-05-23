using System.Threading.Tasks;
using Auth.Domain.ViewModels;

namespace Auth.Domain.Interfaces.Services;

public interface IUsuarioService
{
    Task<UsuarioViewModel> Registrar(UsuarioNovoViewModel user);
    Task DeletarAsync(int userId);
    Task Atualizar(UsuarioAtualizarViewModel user);
    Task<UsuarioViewModel> ObterPorId(int UserId);
    Task<UsuarioViewModel> ObterDadosDoUsuario();
    Task VincularPermissao(PermissaoViewModel user);
    Task ConfirmarCadastroAsync(int userId);
    Task<PaginadoViewModel<UsuarioViewModel>> BuscarPaginadoAsync(int pagina, int tamanho);
}