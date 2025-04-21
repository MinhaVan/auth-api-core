using AutoMapper;
using Auth.Domain.Models;

namespace Auth.Service.Configurations;

public class EnderecoMapper : Profile
{
    public EnderecoMapper()
    {
        CreateMap<EnderecoAdicionarViewModel, Endereco>().ReverseMap();
        CreateMap<EnderecoAtualizarViewModel, Endereco>().ReverseMap();
        CreateMap<EnderecoViewModel, Endereco>().ReverseMap();
    }
}