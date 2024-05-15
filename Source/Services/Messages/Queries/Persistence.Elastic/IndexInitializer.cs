using Elastic.Clients.Elasticsearch;
using Messages.Queries.Persistence.Entities;

namespace Messages.Queries.Persistence.Elastic;

public class IndexInitializer(ElasticsearchClient client)
{
    public async Task InitializeAsync()
    {
        var response = await client.Indices.ExistsAsync(Indices.Messages).ConfigureAwait(false);

        if (!response.Exists)
            await client.Indices.CreateAsync<Message>(Indices.Messages, createIndexRequestDescriptor => 
                createIndexRequestDescriptor.Mappings(mappingDescriptor => 
                    mappingDescriptor.Properties(propertiesDescriptor => 
                        propertiesDescriptor.DateNanos(message => message.SendTime)
                    )
                )
            ).ConfigureAwait(false);
    }
}