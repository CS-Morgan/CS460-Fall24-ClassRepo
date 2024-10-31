
document.addEventListener('DOMContentLoaded', initializePage, false);

function initializePage() {
    const welcome = document.getElementById('welcome');
    welcome.textContent = "Manage users and auctions here!";
    console.log("Manage page loaded");

    // Display all sellers in a table
    fetchAndDisplaySellers();

    // add event listener to the add seller button
    const addSellerButton = document.getElementById('add-seller-button');
    addSellerButton.addEventListener('click', addSeller, false);
}

async function fetchAndDisplaySellers(tableElement) {
    const response = await fetch('/api/seller/sellers');
    const sellers = await response.json();
    console.log(sellers);

    const sellerTable = document.getElementById('seller-table');
    const tbody = sellerTable.querySelector('tbody');
    while (tbody.firstChild) {
        tbody.removeChild(tbody.firstChild);
    }

    sellers.forEach(seller => {
        const row = document.createElement('tr');
        const firstNameCell = document.createElement('td');
        const lastNameCell = document.createElement('td');
        const emailCell = document.createElement('td');
        const phoneCell = document.createElement('td');
        const deleteCell = document.createElement('td');

        firstNameCell.textContent = seller.firstName;
        lastNameCell.textContent = seller.lastName;
        emailCell.textContent = seller.email;
        phoneCell.textContent = seller.phone;
        deleteCell.innerHTML = `<button type="button" class="btn btn-danger btn-sm" onclick="deleteSeller(${seller.id})"><i class="bi bi-trash3"></i></button>`;

        row.appendChild(firstNameCell);
        row.appendChild(lastNameCell);
        row.appendChild(emailCell);
        row.appendChild(phoneCell);
        row.appendChild(deleteCell);

        tbody.appendChild(row);
    });
    sellerTable.style.display = 'block';
}

// This implementation of delete has no user confirmation.  It would be better to use a modal and have the user click either
// delete or cancel.  I'll use a modal for create as an example.
async function deleteSeller(sellerId) {
    const response = await fetch(`/api/seller/${sellerId}`, { method: 'DELETE' });
    if (response.ok) {
        fetchAndDisplaySellers();
    }
    else {
        // Would be better to use a modal here
        window.alert("Something went wrong when trying to delete this seller.  Please try again later.");
    }
}

// This form doesn't yet have any validation beyond the email input element type="email".  If the server rejects it
// we aren't displaying the errors.  We should validate here first, then send it to the server if OK, then display any
// errors the server returns by looking in the ProblemDetails errors object.
async function addSeller() {
    console.log("Adding seller...");
    const firstName = document.getElementById('first-name');
    const lastName = document.getElementById('last-name');
    const email = document.getElementById('email');
    const phone = document.getElementById('phone');
    const taxid = document.getElementById('taxid');

    const seller = {
        id: 0,      // if id == 0 this is a create, otherwise it's an update
        firstName: firstName.value,
        lastName: lastName.value,
        email: email.value,
        phone: phone.value,
        taxidnumber: taxid.value
    };
    // send a PUT request to /api/seller with the seller object as the body
    const response = await fetch('/api/seller/0', {
        method: 'PUT',
        headers: {
            'Accept': 'application/json; application/problem+json; charset=utf-8',
            'Content-Type': 'application/json; charset=UTF-8'
        },
        body: JSON.stringify(seller)
    });
    if (response.ok) {
        // clear the form
        firstName.value = '';
        lastName.value = '';
        email.value = '';
        phone.value = '';
        taxid.value = '';
        // refresh the table, allowing time for the modal to be hidden
        setTimeout(fetchAndDisplaySellers, 500);
    }
    else {
        // Would be better to use the existing modal here and not hide it
        window.alert("Something went wrong when trying to add this seller.  Please try again later.");
    }

    const modal = bootstrap.Modal.getInstance(document.getElementById('add-seller-modal'));
    modal.hide();
}