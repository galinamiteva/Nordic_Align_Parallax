document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");

    function showError(inputName, message) {
        const errorSpan = document.querySelector(`span[data-valmsg-for='${inputName}']`);
        if (errorSpan) {
            errorSpan.textContent = message;
        }
    }

    function clearError(inputName) {
        const errorSpan = document.querySelector(`span[data-valmsg-for='${inputName}']`);
        if (errorSpan) {
            errorSpan.textContent = "";
        }
    }

    function validateField(inputName, value) {
        let isValid = true;
        if (inputName === "TransportNumber") {
            if (value.length < 6) {
                showError(inputName, "Fältet måste innehålla minst 6 tecken.");
                isValid = false;
            } else {
                clearError(inputName);
            }
        }

        if (inputName === "RegistrationDate") {
            if (value === "0001-01-01") {
                showError(inputName, "Fältet får inte vara 01.01.0001.");
                isValid = false;
            } else {
                clearError(inputName);
            }
        }

        if (inputName === "Brand" || inputName === "Model" || inputName === "Color") {
            if (value.length <= 2) {
                showError(inputName, "Fältet måste innehålla mer än 2 tecken.");
                isValid = false;
            } else {
                clearError(inputName);
            }
        }

        if (inputName === "Capacity") {
            const capacity = parseFloat(value);
            if (isNaN(capacity) || capacity <= 0) {
                showError(inputName, "Fältet måste vara större än 0.");
                isValid = false;
            } else {
                clearError(inputName);
            }
        }

        return isValid;
    }

    form.querySelectorAll("input").forEach(input => {
        input.addEventListener("input", function () {
            validateField(input.name, input.value.trim());
        });
        input.addEventListener("blur", function () {
            validateField(input.name, input.value.trim());
        });
    });

    form.addEventListener("submit", function (event) {
        let isValid = true;

        form.querySelectorAll("input").forEach(input => {
            if (!validateField(input.name, input.value.trim())) {
                isValid = false;
            }
        });

        if (!isValid) {
            event.preventDefault(); 
        }
    });
});
