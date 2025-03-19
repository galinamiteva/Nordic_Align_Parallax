document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");

    form.addEventListener("submit", function (event) {
        let isValid = true;

        // Kontrollera fullständigt namn (måste vara minst två ord)
        const fullName = document.querySelector("input[name='FullName']").value.trim();
        const words = fullName.split(/\s+/);
        if (words.length < 2) {
            alert("Faltet Namn maste innehalla minst tva ord.");
        }
        

        // Kontrollera personnummer (måste vara 12 siffror)
        const persnr = document.querySelector("input[name='Personnummer']").value.trim();

        if (!/^\d{12}$/.test(persnr)) {
            isValid = false;
            alert("Personnummer maste innehalla 12 siffror.");
        }

       

        // Kontrollera anställningsdatum (bör inte vara 01.01.0001)
        const employmentDate = document.querySelector("input[name='EmploymentDate']").value;
        if (employmentDate === "01.01.0001") {
            isValid = false;
            alert("Faltet Anstallningsdatum kan inte vara 01.01.0001.");
        }


        // Telefonverifiering (mellan 7 och 13 tecken)
        const phone = document.querySelector("input[name='Phone']").value.trim();
        if (phone.length <= 7 || phone.length >= 13) {
            isValid = false;
            alert("Telefonfaltet maste minnst 7 och max 13 tecken.");
        }

        if (!isValid) {
            event.preventDefault(); // Stoppa inlämning av formulär om ett fel hittas
        }
    });
});