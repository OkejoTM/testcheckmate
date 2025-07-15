using Serilog;
using Infrustructure;
using Application;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentMigrator.Runner;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Infrustructure.Options;

using MediatR;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddOptions<IdentityDataParams>()
    .BindConfiguration(IdentityDataParams.SectionName)
    .ValidateDataAnnotations();

var identityDataParams = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<IdentityDataParams>>().Value;

builder.Services.AddAuthentication()
    .AddJwtBearer(options => {
        options.Authority = identityDataParams.ServerAddress;
        options.TokenValidationParameters.ValidateAudience = false;
        options.RequireHttpsMetadata = false;
    });
builder.Services.AddAuthorization(options => {
    options.AddPolicy("ApiScope", policy => {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");
    });
});

builder.Services.AddControllers();

builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer();

builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope()) {
    var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    migrator.MigrateUp();
}

app.UseSerilogRequestLogging();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}

app.MapGet("/", () => "hello, world");

app.Run();
