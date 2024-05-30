using Messages.Commands.Domain;

namespace Messages.Commands.Application.Tests;

public static class TestData
{
    internal static readonly Guid[] ValidIds =
    [
        new Guid("44c726d6-0758-42d3-ab04-1526b7de55f2"),
        new Guid("5c3467f1-d2f3-4cb9-9e4c-22819010b784"),
        new Guid("a844e0ff-1585-4a5f-a2f6-6bea96afe6cb"),
        new Guid("6f05babc-1aae-45ca-9e91-fd65310594ad"),
        new Guid("c829af4c-3227-48e1-a28c-b61cd9855134"),
        new Guid("17144647-0abf-4fa2-b663-799cce41cb8e"),
        new Guid("7f41d545-f40c-4370-a81c-eb979d3f54e4"),
        new Guid("5e9d1f21-f77d-4027-bb0e-6d260eaa462f"),
        new Guid("6d3c90d6-423d-4aaf-aa80-ff3ba0f15ae2"), 
        new Guid("82305454-671c-49b9-a084-6ae3d966318c")
    ];

    internal static readonly Message[] ValidMessages = ValidIds.Select(x =>
        Message.CreateInstance(x, string.Empty, DateTime.Now, Guid.NewGuid(), Guid.NewGuid()).message).ToArray();
}