document.addEventListener("DOMContentLoaded", function () {
    const senderInput = document.querySelector("input[name='Sender']");
    const deliveryDateInput = document.querySelector("input[name='DeliveryDate']");
    const form = document.querySelector("form");
    const recepientSelect = document.querySelector("select[name='RecepientId']");

   
    function showError(input, message) {
        const errorSpan = input.closest('.inputBox').querySelector('.text-danger');
        if (errorSpan) {
            errorSpan.textContent = message;
            errorSpan.style.display = 'block'; 
        }
    }

   
    function hideError(input) {
        const errorSpan = input.closest('.inputBox').querySelector('.text-danger');
        if (errorSpan) {
            errorSpan.textContent = '';
            errorSpan.style.display = 'none'; 
        }
    }

    
    form.addEventListener("submit", function (event) {
        let isValid = true;

        // Minst 2 ord
        const senderValue = senderInput.value.trim();
        const words = senderValue.split(/\s+/);
        if (words.length < 2) {
            showError(senderInput, "Mottagaren måste innehålla minst två ord.");
            isValid = false;
        } else {
            hideError(senderInput);
        }

        
        const deliveryDateValue = new Date(deliveryDateInput.value);
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        if (deliveryDateValue <= today) {
            showError(deliveryDateInput, "Leveransdatum måste vara senare än idag.");
            isValid = false;
        } else {
            hideError(deliveryDateInput);
        }

        
        if (!recepientSelect.value || recepientSelect.value === "") {
            showError(recepientSelect, "Du måste välja en mottagare.");
            isValid = false;
        } else {
            hideError(recepientSelect); 
        }

        // om nåt av validation är inte valid , så stop 
        if (!isValid) {
            event.preventDefault();
        }
    });

    
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

    recepientSelect.addEventListener('change', function () {
        if (recepientSelect.value && recepientSelect.value !== "") {
            hideError(recepientSelect);
        } else {
            showError(recepientSelect, "Du måste välja en mottagare."); 
        }
    });

    
    function checkLocalStorageErrors() {
        const fields = ['Sender', 'DeliveryDate', 'RecepientId'];
        fields.forEach(field => {
            const errorMessage = localStorage.getItem(field + "_error");
            const input = document.querySelector(`input[name='${field}']`) || document.querySelector(`select[name='${field}']`);
            if (errorMessage && input) {
                showError(input, errorMessage);
            }
        });
    }

    
    checkLocalStorageErrors();
});
