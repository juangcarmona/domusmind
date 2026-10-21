using DomusMind.Application.Abstractions.Integrations.Mcp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DomusMind.Infrastructure.Mcp;

public sealed class McpConfirmedWriteCleanupService(
    IServiceScopeFactory scopeFactory,
    ILogger<McpConfirmedWriteCleanupService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                await scope.ServiceProvider
                    .GetRequiredService<IMcpConfirmedWriteMaintenance>()
                    .PurgeExpiredAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception)
            {
                logger.LogWarning(
                    "MCP confirmed-write retention cleanup failed; it will be retried on the next cycle.");
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
