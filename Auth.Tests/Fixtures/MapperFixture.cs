using Auth.Service.Configurations;
using AutoMapper;

namespace Auth.Tests.Fixtures;

public static class MapperFixture
{
    public static IMapper Get()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PaginadoMapper>();
            cfg.AddProfile<TokenMapper>();
            cfg.AddProfile<UserMapper>();
        });
        var mapper = config.CreateMapper();
        return mapper;
    }
}