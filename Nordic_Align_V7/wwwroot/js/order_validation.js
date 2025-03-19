document.addEventListener("DOMContentLoaded", function () {
    const senderInput = document.querySelector("input[name='Sender']");
    const deliveryDateInput = document.querySelector("input[name='DeliveryDate']");
    const form = document.querySelector("form");
    const recepientSelect = document.querySelector("select[name='RecepientId']");


    form.addEventListener("submit", function (event) {
        let isValid = true;

        // Kontrollera att "Mottagare" består av två ord
        const senderValue = senderInput.value.trim();
        const words = senderValue.split(/\s+/);
        if ( words.length < 2) {
            alert("Mottagaren måste innehålla minst två ord.");
            isValid = false;
        }

        // Kontrollera om "Leveransdatum" är större än det aktuella datumet
        const deliveryDateValue = new Date(deliveryDateInput.value);
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        if (deliveryDateValue <= today) {
            alert("Leveransdatum måste vara senare än idag.");
            isValid = false;
        }

              
        if (senderInput) {
            senderInput.removeAttribute("disabled");
        }

        //  Kontrollera om mottagaren är vald
        if (!recepientSelect.value || recepientSelect.value === "null") {
            alert("Du måste välja en mottagare.");
            isValid = false;
        }

       

        // Om någon kontroll misslyckas, avbryt formulärinlämningen
        if (!isValid) {
            event.preventDefault();
        }
    });
});
