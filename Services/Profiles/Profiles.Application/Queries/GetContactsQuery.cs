using Ardalis.Result;
using Common;
using Common.Abstractions;
using MediatR;
using Profiles.Domain.ViewModels;
using Profiles.Application.Interfaces;
using Profiles.Application.Queries.Specifications;

namespace Profiles.Application.Queries;

public sealed record GetContactsQuery(int Page, int PerPage, string Search, string? SortBy, bool IsDesc = false)
    : IRequest<Result<PaginatedList<ContactViewModel>>>
{
    public sealed class Handler(IContactsReadRepository repository, IUser user)
        : IRequestHandler<GetContactsQuery, Result<PaginatedList<ContactViewModel>>>
    {
        public async Task<Result<PaginatedList<ContactViewModel>>> Handle(GetContactsQuery request,
            CancellationToken cancellationToken)
        {
            if (user.Id is null)
                return Result.Unauthorized();

            var (page, perPage, search, sortBy, isDesc) = request;
            var spec = new GetContactsSpecification(search, sortBy, isDesc);
            return await repository.GetAllPaginatedAsync(spec, page, perPage, cancellationToken).ConfigureAwait(false);
        }
    }
}