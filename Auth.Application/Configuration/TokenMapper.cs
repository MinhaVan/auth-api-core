using AutoMapper;
using Auth.Domain.ViewModels;
using Auth.Domain.Models;

namespace Auth.Service.Configurations;

public class TokenMapper : Profile
{
    public TokenMapper()
    {
        CreateMap<TokenViewModel, Token>().ReverseMap();
    }
}