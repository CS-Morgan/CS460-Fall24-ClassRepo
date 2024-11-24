using GitHubApp.Models;

namespace GitHubApp.Services
{
    public interface IGitHubService
    {

        public Task<IEnumerable<GitRepo>> SearchRepositoriesAsync(string query);


    }
}
