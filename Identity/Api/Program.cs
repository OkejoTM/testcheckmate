using Identity.Api;
using Identity.Api.Validators;
using Microsoft.AspNetCore.Identity;
using Duende.IdentityServer.Services;
using Identity.Api.Services;
using Identity.Api.Options;
using Identity.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<IdentityDataParams>()
    .BindConfiguration(IdentityDataParams.SectionName)
    .ValidateDataAnnotations();

builder.Services.AddOptions<LdapConnectionParams>()
    .BindConfiguration(LdapConnectionParams.SectionName)
    .ValidateDataAnnotations();

builder.Services.AddSingleton<IConfigProvider, ConfigProvider>();
builder.Services.AddScoped<ILdapRepository, LdapRepository>();

var configProvider = builder.Services.BuildServiceProvider().GetRequiredService<IConfigProvider>();

builder.Services.AddIdentityServer()
    .AddInMemoryApiScopes(configProvider.GetApiScopes())
    .AddInMemoryClients(configProvider.GetClients())
    .AddResourceOwnerValidator<LdapResourceOwnerPasswordValidator>()
    .AddProfileService<ProfileService>();

var app = builder.Build();
app.UseIdentityServer();

app.MapGet("/", () => "HELLO");

app.Run();
