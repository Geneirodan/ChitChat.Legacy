using Application.Interfaces;
using Common.Abstractions;
using MediatR;

namespace Application.Queries;

public sealed record GetProfileQuery : IRequest<ProfileViewModel?>;

public sealed class GetProfileQueryHandler(IProfileRepository repository, IUser user)
    : IRequestHandler<GetProfileQuery, ProfileViewModel?>
{
    public async Task<ProfileViewModel?> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetModelById(user.Id!.Value, cancellationToken).ConfigureAwait(false);
    }
}