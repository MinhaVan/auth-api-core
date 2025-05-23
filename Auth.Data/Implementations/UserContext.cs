using System;
using System.Linq;
using Auth.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Http;

namespace Auth.Data.Implementations;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int Empresa
    {
        get
        {
            try
            {
                var claims = _httpContextAccessor.HttpContext.User.Claims;
                var empresaClaim = claims.FirstOrDefault(c => c.Type == "Empresa");
                return int.Parse(empresaClaim.Value);
            }
            catch (System.Exception)
            {
                throw new UnauthorizedAccessException("Erro ao acessar a empresa do token.");
            }
        }
    }

    public int UserId
    {
        get
        {
            try
            {
                var claims = _httpContextAccessor.HttpContext.User.Claims;
                var userId = claims.FirstOrDefault(c => c.Type == "UserId");
                return int.Parse(userId.Value);
            }
            catch (System.Exception)
            {
                throw new UnauthorizedAccessException("rro ao acessar o usuário do token.");
            }
        }
    }
}
