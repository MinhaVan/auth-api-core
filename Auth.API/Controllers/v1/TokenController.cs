using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Auth.Domain.ViewModels;
using Auth.Domain.Interfaces.Services;
using System.Diagnostics.CodeAnalysis;
using System;
using Auth.Domain.Utils;
using Auth.API.Filters;

namespace Auth.API.Controllers.v1;

[ApiController]
[AllowAnonymous]
[Route("v1/[controller]")]
[ExcludeFromCodeCoverage]
public class TokenController : BaseController
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("Gerar/{empresaId}")]
    [ValidateXKeyBatch]
    public ActionResult<TokenViewModel> GerarTokenAsync([FromRoute] int empresaId)
    {
        var token = _tokenService.GerarTokenAsync(empresaId);
        if (token == null)
        {
            return Default(400, "Acesso negado!", true);
        }

        return Success(token);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<TokenViewModel>> LoginAsync([FromBody] UsuarioLoginViewModel user)
    {
        var token = await _tokenService.LoginAsync(user);
        if (token == null)
        {
            return Default(400, "Acesso negado!", true);
        }

        return Success(token);
    }

    [HttpGet("Confirmar/Usuario/{usuarioId}")]
    public async Task<ActionResult> ConfirmarAsync([FromRoute] int usuarioId)
    {
        await _tokenService.ConfirmarUsuarioAsync(usuarioId);
        return Success();
    }

    [HttpPost("Confirmar/Motorista/{usuarioId}")]
    public async Task<ActionResult> ConfirmarMotoristaAsync([FromRoute] int usuarioId, [FromBody] UsuarioLoginViewModel user)
    {
        await _tokenService.ConfirmarMotoristaAsync(usuarioId, user);
        return Success();
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult<TokenViewModel>> RefreshToken([FromBody] RefreshTokenRequest user)
    {
        try
        {
            Console.WriteLine($"Vamos tentar gerar um novo token para o usuário -> {user.ToJson()}");

            var token = await _tokenService.RefreshToken(user);
            if (token == null)
            {
                return Default(400, "Acesso negado!", true);
            }

            return Success(token);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Exception lançada: -> {ex}");
            throw;
        }
    }
}
