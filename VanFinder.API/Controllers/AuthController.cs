using Microsoft.AspNetCore.Mvc;
using VanFinder.Domain.Interfaces.Application;
using VanFinder.Domain.ViewModels;

namespace VanFinder.API.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class AuthController(IAuthService _authService) : BaseController
{
    [HttpPost("Login")]
    public IActionResult Login([FromBody] AuthRequest request)
    {
        var result = _authService.Login(request);
        if (result == null)
            return Unauthorized(new { mensagem = "Credenciais inválidas" });

        return Ok(result);
    }
}