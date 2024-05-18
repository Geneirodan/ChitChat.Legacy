using Ardalis.Specification;
using Common.Abstractions;
using Messages.Commands.Domain;

namespace Messages.Commands.Application.Interfaces;

public interface IMessageRepository : IRepository<Message, Guid>
{
    Task<MessageViewModel?> GetModelByIdAsync(Guid id, CancellationToken cancellationToken = default);
}