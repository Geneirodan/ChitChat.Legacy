using Common.EFCore;
using Profiles.Application.Interfaces;
using Profiles.Domain.Aggregates;

namespace Profiles.Infrastructure.EntityFramework.Repositories;

public class ProfilesRepository(ApplicationContext context) 
    : EntityFrameworkRepository<Profile>(context), IProfilesRepository;
public class ProfilesReadRepository(ApplicationContext context) 
    : EntityFrameworkReadRepository<Profile>(context), IProfilesReadRepository;