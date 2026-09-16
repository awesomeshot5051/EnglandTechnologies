namespace EnglandTechnologies.Models;

public class ServerMetrics
{
    public required string Hostname      { get; set; }
    public required string Label         { get; set; }
    public DateTime        CollectedAt   { get; set; } = DateTime.UtcNow;
    public string?         Uptime        { get; set; }
    public double?         CpuUsage      { get; set; }
    public double?         CpuTemp       { get; set; }
    public long?           RamUsedMiB    { get; set; }
    public long?           RamTotalMiB   { get; set; }
    public double?         RamUsagePct   { get; set; }
    public List<DiskMetric> Disks { get; set; } = [];
}

public class DiskMetric
{
    public string?  Device     { get; set; }
    public double?  UsedGiB    { get; set; }
    public double?  TotalGiB   { get; set; }
    public double?  UsagePct   { get; set; }
}
