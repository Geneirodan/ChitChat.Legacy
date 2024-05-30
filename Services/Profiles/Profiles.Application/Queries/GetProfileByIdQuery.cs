using Ardalis.Result;
using Common.Abstractions;
using Common.MediatR.Attributes;
using MediatR;
using Profiles.Domain.ViewModels;
using Profiles.Application.Interfaces;
using Profiles.Domain.Aggregates;

namespace Profiles.Application.Queries;

[Authorize]
public sealed record GetProfileByIdQuery(Guid Id) : IRequest<Result<ProfileViewModel>>;

public sealed class GetProfileByIdQueryHandler(IProfilesReadRepository repository)
    : IRequestHandler<GetProfileByIdQuery, Result<ProfileViewModel>>
{
    public async Task<Result<ProfileViewModel>> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetByIdSpecification<Profile, ProfileViewModel>(request.Id);
        var model = await repository.GetAsync(spec, cancellationToken).ConfigureAwait(false);
        return model is not null
            ? Result.Success(model)
            : Result.NotFound();
    }
}