using Application.Interfaces;
using Common.Interfaces;
using MediatR;

namespace Application.Queries;

public sealed record GetProfileQuery : IRequest<ProfileViewModel?>;


public sealed record ProfileViewModel(Guid Id, string FirstName, string LastName, string Bio);

public sealed class GetProfileQueryHandler(IProfileRepository repository, IUser user)
    : IRequestHandler<GetProfileQuery, ProfileViewModel?>
{
    public async Task<ProfileViewModel?> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetModelById(user.Id!.Value, cancellationToken).ConfigureAwait(false);
    }
}