using System;
using System.Diagnostics.CodeAnalysis;
using Auth.Domain.Enums;

namespace Auth.Domain.Models;

[ExcludeFromCodeCoverage]
public abstract class Entity
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAlteracao { get; set; }
    public StatusEntityEnum Status { get; set; }
}