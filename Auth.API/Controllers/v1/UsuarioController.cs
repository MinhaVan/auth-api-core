using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Auth.Domain.ViewModels;
using Auth.Domain.Interfaces.Services;

namespace Auth.API.Controllers.v1;

[ApiController]
[Route("v1/[controller]")]
[Authorize("Bearer")]
public class UsuarioController : BaseController
{
    private readonly IUsuarioService _usuarioService;


    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UsuarioViewModel>> ObterPorId([FromRoute] int userId)
    {
        return Success(await _usuarioService.ObterPorId(userId));
    }

    [HttpGet("{pagina}/{tamanho}")]
    public async Task<ActionResult<PaginadoViewModel<UsuarioViewModel>>> Obter([FromQuery] int pagina, [FromQuery] int tamanho)
    {
        var usuarios = await _usuarioService.BuscarPaginadoAsync(pagina, tamanho);
        return Success(usuarios);
    }

    [HttpGet("me")]
    public async Task<ActionResult<UsuarioViewModel>> ObterDadosDoUsuario()
    {
        return Success(await _usuarioService.ObterDadosDoUsuario());
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<UsuarioViewModel>> Register([FromBody] UsuarioNovoViewModel user)
    {
        await _usuarioService.Registrar(user);
        return Success();
    }

    [HttpPut]
    public async Task<ActionResult<UsuarioViewModel>> Update([FromBody] UsuarioAtualizarViewModel user)
    {
        user.PlanoId = user.PlanoId <= 0 ? null : user.PlanoId;
        await _usuarioService.Atualizar(user);
        return Success();
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult<UsuarioViewModel>> DeleteAsync([FromRoute] int userId)
    {
        await _usuarioService.DeletarAsync(userId);
        return Success();
    }

    [HttpPut("{userId}/confirmar")]
    [AllowAnonymous]
    public async Task<ActionResult<UsuarioViewModel>> ConfirmarCadastro([FromRoute] int userId)
    {
        await _usuarioService.ConfirmarCadastroAsync(userId);
        return Success();
    }

    [HttpPost("permissao")]
    public async Task<ActionResult> VincularPermissaoAsync(PermissaoViewModel user)
    {
        await _usuarioService.VincularPermissao(user);
        return Success();
    }
}
