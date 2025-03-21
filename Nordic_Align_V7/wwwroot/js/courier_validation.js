document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");

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

    function validateField(input, regex, minLength, maxLength, message) {
        const value = input.value.trim();
        if (value.length >= minLength && value.length <= maxLength && regex.test(value)) {
            clearError(input.name);
            return true;
        } else {
            setError(input.name, message);
            return false;
        }
    }

    document.querySelector("input[name='FullName']").addEventListener('input', function () {
        const fullName = this.value.trim();
        const words = fullName.split(/\s+/);
        if (words.length >= 2) {
            clearError("FullName");
        } else {
            setError("FullName", "Fältet måste innehålla minst två ord.");
        }
    });

    document.querySelector("input[name='Personnummer']").addEventListener('input', function () {
        const persnr = this.value.trim();
        if (/^\d{12}$/.test(persnr)) {
            clearError("Personnummer");
        } else {
            setError("Personnummer", "Fältet måste innehålla 12 siffror.");
        }
    });

    document.querySelector("input[name='EmploymentDate']").addEventListener('input', function () {
        const employmentDate = this.value;
        if (employmentDate !== "0001-01-01") {
            clearError("EmploymentDate");
        } else {
            setError("EmploymentDate", "Fältet kan inte vara 01.01.0001.");
        }
    });

    document.querySelector("input[name='Phone']").addEventListener('input', function () {
        const phone = this.value.trim();
        if (phone.length >= 7 && phone.length <= 13) {
            clearError("Phone");
        } else {
            setError("Phone", "Fältet måste innehålla minst 7 och max 13 tecken.");
        }
    });

    document.querySelector("input[name='LivingPlace']").addEventListener('input', function () {
        const livingPlace = this.value.trim();
        if (/^[A-Za-zÅÄÖåäö]{2,}/.test(livingPlace)) {
            clearError("LivingPlace");
        } else {
            setError("LivingPlace", "Fältet måste innehålla minst två tecken.");
        }
    });

    form.addEventListener("submit", function (event) {
        let isValid = true;

        const fullName = document.querySelector("input[name='FullName']").value.trim();
        const words = fullName.split(/\s+/);
        if (words.length < 2) {
            isValid = false;
            setError("FullName", "Fältet måste innehålla minst två ord.");
        }

        const persnr = document.querySelector("input[name='Personnummer']").value.trim();
        if (!/^\d{12}$/.test(persnr)) {
            isValid = false;
            setError("Personnummer", "Fältet måste innehålla 12 siffror.");
        }

        const employmentDate = document.querySelector("input[name='EmploymentDate']").value;
        if (employmentDate === "0001-01-01") {
            isValid = false;
            setError("EmploymentDate", "Fältet kan inte vara 01.01.0001.");
        }

        const phone = document.querySelector("input[name='Phone']").value.trim();
        if (phone.length < 7 || phone.length > 13) {
            isValid = false;
            setError("Phone", "Fältet måste innehålla minst 7 och max 13 tecken.");
        }

        const livingPlace = document.querySelector("input[name='LivingPlace']").value.trim();
        if (!/^[A-Za-zÅÄÖåäö]{2,}/.test(livingPlace)) {
            isValid = false;
            setError("LivingPlace", "Fältet måste innehålla minst två tecken.");
        }

        if (!isValid) {
            event.preventDefault(); 
        }
    });
});
