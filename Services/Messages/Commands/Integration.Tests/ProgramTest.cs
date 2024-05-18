using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;


namespace Messages.Commands.Integration.Tests;

public sealed class WebApiApplication : WebApplicationFactory<Program>
{
    public PostgreSqlContainer PostgreSqlContainer { get; } =
        new PostgreSqlBuilder()
            .WithDatabase("Messages")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    public RabbitMqContainer RabbitMqContainer { get; } =
        new RabbitMqBuilder()
            .WithUsername("User")
            .WithPassword("Password")
            .Build();

    public override async ValueTask DisposeAsync()
    {
        await PostgreSqlContainer.StopAsync();
        await RabbitMqContainer.StopAsync();
        await base.DisposeAsync();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        Task.WhenAll(
            PostgreSqlContainer.StartAsync(),
            RabbitMqContainer.StartAsync()
        ).GetAwaiter().GetResult();
        builder.ConfigureHostConfiguration(configBuilder =>
        {
            var dictionary = new Dictionary<string, string?>
            {
                ["ConnectionStrings:Database"] = PostgreSqlContainer.GetConnectionString(),
                ["ConnectionStrings:RabbitMq"] = RabbitMqContainer.GetConnectionString()
            };
            configBuilder.AddInMemoryCollection(dictionary);
        });
        return base.CreateHost(builder);
    }
}