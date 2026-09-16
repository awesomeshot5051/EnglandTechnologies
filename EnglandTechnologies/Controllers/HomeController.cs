using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EnglandTechnologies.Models;

namespace EnglandTechnologies.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Docs");
    }

    public IActionResult Home()
    {
        return RedirectToAction("Index", "Docs");
    }
    [Route("/Home/print")]
    public IActionResult Print() => View();
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}