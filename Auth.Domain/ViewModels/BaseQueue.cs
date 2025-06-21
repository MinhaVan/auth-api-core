using System.Collections.Generic;
using System.Linq;

namespace Auth.Domain.ViewModels;

public class BaseQueue<T>
{
    public T Mensagem { get; set; }
    public int Retry { get; set; } = 0;
    public List<string> Erros { get; set; } = new();
    public bool Sucesso => !Erros.Any();
}

public static class BaseQueueExtensions
{
    public static BaseQueue<T> NewQueue<T>(this T data)
    {
        return new BaseQueue<T>
        {
            Retry = 0,
            Mensagem = data,
        };
    }
}