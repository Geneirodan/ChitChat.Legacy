using Common.Abstractions;
using Profiles.Application.ViewModels;
using Profiles.Domain;

namespace Profiles.Application.Interfaces;

public interface IProfileRepository : IRepository<Profile, Guid>
{
    Task<ProfileViewModel?> GetModelByIdAsync(Guid id, CancellationToken cancellationToken = default);
}