const validateInvoiceForm = (event) => {
    const form = event.target;
    let isValid = true;

    form.querySelectorAll('[required]').forEach(input => {
        switch (input.type) {
            case 'text':
            case 'email':
                isValid = isValid && textValidator(input);
                break;
            case 'number':
                isValid = isValid && numberValidator(input);
                break;
            case 'date':
                isValid = isValid && dateValidator(input);
                break;
        }
    });

    const emailInput = form.querySelector('[name="Email"]');
    if (emailInput && !emailValidator(emailInput)) {
        isValid = false;
    }

    return isValid;
};


const showMessage = (message, type) => {
    let messageElement = document.getElementById("form-message");

    messageElement.textContent = message;
    messageElement.classList.remove("success", "error");
    messageElement.classList.add(type);
    messageElement.style.display = "block";

    setTimeout(() => {
        messageElement.style.display = "none";
    }, 40000);
};


const textValidator = (element, minLength = 2) => {
    if (element.value.trim().length >= minLength) {
        formErrorHandler(element, true);
        return true;
    } else {
        formErrorHandler(element, false);
        return false;
    }
};

const emailValidator = (element) => {
    const regEx = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    const isValid = regEx.test(element.value);
    formErrorHandler(element, isValid);
    return isValid;
};

const numberValidator = (element) => {
    const value = parseFloat(element.value);
    let isValid = !isNaN(value) && value >= 0 && element.value.trim() !== '';

    if (!isValid) {
        let errorMessage = value < 0 ? "Endast positivt tal" : "Detta fält är obligatoriskt.";
        formErrorHandler(element, false, errorMessage);
    } else {
        formErrorHandler(element, true);
    }

    return isValid;
};

const dateValidator = (element) => {
    const isValid = element.value.trim() !== '';
    formErrorHandler(element, isValid);
    return isValid;
};


const formErrorHandler = (element, validationResult, customMessage = "") => {
    let spanElement = document.querySelector(`[data-valmsg-for="${element.getAttribute('name')}"]`);

    if (!spanElement) {
        console.warn(`Warning: Missing span for ${element.name}`);
        return;
    }

    if (validationResult) {
        element.classList.remove('input-validation-error');
        spanElement.classList.remove('field-validation-error');
        spanElement.classList.add('field-validation-valid');
        spanElement.innerHTML = '';
    } else {
        element.classList.add('input-validation-error');
        spanElement.classList.add('field-validation-error');
        spanElement.classList.remove('field-validation-valid');
        spanElement.innerHTML = customMessage || element.dataset.valRequired || "Detta fält är obligatoriskt.";
    }
};


const resetForm = (form) => {
    form.reset();
    form.querySelectorAll('.input-validation-error').forEach(input => input.classList.remove('input-validation-error'));
    form.querySelectorAll('.field-validation-error').forEach(span => {
        span.classList.remove('field-validation-error');
        span.innerHTML = '';
    });
};


document.addEventListener("DOMContentLoaded", function () {
    const invoiceForm = document.querySelector('form');

    if (invoiceForm) {
        invoiceForm.addEventListener('submit', async (e) => {
            e.preventDefault();

            if (!validateInvoiceForm(e)) {
                showMessage("Error: Invalid form input.", "error");
                return;
            }

            
            let formData = new FormData(invoiceForm);

            try {
                let response = await fetch(invoiceForm.action, {
                    method: "POST",
                    body: formData
                });

                let result = await response.text();

                if (response.ok && result.includes("Invoice email has been sent successfully.")) {
                    showMessage("Fakturan har skickats framgångsrikt!", "success");
                    resetForm(invoiceForm);
                } else {
                    showMessage("Fel: Fakturan kunde inte skickas.", "error");
                }
            } catch (error) {
                console.error("Fetch error:", error);
                showMessage("Fel: Ett tekniskt fel uppstod.", "error");
            }
        });
    }
});
