using Common.Abstractions;
using Profiles.Domain.Aggregates;

namespace Profiles.Application.Interfaces;

public interface IContactsReadRepository : IReadRepository<Contact>;