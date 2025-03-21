document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");

    form.addEventListener("submit", function (event) {
        let isValid = true;

        function setError(inputName, message) {
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

        // Функция за валидация на полетата при въвеждане
        function validateField(inputName) {
            const input = document.querySelector(`input[name='${inputName}']`);
            const value = input.value.trim();

            switch (inputName) {
                case "FullName":
                    const words = value.split(/\s+/);
                    if (words.length < 2) {
                        setError(inputName, "Fältet  måste innehålla minst två ord.");
                    } else {
                        clearError(inputName);
                    }
                    break;

                case "PostalCode":
                    if (!/^\d{5}$/.test(value)) {
                        setError(inputName, "Fältet  måste vara 5 siffror långt.");
                    } else {
                        clearError(inputName);
                    }
                    break;

                case "DeliveryAddress":
                    const addressWords = value.split(/\s+/);
                    if (addressWords.length < 2) {
                        setError(inputName, "Fältet  måste innehålla minst två ord.");
                    } else {
                        clearError(inputName);
                    }
                    break;

                case "Phone":
                    if (value.length < 7 || value.length > 13) {
                        setError(inputName, "Fältet måste innehålla minst 7 och max 13 tecken.");
                    } else {
                        clearError(inputName);
                    }
                    break;
            }
        }

        // Добави проверка при въвеждане на данни в полетата
        const inputFields = ['FullName', 'PostalCode', 'DeliveryAddress', 'Phone'];
        inputFields.forEach(function (field) {
            const inputElement = document.querySelector(`input[name='${field}']`);
            inputElement.addEventListener('input', function () {
                validateField(field); // Валидирай веднага при промяна
            });
        });

        // Извърши проверка при изпращане на формата
        inputFields.forEach(function (field) {
            const value = document.querySelector(`input[name='${field}']`).value.trim();
            switch (field) {
                case "FullName":
                    const words = value.split(/\s+/);
                    if (words.length < 2) {
                        isValid = false;
                        setError(field, "Fältet  måste innehålla minst två ord.");
                    } else {
                        clearError(field);
                    }
                    break;

                case "PostalCode":
                    if (!/^\d{5}$/.test(value)) {
                        isValid = false;
                        setError(field, "Fältet  måste vara 5 siffror långt.");
                    } else {
                        clearError(field);
                    }
                    break;

                case "DeliveryAddress":
                    const addressWords = value.split(/\s+/);
                    if (addressWords.length < 2) {
                        isValid = false;
                        setError(field, "Fältet  måste innehålla minst två ord.");
                    } else {
                        clearError(field);
                    }
                    break;

                case "Phone":
                    if (value.length < 7 || value.length > 13) {
                        isValid = false;
                        setError(field, "Fältet måste innehålla minst 7 och max 13 tecken.");
                    } else {
                        clearError(field);
                    }
                    break;
            }
        });

        // Ако формата не е валидна, предотврати нейното изпращане
        if (!isValid) {
            event.preventDefault();
        }
    });
});
