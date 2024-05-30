using Ardalis.Specification;
using Common;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Messages.Queries.Domain.Entities;
using Messages.Queries.Domain.Interfaces;

namespace Messages.Queries.Infrastructure.Elastic;

public sealed class MessageQueryHandler(ElasticsearchClient client) : IMessageQueryHandler
{
    private static readonly string[] Fields = [nameof(Message.Content)];

    public async Task<PaginatedList<Message>> QueryAsync(ISpecification<Message> filter,
        int page, int perPage,
        CancellationToken cancellationToken = default)
    {
        var entities = await client.SearchAsync<Message>(searchRequestDescriptor =>
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
                .From((page - 1) * perPage)
                .Size(perPage);
            foreach (var criteria in filter.SearchCriterias)
                if (!string.IsNullOrEmpty(criteria.SearchTerm))
                    searchRequestDescriptor.Query(queryDescriptor =>
                        queryDescriptor.MultiMatch(multiMatchQueryDescriptor =>
                            multiMatchQueryDescriptor.Fields(Fields).Query(criteria.SearchTerm)
                        )
                    );
        }, cancellationToken).ConfigureAwait(false);
        return new PaginatedList<Message>(entities.Documents, page, perPage, entities.Total);
    }
}