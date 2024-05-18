using AutoFilterer.Enums;
using Common.Abstractions;
using Common.Other;
using Common.Results;
using FluentResults;
using MediatR;
using Profiles.Application.Interfaces;
using Profiles.Application.Queries.Filters;
using Profiles.Application.ViewModels;

namespace Profiles.Application.Queries;

public sealed record GetContactsQuery(int Page, int PerPage, string Search, string? SortBy, bool IsDesc = false) 
    : IRequest<Result<PaginatedList<ContactViewModel>>>
{
    public sealed class Handler(IContactRepository repository, IUser user)
        : IRequestHandler<GetContactsQuery, Result<PaginatedList<ContactViewModel>>>
    {
        public async Task<Result<PaginatedList<ContactViewModel>>> Handle(GetContactsQuery request, CancellationToken cancellationToken)
        {
            if (user.Id is null)
                return ErrorResults.Unauthorized();
            var (page, perPage, search, sortBy, isDesc) = request;
            var filter = new ContactsFilter
            {
                CombineWith = CombineType.And,
                SortBy = isDesc ? Sorting.Descending : Sorting.Ascending, Sort = sortBy,
                Page = page, PerPage = perPage,
                UserId = user.Id.Value, Search = search
            };
            return await repository.GetModelsAsync(filter, cancellationToken).ConfigureAwait(false);
        }
    }
}