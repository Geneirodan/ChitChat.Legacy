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

[TestSubject(typeof(EditMessageCommand))]
public class EditMessageCommandTest
{
    private readonly EditMessageCommand.Handler _handler;
    private readonly EditMessageCommand.Validator _validator;
    private readonly Mock<IMessageRepository> _repository = new();
    private readonly Mock<IUser> _user = new();
    private readonly Mock<IPublisher> _publisher = new();

    public EditMessageCommandTest()
    {
        _handler = new EditMessageCommand.Handler(_repository.Object, _user.Object, _publisher.Object);
        _validator = new EditMessageCommand.Validator();
    }

    [Theory, MemberData(nameof(HandlerTestData))]
    public async Task Handle(EditMessageCommand command, Result expectedResult, Guid? userId)
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
        _publisher.Verify(x => x.Publish(It.IsAny<Message.EditedEvent>(), default), times);
    }

    public static TheoryData<EditMessageCommand, Result, Guid?> HandlerTestData =>
        new()
        {
            {
                new EditMessageCommand(TestData.ValidIds[0], string.Empty),
                Result.Success(), TestData.ValidMessages[0].SenderId
            },
            {
                new EditMessageCommand(TestData.ValidIds[0], string.Empty),
                Result.Forbidden(), TestData.ValidMessages[0].ReceiverId
            },
            {
                new EditMessageCommand(TestData.ValidIds[0], string.Empty),
                Result.Forbidden(), Guid.NewGuid()
            },
            {
                new EditMessageCommand(Guid.NewGuid(), string.Empty),
                Result.Unauthorized(), null
            },
            {
                new EditMessageCommand(Guid.NewGuid(), string.Empty),
                Result.NotFound(), Guid.NewGuid()
            },
        };

    [Theory, MemberData(nameof(ValidatorTestData))]
    public async Task Validator(EditMessageCommand command, IEnumerable<string> expectedErrors)
    {
        var result = await _validator.ValidateAsync(command);
        result.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(expectedErrors);
    }

    public static TheoryData<EditMessageCommand, IEnumerable<string>> ValidatorTestData => new()
    {
        {
            new EditMessageCommand(Guid.NewGuid(), "SomeContent"),
            []
        },
        {
            new EditMessageCommand(Guid.NewGuid(), string.Empty),
            ["'Content' must not be empty."]
        },
        {
            new EditMessageCommand(Guid.NewGuid(), new string('a', 2049)),
            ["The length of 'Content' must be 2048 characters or fewer. You entered 2049 characters."]
        }
    };
}