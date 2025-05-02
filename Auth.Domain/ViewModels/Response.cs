using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Auth.Domain.ViewModels;

[ExcludeFromCodeCoverage]
public class BaseResponse<T>
{
    public bool Sucesso { get; set; }
    public T Data { get; set; }
    public string Mensagem { get; set; }
    public List<string> Erros { get; set; } = new List<string>();
}