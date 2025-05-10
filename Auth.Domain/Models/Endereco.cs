using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Auth.Domain.Models;

[ExcludeFromCodeCoverage]
public class Endereco : Entity
{
    public int? UsuarioId { get; set; }
    public int? EmpresaId { get; set; }

    // Relacionamentos
    // public virtual List<Aluno> EnderecosPartidas { get; set; } = new List<Aluno>();
    // public virtual List<Aluno> EnderecosRetornos { get; set; } = new List<Aluno>();
    // public virtual List<Aluno> EnderecosDestinos { get; set; } = new List<Aluno>();
    public virtual Usuario Usuario { get; set; }
    public virtual Empresa Empresa { get; set; }
}