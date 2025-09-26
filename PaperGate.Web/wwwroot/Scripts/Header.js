const haumburgerMenu = document.querySelector("#mother-bars"); // آیکون bars
const navbar = document.querySelector("#nav");
const closeBtn = document.getElementById("fa-x");

haumburgerMenu.addEventListener("click", () => {
    navbar.classList.add("visible");   // نمایش نوبار
    closeBtn.classList.add("show");    // نمایش ایکس
});

closeBtn.addEventListener("click", () => {
    navbar.classList.remove("visible");
    closeBtn.classList.remove("show"); // مخفی‌کردن ایکس
});