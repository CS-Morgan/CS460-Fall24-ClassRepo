using GitHubApp.Models;
using GitHubApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GitHubApp.Controllers
{
    [Route("api/github")]
    [ApiController]
    public class GitHubAPIController : ControllerBase
    {

        private readonly IGitHubService _gitHubService;
        private readonly ILogger<GitHubAPIController> _logger;
        public GitHubAPIController(IGitHubService gitHubService, ILogger<GitHubAPIController> logger)
        {
            _gitHubService = gitHubService;
            _logger = logger;
        }
        [HttpGet("search/repositories")]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(IEnumerable<GitRepo>))]
        public async Task<IActionResult> SearchRepositoriesAsync([FromQuery(Name = "q")] string query)
        {
            try
            {
                IEnumerable<GitRepo> repos = await _gitHubService.SearchRepositoriesAsync(query);
                return Ok(repos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for repositories");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
