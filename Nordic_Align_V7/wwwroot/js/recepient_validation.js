document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");

    form.addEventListener("submit", function (event) {
        let isValid = true;

        // Fullständig namnkontroll (måste bestå av 2 ord)
        const fullName = document.querySelector("input[name='FullName']").value.trim();
        const words = fullName.split(/\s+/);
        if (words.length < 2) {
            alert("Faltet Fullstandigt namn maste innehalla minst tva ord..");
        }


       

        

        // Kontrollera postnummer (måste vara 5 siffror)
        const postalCode = document.querySelector("input[name='PostalCode']").value.trim();
        if (!/^\d{5}$/.test(postalCode)) {
            isValid = false;
            alert("Postnummer maste vara 5 siffror langt.");
        }

        // Telefonkontroll (måste vara 12 tecken)
        const phone = document.querySelector("input[name='Phone']").value.trim();
        if (phone.length <= 7 || phone.length >= 13) {
            isValid = false;
            alert("Telefonfaltet maste innehalla minst 7 och max 13 tecken.");
        }

        if (!isValid) {
            event.preventDefault(); // Stoppa inlämning av formulär om ett fel hittas
        }
    });
});