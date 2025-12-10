const observer = new IntersectionObserver((entries, obs) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add('show');
            if (entry.target.querySelector('.stat-number')) {
                runCounters(entry.target);
            }

            obs.unobserve(entry.target);
        }
    });
}, { threshold: 0.1 });

document.querySelectorAll('.hidden-up, .hidden-left, .hidden-right').forEach((el) => observer.observe(el));

// (CHẠY SỐ)
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
       SCROLL RESTORATION
       ========================================= */
    if (history.scrollRestoration) {
        history.scrollRestoration = 'manual';
    } else {
        window.onbeforeunload = function () {
            window.scrollTo(0, 0);
        }
    }
