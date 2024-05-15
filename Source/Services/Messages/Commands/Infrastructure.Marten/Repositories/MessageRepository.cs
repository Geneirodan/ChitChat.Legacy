using Messages.Commands.Application.Interfaces;
using Messages.Commands.Domain.Aggregates;

namespace Messages.Commands.Infrastructure.Marten.Repositories;

public class MessageRepository(IDocumentSession session) : Repository<Message>(session), IMessageRepository;