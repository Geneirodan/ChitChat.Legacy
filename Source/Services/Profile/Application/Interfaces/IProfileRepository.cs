using Common.Abstractions;
using Domain;

namespace Application.Interfaces;

public interface IProfileRepository : IRepository<Profile, Guid>
{
    Task<ProfileViewModel?> GetModelById(Guid id, CancellationToken cancellationToken = default);
}