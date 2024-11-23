using GitHubApp.Models;

namespace GitHubApp.Services
{
    public interface IGitHubService
    {

        public Task<GitRepo> SearchRepositoriesAsync(string query);


    }
}
