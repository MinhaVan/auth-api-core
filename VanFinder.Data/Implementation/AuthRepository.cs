using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VanFinder.Data.Context;
using VanFinder.Domain.Entities;
using VanFinder.Domain.Interfaces.Repository;

namespace VanFinder.Data.Implementation;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;

    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario> GetUsuarioByEmailOrCpfAsync(string emailCpf)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == emailCpf || u.CPF == emailCpf);
    }
}