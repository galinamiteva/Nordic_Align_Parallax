

document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");

   
    function showError(input, message) {
        const errorSpan = input.closest(".inputBox-content").querySelector("span.text-danger");
        if (errorSpan) {
            errorSpan.textContent = message;
        }
    }

 
    function clearError(input) {
        const errorSpan = input.closest(".inputBox-content").querySelector("span.text-danger");
        if (errorSpan) {
            errorSpan.textContent = "";
        }
    }

   
    function validateField(input, minLength, errorMessage) {
        const value = input.value.trim();
        if (value.length < minLength) {
            showError(input, errorMessage);
            return false;
        } else {
            clearError(input);
            return true;
        }
    }


    // Kontrollera kapacitet (måste vara större än 0)
    function validateNumberField(input, minValue, errorMessage) {
        const value = parseFloat(input.value.trim());
        if (isNaN(value) || value < minValue) {
            showError(input, errorMessage);
            return false;
        } else {
            clearError(input);
            return true;
        }
    }

    const nameInput = document.querySelector("input[name='Name']");
    const addressInput = document.querySelector("input[name='Address']");
    const phoneInput = document.querySelector("input[name='Phone']");
    const capacityInput = document.querySelector("input[name='Capacity']");

    nameInput.addEventListener("input", () => validateField(nameInput, 3, "Fältet måste innehålla mer än 2 tecken."));
    addressInput.addEventListener("input", () => validateField(addressInput, 3, "Fältet måste innehålla minst 3 tecken."));
    phoneInput.addEventListener("input", () => validateField(phoneInput, 7, "Fältet måste innehålla mellan 7 och 13 tecken."));
    capacityInput.addEventListener("input", () => validateNumberField(capacityInput, 1, "Fältet måste vara större än 0."));

 
    form.addEventListener("submit", function (event) {
        let isValid = true;

        if (!validateField(nameInput, 3, "Fältet måste innehålla mer än 2 tecken.")) isValid = false;
        if (!validateField(addressInput, 3, "Fältet måste innehålla minst 3 tecken.")) isValid = false;
        if (!validateField(phoneInput, 7, "Fältet måste innehålla mellan 7 och 13 tecken.")) isValid = false;
        if (!validateNumberField(capacityInput, 1, "Fältet måste vara större än 0.")) isValid = false;

        if (!isValid) {
            event.preventDefault(); // Stoppa inlämningen av formuläret om ett fel upptäcks
        }
    });
});
