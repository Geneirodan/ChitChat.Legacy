using Messages.Commands.Domain;

namespace Messages.Commands.Application.Tests;

public static class TestData
{
    internal static readonly Guid[] ValidIds = Enumerable.Range(0, 10).Select(_ => Guid.NewGuid()).ToArray();

    internal static readonly Message[] ValidMessages = ValidIds.Select(x =>
        Message.CreateInstance(x, string.Empty, DateTime.Now, Guid.NewGuid(), Guid.NewGuid()).message).ToArray();
}