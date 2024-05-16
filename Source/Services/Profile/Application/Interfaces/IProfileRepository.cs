using Common.Abstractions;

namespace Profile.Application.Interfaces;

public interface IProfileRepository : IRepository<Domain.Profile, Guid>
{
    Task<ProfileViewModel?> GetModelById(Guid id, CancellationToken cancellationToken = default);
}