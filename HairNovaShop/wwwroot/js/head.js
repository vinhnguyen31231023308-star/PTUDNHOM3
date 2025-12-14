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

    // 4. Đóng thanh tìm kiếm
    closeSearchBtn.addEventListener('click', function () {
        searchOverlay.classList.remove('active');
    });

    // 5. Đóng khi nhấn phím ESC
    document.addEventListener('keydown', function (e) {
        if (e.key === "Escape" && searchOverlay.classList.contains('active')) {
            searchOverlay.classList.remove('active');
        }
    });
});
