using AutoMapper;
using Auth.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace Auth.Service.Configurations;

[ExcludeFromCodeCoverage]
public class EnderecoMapper : Profile
{
    public EnderecoMapper()
    {
        CreateMap<EnderecoAdicionarViewModel, Endereco>().ReverseMap();
        CreateMap<EnderecoAtualizarViewModel, Endereco>().ReverseMap();
        CreateMap<EnderecoViewModel, Endereco>().ReverseMap();
    }
}