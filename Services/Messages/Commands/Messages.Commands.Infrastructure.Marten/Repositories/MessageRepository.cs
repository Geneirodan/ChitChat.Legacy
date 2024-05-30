using Messages.Commands.Application.Interfaces;

namespace Messages.Commands.Infrastructure.Marten.Repositories;

internal sealed class MessageRepository(IDocumentSession session) 
    : MartenRepository<Domain.Message,Aggregates.Message>(session), IMessageRepository;