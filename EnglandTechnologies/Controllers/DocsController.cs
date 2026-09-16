using Microsoft.AspNetCore.Mvc;

namespace EnglandTechnologies.Controllers;

public class DocsController : Controller
{
    public IActionResult Index() => View(); // Points to Index.cshtml (Home Lab Overview)

    [Route("/docs/cisco-firewall-england")]
    public IActionResult CiscoFirewall() => View();

    [Route("/docs/database-server-england")]
    public IActionResult DatabaseServer() => View();

    [Route("/docs/file-server-england")]
    public IActionResult FileServer() => View();

    [Route("/docs/file-extension-england")]
    public IActionResult FileExtension() => View();

    [Route("/docs/network-england")]
    public IActionResult NetworkServer() => View();

    [Route("/docs/vpn-server-england")]
    public IActionResult VpnServer() => View();
    
    [Route("/docs/WhyOffline")]
    public IActionResult WhyOffline() => View();
    
    [Route("/docs/ETVaultGuard")]
    public IActionResult EtVaultGuard() => View();
}