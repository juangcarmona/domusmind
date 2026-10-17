using DomusMind.Api.Controllers;
using DomusMind.Api.Realtime;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Lists;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Tests.Realtime;

public sealed class ListChangeNotifierTests
{
    [Fact]
    public async Task Publish_NotifiesOnlySubscribersForThatList()
    {
        var notifier = new ListChangeNotifier();
        var listId = Guid.NewGuid();
        using var subscription = notifier.Subscribe(listId);
        using var otherList = notifier.Subscribe(Guid.NewGuid());

        notifier.Publish(listId);

        Assert.True(await subscription.Changes.WaitToReadAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(1)));
        Assert.True(subscription.Changes.TryRead(out _));
        Assert.False(otherList.Changes.TryRead(out _));
    }

    [Fact]
    public async Task StreamListChanges_WhenFamilyAccessIsDenied_ReturnsForbiddenBeforeStreaming()
    {
        var context = new DefaultHttpContext();
        var controller = new ListsController(
            new StubCurrentUser(Guid.NewGuid()),
            new ListChangeNotifier())
        {
            ControllerContext = new ControllerContext { HttpContext = context },
        };

        await controller.StreamListChanges(
            Guid.NewGuid(),
            new DenyingQueryDispatcher(),
            CancellationToken.None);

        Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
        Assert.Equal(0, context.Response.Body.Length);
    }

    private sealed class DenyingQueryDispatcher : IQueryDispatcher
    {
        public Task<TResponse> Dispatch<TResponse>(
            IQuery<TResponse> query,
            CancellationToken cancellationToken = default)
            => Task.FromException<TResponse>(
                new ListException(ListErrorCode.AccessDenied, "Access denied."));
    }

    private sealed class StubCurrentUser(Guid userId) : ICurrentUser
    {
        public Guid? UserId => userId;
        public string? Email => null;
        public bool IsAuthenticated => true;
        public IReadOnlyCollection<string> Roles => [];
    }
}
