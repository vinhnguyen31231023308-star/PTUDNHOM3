document.addEventListener("DOMContentLoaded", function () {
    // ===== FORM ĐĂNG NHẬP (DEMO) =====
    const loginForm = document.getElementById("loginForm");

    if (loginForm) {
        loginForm.addEventListener("submit", function (e) {
            e.preventDefault();

            const username = document.getElementById("username").value.trim();
            const password = document.getElementById("password").value.trim();

            if (!username || !password) {
                alert("Vui lòng nhập đầy đủ Tên đăng nhập và Mật khẩu.");
                return;
            }

            alert("Đăng nhập thành công (demo).");
            window.location.href = "user.html";
        });
    }

    // ===== FORM ĐĂNG KÝ (DEMO) =====
    const registerForm = document.getElementById("registerForm");

    if (registerForm) {
        registerForm.addEventListener("submit", function (e) {
            e.preventDefault();

            const fullname = document.getElementById("fullname").value.trim();
            const phone = document.getElementById("phone").value.trim();
            const pass = document.getElementById("reg-password").value.trim();
            const confirm = document.getElementById("confirm-password").value.trim();

            if (!fullname || !phone || !pass || !confirm) {
                alert("Vui lòng điền đầy đủ tất cả thông tin.");
                return;
            }

            if (pass !== confirm) {
                alert("Mật khẩu và Xác nhận mật khẩu không trùng khớp.");
                return;
            }

            alert("Tạo tài khoản thành công (demo).");
        });
    }

    // ===== HIỆU ỨNG BONG BÓNG =====
    const bubbles = document.querySelectorAll(".bubble");

    const configs = Array.from(bubbles).map((el, index) => {
        return {
            el,
            ampX: 300 + index * 8,
            ampY: 200 + index * 6,
            speedX: 0.0005 + index * 0.0001,
            speedY: 0.0004 + index * 0.00008,
            phaseX: Math.random() * Math.PI * 2,
            phaseY: Math.random() * Math.PI * 2
        };
    });

    function animateBubbles(timestamp) {
        configs.forEach((cfg) => {
            const offsetX =
                Math.sin(timestamp * cfg.speedX + cfg.phaseX) * cfg.ampX;
            const offsetY =
                Math.cos(timestamp * cfg.speedY + cfg.phaseY) * cfg.ampY;

            cfg.el.style.transform = `translate3d(${offsetX}px, ${offsetY}px, 0)`;
        });

        requestAnimationFrame(animateBubbles);
    }

    requestAnimationFrame(animateBubbles);
});


// ===== QUÊN MẬT KHẨU (DEMO) =====
const forgotForm = document.getElementById("forgotForm");

if (forgotForm) {
    forgotForm.addEventListener("submit", function (e) {
        e.preventDefault();

        const input = document.getElementById("forgot-input").value.trim();

        if (!input) {
            alert("Vui lòng nhập Email hoặc Số điện thoại.");
            return;
        }

        alert("Yêu cầu đặt lại mật khẩu đã được gửi (demo).");
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