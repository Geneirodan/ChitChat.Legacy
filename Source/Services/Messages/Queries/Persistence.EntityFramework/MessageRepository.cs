using Messages.Queries.Persistence.Entities;
using Messages.Queries.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Messages.Queries.Persistence.EntityFramework;

public sealed record MessageRepository(ApplicationContext context) : IMessageRepository
{
    private readonly DbSet<Message> _messages = context.Messages;

    public async Task<Message?> FindAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _messages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);

    public Task AddAsync(Message entity, CancellationToken cancellationToken = default)
    {
        _messages.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Message entity, CancellationToken cancellationToken = default)
    {
        _messages.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Message entity, CancellationToken cancellationToken = default)
    {
        _messages.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
}