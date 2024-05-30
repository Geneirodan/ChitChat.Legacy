using Common.EFCore;
using Profiles.Application.Interfaces;
using Profiles.Domain.Aggregates;

namespace Profiles.Infrastructure.EntityFramework.Repositories;

public class ContactsRepository(ApplicationContext context) 
    : EntityFrameworkRepository<Contact>(context), IContactsRepository;

public class ContactsReadRepository(ApplicationContext context) 
    : EntityFrameworkReadRepository<Contact>(context), IContactsReadRepository;
