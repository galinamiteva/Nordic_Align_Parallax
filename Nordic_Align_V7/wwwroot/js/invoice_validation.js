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

const validateForm = (event) => {
    const form = event.target;
    let isValid = true;

    form.querySelectorAll('[data-val="true"]').forEach(input => {
        switch (input.type) {
            case 'text':
            case 'email':
                isValid = isValid && textValidator(input);
                break;
            case 'password':
                isValid = isValid && passValidator(input);
                break;
            case 'checkbox':
                isValid = isValid && checkboxValidator(input);
                break;
        }
    });

    // Prevent form submission if any invalid input exists
    if (!isValid) {
        event.preventDefault();  // Prevent form from being submitted
    }

    return isValid;
};

// Function to handle field errors
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


// Validate text fields
const textValidator = (element, minLength = 2) => {
    if (element.value.trim().length >= minLength) {
        formErrorHandler(element, true);
        return true;
    } else {
        formErrorHandler(element, false);
        return false;
    }
};

// Validate email
const emailValidator = (element) => {
    const regEx = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    const isValid = regEx.test(element.value);
    formErrorHandler(element, isValid);
    return isValid;
};


// Validate number fields (must be positive)
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

// Validate postal code (at least 5 characters)
const postalCodeValidator = (element) => {
    const isValid = element.value.trim().length >= 5;
    formErrorHandler(element, isValid);
    return isValid;
};

// Validate date fields
const dateValidator = (element) => {
    const isValid = element.value.trim() !== '';
    formErrorHandler(element, isValid);
    return isValid;
};

// Add listeners for validation
document.addEventListener("DOMContentLoaded", function () {
    const invoiceForm = document.querySelector('form');
    if (invoiceForm) {
        invoiceForm.addEventListener('submit', (e) => {
            // Check if the form is valid before submitting
            if (!validateInvoiceForm(e)) {
                e.preventDefault();  // Stop form submission if validation fails
            }
        });
    }

    document.querySelectorAll('input[type="number"]').forEach(input => {
        input.addEventListener('input', function () {
            if (this.value < 0) this.value = '';
        });
    });

    // Initialize input field validators
    let inputs = document.querySelectorAll('input[required], textarea[required]');

    inputs.forEach(input => {
        input.addEventListener('keyup', (e) => {
            switch (e.target.type) {
                case 'text':
                    if (e.target.id === "PostalCode") {
                        postalCodeValidator(e.target);
                    } else {
                        textValidator(e.target);
                    }
                    break;
                case 'email':
                    emailValidator(e.target);
                    break;
                case 'number':
                    numberValidator(e.target);
                    break;
                case 'date':
                    dateValidator(e.target);
                    break;
            }
        });

        input.addEventListener('blur', (e) => {
            switch (e.target.type) {
                case 'text':
                    if (e.target.id === "PostalCode") {
                        postalCodeValidator(e.target);
                    } else {
                        textValidator(e.target);
                    }
                    break;
                case 'email':
                    emailValidator(e.target);
                    break;
                case 'number':
                    numberValidator(e.target);
                    break;
                case 'date':
                    dateValidator(e.target);
                    break;
            }
        });
    });
});


