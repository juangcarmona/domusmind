using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace DomusMind.Api.Telemetry;

internal static class OpenTelemetryConfiguration
{
    public static void AddDevelopmentTelemetry(this WebApplicationBuilder builder)
    {
        if (!builder.Environment.IsDevelopment() ||
            string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]))
        {
            return;
        }

        builder.Logging.AddFilter("System.Net.Http", LogLevel.None);
        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation())
            .WithTracing(tracing => tracing
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation(options =>
                    options.EnrichWithHttpRequest = static (activity, _) =>
                    {
                        activity.SetTag("url.path", null);
                        activity.SetTag("url.query", null);
                        activity.SetTag("http.target", null);
                    })
                .AddHttpClientInstrumentation(options =>
                    options.EnrichWithHttpRequestMessage = static (activity, request) =>
                    {
                        activity.DisplayName = $"HTTP {request.Method.Method} {request.RequestUri?.Host ?? "outbound"}";
                        activity.SetTag("url.full", null);
                        activity.SetTag("url.path", null);
                        activity.SetTag("url.query", null);
                        activity.SetTag("http.url", null);
                        activity.SetTag("http.target", null);
                    }));

        builder.Services.AddOpenTelemetry().UseOtlpExporter();
    }
}
