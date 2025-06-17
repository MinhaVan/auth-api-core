using AutoMapper;
using Auth.Domain.ViewModels;
using Auth.Domain.Models;
using System.Diagnostics.CodeAnalysis;
using Auth.Domain.ViewModels.Usuario;

namespace Auth.Service.Configurations;

[ExcludeFromCodeCoverage]
public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<UsuarioBaseViewModel, Usuario>().ReverseMap();
        CreateMap<UsuarioDetalheViewModel, Usuario>().ReverseMap();

        CreateMap<UsuarioViewModel, Usuario>()
            .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => src.Senha))
            .ReverseMap()
            .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => (string)null));

        CreateMap<UsuarioNovoViewModel, Usuario>().ReverseMap();
        CreateMap<UsuarioLoginViewModel, Usuario>().ReverseMap();
        CreateMap<UsuarioAtualizarViewModel, Usuario>().ReverseMap();
    }
}