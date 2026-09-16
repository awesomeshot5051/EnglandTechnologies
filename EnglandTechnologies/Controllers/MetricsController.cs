using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using EnglandTechnologies.Hubs;
using EnglandTechnologies.Models;
using EnglandTechnologies.Services;

namespace EnglandTechnologies.Controllers;

[ApiController]
[Route("api/metrics")]
public class MetricsController : ControllerBase
{
    private readonly IHubContext<MetricsHub>    _hub;
    private readonly IConfiguration             _config;
    private readonly ILogger<MetricsController> _logger;
    private readonly MetricsStore                _store;

    public MetricsController(
        IHubContext<MetricsHub>      hub,
        IConfiguration               config,
        ILogger<MetricsController>   logger,
        MetricsStore                 store)
    {
        _hub    = hub;
        _config = config;
        _logger = logger;
        _store  = store;
    }

    /// <summary>
    /// Snapshot of the most recently received metrics for every known host.
    /// Lets a client that connects (or reconnects) after a host has gone offline
    /// still learn the real last-seen time instead of showing nothing.
    /// </summary>
    [HttpGet("latest")]
    public IActionResult GetLatest() => Ok(_store.GetAll());

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] ServerMetrics metrics)
    {
        var expectedKey = _config["MetricsApiKey"];
        if (string.IsNullOrEmpty(expectedKey))
        {
            _logger.LogError("MetricsApiKey is not configured.");
            return StatusCode(500, "Server misconfiguration.");
        }

        if (!Request.Headers.TryGetValue("X-Metrics-Key", out var incomingKey)
            || incomingKey != expectedKey)
        {
            _logger.LogWarning("Rejected metrics POST from {IP} — bad API key.",
                HttpContext.Connection.RemoteIpAddress);
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(metrics.Hostname))
            return BadRequest("Hostname is required.");

        metrics.CollectedAt = DateTime.UtcNow;
        _store.Update(metrics);

        await _hub.Clients.All.SendAsync("ReceiveMetrics", metrics);

        _logger.LogInformation("Metrics received and broadcast for [{Host}]", metrics.Hostname);
        return Ok();
    }
}
