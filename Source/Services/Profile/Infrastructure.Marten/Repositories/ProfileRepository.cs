using Profiles.Application.Interfaces;
using Profiles.Application.ViewModels;

namespace Profiles.Infrastructure.Marten.Repositories;

public sealed class ProfileRepository(IDocumentSession documentSession) 
    : MartenRepository<Domain.Profile, Aggregates.Profile>(documentSession), IProfileRepository
{
    private readonly IDocumentSession _documentSession = documentSession;

    public async Task<ProfileViewModel?> GetModelByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _documentSession.Query<ProfileViewModel>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}