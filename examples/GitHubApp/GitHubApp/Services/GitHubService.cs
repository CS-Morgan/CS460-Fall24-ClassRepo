using GitHubApp.Models;
using System.Text.Json;


namespace GitHubApp.Services
{
    class Owner
    {
        public string Avatar_Url { get; set; }
    }

    class GitRepoAll
    {
        public string Name { get; set; }

        public Owner Owner { get; set; }

        public string Html_Url { get; set; }
    }
    class GitSearchRepoResponse
    {
        public int Total_count { get; set; }
        public bool Incomplete_results { get; set; }
        public IList<GitRepoAll> Items { get; set; }

    }
    public class GitHubService : IGitHubService
{
    readonly HttpClient _httpClient;
    readonly ILogger<GitHubService> _logger;

    public GitHubService(HttpClient httpClient, ILogger<GitHubService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
        //The method SearchRepositoriesAsync is designed to search for repositories on GitHub
        //based on a query string. Here's a step-by-step breakdown of what this method does:   
        public async Task<IEnumerable<GitRepo>> SearchRepositoriesAsync(string query)
    {
        string endpoint =$"/search/repositories/${query}";
            _logger.LogInformation($"Calling GitHub API at {endpoint}");   
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        string responseBody;
        if (response.IsSuccessStatusCode)
        {
            responseBody = await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new Exception($"Error: {response.StatusCode}");
        }
        JsonSerializerOptions options = new JsonSerializerOptions
        { 
            PropertyNameCaseInsensitive = true 
        };
            GitSearchRepoResponse repos = JsonSerializer.Deserialize<GitSearchRepoResponse>(responseBody, options);
            return repos.Items.Select(r => new GitRepo
            {
                Name = r.Name,
                Avatar_Url = r.Owner.Avatar_Url,
                Repo_Url = r.Html_Url
            });
        }

}
}
