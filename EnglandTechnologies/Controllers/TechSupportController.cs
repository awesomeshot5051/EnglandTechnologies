using EnglandTechnologies.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnglandTechnologies.Controllers;

public class TechSupportController : Controller
{
    private readonly IConfiguration _config;

    public TechSupportController(IConfiguration config) => _config = config;

    public IActionResult Index() => View();

    public IActionResult About() => View();

    public IActionResult ContactUs() => View();

    public IActionResult Services() => View();

    public IActionResult Legal() => View();

    // Privacy and Terms now share one collapsible page; old links keep working.
    public IActionResult Privacy() => RedirectPermanent("/legal#privacy");

    public IActionResult Terms() => RedirectPermanent("/legal#terms");

    // Scheduling is handled by Easy!Appointments; this page just frames it.
    // ?service=<slug> preselects a service. Easy!Appointments skips straight to the
    // date/time step when both service and provider are given in the URL.
    public IActionResult Contact(string? service)
    {
        var url = _config["Booking:Url"];
        if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(service))
        {
            // Only known slugs from config reach the iframe URL.
            var serviceId = _config[$"Booking:ServiceIds:{service}"];
            if (!string.IsNullOrEmpty(serviceId))
            {
                url = $"{url.TrimEnd('/')}/?service={Uri.EscapeDataString(serviceId)}";
                var providerId = _config["Booking:ProviderId"];
                if (!string.IsNullOrEmpty(providerId))
                    url += $"&provider={Uri.EscapeDataString(providerId)}";
                ViewData["SelectedService"] = ServiceCatalog.Find(service)?.Title;
            }
        }
        // No service picked: still preselect the provider so the scheduler has nothing else to resolve.
        if (!string.IsNullOrWhiteSpace(url) && ViewData["SelectedService"] is null)
        {
            var providerId = _config["Booking:ProviderId"];
            if (!string.IsNullOrEmpty(providerId))
                url = $"{url.TrimEnd('/')}/?provider={Uri.EscapeDataString(providerId)}";
        }
        ViewData["BookingUrl"] = url;
        return View();
    }
}
