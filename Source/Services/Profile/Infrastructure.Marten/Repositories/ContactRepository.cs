using AutoFilterer.Extensions;
using Common.Other;
using Profiles.Application.Interfaces;
using Profiles.Application.Queries.Filters;
using Profiles.Application.ViewModels;

namespace Profiles.Infrastructure.Marten.Repositories;

public sealed class ContactRepository(IDocumentSession documentSession)
    : MartenRepository<Domain.Contact, Aggregates.Contact>(documentSession), IContactRepository
{
    private readonly IDocumentSession _documentSession = documentSession;

    public async Task<ContactViewModel?> GetModelByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _documentSession.Query<ContactViewModel>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

    public async Task<PaginatedList<ContactViewModel>> GetModelsAsync(ContactsFilter filter,
        CancellationToken cancellationToken = default)
    {
        var models = _documentSession.Query<ContactViewModel>().ApplyFilterWithoutPagination(filter);
        var count = await models.CountAsync(cancellationToken).ConfigureAwait(false);
        var paged = models.ToPaged(filter.Page, filter.PerPage);
        return new PaginatedList<ContactViewModel>(paged, count);
    }
}