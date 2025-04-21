using System;
using Auth.Domain.Enums;

namespace Auth.Domain.Models;

public abstract class Entity
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAlteracao { get; set; }
    public StatusEntityEnum Status { get; set; }
}