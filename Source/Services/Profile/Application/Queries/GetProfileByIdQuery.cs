using Common.Abstractions;
using Common.Results;
using FluentResults;
using MediatR;
using Profiles.Application.Interfaces;
using Profiles.Application.ViewModels;

namespace Profiles.Application.Queries;

public sealed record GetProfileByIdQuery(Guid Id) : IRequest<Result<ProfileViewModel>>;

public sealed class GetProfileByIdQueryHandler(IProfileRepository repository, IUser user)
    : IRequestHandler<GetProfileByIdQuery, Result<ProfileViewModel>>
{
    public async Task<Result<ProfileViewModel>> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        if (user.Id != request.Id)
            ErrorResults.Forbidden();
        var model = await repository.GetModelByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        return model is null ? ErrorResults.NotFound() : model;
    }
}