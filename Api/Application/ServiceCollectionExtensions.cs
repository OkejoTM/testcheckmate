using Application.Validation;
using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Shared.Repositories;
using Shared.Options;

namespace Application;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services) {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddScoped<ILdapRepository, LdapRepository>();
        services.AddOptions<LdapConnectionParams>()
            .BindConfiguration(LdapConnectionParams.SectionName)
            .ValidateDataAnnotations();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

        return services;
    }
}
