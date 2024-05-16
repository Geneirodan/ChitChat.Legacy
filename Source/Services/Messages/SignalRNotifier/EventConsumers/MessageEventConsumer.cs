using MassTransit;
using MassTransit.SignalR.Contracts;
using MassTransit.SignalR.Utils;
using Messages.Contracts;
using Messages.SignalRNotifier.Hubs;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace Messages.SignalRNotifier.EventConsumers;

// public abstract class MessageEventConsumer<TEvent>(IHubContext<ChatHub> hubContext) 
//     : IConsumer<TEvent> 
//     where TEvent : MessageEvent
// {
//     public async Task Consume(ConsumeContext<TEvent> context)
//     {
//         var message = context.Message;
//         await hubContext.Clients
//             .All//.User(message.ReceiverId.ToString())
//             .SendAsync(typeof(TEvent).Name, message);
//     }
// }
public abstract class MessageEventConsumer<TEvent>(IPublishEndpoint endpoint) 
    : IConsumer<TEvent> 
    where TEvent : MessageEvent
{
    private readonly IHubProtocol[] _protocols = [new JsonHubProtocol()];
    public async Task Consume(ConsumeContext<TEvent> context)
    {
        var message = context.Message;
        await endpoint.Publish<User<ChatHub>>(new
        {
            UserId = message.ReceiverId.ToString(),
            Messages = _protocols.ToProtocolDictionary(typeof(TEvent).Name, [message])
        }).ConfigureAwait(false);
    }
}