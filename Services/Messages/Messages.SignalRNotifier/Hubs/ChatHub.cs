using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messages.SignalRNotifier.Hubs;

[Authorize]
internal sealed class ChatHub : Hub;