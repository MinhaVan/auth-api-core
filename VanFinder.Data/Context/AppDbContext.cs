using Microsoft.EntityFrameworkCore;
using VanFinder.Domain.Entities;

namespace VanFinder.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
}