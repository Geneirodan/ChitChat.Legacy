namespace Messages.Queries.WebApi.Requests;


internal sealed record GetMessagesRequest(Guid ReceiverId, string Search = "", int Page = 1, int PerPage = 10);
