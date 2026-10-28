using DomusMind.Api.OpenApi;
using DomusMind.Application.DependencyInjection;
using DomusMind.Infrastructure.Auth;
using DomusMind.Infrastructure.DependencyInjection;
using DomusMind.Infrastructure.Languages;
using DomusMind.Infrastructure.Messaging;
using DomusMind.Infrastructure.Persistence;
using DomusMind.Infrastructure.Integrations.Calendar.Microsoft;
using DomusMind.Api.Realtime;
using DomusMind.Api.Telemetry;
using DomusMind.Api.Auth;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

var reverseProxySettings = builder.Configuration.GetSection("ReverseProxy");
var knownProxyAddresses = ParseList(reverseProxySettings["KnownProxies"])
    .Select(value => IPAddress.TryParse(value, out var address)
        ? address
        : throw new InvalidOperationException($"ReverseProxy:KnownProxies contains invalid IP address '{value}'."))
    .ToArray();
var knownProxyNetworks = ParseList(reverseProxySettings["KnownIPNetworks"])
    .Select(value => System.Net.IPNetwork.TryParse(value, out var network)
        ? network
        : throw new InvalidOperationException($"ReverseProxy:KnownIPNetworks contains invalid CIDR network '{value}'."))
    .ToArray();
var allowedForwardedHosts = ParseList(reverseProxySettings["AllowedHosts"]);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Clear();
    options.KnownIPNetworks.Clear();
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost;
    options.ForwardLimit = 1;
    foreach (var address in knownProxyAddresses)
    {
        options.KnownProxies.Add(address);
    }

    foreach (var network in knownProxyNetworks)
    {
        options.KnownIPNetworks.Add(network);
    }

    foreach (var host in allowedForwardedHosts)
    {
        options.AllowedHosts.Add(host);
    }
});

var swaggerEnabled = builder.Configuration.GetValue<bool?>("Swagger:Enabled")
    ?? builder.Environment.IsDevelopment();

builder.Services.AddControllers();
builder.Services.AddSingleton<IListChangeNotifier, ListChangeNotifier>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDomusMindAuthentication(builder.Configuration);
var developmentBff = BffAuthenticationExtensions.IsEnabled(builder.Environment, builder.Configuration);
if (developmentBff)
    builder.Services.AddDevelopmentBff(builder.Configuration);
builder.Services.AddDomusMindAuthorization();
builder.Services.AddDomusMindOpenApi();
builder.AddDevelopmentTelemetry();

var app = builder.Build();

// Validate provider secrets before database migrations can modify existing token material.
_ = app.Services.GetRequiredService<IOptions<MicrosoftGraphOptions>>().Value;
app.Logger.LogInformation(
    "DomusMind version {Version} (commit {Commit}) starting",
    builder.Configuration["DOMUSMIND_VERSION"] ?? "unknown",
    builder.Configuration["DOMUSMIND_COMMIT"] ?? "unknown");

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionFeature?.Error;
        var logger = context.RequestServices
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("GlobalExceptionHandler");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        if (exception is DbUpdateConcurrencyException)
        {
            logger.LogInformation("Concurrent update rejected for a versioned resource");
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new
            {
                code = "concurrency_conflict",
                error = "This list changed in another session. Refresh and retry.",
                traceId = context.TraceIdentifier,
            });
            return;
        }

        if (exception is HandlerResolutionException resolutionException)
        {
            logger.LogError(
                resolutionException,
                "Dispatcher handler resolution failed for {HandlerType}",
                resolutionException.HandlerType);

            await context.Response.WriteAsJsonAsync(new
            {
                code = "handler_resolution_failed",
                message = "An internal handler registration is missing.",
                traceId = context.TraceIdentifier,
            });
            return;
        }

        logger.LogError(exception, "Unhandled server exception");
        await context.Response.WriteAsJsonAsync(new
        {
            code = "internal_server_error",
            message = "An unexpected error occurred.",
            traceId = context.TraceIdentifier,
        });
    });
});

app.UseForwardedHeaders();

if (!developmentBff)
    app.UseHttpsRedirection();

if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
if (developmentBff)
    app.UseMiddleware<BffRequestBoundary>();
app.UseAuthorization();

var webRootPath = app.Environment.WebRootPath ?? Path.Combine(app.Environment.ContentRootPath, "wwwroot");
if (Directory.Exists(webRootPath))
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

app.MapControllers();
var spaIndexPath = Path.Combine(webRootPath, "index.html");
if (File.Exists(spaIndexPath))
{
    app.MapFallback(async context =>
    {
        if (context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase) ||
            context.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.SendFileAsync(spaIndexPath);
    });
}
else
{
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DomusMindDbContext>();
    await dbContext.Database.MigrateAsync();
}
await AuthSeedService.SeedAdminAsync(app.Services, CancellationToken.None);
if (developmentBff)
    await DevelopmentBffTestUserSeeder.SeedAsync(app.Services, app.Configuration, CancellationToken.None);
await LanguageSeedService.SeedAsync(app.Services, CancellationToken.None);

app.Run();

static string[] ParseList(string? values) => string.IsNullOrWhiteSpace(values)
    ? []
    : values.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
