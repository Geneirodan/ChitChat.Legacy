using Ardalis.Specification;
using Mapster;

namespace Common.Abstractions;

public interface IPaginatedSpecification<T>;

public interface IGetByIdSpecification<T>
{
    public Guid Id { get; }
}

public interface IGetByIdSpecification<T, TResult> : IGetByIdSpecification<T>;

public class GetByIdSpecification<T>
    : SingleResultSpecification<T>, IGetByIdSpecification<T>
    where T : IEntity<Guid>
{
    // ReSharper disable once VirtualMemberCallInConstructor
    public GetByIdSpecification(Guid id)
    {
        Id = id;
        Query.Where(x => id.Equals(x.Id));
    }

    public Guid Id { get; }
}

public class GetByIdSpecification<T, TResult>
    : SingleResultSpecification<T, TResult>, IGetByIdSpecification<T, TResult>
    where T : IEntity<Guid>
{
    // ReSharper disable VirtualMemberCallInConstructor
    public GetByIdSpecification(Guid id)
    {
        Id = id;
        Query.Where(x => id.Equals(x.Id));
        Query.Select(x => x.Adapt<TResult>());
    }

    public Guid Id { get; }
}