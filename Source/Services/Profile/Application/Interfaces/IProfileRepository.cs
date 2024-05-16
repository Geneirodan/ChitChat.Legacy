using Application.Queries;
using Common.Interfaces;
using Domain;

namespace Application.Interfaces;

public interface IProfileRepository : IRepository<Profile, Guid>
{
    Task<ProfileViewModel?> GetModelById(Guid id, CancellationToken cancellationToken = default);
}