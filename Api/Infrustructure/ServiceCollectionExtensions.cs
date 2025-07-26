using Infrustructure.Options;
using Infrustructure.Services;
using Infrustructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors.Postgres;

namespace Infrustructure;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration config) {
        services.AddOptions<StorageServiceParams>()
            .BindConfiguration(StorageServiceParams.SectionName)
            .ValidateDataAnnotations();
        services.AddOptions<NextCloudConnectionParams>()
            .BindConfiguration(NextCloudConnectionParams.SectionName)
            .ValidateDataAnnotations();

        string connection = config.GetConnectionString(CheckMateDbContext.ConnectionFieldName);
        services.AddDbContext<CheckMateDbContext>(options => options.UseNpgsql(connection));

        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connection)
                .ScanIn(typeof(CheckMateDbContext).Assembly));

        services.AddScoped<IStorageService, StandardStorageService>();
        services.AddScoped<ICloudStorageService, NextCloudStorageService>();
        services.AddScoped<IReceiptFolderService, ReceiptFolderService>();

        return services;
    }
}
