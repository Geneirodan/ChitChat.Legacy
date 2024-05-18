using Common.Other;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Messages.Queries.Persistence.Entities;
using Messages.Queries.Persistence.Filters;
using Messages.Queries.Persistence.Interfaces;

namespace Messages.Queries.Persistence.Elastic;

public sealed class MessageQueryHandler(ElasticsearchClient client) : IMessageQueryHandler
{
    private static readonly string[] Fields = [nameof(Message.Content)];

    public async Task<PaginatedList<Message>> QueryAsync(MessagesFilter filter,
        CancellationToken cancellationToken = default)
    {
        var entities = await client.SearchAsync(ConfigureRequest(filter), cancellationToken).ConfigureAwait(false);
        return new PaginatedList<Message>(entities.Documents, entities.Total);
    }

    private static Action<SearchRequestDescriptor<Message>> ConfigureRequest(MessagesFilter filter) =>
        searchRequestDescriptor =>
        {
            searchRequestDescriptor
                .Index(Indices.Messages)
                .TrackTotalHits(new TrackHits(true))
                .Sort(sortOptionsDescriptor => sortOptionsDescriptor
                    .Field(
                        message => message.SendTime,
                        fieldSortDescriptor => fieldSortDescriptor.Order(SortOrder.Desc)
                    )
                )
                .From((filter.Page - 1) * filter.PerPage)
                .Size(filter.PerPage);
            if (!string.IsNullOrEmpty(filter.Search))
                searchRequestDescriptor.Query(queryDescriptor => queryDescriptor
                    .MultiMatch(multiMatchQueryDescriptor => multiMatchQueryDescriptor
                        .Fields(Fields)
                        .Query(filter.Search)
                    )
                );
        };
}