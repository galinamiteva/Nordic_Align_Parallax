document.addEventListener("DOMContentLoaded", function () {
    console.log("main.js are here");

    const BurgerBtn = document.querySelector(".burger");
    const BurgerMenu = document.querySelector(".menu");

    if (BurgerBtn && BurgerMenu) {
        BurgerBtn.addEventListener("click", function () {
            this.classList.toggle("burger--active");
            BurgerMenu.classList.toggle("menu--active");
        });
    } else {
        console.error("Burger button or menu not found!");
    }

    document.addEventListener("scroll", function () {
        document.body.style.setProperty("--scrollTop", `${window.scrollY}px`);
    });

    setTimeout(() => {
        const menuLinks = document.querySelectorAll(".menu__list-link");

        menuLinks.forEach((link) => {
            link.addEventListener("click", function (e) {
                let targetHref = this.getAttribute("href"); // Use getAttribute

                console.log("Clicked link:", targetHref);

                if (!targetHref || targetHref === "#") {
                    console.log("Invalid href, allowing navigation.");
                    return; 
                }

                if (targetHref.startsWith("http") || targetHref.startsWith("/") || this.hasAttribute("asp-controller")) {
                    console.log("External or ASP.NET link - allowing navigation.");
                    return;
                }

                e.preventDefault();
                console.log("Prevented default navigation for:", targetHref);

                const targetId = targetHref.split("#")[1];
                const targetElement = document.getElementById(targetId);

                if (targetElement) {
                    console.log("Scrolling to:", targetId);
                    window.scrollTo({
                        top: targetElement.offsetTop - 50,
                        behavior: "smooth",
                    });
                }
            });
        });
    }, 100); 
});
