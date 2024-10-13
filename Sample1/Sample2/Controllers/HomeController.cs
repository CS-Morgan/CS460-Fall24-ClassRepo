using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System;
using Sample2.Models;  // Import your models including ErrorViewModel and SongViewModel
using Microsoft.Extensions.Hosting;  // For IWebHostEnvironment
using System.Text.RegularExpressions;

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

    // GET: Index method to display the input form
    [HttpGet]
    public IActionResult Index()
    {
        return View(new SongViewModel());  // Return a new instance of the model
    }

    // POST: Teams method to handle form submission
    [HttpPost]
    public IActionResult Teams(SongViewModel model, string userNames)
    {
        // Split the multiline input into individual names
        var splitNames = userNames.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        model.UserNames = new List<string>();

        // Validate user names using regex
        string userNamePattern = @"^[a-zA-Z\s\.\-_']+$";
        foreach (var name in splitNames)
        {
            if (!Regex.IsMatch(name, userNamePattern))
            {
                ModelState.AddModelError("UserNames", "Only letters, spaces, and , . - _ ' are allowed.");
            }
            else
            {
                model.UserNames.Add(name.Trim());
            }
        }

        // Validate the number of teams
        if (model.NumberOfTeams < 2 || model.NumberOfTeams > 10)
        {
            ModelState.AddModelError("NumberOfTeams", "Please enter a valid number of teams between 2 and 10.");
        }

        // Check if the model state is valid
        if (!ModelState.IsValid)
        {
            return View("Index", model);  // Return to the Index view with the invalid model
        }

        // Logic for assigning teams
        var songFilePath = Path.Combine(_env.ContentRootPath, "App_Data", "TSwift.txt");
        var allSongs = System.IO.File.ReadAllLines(songFilePath).ToList();

        // Check if there are enough songs for the number of teams requested
        if (model.NumberOfTeams > allSongs.Count)
        {
            ModelState.AddModelError("", "Not enough songs available for team names.");
            return View("Index", model);  // Return to the Index view if not enough songs
        }

        // Shuffle the user names and assign them to teams
        var random = new Random();
        var shuffledUsers = model.UserNames.OrderBy(x => random.Next()).ToList();
        var teamSize = (int)Math.Ceiling((double)shuffledUsers.Count / model.NumberOfTeams);

        // Shuffle and select random team names from the songs list
        var teamNames = allSongs.OrderBy(x => random.Next()).Take(model.NumberOfTeams).ToList();

        // Distribute the shuffled users into random teams
        model.Teams = new Dictionary<string, List<string>>();
        for (int i = 0; i < model.NumberOfTeams; i++)
        {
            var teamMembers = shuffledUsers.Skip(i * teamSize).Take(teamSize).ToList();
            if (teamMembers.Any())
            {
                model.Teams.Add(teamNames[i], teamMembers);
            }
        }

        // Return the Teams view with the model containing the generated teams
        return View("Teams", model);  
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
