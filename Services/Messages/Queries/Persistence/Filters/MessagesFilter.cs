using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Types;
using Messages.Queries.Persistence.Entities;

namespace Messages.Queries.Persistence.Filters;

public sealed class MessagesFilter : PaginationFilterBase
{
    public required Guid SenderId { get; init; }
    public required Guid ReceiverId { get; init; }

    [CompareTo(nameof(Message.Content))]
    [StringFilterOptions(StringFilterOption.Contains)]
    public string Search { get; init; } = string.Empty;
}