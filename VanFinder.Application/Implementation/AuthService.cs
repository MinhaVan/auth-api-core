using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VanFinder.Domain.Entities;
using VanFinder.Domain.Interfaces.Application;
using VanFinder.Domain.Interfaces.Repository;
using VanFinder.Domain.ViewModels;

namespace VanFinder.Application.Implementation;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Login(AuthRequest authRequest)
    {
        var usuario = await _usuarioRepository.GetByEmailOrCpfAsync(authRequest.EmailOuCpf);
        if (usuario == null || !_passwordHasher.Verify(authRequest.Senha, usuario.SenhaHash))
            throw new UnauthorizedAccessException("Usuário ou senha inválido");

        return _tokenService.GenerateToken(usuario);
    }

    public async Task RegisterAsync(RegistrarUsuarioRequest registrarUsuario)
    {
        registrarUsuario.Senha = _passwordHasher.Hash(registrarUsuario.Senha);

        var usuario = new Usuario
        {
            PrimeiroNome = registrarUsuario.PrimeiroNome,
            UltimoNome = registrarUsuario.UltimoNome,
            Email = registrarUsuario.Email,
            Cpf = registrarUsuario.Cpf,
            SenhaHash = registrarUsuario.Senha,
            DataNascimento = registrarUsuario.DataNascimento,
            Contato = registrarUsuario.Contato
        };

        await _usuarioRepository.AddAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();
    }
}