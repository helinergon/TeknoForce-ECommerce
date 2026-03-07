"use strict";

/* ===============================
   SAFE LOAD HANDLER
================================= */
window.addEventListener("load", function () {

    // Before / After
    const divisor = document.getElementById("divisor");
    const handle = document.getElementById("handle");
    const slider = document.getElementById("slider");

    if (slider && handle && divisor) {
        handle.style.left = slider.value + "%";
        divisor.style.width = slider.value + "%";
    }

    // Preloader
    const preloaderWrapper = document.getElementById("preloader");
    if (preloaderWrapper) {
        preloaderWrapper.classList.add("loaded");
    }

});


/* ===============================
   UTIL FUNCTIONS
================================= */

function TopOffset(el) {
    if (!el) return { top: 0 };
    let rect = el.getBoundingClientRect(),
        scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    return { top: rect.top + scrollTop };
}


/* ===============================
   HEADER STICKY
================================= */

const headerStickyWrapper = document.querySelector("header");
const headerStickyTarget = document.querySelector(".header__sticky");

if (headerStickyWrapper && headerStickyTarget) {
    window.addEventListener("scroll", function () {
        let StickyTargetElement = TopOffset(headerStickyWrapper);
        let TargetElementTopOffset = StickyTargetElement.top;

        if (window.scrollY > TargetElementTopOffset) {
            headerStickyTarget.classList.add("sticky");
        } else {
            headerStickyTarget.classList.remove("sticky");
        }
    });
}


/* ===============================
   SCROLL TOP
================================= */

const scrollTopBtn = document.getElementById("scroll__top");

if (scrollTopBtn) {
    scrollTopBtn.addEventListener("click", function () {
        window.scroll({ top: 0, left: 0, behavior: "smooth" });
    });

    window.addEventListener("scroll", function () {
        if (window.scrollY > 300) {
            scrollTopBtn.classList.add("active");
        } else {
            scrollTopBtn.classList.remove("active");
        }
    });
}


/* ===============================
   SWIPER SAFE INIT
================================= */

function initSwiper(selector, options) {
    if (typeof Swiper !== "undefined" && document.querySelector(selector)) {
        new Swiper(selector, options);
    }
}

initSwiper(".hero__slider--activation", {
    slidesPerView: 1,
    loop: true,
    speed: 500,
    spaceBetween: 30,
    pagination: { el: ".swiper-pagination", clickable: true }
});

initSwiper(".shop__collection--column5", {
    slidesPerView: 5,
    loop: true,
    spaceBetween: 30,
    navigation: { nextEl: ".swiper-button-next", prevEl: ".swiper-button-prev" }
});

initSwiper(".product__swiper--column4", {
    slidesPerView: 4,
    loop: true,
    spaceBetween: 30,
    navigation: { nextEl: ".swiper-button-next", prevEl: ".swiper-button-prev" }
});

initSwiper(".testimonial__swiper--activation", {
    slidesPerView: 4,
    loop: true,
    spaceBetween: 30,
    pagination: { el: ".swiper-pagination", clickable: true }
});

initSwiper(".blog__swiper--activation", {
    slidesPerView: 3,
    loop: true,
    spaceBetween: 30,
    navigation: { nextEl: ".swiper-button-next", prevEl: ".swiper-button-prev" }
});


/* ===============================
   OFFCANVAS
================================= */

function offcanvsSidebar(openTrigger, closeTrigger, wrapper) {

    let openButtons = document.querySelectorAll(openTrigger);
    let closeButton = document.querySelector(closeTrigger);
    let sidebar = document.querySelector(wrapper);

    if (!sidebar) return;

    openButtons.forEach(function (btn) {
        btn.addEventListener("click", function (e) {
            e.preventDefault();
            sidebar.classList.add("active");
            document.body.classList.add("sidebar_active");
        });
    });

    if (closeButton) {
        closeButton.addEventListener("click", function () {
            sidebar.classList.remove("active");
            document.body.classList.remove("sidebar_active");
        });
    }
}

offcanvsSidebar(".minicart__open--btn", ".minicart__close--btn", ".offCanvas__minicart");
offcanvsSidebar(".search__open--btn", ".predictive__search--close__btn", ".predictive__search--box");


/* ===============================
   LIGHTBOX SAFE
================================= */

if (typeof GLightbox !== "undefined") {
    GLightbox({ touchNavigation: true, loop: true });
}


/* ===============================
   COUNTER SAFE
================================= */

const counterWrapper = document.getElementById("funfactId");

if (counterWrapper) {
    const counters = counterWrapper.querySelectorAll(".js-counter");
    const duration = 1000;
    let isCounted = false;

    document.addEventListener("scroll", function () {
        const wrapperPos = counterWrapper.offsetTop - window.innerHeight;

        if (!isCounted && window.scrollY > wrapperPos) {
            counters.forEach((counter) => {
                const countTo = parseInt(counter.dataset.count);
                const countPerMs = countTo / duration;
                let currentCount = 0;

                const countInterval = setInterval(function () {
                    if (currentCount >= countTo) {
                        clearInterval(countInterval);
                    }
                    counter.textContent = Math.round(currentCount);
                    currentCount += countPerMs;
                }, 1);
            });

            isCounted = true;
        }
    });
}