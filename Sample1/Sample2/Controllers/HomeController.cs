using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System;
using Sample2.Models;  // Import your models including ErrorViewModel and SongViewModel
using Microsoft.Extensions.Hosting;  // For IWebHostEnvironment

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _env;

    // Inject ILogger and IWebHostEnvironment
    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    // Index method with Get and Post handlers for the form submission
    [HttpGet]
    public IActionResult Index()
    {
        // Show the initial form for input
        return View(new SongViewModel());
    }

    [HttpPost]
    public IActionResult Index(SongViewModel model)
    {
        // Check if the model state is valid
        if (!ModelState.IsValid)
        {
            // If there are validation errors, return the view to show errors
            return View(model);
        }

        // Logic for selecting random songs after validation succeeds
        var songFilePath = Path.Combine(_env.ContentRootPath, "App_Data", "TSwift.txt");
        var allSongs = System.IO.File.ReadAllLines(songFilePath).ToList();

        // Check if enough songs exist in the file
        if (model.NumberOfSongs > allSongs.Count)
        {
            ModelState.AddModelError("", "Not enough songs available in the file.");
            return View(model);  // Return the view with error if not enough songs
        }

        // Shuffle and select the requested number of songs
        var random = new Random();
        model.SelectedSongs = allSongs.OrderBy(x => random.Next()).Take(model.NumberOfSongs).ToList();

        // Pass the updated model (with the selected songs) back to the view
        return View(model);
    }


    // Privacy page
    public IActionResult Privacy()
    {
        return View();
    }

    // Error handling method
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
