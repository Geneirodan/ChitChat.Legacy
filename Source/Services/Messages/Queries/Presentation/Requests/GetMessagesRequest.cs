namespace Messages.Queries.Presentation.Requests;


public sealed record GetMessagesRequest(Guid ReceiverId, string Search = "", int Page = 1, int PerPage = 10);
