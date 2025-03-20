

document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");

    // Показване на грешка
    function showError(input, message) {
        const errorSpan = input.closest(".inputBox-content").querySelector("span.text-danger");
        if (errorSpan) {
            errorSpan.textContent = message;
        }
    }

    // Премахване на грешка
    function clearError(input) {
        const errorSpan = input.closest(".inputBox-content").querySelector("span.text-danger");
        if (errorSpan) {
            errorSpan.textContent = "";
        }
    }

    // Основна валидационна функция
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

    // Валидация на числово поле (Capacity)
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

    // Вземане на всички input полета
    const nameInput = document.querySelector("input[name='Name']");
    const addressInput = document.querySelector("input[name='Address']");
    const phoneInput = document.querySelector("input[name='Phone']");
    const capacityInput = document.querySelector("input[name='Capacity']");

    // Слушатели за премахване на грешката при коригиране на полето
    nameInput.addEventListener("input", () => validateField(nameInput, 3, "Fältet måste innehålla mer än 2 tecken."));
    addressInput.addEventListener("input", () => validateField(addressInput, 3, "Adressen måste innehålla minst 3 tecken."));
    phoneInput.addEventListener("input", () => validateField(phoneInput, 7, "Fältet måste innehålla mellan 7 och 13 tecken."));
    capacityInput.addEventListener("input", () => validateNumberField(capacityInput, 1, "Fältet måste vara större än 0."));

    // Валидация при изпращане на формата
    form.addEventListener("submit", function (event) {
        let isValid = true;

        if (!validateField(nameInput, 3, "Fältet måste innehålla mer än 2 tecken.")) isValid = false;
        if (!validateField(addressInput, 3, "Adressen måste innehålla minst 3 tecken.")) isValid = false;
        if (!validateField(phoneInput, 7, "Fältet måste innehålla mellan 7 och 13 tecken.")) isValid = false;
        if (!validateNumberField(capacityInput, 1, "Fältet måste vara större än 0.")) isValid = false;

        if (!isValid) {
            event.preventDefault(); // Спира изпращането на формата при грешки
        }
    });
});
