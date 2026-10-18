namespace DomusMind.Application.Features.Chats.Assistant;

public enum AssistantIntent
{
    HouseholdTimeline,
    UpcomingPlans,
    TasksDueSoon,
    Responsibilities
}

public interface IAssistantIntentClassifier
{
    AssistantIntent Classify(string question);
}

public interface IAssistantToolRouter
{
    Task<string> GetAuthorizedContextAsync(AssistantIntent intent, Guid familyId, Guid userId,
        CancellationToken cancellationToken);
}

public interface IChatAssistant
{
    Task<string> AnswerAsync(string question, string authorizedContext, CancellationToken cancellationToken);
}

public sealed class ChatAssistantUnavailableException(string message, Exception? innerException = null)
    : Exception(message, innerException);
