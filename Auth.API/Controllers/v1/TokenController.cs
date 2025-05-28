using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Auth.Domain.ViewModels;
using Auth.Domain.Interfaces.Services;
using System.Diagnostics.CodeAnalysis;

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

    [HttpPost("login")]
    public async Task<ActionResult<TokenViewModel>> Login([FromBody] UsuarioLoginViewModel user)
    {
        var token = await _tokenService.Login(user);
        if (token == null)
        {
            return Default(400, "Acesso negado!", true);
        }

        return Success(token);
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult<TokenViewModel>> RefreshToken([FromBody] RefreshTokenRequest user)
    {
        var token = await _tokenService.RefreshToken(user);
        if (token == null)
        {
            return Default(400, "Acesso negado!", true);
        }

        return Success(token);
    }
}
