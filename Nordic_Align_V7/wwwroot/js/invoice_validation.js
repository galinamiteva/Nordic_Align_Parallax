document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");  // Вземи формуляра

    form.addEventListener("submit", function (event) {
        // För att förhindra att blanketten skickas in normalt, om det behövs
        event.preventDefault();

        // Få värden från fält
        const email = document.querySelector("input[name='email']").value.trim();
        const clientName = document.querySelector("input[name='clientName']").value.trim();
        const productName = document.querySelector("input[name='productName']").value.trim();
        const amount = document.querySelector("input[name='amount']").value.trim();

        // Verifiering av inmatade data
        if (!email || !clientName || !productName || !amount) {
            alert("Vanligen fyll i alla falt.");
            return;
        }

        // Skicka data till servern
        fetch('/Invoice/CreateInvoice', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `email=${email}&clientName=${clientName}&productName=${productName}&amount=${amount}`
        })
            .then(response => response.text())
            .then(result => {
                alert(result);  // Vi visar resultatet (framgång eller fel)
            })
            .catch(error => {
                console.error('Fel:', error);
                alert('Ett fel uppstod nar fakturan skickades.');
            });
    });
});
