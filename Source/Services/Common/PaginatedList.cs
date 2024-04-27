using System.Collections;

namespace Common;

public sealed record PaginatedList<T>(IEnumerable<T> List, int PageCount) : IEnumerable<T>
{
    public IEnumerator<T> GetEnumerator() => List.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (List as IEnumerable).GetEnumerator();
}
