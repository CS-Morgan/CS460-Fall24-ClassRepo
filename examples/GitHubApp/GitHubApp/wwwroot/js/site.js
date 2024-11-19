// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', initializePage, false);

function initializePage() {
    console.log('Page loaded, intializing...');
    const searchButton = document.getElementById("search-button");
    searchButton.addEventListener('click', searchRepositories, false);

    function searchRepositories(e) {
        e.preventDefault();
        const searchInput = document.getElementById("search-input");
        const searchResultsDiv = document.getElementById("search-results");
        const searchTerm = searchInput.value;
        if (!searchTerm) {
            searchResultsDiv.innerHTML = '<h3 class="text-danger text-center">Please enter a search term</h3>';
            return;
        }
        console.log(`Searching for ${searchTerm}`);
        searchResultsDiv.textContent = '';
    }
}