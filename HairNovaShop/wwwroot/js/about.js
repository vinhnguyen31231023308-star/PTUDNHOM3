document.addEventListener("DOMContentLoaded", function () {
    const observerOptions = {
        root: null,
        rootMargin: "0px",
        threshold: 0.1 
    };

    const observer = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add("show");
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);
    const hiddenElements = document.querySelectorAll('.hidden-up, .hidden-left, .hidden-right');
    hiddenElements.forEach((el) => {
        // Check if element is already in viewport on load
        const rect = el.getBoundingClientRect();
        const isInViewport = rect.top < window.innerHeight && rect.bottom > 0;
        if (isInViewport) {
            // Show immediately if already in viewport
            setTimeout(() => el.classList.add("show"), 100);
        } else {
            // Otherwise observe for intersection
            observer.observe(el);
        }
    });

    // Animate stats numbers
    const statNumbers = document.querySelectorAll('.stat-number');
    statNumbers.forEach(stat => {
        const target = parseInt(stat.getAttribute('data-target'));
        const duration = 2000;
        const increment = target / (duration / 16);
        let current = 0;
        
        const updateStat = () => {
            current += increment;
            if (current < target) {
                stat.textContent = Math.floor(current);
                requestAnimationFrame(updateStat);
            } else {
                stat.textContent = target;
            }
        };
        
        const statObserver = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    updateStat();
                    statObserver.unobserve(entry.target);
                }
            });
        });
        statObserver.observe(stat);
    });
});
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
