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

    return isValid;
};




 //Function for handling field errors
const formErrorHandler = (element, validationResult) => {
    let spanElement = document.querySelector(`[data-valmsg-for="${element.name}"]`);
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
        spanElement.innerHTML = element.dataset.valRequired;
    }
};

// Validate text fields
const textValidator = (element, minLength = 2) => {
    if (element.value.length >= minLength) {
        formErrorHandler(element, true);
    } else {
        formErrorHandler(element, false);
    }
};

// Validate  email
const emailValidator = (element) => {
    const regEx = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    formErrorHandler(element, regEx.test(element.value));
};

// Validate pass
const passValidator = (element) => {
    if (element.dataset.valEqualToOther !== undefined) {
        let password = document.getElementsByName(element.dataset.valEqualToOther.replace('*', 'Form'))[0].value;

        if (element.value === password) {
            formErrorHandler(element, true);
        } else {
            formErrorHandler(element, false);
        }
    } else {
        const regEx = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$/;
        formErrorHandler(element, regEx.test(element.value));
    }
};

// Password confirmation validation
const confirmPasswordValidator = (passwordElement, confirmPasswordElement) => {
    if (passwordElement.value === confirmPasswordElement.value) {
        formErrorHandler(confirmPasswordElement, true);
    } else {
        formErrorHandler(confirmPasswordElement, false);
    }
};

// Check for all fields
const checkFields = () => {
    let allFieldsFilled = true;

    inputs.forEach(input => {
        if (!input.value) {
            allFieldsFilled = false;
        }
    });

    return allFieldsFilled;
};

// Initializing input field validators
let inputs = document.querySelectorAll('input');

inputs.forEach(input => {
    if (input.dataset.val === 'true') {
        if (input.type === 'checkbox') {
            input.addEventListener('change', (e) => {
                checkboxValidator(e.target);
            });
        } else {
            input.addEventListener('keyup', (e) => {
                switch (e.target.type) {
                    case 'text':
                        textValidator(e.target);
                        break;
                    case 'email':
                        emailValidator(e.target);
                        break;
                    case 'password':
                        passValidator(e.target);
                        break;
                }
            });
        }
    }
});

// Validation for the registration form
const validateRegisterForm = () => {
    let valid = true;

    const password = document.getElementById("Password1");
    const confirmPassword = document.getElementById("Password2");

    if (password && confirmPassword) {
        confirmPasswordValidator(password, confirmPassword);
    }

    inputs.forEach(input => {
        switch (input.name) {
            case 'FirstName':
                textValidator(input, 2);
                break;
            case 'LastName':
                textValidator(input, 2);
                break;
            case 'Email':
                emailValidator(input);
                break;
            case 'Password':
            case 'ConfirmPassword':
                passValidator(input);
                break;
        }
    });

    return valid && checkFields();
};

// Login form validation
const validateLoginForm = () => {
    let valid = true;

    const emailInput = document.getElementById("email");
    const passwordInput = document.getElementById("password");

    if (emailInput && passwordInput) {
        textValidator(emailInput, 1);
        passValidator(passwordInput);
    }

    return valid && checkFields();
};

// Adding listeners for the forms
document.addEventListener("DOMContentLoaded", function () {
    const registerForm = document.querySelector('form#signupForm');
    if (registerForm) {
        registerForm.addEventListener('submit', (e) => {
            if (!validateRegisterForm()) {
                e.preventDefault();
            }
        });
    }

    const loginForm = document.querySelector('form#signinForm');
    if (loginForm) {
        loginForm.addEventListener('submit', (e) => {
            if (!validateLoginForm()) {
                e.preventDefault();
            }
        });
    }
});
