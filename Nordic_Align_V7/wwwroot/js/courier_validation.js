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

    // FullName
    document.querySelector("input[name='FullName']").addEventListener('input', function () {
        const fullName = this.value.trim();
        const words = fullName.split(/\s+/);
        if (words.length >= 2) {
            clearError("FullName");
        } else {
            setError("FullName", "Fältet måste innehålla minst två ord.");
        }
    });

    // Personnummer
    document.querySelector("input[name='Personnummer']").addEventListener('input', function () {
        const persnr = this.value.trim();
        if (/^\d{12}$/.test(persnr)) {
            clearError("Personnummer");
        } else {
            setError("Personnummer", "Fältet måste innehålla 12 siffror.");
        }
    });

    // EmploymentDate
    const employmentDateInput = document.querySelector("input[name='EmploymentDate']");
    employmentDateInput.addEventListener('input', function () {
        const employmentDate = new Date(this.value);
        const minDate = new Date("2024-07-01");

        if (isNaN(employmentDate)) {
            setError("EmploymentDate", "Vänligen ange ett giltigt datum.");
        } else if (employmentDate < minDate) {
            setError("EmploymentDate", "Anställningsdatumet kan inte vara före 1 juli 2024.");
        } else {
            clearError("EmploymentDate");
        }
    });

    // Phone
    document.querySelector("input[name='Phone']").addEventListener('input', function () {
        const phone = this.value.trim();
        if (phone.length >= 7 && phone.length <= 13) {
            clearError("Phone");
        } else {
            setError("Phone", "Fältet måste innehålla minst 7 och max 13 tecken.");
        }
    });

    // LivingPlace
    document.querySelector("input[name='LivingPlace']").addEventListener('input', function () {
        const livingPlace = this.value.trim();
        if (/^[A-Za-zÅÄÖåäö]{2,}/.test(livingPlace)) {
            clearError("LivingPlace");
        } else {
            setError("LivingPlace", "Fältet måste innehålla minst två tecken.");
        }
    });

    // StartWorkTime и EndWorkTime
    const startWorkTimeInput = document.querySelector("input[name='StartWorkTime']");
    const endWorkTimeInput = document.querySelector("input[name='EndWorkTime']");

    startWorkTimeInput.addEventListener('input', validateWorkTime);
    endWorkTimeInput.addEventListener('input', validateWorkTime);

    function validateWorkTime() {
        const startWorkTime = startWorkTimeInput.value;
        const endWorkTime = endWorkTimeInput.value;

        clearError("StartWorkTime");
        clearError("EndWorkTime");

        if (startWorkTime && endWorkTime) {
            const [startHours, startMinutes] = startWorkTime.split(":").map(Number);
            const [endHours, endMinutes] = endWorkTime.split(":").map(Number);

            if (startHours > endHours || (startHours === endHours && startMinutes >= endMinutes)) {
                setError("EndWorkTime", "Sluttiden måste vara senare än starttiden.");
            }
        }
    }

    // Submit 
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

        const employmentDateValue = employmentDateInput.value;
        const employmentDate = new Date(employmentDateValue);
        const minDate = new Date("2024-07-01");

        if (isNaN(employmentDate)) {
            isValid = false;
            setError("EmploymentDate", "Vänligen ange ett giltigt datum.");
        } else if (employmentDate < minDate) {
            isValid = false;
            setError("EmploymentDate", "Anställningsdatumet kan inte vara före 1 juli 2024.");
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

        const startWorkTime = startWorkTimeInput.value;
        const endWorkTime = endWorkTimeInput.value;

        clearError("StartWorkTime");
        clearError("EndWorkTime");

        if (startWorkTime === "" || endWorkTime === "") {
            isValid = false;
            setError("StartWorkTime", "Vänligen ange starttid.");
            setError("EndWorkTime", "Vänligen ange sluttid.");
        } else {
            const [startHours, startMinutes] = startWorkTime.split(":").map(Number);
            const [endHours, endMinutes] = endWorkTime.split(":").map(Number);

            if (startHours > endHours || (startHours === endHours && startMinutes >= endMinutes)) {
                isValid = false;
                setError("EndWorkTime", "Sluttiden måste vara efter starttiden.");
            }
        }

        if (!isValid) {
            event.preventDefault();
        }
    });

});