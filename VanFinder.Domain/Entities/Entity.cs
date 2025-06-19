using System;

namespace VanFinder.Domain.Entities;

public abstract class Entity
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; set; }
    public DateTime? DataExclusao { get; set; }
    public bool Ativo { get; set; } = true;
}