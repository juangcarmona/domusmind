using DomusMind.Application.Abstractions.Languages;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Integrations.Calendar;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Infrastructure.Auth;
using DomusMind.Infrastructure.BackgroundJobs.Calendar;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Integrations.Calendar.Microsoft;
using DomusMind.Infrastructure.Languages;
using DomusMind.Infrastructure.Messaging;
using DomusMind.Infrastructure.Persistence;
using DomusMind.Application.Features.Inbox;
using DomusMind.Application.Features.Chats.Assistant;
using DomusMind.Infrastructure.Chats;
using DomusMind.Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DomusMind.Infrastructure.DependencyInjection;

public static class InfrastructureServices
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DomusMindDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("domusmind")));

        services.AddScoped<IDomusMindDbContext>(sp =>
            sp.GetRequiredService<DomusMindDbContext>());

        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IEventLogWriter, EventLogWriter>();
        services.AddHttpClient(nameof(OllamaInboxInterpreter)).ConfigureHttpClient(client =>
            client.Timeout = TimeSpan.FromMinutes(2));
        services.AddHttpClient(nameof(OpenAiCompatibleInboxInterpreter)).ConfigureHttpClient(client =>
            client.Timeout = TimeSpan.FromMinutes(2));
        services.AddHttpClient(nameof(OllamaChatAssistant)).ConfigureHttpClient(client =>
            client.Timeout = TimeSpan.FromMinutes(2));
        services.AddHttpClient(nameof(OpenAiCompatibleChatAssistant)).ConfigureHttpClient(client =>
            client.Timeout = TimeSpan.FromMinutes(2));
        var dataProtection = services.AddDataProtection();
        var keyPath = configuration["DataProtection:KeysPath"];
        if (!string.IsNullOrWhiteSpace(keyPath))
            dataProtection.PersistKeysToFileSystem(new DirectoryInfo(keyPath));
        services.AddScoped<IAssistantProviderRouteStore, AssistantProviderRouteStore>();
        services.AddScoped<IInboxInterpreterProvider, OllamaInboxInterpreter>();
        services.AddScoped<IInboxInterpreterProvider, OpenAiCompatibleInboxInterpreter>();
        services.AddScoped<IChatAssistantProvider, OllamaChatAssistant>();
        services.AddScoped<IChatAssistantProvider, OpenAiCompatibleChatAssistant>();

        services.AddScoped<ISupportedLanguageReader, SupportedLanguageReader>();

        // External calendar integrations
        services.AddOptions<MicrosoftGraphOptions>()
            .Bind(configuration.GetSection(MicrosoftGraphOptions.SectionName))
            .Validate(
                options => options.Validate() is null,
                "MicrosoftGraph requires both credentials, an allowed callback URI, and a Base64 encryption key containing exactly 32 bytes.")
            .ValidateOnStart();
        services.AddScoped<IExternalCalendarAuthService, MicrosoftGraphCalendarAuthService>();
        services.AddScoped<IExternalCalendarProviderClient, MicrosoftGraphCalendarClient>();
        services.AddScoped<IExternalCalendarSyncLeaseService, ExternalCalendarConnectionLeaseService>();
        services.AddScoped<ICalendarSubscriptionTokenGenerator, CalendarSubscriptionTokenGenerator>();
        services.AddSingleton<TimeProvider>(TimeProvider.System);
        services.AddScoped<ICalendarSubscriptionTokenGenerator, CalendarSubscriptionTokenGenerator>();
        services.AddSingleton(TimeProvider.System);
        services.AddHttpClient("MicrosoftGraph")
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://graph.microsoft.com/"));

        // Background worker
        services.Configure<ExternalCalendarRefreshOptions>(
            configuration.GetSection(ExternalCalendarRefreshOptions.SectionName));
        services.AddHostedService<ExternalCalendarRefreshWorker>();

        return services;
    }
}
