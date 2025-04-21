using AutoMapper;
using Auth.Domain.ViewModels;
using Auth.Domain.Models;

namespace Auth.Service.Configurations;

public class AlunoMapper : Profile
{
    public AlunoMapper()
    {
        CreateMap<AlunoViewModel, Aluno>().ReverseMap();
        CreateMap<AlunoAdicionarViewModel, Aluno>().ReverseMap();
    }
}