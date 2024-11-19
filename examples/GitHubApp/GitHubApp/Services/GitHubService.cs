using GitHubApp.Models; 

namespace GitHubApp.Services
{
    public class GitHubService : IGitHubService
    {
        readonly HttpClient _httpClient;
        readonly ILogger <GitHubService> _logger;

        public GitHubService(HttpClient httpClient, ILogger<GitHubService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public Task<IEnumerable<GitRepo>> SearchRepositoriesAsync(string query)
        {
            return null;
        }
    }
}
