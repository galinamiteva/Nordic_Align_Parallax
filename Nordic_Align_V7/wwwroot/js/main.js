


document.addEventListener("DOMContentLoaded", function () {
    console.log("main.js are here");

    // Бургер меню
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


    const menuLinks = document.querySelectorAll(".menu__list-link");

    menuLinks.forEach((link) => {
        link.addEventListener("click", function (e) {
            e.preventDefault();
            const targetId = this.getAttribute("href").substring(1);
            const targetElement = document.getElementById(targetId);

            if (targetElement) {
                window.scrollTo({
                    top: targetElement.offsetTop - 50,
                    behavior: "smooth",
                });
            }
        });
    });
});

