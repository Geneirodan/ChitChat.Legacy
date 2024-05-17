using Common.Abstractions;
using Common.Other;
using Profiles.Application.Queries.Filters;
using Profiles.Application.ViewModels;
using Profiles.Domain;

namespace Profiles.Application.Interfaces;

public interface IContactRepository : IRepository<Contact, Guid>
{
    Task<ContactViewModel?> GetModelByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedList<ContactViewModel>> GetModelsAsync(ContactsFilter filter, CancellationToken cancellationToken = default);
}