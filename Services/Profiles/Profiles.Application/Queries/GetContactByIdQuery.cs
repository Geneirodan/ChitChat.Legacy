using Ardalis.Result;
using Common.Abstractions;
using MediatR;
using Profiles.Domain.Aggregates;
using Profiles.Domain.ViewModels;
using Profiles.Application.Interfaces;

namespace Profiles.Application.Queries;

public sealed record GetContactByIdQuery(Guid Id) : IRequest<Result<ContactViewModel>>
{
    public sealed class Handler(IContactsReadRepository repository, IUser user)
        : IRequestHandler<GetContactByIdQuery, Result<ContactViewModel>>
    {
        public async Task<Result<ContactViewModel>> Handle(
            GetContactByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var spec = new GetByIdSpecification<Contact, ContactViewModel>(request.Id);
            
            var model = await repository.GetAsync(spec, cancellationToken).ConfigureAwait(false);
            
            if (model is null)
                return Result.NotFound();
            
            return model.UserId == user.Id ? model : Result.Forbidden();
        }
    }
}