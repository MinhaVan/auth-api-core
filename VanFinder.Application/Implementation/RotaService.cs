using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VanFinder.Domain.Entities;
using VanFinder.Domain.Interfaces.Application;
using VanFinder.Domain.Interfaces.Repository;
using VanFinder.Domain.ViewModels;

namespace VanFinder.Application.Implementation;

public class RotaService : IRotaService
{
    private readonly IRotaRepository _rotaRepository;

    public RotaService(IRotaRepository rotaRepository)
    {
        _rotaRepository = rotaRepository;
    }

    public async Task<IEnumerable<RotaViewModel>> GetByEndereco(string endereco)
    {
        var rotas = await _rotaRepository.GetByEnderecoAsync(endereco);
        var rotasViewModel = rotas.Select(x => new RotaViewModel
        {
            Id = x.Id,
            Endereco = new EnderecoViewModel
            {
                Id = x.Endereco.Id,
                Logradouro = x.Endereco.Logradouro,
                Bairro = x.Endereco.Bairro,
                Cidade = x.Endereco.Cidade,
                Estado = x.Endereco.Estado,
                CEP = x.Endereco.CEP,
                Latitude = x.Endereco.Latitude,
                Longitude = x.Endereco.Longitude
            },
            Descricao = x.Descricao,
            Ativo = x.Ativo,
            DiaSemana = x.DiaSemana,
            Horario = x.Horario,
            Nome = x.Nome,
            TipoRota = x.TipoRota,
            Veiculo = new VeiculoViewModel
            {
                Id = x.Veiculo.Id,
                Placa = x.Veiculo.Placa,
                Modelo = x.Veiculo.Modelo,
                Marca = x.Veiculo.Marca,
                Cor = x.Veiculo.Cor
            }
        });

        return rotasViewModel;
    }

    public async Task<RotaViewModel> CreateAsync(RotaViewModel request)
    {
        var rota = new Rota
        {
            Descricao = request.Descricao,
            Ativo = request.Ativo,
            DiaSemana = request.DiaSemana,
            Horario = request.Horario,
            Nome = request.Nome,
            TipoRota = request.TipoRota,
            VeiculoId = request.Veiculo.Id
        };

        await _rotaRepository.AddAsync(rota);
        await _rotaRepository.SaveChangesAsync();
        return request;
    }
}