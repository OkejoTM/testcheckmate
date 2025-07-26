using Serilog;
using Infrustructure;
using Application;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentMigrator.Runner;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Infrustructure.Options;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Api.Filters;

using MediatR;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(opt => {
    opt.DocumentFilter<AuthTokenDocumentFilter>();

    opt.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearerAuth"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddOptions<IdentityDataParams>()
    .BindConfiguration(IdentityDataParams.SectionName)
    .ValidateDataAnnotations();

var identityDataParams = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<IdentityDataParams>>().Value;

builder.Services.AddAuthentication()
    .AddJwtBearer(options => {
        options.Authority = identityDataParams.ServerAddress;
        options.TokenValidationParameters.ValidIssuer = identityDataParams.ServerAddress;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.ValidateLifetime = true;
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
app.UseSwaggerUI(opt => {
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    opt.RoutePrefix = string.Empty;
});

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

app.Run();
