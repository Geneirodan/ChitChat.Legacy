using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Types;
using Profiles.Domain;

namespace Profiles.Application.Queries.Filters;

public sealed class ContactsFilter : PaginationFilterBase
{
    public required Guid UserId { get; init; }

    [CompareTo(nameof(Contact.FirstName), nameof(Contact.LastName), CombineWith = CombineType.Or)]
    [StringFilterOptions(StringFilterOption.Contains)]
    public string Search { get; init; } = string.Empty;
}