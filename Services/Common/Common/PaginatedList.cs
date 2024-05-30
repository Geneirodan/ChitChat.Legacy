namespace Common;

public sealed record PaginatedList<T>(IEnumerable<T> Items, int Page, int PerPage, long TotalCount)
{
    public bool HasNextPage => Page * PerPage < TotalCount;
    public bool HasPrevPage => Page > 1;
}
