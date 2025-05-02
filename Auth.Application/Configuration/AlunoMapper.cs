using AutoMapper;
using Auth.Domain.ViewModels;
using Auth.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace Auth.Service.Configurations;

[ExcludeFromCodeCoverage]
public class AlunoMapper : Profile
{
    public AlunoMapper()
    {
        CreateMap<AlunoViewModel, Aluno>().ReverseMap();
        CreateMap<AlunoAdicionarViewModel, Aluno>().ReverseMap();
    }
}