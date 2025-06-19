namespace VanFinder.Domain.Entities;

public class Review : Entity
{
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public int PrestadorId { get; set; }
    public Usuario Prestador { get; set; } = null!;
    public string Comentario { get; set; } = string.Empty;
    public int Nota { get; set; }
}