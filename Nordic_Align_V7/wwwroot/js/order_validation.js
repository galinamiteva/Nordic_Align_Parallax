document.addEventListener("DOMContentLoaded", function () {
    const senderInput = document.querySelector("input[name='Sender']");
    const deliveryDateInput = document.querySelector("input[name='DeliveryDate']");
    const form = document.querySelector("form");

    form.addEventListener("submit", function (event) {
        let isValid = true;

        // Kontrollera att "Mottagare" består av två ord
        const senderValue = senderInput.value.trim();
        const words = fullName.split(/\s+/);
        if (words.length < 2) {
            alert("Mottagaren maste innehalla minst tva ord.");
            isValid = false;
        }

        // Kontrollera om "Leveransdatum" är större än det aktuella datumet
        const deliveryDateValue = new Date(deliveryDateInput.value);
        const today = new Date();

        if (deliveryDateValue <= today) {
            alert("Leveransdatum maste vara senare an idag.");
            isValid = false;
        }

        // Om någon kontroll misslyckas, avbryt formulärinlämningen
        if (!isValid) {
            event.preventDefault();
        }
    });
});