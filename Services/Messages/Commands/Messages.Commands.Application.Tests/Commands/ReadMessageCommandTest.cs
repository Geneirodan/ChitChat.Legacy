using Ardalis.Result;
using Common.Abstractions;
using FluentAssertions;
using JetBrains.Annotations;
using MediatR;
using Messages.Commands.Application.Commands;
using Messages.Commands.Application.Interfaces;
using Messages.Commands.Domain;
using Moq;
using Xunit;

namespace Messages.Commands.Application.Tests.Commands;

[TestSubject(typeof(ReadMessageCommand))]
public class ReadMessageCommandTest
{
    private readonly ReadMessageCommand.Handler _handler;
    private readonly Mock<IMessageRepository> _repository = new();
    private readonly Mock<IUser> _user = new();
    private readonly Mock<IPublisher> _publisher = new();

    public ReadMessageCommandTest() =>
        _handler = new ReadMessageCommand.Handler(_repository.Object, _user.Object, _publisher.Object);

    [Theory, MemberData(nameof(HandlerTestData))]
    public async Task Handle(ReadMessageCommand command, Result expectedResult, Guid? userId)
    {
        _user.Setup(x => x.Id)
            .Returns(userId);
        _repository.Setup(x => x.FindAsync(It.Is<GetByIdSpecification<Message>>(s => s.Id == command.Id), default))
            .ReturnsAsync(TestData.ValidMessages.FirstOrDefault(message => message.Id == command.Id));

        var result = await _handler.Handle(command, default);

        result.IsSuccess.Should().Be(expectedResult.IsSuccess);

        var times = expectedResult.IsSuccess ? Times.Once() : Times.Never();
        _repository.Verify(x => x.UpdateAsync(It.IsAny<Message>(), default), times);
        _repository.Verify(x => x.SaveChangesAsync(default), times);
        _publisher.Verify(x => x.Publish(It.IsAny<Message.ReadEvent>(), default), times);
    }

    public static TheoryData<ReadMessageCommand, Result, Guid?> HandlerTestData =>
        new()
        {
            {
                new ReadMessageCommand(TestData.ValidIds[0]),
                Result.Forbidden(), TestData.ValidMessages[0].SenderId
            },
            {
                new ReadMessageCommand(TestData.ValidIds[0]),
                Result.Success(), TestData.ValidMessages[0].ReceiverId
            },
            {
                new ReadMessageCommand(TestData.ValidIds[0]),
                Result.Forbidden(), Guid.NewGuid()
            },
            {
                new ReadMessageCommand(Guid.NewGuid()),
                Result.Unauthorized(), null
            },
            {
                new ReadMessageCommand(Guid.NewGuid()),
                Result.NotFound(), Guid.NewGuid()
            },
        };
}