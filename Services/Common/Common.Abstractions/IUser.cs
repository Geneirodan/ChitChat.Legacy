namespace Common.Abstractions;

public interface IUser
{
    Guid? Id { get; }
    bool IsInRole(string role);
}