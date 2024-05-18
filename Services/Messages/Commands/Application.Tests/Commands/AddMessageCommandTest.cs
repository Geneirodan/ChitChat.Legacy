using Common.Abstractions;
using Common.Results;
using FluentAssertions;
using FluentResults;
using JetBrains.Annotations;
using MediatR;
using Messages.Commands.Application.Commands;
using Messages.Commands.Application.Interfaces;
using Messages.Commands.Domain;
using Moq;
using Xunit;

namespace Messages.Commands.Application.Tests.Commands;

[TestSubject(typeof(AddMessageCommand))]
public class AddMessageCommandTest
{
    private readonly AddMessageCommand.Handler _handler;
    private readonly AddMessageCommand.Validator _validator;
    private readonly Mock<IMessageRepository> _repository = new();
    private readonly Mock<IUser> _user = new();
    private readonly Mock<IPublisher> _publisher = new();

    public AddMessageCommandTest()
    {
        _handler = new AddMessageCommand.Handler(_repository.Object, _user.Object, _publisher.Object);
        _validator = new AddMessageCommand.Validator();
    }

    [Theory, MemberData(nameof(HandlerTestData))]
    internal async Task Handle(AddMessageCommand command, Result<MessageViewModel?> expectedResult, Guid? userId)
    {
        _user.Setup(x => x.Id)
            .Returns(userId);
        _repository.Setup(x => x.GetModelByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync(expectedResult.ValueOrDefault);

        var result = await _handler.Handle(command, default);

        result.IsSuccess.Should().Be(expectedResult.IsSuccess);

        var times = expectedResult.IsSuccess ? Times.Once() : Times.Never();
        _repository.Verify(x => x.AddAsync(It.IsAny<Message>(), default), times);
        _repository.Verify(x => x.SaveChangesAsync(default), times);
        _publisher.Verify(x => x.Publish(It.IsAny<Message.CreatedEvent>(), default), times);

        if (expectedResult.IsSuccess) 
            result.ValueOrDefault.Should().BeEquivalentTo(expectedResult.ValueOrDefault);
    }

    public static TheoryData<AddMessageCommand, Result<MessageViewModel?>, Guid?> HandlerTestData =>
        new()
        {
            {
                new AddMessageCommand("somestring", DateTime.Today, Guid.NewGuid()),
                Result.Ok(new MessageViewModel("somestring", false, DateTime.Today))!,
                Guid.NewGuid()
            },
            {
                new AddMessageCommand("somestring", DateTime.Today, Guid.NewGuid()),
                ErrorResults.Unauthorized(),
                null
            },
            {
                new AddMessageCommand(string.Empty, DateTime.Today, Guid.NewGuid()),
                Result.Ok(new MessageViewModel(string.Empty, false, DateTime.Today))!,
                Guid.NewGuid()
            },
        };

    [Theory, MemberData(nameof(ValidatorTestData))]
    internal async Task Validator(AddMessageCommand command, IEnumerable<string> expectedErrors)
    {
        var result = await _validator.ValidateAsync(command);
        result.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(expectedErrors);
    }

    public static TheoryData<AddMessageCommand, IEnumerable<string>> ValidatorTestData => new()
    {
        {
            new AddMessageCommand("somestring", DateTime.UtcNow, Guid.NewGuid()),
            []
        },
        {
            new AddMessageCommand(string.Empty, DateTime.UtcNow, Guid.NewGuid()),
            ["'Content' must not be empty."]
        },
        {
            new AddMessageCommand("somestring", DateTime.MinValue, Guid.NewGuid()),
            ["'Timestamp' must not be empty."]
        },
        {
            new AddMessageCommand("somestring", DateTime.UtcNow, Guid.Empty),
            ["'Receiver Id' must not be empty."]
        },
        {
            new AddMessageCommand(string.Empty, DateTime.MinValue, Guid.NewGuid()),
            ["'Content' must not be empty.", "'Timestamp' must not be empty."]
        },
        {
            new AddMessageCommand("somestring", DateTime.MinValue, Guid.Empty),
            ["'Timestamp' must not be empty.", "'Receiver Id' must not be empty."]
        },
        {
            new AddMessageCommand(string.Empty, DateTime.UtcNow, Guid.Empty),
            ["'Content' must not be empty.", "'Receiver Id' must not be empty."]
        },
        {
            new AddMessageCommand(string.Empty, DateTime.MinValue, Guid.Empty),
            ["'Content' must not be empty.", "'Timestamp' must not be empty.", "'Receiver Id' must not be empty."]
        }
    };
}