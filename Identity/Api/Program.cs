using Identity.Api;
using Identity.Api.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Duende.IdentityServer.Services;
using Identity.Api.Services;
using Identity.Api.Options;
using Shared.Options;
using Shared.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<IdentityDataParams>()
    .BindConfiguration(IdentityDataParams.SectionName)
    .ValidateDataAnnotations();

builder.Services.AddOptions<LdapConnectionParams>()
    .BindConfiguration(LdapConnectionParams.SectionName)
    .ValidateDataAnnotations();

builder.Services.AddSingleton<IConfigProvider, ConfigProvider>();
builder.Services.AddScoped<ILdapRepository, LdapRepository>();

var serviceProvider = builder.Services.BuildServiceProvider();
var configProvider = serviceProvider.GetRequiredService<IConfigProvider>();

builder.Services.AddIdentityServer(opt => {
        opt.IssuerUri = serviceProvider.GetRequiredService<IOptions<IdentityDataParams>>().Value.IssuerUri;
    })
    .AddInMemoryApiScopes(configProvider.GetApiScopes())
    .AddInMemoryClients(configProvider.GetClients())
    .AddResourceOwnerValidator<LdapResourceOwnerPasswordValidator>()
    .AddProfileService<ProfileService>()
    .AddDeveloperSigningCredential();

var app = builder.Build();
app.UseIdentityServer();

app.Run();
