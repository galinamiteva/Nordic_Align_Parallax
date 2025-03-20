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

        // Fullständig namnkontroll (måste bestå av 2 ord)
        const fullName = document.querySelector("input[name='FullName']").value.trim();
        const words = fullName.split(/\s+/);
        if (words.length < 2) {
            isValid = false;
            setError("FullName", "Fältet  måste innehålla minst två ord.");
        } else {
            clearError("FullName");
        }

        // Kontrollera postnummer (måste vara 5 siffror)
        const postalCode = document.querySelector("input[name='PostalCode']").value.trim();
        if (!/^\d{5}$/.test(postalCode)) {
            isValid = false;
            setError("PostalCode", "Fältet  måste vara 5 siffror långt.");
        } else {
            clearError("PostalCode");
        }

        // Kontrollera leveransadress (måste bestå av minst 2 ord)
        const deliveryAddress = document.querySelector("input[name='DeliveryAddress']").value.trim();
        const addressWords = deliveryAddress.split(/\s+/);
        if (addressWords.length < 2) {
            isValid = false;
            setError("DeliveryAddress", "Fältet  måste innehålla minst två ord.");
        } else {
            clearError("DeliveryAddress");
        }

        // Telefonkontroll (måste vara mellan 7 och 13 tecken)
        const phone = document.querySelector("input[name='Phone']").value.trim();
        if (phone.length < 7 || phone.length > 13) {
            isValid = false;
            setError("Phone", "Fältet måste innehålla minst 7 och max 13 tecken.");
        } else {
            clearError("Phone");
        }

        if (!isValid) {
            event.preventDefault();
        }
    });
});
