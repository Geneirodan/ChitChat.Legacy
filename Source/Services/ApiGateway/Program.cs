using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

configuration.AddOcelotWithSwaggerSupport(options => options.Folder = "Routes");
services.AddOcelot(configuration).AddPolly();
services.AddSwaggerForOcelot(configuration);

configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddOcelot("Routes", builder.Environment)
    .AddEnvironmentVariables();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerForOcelotUI(options => options.PathToSwaggerGenerator = "/swagger/docs");
}

await app.UseOcelot();

await app.RunAsync();
