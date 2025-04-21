using AutoMapper;
using Auth.Domain.ViewModels;
using Auth.Domain.Models;

namespace Auth.Service.Configurations;

public class PaginadoMapper : Profile
{
    public PaginadoMapper()
    {
        #region Paginado
        CreateMap(typeof(Paginado<>), typeof(PaginadoViewModel<>)).ReverseMap();
        #endregion
    }
}