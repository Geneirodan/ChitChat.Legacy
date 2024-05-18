using System.Net;
using FluentAssertions;
using JetBrains.Annotations;
using Messages.Commands.Domain;
using Messages.Commands.Integration.Tests.Extensions;
using Messages.Commands.Presentation.Endpoints;
using Messages.Commands.Presentation.Requests;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Messages.Commands.Integration.Tests.Endpoints;

[TestSubject(typeof(MessageRouteGroup))]
public class MessageRouteGroupTest : IntegrationTest
{
    protected override string Url => $"{base.Url}/messages";


    [Theory, MemberData(nameof(AddMessageTestData))]
    internal async Task AddMessage(AddMessageRequest request, HttpStatusCode expectedStatusCode)
    {
        var response = await TestClient.PostAsync(Url, request.ToJson());

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        if(expectedStatusCode == HttpStatusCode.Created)
        {
            var model = await response.AsContent<MessageViewModel>();
            model.Should().NotBeNull();
            model?.Content.Should().BeEquivalentTo(request.Content);
            model?.SendTime.Should().Be(request.Timestamp);
            model?.IsRead.Should().BeFalse();
        }
    }

    public static TheoryData<AddMessageRequest, HttpStatusCode> AddMessageTestData => new()
    {
        { new AddMessageRequest("", DateTime.Now, Guid.NewGuid()), HttpStatusCode.Created }
    };
}