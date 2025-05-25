using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Auth.Domain.ViewModels;
using Auth.Domain.Interfaces.Services;
using System.Diagnostics.CodeAnalysis;

namespace Auth.API.Controllers.v1;

[ApiController]
[Route("v1/[controller]")]
[Authorize("Bearer")]
[ExcludeFromCodeCoverage]
public class EmpresaController : BaseController
{
    private readonly IEmpresaService _empresaService;
    public EmpresaController(IEmpresaService empresaService)
    {
        _empresaService = empresaService;
    }

    [HttpGet("{empresaId}")]
    public async Task<ActionResult<UsuarioViewModel>> ObterPorId(
        [FromRoute] int empresaId)
    {
        return Success(
            await _empresaService.ObterPorIdAsync(empresaId),
            "Empresa encontrada com sucesso"
        );
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<PaginadoViewModel<UsuarioViewModel>>> AdicionarAsync(
        [FromBody] EmpresaAdicionarViewModel empresa)
    {
        return Success(
            await _empresaService.CriarAsync(empresa),
            "Empresa criada com sucesso"
        );
    }

    [HttpPut("{empresaId}")]
    public async Task<ActionResult<PaginadoViewModel<UsuarioViewModel>>> AtualizarAsync(
        [FromRoute] int empresaId,
        [FromBody] EmpresaAdicionarViewModel empresa)
    {
        await _empresaService.AtualizarAsync(empresaId, empresa);
        return Success();
    }
}
