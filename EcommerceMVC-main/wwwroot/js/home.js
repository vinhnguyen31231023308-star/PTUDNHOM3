document.addEventListener('DOMContentLoaded', function () {
    const homeScope = document.querySelector('.home');

    if (!homeScope) return;

    console.log("Website HairNova (Home Scope) đã tải xong!");

    /* =========================================
       BUTTON EFFECTS (Scoped)
       ========================================= */
    const buttons = homeScope.querySelectorAll('.btn');
    buttons.forEach(btn => {
        btn.addEventListener('click', function (e) {
            if (this.getAttribute('href') === '#') {
                e.preventDefault();
            }
        });
    });

    /* =========================================
       SCROLL RESTORATION (Giữ nguyên logic)
       ========================================= */
    if (history.scrollRestoration) {
        history.scrollRestoration = 'manual';
    } else {
        window.onbeforeunload = function () {
            window.scrollTo(0, 0);
        }
    }

    /* =========================================
       INTERSECTION OBSERVER & ANIMATION 
       ========================================= */
    const observerOptions = {
        root: null,
        rootMargin: "0px",
        threshold: 0.1
    };

    const observer = new IntersectionObserver((entries, obs) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('show');

                // Kích hoạt chạy số nếu có
                if (entry.target.querySelector('.stat-number')) {
                    runCounters(entry.target);
                }
                obs.unobserve(entry.target);
            }
        });
    }, observerOptions);

    // Chỉ tìm các phần tử hidden bên trong .home
    const hiddenElements = homeScope.querySelectorAll('.hidden-up, .hidden-left, .hidden-right');
    hiddenElements.forEach((el) => observer.observe(el));

    /* =========================================
       COUNTER LOGIC (Hàm nội bộ)
       ========================================= */
    function runCounters(container) {
        const counters = container.querySelectorAll('.stat-number');
        const speed = 1000;

        counters.forEach(counter => {
            const animate = () => {
                const value = +counter.getAttribute('data-target');
                const data = +counter.innerText;
                const time = value / speed;

                if (data < value) {
                    counter.innerText = Math.ceil(data + time + 1);
                    setTimeout(animate, 30);
                } else {
                    counter.innerText = value + "+";
                }
            }
            animate();
        });
    }

    /* =========================================
       GLOBAL FUNCTIONS (Gắn vào window để gọi từ HTML onclick)
       ========================================= */

    // Slider Sản phẩm
    window.moveSlider = function (direction) {
        // Tìm ID bên trong .home để tránh nhầm với trang khác
        const track = homeScope.querySelector('#productTrack');
        if (!track) return;

        const cardWidth = 310;
        const scrollAmount = cardWidth * 2;
        if (direction === 1) {
            track.scrollBy({ left: scrollAmount, behavior: 'smooth' });
        } else {
            track.scrollBy({ left: -scrollAmount, behavior: 'smooth' });
        }
    };

    // Slider New Arrival
    window.moveArrivalSlider = function (direction) {
        const track = homeScope.querySelector('#arrivalTrack');
        if (!track) return;

        const cardWidth = 310;
        const scrollAmount = cardWidth;

        if (direction === 1) {
            track.scrollBy({ left: scrollAmount, behavior: 'smooth' });
        } else {
            track.scrollBy({ left: -scrollAmount, behavior: 'smooth' });
        }
    };

    // Copy Code Coupon
    window.copyCode = function (btn, code) {
        navigator.clipboard.writeText(code);
        const originalText = btn.innerText;
        btn.innerText = "ĐÃ CHÉP";
        btn.style.backgroundColor = "#ffb524";

        setTimeout(() => {
            btn.innerText = originalText;
            btn.style.backgroundColor = "";
        }, 2000);
    };

    /* =========================================
       SWIPER SLIDER 
       ========================================= */
    // Chỉ khởi tạo Swiper nếu tìm thấy class trong .home
    if (homeScope.querySelector('.myTestimonialSwiper')) {

        // Logic clone slide (giữ nguyên logic cũ của bạn)
        var swiperWrapper = homeScope.querySelector('.myTestimonialSwiper .swiper-wrapper');
        if (swiperWrapper) {
            var slides = swiperWrapper.querySelectorAll('.swiper-slide');
            if (slides.length > 0 && slides.length < 6) {
                slides.forEach(function (slide) {
                    swiperWrapper.appendChild(slide.cloneNode(true));
                });
                slides.forEach(function (slide) {
                    swiperWrapper.appendChild(slide.cloneNode(true));
                });
            }
        }

        // Khởi tạo Swiper với selector cụ thể .home
        var swiper = new Swiper(".home .myTestimonialSwiper", {
            slidesPerView: 1,
            centeredSlides: true,
            spaceBetween: 10,
            loop: true,
            initialSlide: 1,
            pagination: {
                el: ".home .swiper-pagination", // Scope pagination
                clickable: true,
            },
            navigation: {
                nextEl: ".home .next-btn", // Scope buttons
                prevEl: ".home .prev-btn",
            },
            autoplay: {
                delay: 4000,
                disableOnInteraction: false,
            },
            breakpoints: {
                768: { slidesPerView: 2, spaceBetween: 20 },
                1024: { slidesPerView: 3, spaceBetween: 30 }
            },
            on: {
                init: function () { this.update(); },
                resize: function () { this.update(); }
            }
        });
    }

    /* =========================================
       NEWSLETTER FORM
       ========================================= */
    const form = homeScope.querySelector('#newsletterForm');
    const messageBox = homeScope.querySelector('#formMessage');
    const emailInput = homeScope.querySelector('#emailInput');

    if (form) {
        form.addEventListener('submit', function (event) {
            event.preventDefault();
            const email = emailInput.value;
            if (email.includes('@')) {
                messageBox.style.display = 'block';
                messageBox.style.color = '#fff';
                messageBox.textContent = `Tuyệt vời! Email ${email} đã được đăng ký.`;
                emailInput.value = '';
                setTimeout(() => { messageBox.style.display = 'none'; }, 4000);
            }
        });
    }

    /* =========================================
       LOADING SPINNER 
       ========================================= */
    var spinner = homeScope.querySelector('#spinner');
    if (spinner) {
        setTimeout(function () {
            spinner.classList.remove('show');
        }, 500);
    }
});