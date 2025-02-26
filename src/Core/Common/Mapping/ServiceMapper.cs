namespace GamaEdtech.Common.Mapping
{
    using System;

    [DataAnnotation.ServiceLifetime(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)]
    public class ServiceMapper(IServiceProvider serviceProvider, TypeAdapterConfig config) : MapsterMapper.ServiceMapper(serviceProvider, config), IMapper
    {
    }
}
