namespace DomusMind.Application.Features.Chats.Assistant;

public enum AssistantIntent
{
    HouseholdTimeline,
    UpcomingPlans,
    TasksDueSoon,
    Responsibilities,
    Lists
}

public interface IAssistantIntentClassifier
{
    AssistantIntent Classify(string question);
}

public interface IAssistantToolRouter
{
    Task<string> GetAuthorizedContextAsync(AssistantIntent intent, Guid familyId, Guid userId,
        CancellationToken cancellationToken);
    Task<bool> IsProposalTargetAuthorizedAsync(AssistantActionProposal proposal, Guid familyId,
        Guid userId, CancellationToken cancellationToken);
}
