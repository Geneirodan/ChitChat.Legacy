using System.Reflection;
using Common.Http;
using FluentValidation;
using Identity.Endpoints;
using Identity.Options;
using Identity.Options.Configurations;
using Identity.Persistence;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("Database");
services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString))
    .AddScoped<ApplicationContextInitializer>();

services.AddHealthChecks()
    .AddDbContextCheck<ApplicationContext>();

services.AddSingleton<IConfigureOptions<IdentityOptions>, IdentityOptionsConfiguration>()
    .AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();

services
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true)
    .AddFluentValidationAutoValidation()
    .AddAuthenticationAndAuthorization(configuration.GetSection("Jwt"))
    .AddSwagger()
    .AddProblemDetails()
    .Configure<ExpirationOptions>(configuration.GetSection("Expiration"))
    .Configure<EmailOptions>(configuration.GetSection("EmailSettings"))
    .AddScoped<ITokenService, TokenService>()
    .AddTransient<IEmailSender, EmailSender>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.GetRequiredService<ApplicationContextInitializer>().SeedAsync();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger().UseSwaggerUI();
}

app.UseExceptionHandler()
    .UseStatusCodePages()
    .UseAuthentication()
    .UseAuthorization();

app.MapGroup("api/v1").MapIdentity();

await app.RunAsync().ConfigureAwait(false);

public partial class Program;