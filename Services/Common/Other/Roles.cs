namespace Common.Other;

public static class Roles
{
    public const string Admin = nameof(Admin);
    public const string User = nameof(User);

    public static IEnumerable<string> All => [Admin, User];
}