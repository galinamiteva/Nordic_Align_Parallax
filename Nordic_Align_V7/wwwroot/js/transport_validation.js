
document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");

    form.addEventListener("submit", function (event) {
        let isValid = true;

        // Visa error funktion
        function showError(inputName, message) {
            const errorSpan = document.querySelector(`span[data-valmsg-for='${inputName}']`);
            if (errorSpan) {
                errorSpan.textContent = message;
            }
        }

        // Rena error funktion
        function clearErrors() {
            document.querySelectorAll("span.text-danger").forEach(span => span.textContent = "");
        }

        clearErrors();

        // Kontrollera transportnummer (minst 6 tecken)
        const transportNumberInput = document.querySelector("input[name='TransportNumber']");
        const transportNumber = transportNumberInput.value.trim();
        if (transportNumber.length < 6) {
            isValid = false;
            showError("TransportNumber", "Fältet måste innehålla minst 6 tecken.");
        }

        // Kontrollera registreringsdatum (bör inte vara "0001-01-01")
        const registrationDateInput = document.querySelector("input[name='RegistrationDate']");
        const registrationDate = registrationDateInput.value.trim();
        if (registrationDate === "0001-01-01") {
            isValid = false;
            showError("RegistrationDate", "Fältet får inte vara 01.01.0001.");
        }

        //  Brand (bör innehålla mer än 2 tecken)
        const brandInput = document.querySelector("input[name='Brand']");
        const brand = brandInput.value.trim();
        if (brand.length <= 2) {
            isValid = false;
            showError("Brand", "Fältet måste innehålla mer än 2 tecken.");
        }

        //  Model (bör innehålla mer än 2 tecken)
        const modelInput = document.querySelector("input[name='Model']");
        const model = modelInput.value.trim();
        if (model.length <= 2) {
            isValid = false;
            showError("Model", "Fältet måste innehålla mer än 2 tecken.");
        }

        //  (bör innehålla mer än 2 tecken)
        const colorInput = document.querySelector("input[name='Color']");
        const color = colorInput.value.trim();
        if (color.length <= 2) {
            isValid = false;
            showError("Color", "Fältet måste innehålla mer än 2 tecken.");
        }

        // ВKontrollera kapacitet (måste vara större än 0)
        const capacityInput = document.querySelector("input[name='Capacity']");
        const capacity = parseFloat(capacityInput.value.trim());
        if (isNaN(capacity) || capacity <= 0) {
            isValid = false;
            showError("Capacity", "Fältet måste vara större än 0.");
        }

        if (!isValid) {
            event.preventDefault(); // Stoppa inlämningen av formuläret om ett fel upptäcks
        }
    });
});