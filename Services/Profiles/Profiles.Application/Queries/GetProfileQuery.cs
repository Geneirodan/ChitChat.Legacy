using Ardalis.Result;
using Common.Abstractions;
using MediatR;
using Profiles.Domain.ViewModels;
using Profiles.Application.Interfaces;
using Profiles.Domain.Aggregates;

namespace Profiles.Application.Queries;

public sealed record GetProfileQuery : IRequest<Result<ProfileViewModel>>;

public sealed class GetProfileQueryHandler(IProfilesReadRepository repository, IUser user)
    : IRequestHandler<GetProfileQuery, Result<ProfileViewModel>>
{
    public async Task<Result<ProfileViewModel>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        if (user.Id is null)
            return Result.Unauthorized();

        var spec = new GetByIdSpecification<Profile, ProfileViewModel>(user.Id.Value);
        var model = await repository.GetAsync(spec, cancellationToken).ConfigureAwait(false);
        return model is not null
            ? Result.Success(model)
            : Result.NotFound();
    }
}