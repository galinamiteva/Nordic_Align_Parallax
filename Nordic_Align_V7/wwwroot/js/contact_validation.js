


    //Contact script
    document.addEventListener("DOMContentLoaded", function () {
        let form = document.querySelector(".contact__form");
    let messageElement = document.getElementById("form-message");

    if (!form) {
        console.error("Ingen form!");
    return;
        }

    form.addEventListener("submit", async function (event) {
        event.preventDefault();

    let name = document.getElementById("name").value.trim();
    let email = document.getElementById("email").value.trim();
    let message = document.getElementById("message").value.trim();

        if (!name || !email || !message) {
            showMessage("Alla fält är obligatoriska", "error");
            form.reset();
            return;
        }

        let regEx = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        if (!regEx.test(email)) {
            showMessage("Ange en giltig e-postadress", "error");
            form.reset();
            return;
        }

        try {
            let response = await fetch("/Contact/SendMessage", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ Name: name, Email: email, Message: message })
            });

            let result = await response.text();
            if (response.ok) {
                showMessage(result, "success");
            } else {
                showMessage(result, "error");
            }
        } catch (error) {
            console.error("Fel skickades", error);
            showMessage("Fel vid sändning. Försök igen.", "error");
        }

        form.reset();
    });

        function showMessage(message, type) {
            messageElement.textContent = message;
            messageElement.classList.remove("success", "error");
            messageElement.classList.add(type);
            messageElement.style.display = "block";

            setTimeout(function () {
                messageElement.style.display = "none";
            }, 1000);
        }

        
    });