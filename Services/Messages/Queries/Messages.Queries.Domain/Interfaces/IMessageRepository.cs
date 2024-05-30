using Common.Abstractions;
using Messages.Queries.Domain.Entities;

namespace Messages.Queries.Domain.Interfaces;

public interface IMessageRepository : IRepository<Message>;