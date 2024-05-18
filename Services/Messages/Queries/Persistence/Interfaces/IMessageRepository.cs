using Common.Abstractions;
using Messages.Queries.Persistence.Entities;

namespace Messages.Queries.Persistence.Interfaces;

public interface IMessageRepository : IRepository<Message, Guid>;