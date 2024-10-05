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
        // Validate the form input
        if (ModelState.IsValid)
        {
            // Build the path to the Songs.txt file in the App_Data folder
            var songFilePath = Path.Combine(_env.ContentRootPath, "App_Data", "Songs.txt");

            if (System.IO.File.Exists(songFilePath))
            {
                // Read all song names from the text file
                var allSongs = System.IO.File.ReadAllLines(songFilePath).ToList();

                // Validate if there are enough songs in the file
                if (model.NumberOfSongs > allSongs.Count)
                {
                    ModelState.AddModelError("", "Not enough songs available in the file.");
                    return View(model);
                }

                // Pick random songs
                var random = new Random();
                model.SelectedSongs = allSongs.OrderBy(x => random.Next()).Take(model.NumberOfSongs).ToList();

                // Return the view with the list of random songs
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "The songs file could not be found.");
            }
        }

        // If there are validation errors or the file is missing, return the same view
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
