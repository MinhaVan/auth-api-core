using AutoMapper;
using Auth.Domain.Models;
using System.Diagnostics.CodeAnalysis;
using Auth.Domain.ViewModels;

namespace Auth.Service.Configurations;

[ExcludeFromCodeCoverage]
public class EmpresaMapper : Profile
{
    public EmpresaMapper()
    {
        CreateMap<EmpresaViewModel, Empresa>().ReverseMap();
        CreateMap<EmpresaAdicionarViewModel, Empresa>().ReverseMap();
    }
}