document.addEventListener("DOMContentLoaded", () => {
    document.addEventListener("mousemove", (event) => {
        document.documentElement.style.setProperty(
            "--move-x",
            `${(event.clientX - window.innerWidth / 2) * -0.005}deg`
        );
        document.documentElement.style.setProperty(
            "--move-y",
            `${(event.clientY - window.innerHeight / 2) * -0.01}deg`
        );
    });
});