using Elastic.Clients.Elasticsearch;
using Messages.Queries.Persistence.Entities;
using Messages.Queries.Persistence.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Messages.Queries.Persistence.Elastic;

public static class DependencyInjection
{
    public static IServiceCollection AddElastic(this IServiceCollection services, string connectionString)
    {
        var settings = new ElasticsearchClientSettings(new Uri(connectionString))
            .DefaultMappingFor<Message>(mappingDescriptor => mappingDescriptor
                .IndexName(Indices.Messages)
                .IdProperty(message => message.Id)
            )
            .PrettyJson()
            .MaximumRetries(10);

        var client = new ElasticsearchClient(settings);
        return services.AddSingleton(client)
            .AddSingleton<IndexInitializer>()
            .AddScoped<IMessageQueryHandler, MessageQueryHandler>()
            .AddScoped<IMessageRepository, MessageRepository>();
    }
}