document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");

    form.addEventListener("submit", function (event) {
        let isValid = true;

        // Telefonverifiering (mellan 7 och 13 tecken)
        const phone = document.querySelector("input[name='Phone']").value.trim();
        if (phone.length <= 7 || phone.length >= 13) {
            isValid = false;
            alert("Telefonfaltet maste minnst 7 och max 13 tecken.");
        }

        // Kontrollera kapacitet (måste vara större än 0)
        const capacity = parseFloat(document.querySelector("input[name='Capacity']").value.trim());
        if (isNaN(capacity) || capacity <= 0) {
            isValid = false;
            alert("Kapaciteten maste vara storre an 0.");
        }

       

        if (!isValid) {
            event.preventDefault(); // Stoppa inlämningen av formuläret om ett fel upptäcks
        }
    });
});