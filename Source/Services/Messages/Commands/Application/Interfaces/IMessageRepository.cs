using Common.Interfaces;
using Messages.Commands.Domain.Aggregates;

namespace Messages.Commands.Application.Interfaces;

public interface IMessageRepository : IRepository<Message, Guid>;