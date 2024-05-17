using Common.Abstractions;
using Common.Results;
using FluentResults;
using MediatR;
using Profiles.Application.Interfaces;
using Profiles.Application.ViewModels;

namespace Profiles.Application.Queries;

public sealed record GetProfileQuery : IRequest<Result<ProfileViewModel>>;

public sealed class GetProfileQueryHandler(IProfileRepository repository, IUser user)
    : IRequestHandler<GetProfileQuery, Result<ProfileViewModel>>
{
    public async Task<Result<ProfileViewModel>> Handle(GetProfileQuery request, CancellationToken cancellationToken) => 
        await repository.GetModelByIdAsync(user.Id!.Value, cancellationToken).ConfigureAwait(false) is { } model 
            ? Result.Ok(model)
            : ErrorResults.NotFound();
}