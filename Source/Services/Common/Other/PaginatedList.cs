namespace Common.Other;

public sealed record PaginatedList<T>(IEnumerable<T> List, long PageCount);
