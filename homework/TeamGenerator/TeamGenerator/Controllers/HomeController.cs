using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TeamGenerator.Models;
using TeamGenerator.ViewModels;

namespace TeamGenerator.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        TeamGeneratorVM vm = new TeamGeneratorVM();
        return View(vm);
    }

    [HttpGet]
    public IActionResult Teams()
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Teams(TeamGeneratorVM vm)
    {
        if(!ModelState.IsValid)
        {
            return View("Index", vm);
        }
        Teams teams = new Teams(vm.Names, vm.TeamSize);
        return View(teams);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
