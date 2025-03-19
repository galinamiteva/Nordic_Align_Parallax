document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");

    form.addEventListener("submit", function (event) {
        let isValid = true;

        // Kontrollera transportnummer (minst 6 tecken)
        const transportNumber = document.querySelector("input[name='TransportNumber']").value.trim();
        if (transportNumber.length < 6) {
            isValid = false;
            alert("Fordonets registreringsnummer maste innehalla minst 6 tecken.");
        }

        // Kontrollera registreringsdatum (bör inte vara "0001-01-01")
        const registrationDate = document.querySelector("input[name='RegistrationDate']").value.trim();
        if (registrationDate === "01.01.0001") {
            isValid = false;
            alert("Registreringsdatum far inte vara 01.01.0001.");
        }

        // Kontrollera kapacitet (måste vara större än 0)
        const capacity = parseFloat(document.querySelector("input[name='Capacity']").value.trim());
        if (isNaN(capacity) || capacity <= 0) {
            isValid = false;
            alert("Kapaciteten maste vara storre an 0.");
        }

        if (!isValid) {
            event.preventDefault(); // Stoppa inlämningen av formuläret om ett fel upptäcks
        }
    });
});