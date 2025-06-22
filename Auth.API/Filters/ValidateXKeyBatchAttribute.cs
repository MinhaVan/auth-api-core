using System;
using Auth.Service.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.API.Filters;

public class ValidateXKeyBatchAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var services = context.HttpContext.RequestServices;
        var secretManager = services.GetRequiredService<SecretManager>();

        var expectedValue = secretManager.Keys.KayForAuth;
        var hasHeader = context.HttpContext.Request.Headers.TryGetValue("X-Consumer-Key", out var headerValue);

        if (!hasHeader || headerValue != expectedValue)
        {
            context.Result = new UnauthorizedObjectResult("Cabeçalho X-Consumer-Key inválido ou ausente.");
        }
    }
}