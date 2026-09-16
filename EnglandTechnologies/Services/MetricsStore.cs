using System.Collections.Concurrent;
using EnglandTechnologies.Models;

namespace EnglandTechnologies.Services;

/// <summary>
/// Keeps the most recently received ServerMetrics per host in memory so clients
/// that connect after a host has gone offline can still learn when it was last seen.
/// </summary>
public class MetricsStore
{
    private readonly ConcurrentDictionary<string, ServerMetrics> _latestByHost = new();

    public void Update(ServerMetrics metrics) => _latestByHost[metrics.Hostname] = metrics;

    public IReadOnlyCollection<ServerMetrics> GetAll() => _latestByHost.Values.ToList();
}
