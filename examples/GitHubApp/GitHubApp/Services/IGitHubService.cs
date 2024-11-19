using GitHubApp.Models;

namespace GitHubApp.Services
{
    public interface IGitHubService

    { 
        Task<IEnumerable<GitRepo>> SearchRepositoriesAsync (string query);
    }
}
