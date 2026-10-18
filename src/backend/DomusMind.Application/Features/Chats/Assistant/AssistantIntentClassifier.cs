namespace DomusMind.Application.Features.Chats.Assistant;

public sealed class AssistantIntentClassifier : IAssistantIntentClassifier
{
    public AssistantIntent Classify(string question)
    {
        var text = question.ToLowerInvariant();
        if (text.Contains("responsibil") || text.Contains("owner") || text.Contains("area"))
            return AssistantIntent.Responsibilities;
        if (text.Contains("task") || text.Contains("due") || text.Contains("deadline") || text.Contains("overdue"))
            return AssistantIntent.TasksDueSoon;
        if (text.Contains("timeline") || text.Contains("everything") || text.Contains("what happened"))
            return AssistantIntent.HouseholdTimeline;
        return AssistantIntent.UpcomingPlans;
    }
}
