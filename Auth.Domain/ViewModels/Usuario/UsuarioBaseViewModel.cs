namespace Auth.Domain.ViewModels.Usuario;

public abstract class UsuarioBaseViewModel
{
    public string CPF { get; set; }
    public string Senha { get; set; }
    public string Contato { get; set; }
    public string Email { get; set; }
    public string PrimeiroNome { get; set; }
    public string UltimoNome { get; set; }
}