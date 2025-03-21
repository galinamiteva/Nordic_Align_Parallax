document.addEventListener("DOMContentLoaded", function () {
    const senderInput = document.querySelector("input[name='Sender']");
    const deliveryDateInput = document.querySelector("input[name='DeliveryDate']");
    const form = document.querySelector("form");
    const recepientSelect = document.querySelector("select[name='RecepientId']");

    // Функция за показване на съобщение за грешка
    function showError(input, message) {
        const errorSpan = input.closest('.inputBox').querySelector('.text-danger');
        errorSpan.textContent = message;
        errorSpan.style.display = 'block'; // Показваме съобщението за грешка
    }

    // Функция за скриване на съобщение за грешка
    function hideError(input) {
        const errorSpan = input.closest('.inputBox').querySelector('.text-danger');
        errorSpan.textContent = '';
        errorSpan.style.display = 'none'; // Скриваме съобщението за грешка
    }

    // Проверка при подаване на формуляра
    form.addEventListener("submit", function (event) {
        let isValid = true;

        // Проверка за "Sender" (Мотагаренето трябва да съдържа минимум две думи)
        const senderValue = senderInput.value.trim();
        const words = senderValue.split(/\s+/);
        if (words.length < 2) {
            showError(senderInput, "Mottagaren måste innehålla minst två ord.");
            isValid = false;
        } else {
            hideError(senderInput);
        }

        // Проверка за "DeliveryDate" (датата трябва да е по-късно от днешната дата)
        const deliveryDateValue = new Date(deliveryDateInput.value);
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        if (deliveryDateValue <= today) {
            showError(deliveryDateInput, "Leveransdatum måste vara senare än idag.");
            isValid = false;
        } else {
            hideError(deliveryDateInput);
        }

        // Проверка за "RecepientId" (трябва да бъде избран получател)
        if (!recepientSelect.value || recepientSelect.value === "null") {
            showError(recepientSelect, "Du måste välja en mottagare.");
            isValid = false;
        } else {
            hideError(recepientSelect);
        }

        // Ако някоя проверка не е успешна, спираме изпращането на формуляра
        if (!isValid) {
            event.preventDefault();
        }
    });

    // Скриваме съобщенията за грешка, когато потребителят започне да пише
    senderInput.addEventListener('input', function () {
        const words = senderInput.value.trim().split(/\s+/);
        if (words.length >= 2) {
            hideError(senderInput);
        }
    });

    deliveryDateInput.addEventListener('input', function () {
        const deliveryDateValue = new Date(deliveryDateInput.value);
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        if (deliveryDateValue > today) {
            hideError(deliveryDateInput);
        }
    });

    // Добавяме слушател за събитие 'change' за полето select за получател
    recepientSelect.addEventListener('change', function () {
        // Проверяваме дали е избран валиден получател
        if (recepientSelect.value && recepientSelect.value !== "null") {
            hideError(recepientSelect);  // Скриваме грешката, когато е избран получател
        } else {
            showError(recepientSelect, "Du måste välja en mottagare."); // Покажи грешка ако няма валиден получател
        }
    });

    // Скриваме съобщението за грешка, ако полето за получател е променено чрез JavaScript (например, ако се избере нов получател автоматично)
    if (recepientSelect.value && recepientSelect.value !== "null") {
        hideError(recepientSelect);
    }
});
