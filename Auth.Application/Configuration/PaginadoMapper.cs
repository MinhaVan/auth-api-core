using AutoMapper;
using Auth.Domain.ViewModels;
using Auth.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace Auth.Service.Configurations;

[ExcludeFromCodeCoverage]
public class PaginadoMapper : Profile
{
    public PaginadoMapper()
    {
        #region Paginado
        CreateMap(typeof(Paginado<>), typeof(PaginadoViewModel<>)).ReverseMap();
        #endregion
    }
}