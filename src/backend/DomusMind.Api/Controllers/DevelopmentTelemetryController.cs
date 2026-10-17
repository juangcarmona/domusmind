using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Route("api/system/telemetry")]
public sealed class DevelopmentTelemetryController(
    IConfiguration configuration,
    IHostEnvironment environment,
    IHttpClientFactory httpClientFactory) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("outbound-probe")]
    public async Task<IActionResult> OutboundProbe(CancellationToken cancellationToken)
    {
        if (!environment.IsDevelopment() ||
            !Uri.TryCreate(
                configuration["DevelopmentTelemetry:OutboundProbeUrl"],
                UriKind.Absolute,
                out var target))
        {
            return NotFound();
        }

        if (target.Scheme != Uri.UriSchemeHttp || !target.IsLoopback)
        {
            return NotFound();
        }

        using var response = await httpClientFactory
            .CreateClient("DevelopmentTelemetryProbe")
            .GetAsync(target, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        return Ok(new { outboundStatusCode = (int)response.StatusCode });
    }
}
