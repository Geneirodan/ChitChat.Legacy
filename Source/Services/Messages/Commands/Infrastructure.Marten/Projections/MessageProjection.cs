// using Application.Responses;
// using Domain.Events.Messages;
// using Marten.Events.Aggregation;
//
// namespace Infrastructure.Marten.Projections;
//
// public class MessageProjection : SingleStreamProjection<MessageResponse>
// {
//     public MessageProjection() => DeleteEvent<MessageDeletedEvent>();
//
//     public static MessageResponse Create(MessageCreatedEvent @event) => 
//         new(@event.Id, @event.Content, false, @event.SendTime, @event.SenderId.ToString());
//
//     public static MessageResponse Apply(MessageEditedEvent @event, MessageResponse current) =>
//         current with { Content = @event.Content };
//
//     public static MessageResponse Apply(MessageReadEvent _, MessageResponse current) =>
//         current with { IsRead = true };
// }
