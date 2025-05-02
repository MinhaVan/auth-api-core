using AutoMapper;
using Auth.Domain.ViewModels;
using Auth.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace Auth.Service.Configurations;

[ExcludeFromCodeCoverage]
public class TokenMapper : Profile
{
    public TokenMapper()
    {
        CreateMap<TokenViewModel, Token>().ReverseMap();
    }
}