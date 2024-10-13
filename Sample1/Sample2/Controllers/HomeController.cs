/*Controller code used to link both the View and Model together to delivery exceptional user experience*/
/*This code was made in contribution to ChatGPT and Co-Pilot*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System;
using Sample2.Models;  // Import your models including ErrorViewModel and SongViewModel
using Microsoft.Extensions.Hosting;  // For IWebHostEnvironment
using System.Text.RegularExpressions;
using Newtonsoft.Json; // Import for JSON serialization

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
    public IActionResult Index(SongViewModel model, string userNames)
    {
        // Split the multiline input into individual names
        var splitNames = userNames.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        // Validate each user name using regex (allow letters, spaces, etc.)
        string userNamePattern = @"^[a-zA-Z\s\.\-_']+$"; // Updated regex pattern

        model.UserNames = new List<string>();

        foreach (var name in splitNames)
        {
            if (!Regex.IsMatch(name, userNamePattern))
            {
                ModelState.AddModelError("UserNames", "Only letters, spaces, and ( . - _ ') are allowed.");
            }
            else
            {
                model.UserNames.Add(name.Trim());  // Add valid name to the list
            }
        }

        // Validate the number of teams
        if (model.NumberOfTeams < 2 || model.NumberOfTeams > 10)
        {
            ModelState.AddModelError("NumberOfTeams", "Please enter a valid number of teams between 2 and 10.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);  // Return the view with errors if validation fails
        }

        // Store the user names and number of teams in TempData for use in the Teams action
        TempData["UserNames"] = string.Join(",", model.UserNames);
        TempData["NumberOfTeams"] = model.NumberOfTeams;

        // Redirect to the Teams page without query parameters
        return RedirectToAction("Teams");
    }

    [HttpGet]
    public IActionResult Teams(SongViewModel model)
    {
        // Retrieve user names and number of teams from TempData
        var userNames = TempData["UserNames"]?.ToString().Split(',').ToList() ?? new List<string>();
        var numberOfTeams = (int)(TempData["NumberOfTeams"] ?? 2); // Default to 2 if not set

        // Set default team names from a predefined list
        model.Teams = new Dictionary<string, List<string>>();

        var defaultTeamNames = new[] { "Team A", "Team B", "Team C", "Team D", "Team E", "Team F", "Team G", "Team H", "Team I", "Team J" };
        
        foreach (var teamName in defaultTeamNames.Take(numberOfTeams))
        {
            model.Teams[teamName] = new List<string>(); // Initialize with empty teams
        }

        // Shuffle the user names to randomize their order
        Shuffle(userNames);

        // Distribute the user names to the teams
        if (userNames.Count > 0)
        {
            var teamIndex = 0;
            for (int i = 0; i < userNames.Count; i++)
            {
                var teamName = defaultTeamNames[teamIndex];
                model.Teams[teamName].Add(userNames[i]);
                teamIndex = (teamIndex + 1) % numberOfTeams; // Move to the next team
            }
        }

        return View(model);
    }

    // Shuffle method to randomize a list
    private void Shuffle<T>(IList<T> list)
    {
        var random = new Random();
        int n = list.Count;

        while (n > 1)
        {
            int k = random.Next(n--);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // API method to get random team names
    [HttpGet("Home/GetRandomTeamNames")]
    public JsonResult GetRandomTeamNames()
    {
        var songFilePath = Path.Combine(_env.ContentRootPath, "App_Data", "TSwift.txt");
        var allSongs = System.IO.File.ReadAllLines(songFilePath).ToList();

        var random = new Random();
        var teamNames = allSongs.OrderBy(x => random.Next()).Take(10).ToList(); // Adjust number as needed

        return Json(teamNames);
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
