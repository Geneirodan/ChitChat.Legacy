using Common.Abstractions;
using Common.Results;
using FluentResults;
using MediatR;
using Profiles.Application.Interfaces;
using Profiles.Application.ViewModels;

namespace Profiles.Application.Queries;

public sealed record GetContactByIdQuery(Guid Id) : IRequest<Result<ContactViewModel>>
{
    public sealed class Handler(IContactRepository repository, IUser user)
        : IRequestHandler<GetContactByIdQuery, Result<ContactViewModel>>
    {
        public async Task<Result<ContactViewModel>> Handle(
            GetContactByIdQuery request,
            CancellationToken cancellationToken
        ) =>
            await repository.GetModelByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
                is { } model && model.UserId == user.Id
                ? model
                : ErrorResults.NotFound();
    }
}