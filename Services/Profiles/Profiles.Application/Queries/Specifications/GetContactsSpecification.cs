using System.Linq.Expressions;
using Ardalis.Specification;
using Profiles.Domain.Aggregates;
using Profiles.Domain.ViewModels;

namespace Profiles.Application.Queries.Specifications;

public sealed class GetContactsSpecification : Specification<Contact, ContactViewModel>
{
    public GetContactsSpecification(string search, string? sortBy, bool isDesc)
    {
        if (!string.IsNullOrWhiteSpace(search))
            Query.Where(x =>
                (x.FirstName != null && x.FirstName.Contains(search)) ||
                (x.LastName != null && x.LastName.Contains(search)));

        if (sortBy is null) return;

        if (!_dictionary.TryGetValue(sortBy.Trim().ToLower(), out var func))
            throw new ArgumentOutOfRangeException(nameof(sortBy), sortBy,
                $"Parameter sortBy may only be in range {{{string.Join(", ", _dictionary.Keys)}}}");

        if (isDesc)
            Query.OrderByDescending(func);
        else
            Query.OrderBy(func);
    }

    private readonly Dictionary<string, Expression<Func<Contact, object?>>> _dictionary = new()
    {
        { nameof(Contact.FirstName).ToLower(), contact => contact.FirstName },
        { nameof(Contact.LastName).ToLower(), contact => contact.LastName }
    };
}