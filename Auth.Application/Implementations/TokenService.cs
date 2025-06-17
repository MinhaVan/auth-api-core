using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Domain.Models;
using Auth.Service.Configuration;
using Auth.Domain.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using Auth.Domain.ViewModels;
using System.Threading.Tasks;
using Auth.Domain.Interfaces.Repository;
using Auth.Service.Exceptions;
using AutoMapper;

namespace Auth.Service.Implementations;

public class TokenService(
    IMapper _mapper,
    TokenConfigurations _configuration,
    IUsuarioRepository _usuarioRepository) : ITokenService
{
    private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

    public async Task ConfirmarUsuarioAsync(int usuarioId)
    {
        if (usuarioId <= 0)
        {
            throw new BusinessRuleException("ID do usuário inválido.");
        }

        var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
        if (usuario is null)
        {
            throw new BusinessRuleException("Usuário não encontrado.");
        }

        usuario.UsuarioValidado = true;
        await _usuarioRepository.AtualizarAsync(usuario);
    }

    public async Task<TokenViewModel> Login(UsuarioLoginViewModel user)
    {
        user.Senha = Base64ToString(user.Senha);
        user.Senha = _usuarioRepository.ComputeHash(user.Senha);

        var userModel = await _usuarioRepository.LoginAsync(user);
        _ = userModel ?? throw new BusinessRuleException("Login ou senha inválido!");

        if (!userModel.UsuarioValidado)
        {
            throw new BusinessRuleException("Usuário não foi validado ainda. Favor acessar o e-mail e confirmar!");
        }

        return await GenerateTokensForUser(userModel);
    }

    public async Task<TokenViewModel> RefreshToken(RefreshTokenRequest user)
    {
        Console.WriteLine($"Gerando tokens para o usuário: {user.RefreshToken}");

        var userModel = await _usuarioRepository.BuscarPorRefreshTokenAsync(user.RefreshToken);
        if (userModel == null || userModel?.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            Console.WriteLine($"Não foi possível encontrar o usuário com o refresh token.");
            throw new BusinessRuleException("Sessão encerrada. Favor reconectar!");
        }

        return await GenerateTokensForUser(userModel);
    }

    private async Task<TokenViewModel> GenerateTokensForUser(Usuario userModel)
    {
        var now = DateTime.UtcNow;
        var expirationDate = now.AddMinutes(_configuration.Minutes);
        var accessToken = GenerateAccessToken(userModel);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiryTime = now.AddDays(_configuration.DaysToExpiry);

        await _usuarioRepository.AtualizarRefreshTokenAsync(userModel.Id, refreshToken, refreshTokenExpiryTime);

        Console.WriteLine($"Novo token gerado com sucesso!.");

        return new TokenViewModel(
            authenticated: true,
            created: now.ToString(DATE_FORMAT),
            expiration: expirationDate.ToString(DATE_FORMAT),
            accessToken: accessToken,
            refreshToken: refreshToken,
            userDto: _mapper.Map<UsuarioViewModel>(userModel)
        );
    }

    public string Base64ToString(string base64)
    {
        return base64;
        // var valueBytes = Convert.FromBase64String(base64);
        // return Encoding.UTF8.GetString(valueBytes);
    }

    private string GenerateAccessToken(Usuario usuario)
    {
        if (string.IsNullOrWhiteSpace(_configuration.Secret) || _configuration.Secret.Length < 32)
            throw new ArgumentException("A chave secreta deve ter pelo menos 32 caracteres.");

        var claims = new List<Claim>
        {
            new Claim("UserId", usuario.Id.ToString()),
            new Claim("Perfil", usuario.Perfil.ToString()),
            new Claim("Empresa", usuario.EmpresaId.ToString())
        };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_configuration.Minutes),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
