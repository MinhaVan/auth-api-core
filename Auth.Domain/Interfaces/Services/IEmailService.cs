using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auth.Domain.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(string destino, string titulo, string mensagem);
    Task SendEmailAsync(List<string> destinos, string titulo, string mensagem);
}
