namespace Profiles.WebAPI.Requests;

public sealed record GetContactsRequest(int Page = 1, int PerPage = 10, string Search = "", string? SortBy = null, bool IsDesc = false);