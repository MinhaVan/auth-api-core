using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Auth.Domain.Enums;
using Auth.Domain.ViewModels.Perfil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers.v1;

[ApiController]
[Route("v1/[controller]")]
[Authorize("Bearer")]
[ExcludeFromCodeCoverage]
public class PerfilController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Obter()
    {
        var perfil = new List<PerfilViewModel>();
        foreach (var value in Enum.GetValues(typeof(PerfilEnum)))
        {
            perfil.Add(new PerfilViewModel
            {
                Id = (int)value,
                Nome = value.ToString()
            });
        }
        return Success(perfil);
    }
}