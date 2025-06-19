using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VanFinder.Domain.Interfaces.Application;
using VanFinder.Domain.Interfaces.Repository;
using VanFinder.Domain.ViewModels;

namespace VanFinder.Application.Implementation;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioViewModel> GetByIdAsync(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        var viewModel = new UsuarioViewModel
        {
            Id = usuario.Id,
            PrimeiroNome = usuario.PrimeiroNome,
            UltimoNome = usuario.UltimoNome,
            Email = usuario.Email,
            Cpf = usuario.Cpf,
            DataNascimento = usuario.DataNascimento,
            Contato = usuario.Contato,
            Ativo = usuario.Ativo
        };

        return viewModel;
    }
}