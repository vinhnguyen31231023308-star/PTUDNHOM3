document.addEventListener('DOMContentLoaded', function () {
    // 1. Khai báo các phần tử
    const searchBtn = document.getElementById('searchBtn');       // Nút kính lúp
    const searchOverlay = document.getElementById('searchOverlay'); // Khung tìm kiếm
    const closeSearchBtn = document.getElementById('closeSearchBtn'); // Nút đóng
    const searchInput = document.getElementById('searchInput');   // Ô nhập liệu

    // 2. Kiểm tra an toàn (Debug)
    if (!searchBtn || !searchOverlay || !closeSearchBtn) {
        console.error("Lỗi: Không tìm thấy ID của các phần tử tìm kiếm trong HTML.");
        return;
    }

    // 3. Mở thanh tìm kiếm
    searchBtn.addEventListener('click', function (e) {
        e.preventDefault();
        searchOverlay.classList.add('active');
        // Tự động focus vào ô nhập sau 0.2s để animation chạy xong
        setTimeout(() => searchInput.focus(), 200);
    });

    // 5. Đóng thanh tìm kiếm
    closeSearchBtn.addEventListener('click', function () {
        searchOverlay.classList.remove('active');
    });

    // 5. Đóng khi nhấn phím ESC
    document.addEventListener('keydown', function (e) {
        if (e.key === "Escape" && searchOverlay.classList.contains('active')) {
            searchOverlay.classList.remove('active');
        }
    });

    // 7. Xử lý submit form tìm kiếm
    const searchSubmitBtn = document.querySelector('.btn-search-submit');
    if (searchInput && searchSubmitBtn) {
        // Submit khi nhấn Enter trong ô input
        searchInput.addEventListener('keypress', function (e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                performSearch();
            }
        });

        // Submit khi click nút "Tìm kiếm"
        searchSubmitBtn.addEventListener('click', function (e) {
            e.preventDefault();
            performSearch();
        });

        function performSearch() {
            const searchTerm = searchInput.value.trim();
            if (searchTerm) {
                // Chuyển đến trang Shop với parameter search
                const url = new URL('/Shop', window.location.origin);
                url.searchParams.set('search', searchTerm);
                window.location.href = url.toString();
            } else {
                // Nếu không có từ khóa, chỉ chuyển đến Shop
                window.location.href = '/Shop';
            }
        }
    }

    // 6. User dropdown menu (Additional for ASP.NET Core)
    const userBtn = document.getElementById('userBtn');
    const userDropdown = document.querySelector('.user-dropdown');
    const userDropdownMenu = document.getElementById('userDropdownMenu');

    if (userBtn && userDropdown) {
        // Toggle dropdown khi click vào icon user
        userBtn.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            userDropdown.classList.toggle('active');
        });

        // Đóng dropdown khi click ra ngoài
        document.addEventListener('click', function (e) {
            if (userDropdown && !userDropdown.contains(e.target)) {
                userDropdown.classList.remove('active');
            }
        });

        // Đóng dropdown khi nhấn phím ESC
        document.addEventListener('keydown', function (e) {
            if (e.key === "Escape" && userDropdown && userDropdown.classList.contains('active')) {
                userDropdown.classList.remove('active');
            }
        });

        // Đóng dropdown khi click vào link trong menu
        if (userDropdownMenu) {
            userDropdownMenu.addEventListener('click', function (e) {
                // Chỉ đóng nếu click vào link (không phải divider)
                if (e.target.tagName === 'A') {
                    setTimeout(() => {
                        userDropdown.classList.remove('active');
                    }, 100);
                }
            });
        }
    }
});
