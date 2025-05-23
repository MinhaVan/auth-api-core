using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auth.Domain.Interfaces.Services;

public interface IAmazonService
{
    Task SendEmail(string destino, string titulo, string mensagem);
    Task SendEmail(List<string> destinos, string titulo, string mensagem);
}
