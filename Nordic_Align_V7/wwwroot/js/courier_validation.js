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

        // Kontrollera fullständigt namn (måste vara minst två ord)
        const fullName = document.querySelector("input[name='FullName']").value.trim();
        const words = fullName.split(/\s+/);
        if (words.length < 2) {
            isValid = false;
            setError("FullName", "Faltet Namn maste innehalla minst tva ord.");
        } else {
            clearError("FullName");
        }

        // Kontrollera personnummer (måste vara 12 siffror)
        const persnr = document.querySelector("input[name='Personnummer']").value.trim();
        if (!/^\d{12}$/.test(persnr)) {
            isValid = false;
            setError("Personnummer", "Personnummer maste innehalla 12 siffror.");
        } else {
            clearError("Personnummer");
        }

        // Kontrollera anställningsdatum (bör inte vara 01.01.0001)
        const employmentDate = document.querySelector("input[name='EmploymentDate']").value;
        if (employmentDate === "0001-01-01") {
            isValid = false;
            setError("EmploymentDate", "Faltet Anstallningsdatum kan inte vara 01.01.0001.");
        } else {
            clearError("EmploymentDate");
        }

        // Telefonverifiering (mellan 7 och 13 tecken)
        const phone = document.querySelector("input[name='Phone']").value.trim();
        if (phone.length < 7 || phone.length > 13) {
            isValid = false;
            setError("Phone", "Telefonfaltet maste innehalla minst 7 och max 13 tecken.");
        } else {
            clearError("Phone");
        }
        //Test för Stad
        const livingPlace = document.querySelector("input[name='LivingPlace']").value.trim();
        if (!/^[A-Za-zÅÄÖåäö]{2,}/.test(livingPlace)) {
            isValid = false;
            setError("LivingPlace", "Fältet måste innehålla minst två tecken.");
        } else {
            clearError("LivingPlace");
        }


        if (!isValid) {
            event.preventDefault(); // Stoppa inlämning av formulär om ett fel hittas
        }
    });
});
