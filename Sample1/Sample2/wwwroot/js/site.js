// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function () {
    const submitButton = document.getElementById('submit-button'); // Adjust the ID as necessary

    submitButton.addEventListener('click', function (event) {
        event.preventDefault(); // Prevent the default form submission

        const userNames = document.getElementById('user-names').value; // Adjust the ID as necessary
        const numberOfTeams = document.getElementById('number-of-teams').value; // Adjust the ID as necessary

        // Validate inputs if necessary

        // Send AJAX request
        fetch('/Home/AssignTeams', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ userNames, numberOfTeams }),
        })
        .then(response => response.json())
        .then(data => {
            // Handle successful response
            console.log(data);
            // Update the DOM with team results
        })
        .catch(error => {
            // Handle error
            console.error('Error:', error);
        });
    });
});
