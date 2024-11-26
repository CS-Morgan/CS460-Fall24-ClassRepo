document.addEventListener('DOMContentLoaded', initializePage, false);

function initializePage() {
    console.log('Page loaded, initializing...');
    const searchButton = document.getElementById('search-button');
    searchButton.addEventListener('click', searchRepositories, false);
}
function showRepoDetails(e) {
    console.log('Show repo details');
    const repoUrl = e.currentTarget.dataset.repoURL;
    console.log(repoUrl);

    // go on with a new fetch request...
}
async function searchRepositories(e) {
    e.preventDefault();
    const searchInput = document.getElementById('search-input');
    const searchResultsDiv = document.getElementById('search-results');
    const searchTerm = searchInput.value;
    if (!searchTerm) {
        searchResultsDiv.innerHTML = '<h3 class="text-danger text-center">Please enter a search term</h3>';
        return;
    }

    const url = `https://api.github.com/search/repositories?q=${searchTerm}`;
    console.log(url);
    const response = await fetch(url);
    const repos = await response.json();
    // TODO: error on anything other than 200
    console.log(repos);

    searchResultsDiv.textContent = '';
    const repoTemplate = document.getElementById("repo-template");
    repos.items.forEach(repo => {
        const clone = repoTemplate.content.cloneNode(true);
        const img = clone.querySelector('img');
        img.src = repo.owner.avatar_url;
        img.width = 100;
        img.dataset.repoURL = repo.repo_url;
        img.addEventListener('click', showRepoDetails);
        searchResultsDiv.appendChild(clone);
    });



}