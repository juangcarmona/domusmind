using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Chats.Assistant;
using DomusMind.Contracts.Chats;
using DomusMind.Domain.Chats;
using DomusMind.Domain.Chats.ValueObjects;
using DomusMind.Application.Temporal;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Chats.AnswerAssistant;

public sealed class AnswerAssistantQuestionHandler(IDomusMindDbContext db, IEventLogWriter events,
    IFamilyAuthorizationService auth, IAssistantIntentClassifier classifier,
    IAssistantToolRouter tools, IChatAssistant assistant)
    : ICommandHandler<AnswerAssistantQuestionCommand, ChatMessageModel>
{
    public async Task<ChatMessageModel> Handle(AnswerAssistantQuestionCommand command, CancellationToken ct)
    {
        var message = await db.Set<ChatMessage>().AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == ChatMessageId.From(command.ChatMessageId), ct)
            ?? throw new KeyNotFoundException("Chat message was not found.");
        if (message.AuthorType != "Member" || !ChatAssistantInvocation.IsExplicit(message.Text))
            throw new InvalidOperationException("Only a member message explicitly addressed to @dm can be answered.");
        if (!await auth.CanAccessFamilyAsync(command.UserId, message.FamilyId.Value, ct))
            throw new UnauthorizedAccessException();

        var thread = await db.Set<ChatThread>().AsNoTracking().Include(x => x.Participants)
            .SingleAsync(x => x.Id == ChatThreadId.From(message.ChatThreadId), ct);
        var family = await db.Set<Domain.Family.Family>().AsNoTracking().Include(x => x.Members)
            .SingleAsync(x => x.Id == message.FamilyId, ct);
        var member = family.Members.SingleOrDefault(x => x.AuthUserId == command.UserId);
        if (member is null || !thread.Participants.Any(x => x.MemberId == member.Id.Value))
            throw new UnauthorizedAccessException();

        var existing = await db.Set<ChatMessage>().AsNoTracking()
            .SingleOrDefaultAsync(x => x.ReplyToMessageId == message.Id.Value, ct);
        if (existing is not null)
            return Map(existing);

        var question = ChatAssistantInvocation.GetQuestion(message.Text);
        if (string.IsNullOrWhiteSpace(question))
            return await SaveAnswer(message, "What would you like to know about your household?", null, null, null, ct);

        var context = await tools.GetAuthorizedContextAsync(classifier.Classify(question),
            message.FamilyId.Value, command.UserId, ct);
        var conversation = await db.Set<ChatMessage>().AsNoTracking()
            .Where(x => x.ChatThreadId == message.ChatThreadId && x.Id != message.Id)
            .OrderByDescending(x => x.CreatedAtUtc).ThenByDescending(x => x.Id)
            .Take(12).ToListAsync(ct);
        var request = new AssistantProviderRequest(question,
            conversation.OrderBy(x => x.CreatedAtUtc).ThenBy(x => x.Id)
                .Select(x => new AssistantConversationTurn(x.AuthorType == "Assistant" ? "assistant" : "user", x.Text))
                .ToArray(), context, thread.Kind == "Household");
        var result = await assistant.CompleteAsync(message.FamilyId.Value, request, ct);
        var answer = result.Completion.Answer;
        AssistantProposalDraft? proposal = null;
        if (thread.Kind == "Private" && result.Completion.Proposal is not null)
            answer = "For privacy, I can't copy a request from this private conversation into the household Inbox. Please capture it there for review.";
        else if (result.Completion.Proposal is { } candidate)
        {
            if (!HasRequiredDetails(candidate))
                answer = ClarificationFor(candidate);
            else if (!await tools.IsProposalTargetAuthorizedAsync(candidate, message.FamilyId.Value,
                         command.UserId, ct))
                answer = ClarificationForTarget(candidate);
            else
            {
                proposal = ToDomainProposal(candidate);
                answer = "I prepared a proposal in Inbox for you to review. No household record has changed.";
            }
        }
        return await SaveAnswer(message, answer, result, command.UserId, proposal, ct);
    }

    private async Task<ChatMessageModel> SaveAnswer(ChatMessage source, string text,
        AssistantProviderResult? provider, Guid? requestingUserId, AssistantProposalDraft? proposal,
        CancellationToken ct)
    {
        var response = ChatMessage.PostAssistant(ChatMessageId.New(), source.ChatThreadId,
            source.FamilyId, text, source.Id.Value, DateTime.UtcNow,
            provider?.ProviderName, provider?.Hosting, provider?.FallbackOccurred ?? false,
            requestingUserId, proposal);
        db.Set<ChatMessage>().Add(response);
        await events.WriteAsync([response], ct);
        response.ClearDomainEvents();
        return Map(response);
    }

    private static ChatMessageModel Map(ChatMessage message) => new(message.Id.Value,
        message.ChatThreadId, null, "Assistant", "DomusMind", message.Text,
        message.ReplyToMessageId, message.CreatedAtUtc, message.ProviderName,
        message.ProviderHosting, message.FallbackOccurred);

    private static bool HasRequiredDetails(AssistantActionProposal p) => p.Type switch
    {
        "CreatePlan" => !string.IsNullOrWhiteSpace(p.Title) && IsDate(p.Date) && IsDate(p.EndDate)
            && IsValidEventTime(p.Date, p.Time, p.EndDate, p.EndTime),
        "UpdatePlan" => Guid.TryParse(p.TargetId, out _) &&
            (!string.IsNullOrWhiteSpace(p.Title) || p.Description is not null
                || p.Date is not null || p.Time is not null || p.EndDate is not null || p.EndTime is not null)
            && (p.Date is null && p.EndDate is null || IsDate(p.Date) && IsDate(p.EndDate)
                && IsValidEventTime(p.Date, p.Time, p.EndDate, p.EndTime)),
        "AddListItem" => Guid.TryParse(p.TargetId, out _) && !string.IsNullOrWhiteSpace(p.Title),
        "UpdateListItem" => Guid.TryParse(p.TargetId, out _) && Guid.TryParse(p.ItemId, out _)
            && (!string.IsNullOrWhiteSpace(p.Title) || p.Quantity is not null || p.Note is not null),
        _ => false
    };

    private static bool IsDate(string? value) => DateOnly.TryParseExact(value, "yyyy-MM-dd", out _);

    private static bool IsValidEventTime(string? date, string? time, string? endDate, string? endTime)
    {
        try { _ = TemporalParser.ParseEventTime(date!, time, endDate!, endTime); return true; }
        catch (Exception ex) when (ex is FormatException or ArgumentException or InvalidOperationException) { return false; }
    }

    private static AssistantProposalDraft ToDomainProposal(AssistantActionProposal p) => new(
        p.Type, Guid.TryParse(p.TargetId, out var targetId) ? targetId : null,
        Guid.TryParse(p.ItemId, out var itemId) ? itemId : null, p.Title?.Trim(),
        p.Description, p.Date, p.Time, p.EndDate, p.EndTime, p.Quantity, p.Note);

    private static string ClarificationFor(AssistantActionProposal p) => p.Type switch
    {
        "CreatePlan" => "What date and time should I use for the plan? Please include its start and end.",
        "UpdatePlan" => "Which plan should I update, and what would you like changed?",
        "AddListItem" => "Which list should I add that item to?",
        "UpdateListItem" => "Which list item should I update, and what should change?",
        _ => "What details should I use?"
    };

    private static string ClarificationForTarget(AssistantActionProposal p) => p.Type switch
    {
        "UpdatePlan" => "I couldn't match that to a plan you can access. Which plan should I update?",
        "AddListItem" => "I couldn't match that to a list you can access. Which list should I use?",
        "UpdateListItem" => "I couldn't match that to a list item you can access. Which item should I update?",
        _ => "I couldn't match that request to household information you can access. Can you clarify?"
    };

}
