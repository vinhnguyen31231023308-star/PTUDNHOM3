document.addEventListener('DOMContentLoaded', function () {
    // 1. QUAN TRỌNG: Tìm vùng bao quanh trang Shop
    const shopScope = document.querySelector('.shop');
    if (!shopScope) return;

    console.log("Đang ở trang Shop - Script đã được kích hoạt");

    // =========================================
    // DỮ LIỆU SẢN PHẨM (Nội bộ trong trang Shop)
    // =========================================
    const products = [
        { id: "DG1", name: "Dầu gội Dove Biotin Giảm Gãy Rụng/Hư Tổn", price: 195000, sold: 120, img: "img/imgshop/pd1.jpg", rating: 5.0, brand: "DOVE"},
        { id: "DG2", name: "Dầu Gội Khô Batiste Dry Shampoo 200ml", price: 150000, sold: 86, img: "img/imgshop/pd2.jpg", rating: 5.0, brand: "BATISTE" },
        { id: "DG3", name: "Dầu Gội Phủ Bạc Kella Nâu Tự Nhiên 150ml", price: 165000, sold: 62, img: "img/imgshop/pd3.jpg", rating: 5.0, brand: "KELLA" },
        { id: "DT1", name: "Nước Dưỡng Tóc Cocoon Tinh Dầu Bưởi 140ml", price: 110000, sold: 98, img: "img/imgshop/pd4.jpg", rating: 5.0, brand: "COCOON" },
        { id: "DG4", name: "Dầu Gội Phủ Bạc Lavox Health & Live", price: 40000, sold: 150, img: "img/imgshop/pd5.jpg", rating: 5.0, brand: "LAVOX" },
        { id: "DG5", name: "Dầu Gội L’Oreal Full Resist Purifying Shp", price: 225000, sold: 77, img: "img/imgshop/pd6.jpg", rating: 5.0, brand: "L'OREAL" },
        { id: "DG6", name: "Dầu Gội Bảo Vệ Màu Tóc L’Oreal 280ml", price: 130000, sold: 54, img: "img/imgshop/pd7.jpg", rating: 5.0, brand: "L'OREAL" },
        { id: "DG7", name: "Dầu Gội TRESemmé Detox Tóc Chắc Khỏe", price: 300000, sold: 39, img: "img/imgshop/pd8.jpg", rating: 5.0, brand: "TRESemmé" },
        { id: "DX1", name: "Dầu xả bưởi MILAGANICS 250ml", price: 129000, sold: 36, img: "img/imgshop/pd9.jpg", rating: 4.9, brand: "MILAGANICS" },
        { id: "DT2", name: "Tinh dầu Raip Argan Hair Oil R3 100ml", price: 130000, sold: 102, img: "img/imgshop/pd10.jpg", rating: 4.9, brand: "MILAGANICS" },
        { id: "DX2", name: "Dầu Xả Tsubaki chăm sóc tóc 450ml", price: 250000, sold: 98, img: "img/imgshop/pd11.jpg", rating: 5.0, brand: "TSUBAKI" },
        { id: "DT3", name: "Kem Ủ Tóc Ellips Pro-Keratin Complex 18g", price: 15000, sold: 73, img: "img/imgshop/pd12.png", rating: 4.8, brand: "ELLIPS" },
        { id: "DX3", name: "Dầu Xả L’Oreal Paris Cho Tóc Nhuộm 280ml", price: 118000, sold: 21, img: "img/imgshop/pd13.jpg", rating: 5.0, brand: "L'OREAL" },
        { id: "TN1", name: "Thuốc nhuộm tóc Davines màu 5.0", price: 229000, sold: 68, img: "img/imgshop/pd14.jpg", rating: 4.8, brand: "DAVINES" },
        { id: "TN2", name: "Thuốc Nhuộm Tóc GOLDWELL TOPCHIC", price: 18000, sold: 18, img: "img/imgshop/pd15.jpg", rating: 4.9, brand: "GOLDWELL" },
        { id: "TN3", name: "Thuốc Nhuộm Tóc Garnier Gói - GenB", price: 85000, sold: 72, img: "img/imgshop/pd16.jpg", rating: 4.8, brand: "GARNIER" },
        { id: "DT4", name: "Kem ủ tóc phục hồi Collagen KARSEELL dạng túi", price: 292000, sold: 30, img: "img/imgshop/pd17.jpg", rating: 4.9, brand: "KARSEELL" },
        { id: "DX4", name: "Kem Xả Dove Phục Hồi Hư Tổn 610g", price: 128000, sold: 64, img: "img/imgshop/pd18.jpg", rating: 5.0, brand: "DOVE" },
        { id: "DX5", name: "Dầu Xả Bưởi Cocoon Dưỡng Chất 310ml", price: 149000, sold: 82, img: "img/imgshop/pd19.jpg", rating: 5.0, brand: "COCOON" },
        { id: "DT5", name: "Kem Ủ Tóc Fino Premium Touch 230g", price: 135000, sold: 10, img: "img/imgshop/pd20.jpg", rating: 4.9, brand: "FINO" },
        { id: "DT6", name: "Serum Dưỡng Tóc Ellips Vitamin 6 Viên", price: 21000, sold: 49, img: "img/imgshop/pd21.jpg", rating: 4.8, brand: "ELLIPS" },
        { id: "DT7", name: "Xịt Tóc Double Rich Cho Tóc Hư Tổn 250ml", price: 55000, sold: 8, img: "img/imgshop/pd22.jpg", rating: 4.9, brand: "DOUBLE RICH" },
        { id: "DX6", name: "Dầu Xả TRESemmé Salon Detox Conditioner 620g", price: 230000, sold: 11, img: "img/imgshop/pd23.jpg", rating: 5.0, brand: "TRESemmé" }
    ];

    // Lấy các phần tử DOM bên trong shopScope
    const productGrid = shopScope.querySelector("#productGrid");
    const priceRange = shopScope.querySelector("#priceRange");
    const priceValue = shopScope.querySelector("#priceValue");
    const productCount = shopScope.querySelector("#productCount");

    // Nếu thiếu element quan trọng thì dừng để tránh lỗi
    if (!productGrid || !priceRange) return;

    const ITEMS_PER_PAGE = 16; // số sp mỗi trang
    let currentPage = 1;
    let currentFiltered = [];   // lưu danh sách sau khi lọc

    let currentCategory = "all";
    let currentSort = "popular";
    // =========================================
    // HÀM XỬ LÝ (FORMAT & RENDER)
    // =========================================
    function formatPrice(v) {
        return v.toLocaleString("vi-VN") + " đ";
    }

    function renderProducts(list) {
        productGrid.innerHTML = "";
        list.forEach((p) => {
            const oldPrice = Math.round(p.price * 1.25);
            const discount = Math.round((1 - p.price / oldPrice) * 100);

            const card = document.createElement("div");
            card.className = "product-card";
            card.dataset.id = p.id;

            card.innerHTML = `
              <button class="product-wishlist-btn" type="button" aria-label="Yêu thích">
                <i class="fa-regular fa-heart"></i>
              </button>

              <div class="product-image-wrapper">
                <img src="${p.img}" alt="${p.name}">
                <div class="product-overlay-actions">
                  <button class="product-overlay-btn secondary" type="button">Thêm vào giỏ hàng</button>
                  <button class="product-overlay-btn primary" type="button">Mua ngay</button>
                </div>
              </div>

              <a href="detail.html" class="product-link">
                <div class="product-meta-row">
                  <span class="product-rating"><span class="star">★</span> ${p.rating.toFixed(1)}</span>
                  <span class="product-sold">Đã bán ${p.sold}</span>
                </div>
                <h4 class="product-name">${p.name}</h4>
                <div class="product-price-row">
                  <span class="product-price-current">${formatPrice(p.price)}</span>
                  <span class="product-price-old">${formatPrice(oldPrice)}</span>
                  <span class="product-discount-badge">-${discount}%</span>
                </div>
              </a>
            `;
            productGrid.appendChild(card);
        });

        if (productCount) productCount.textContent = list.length;
    }

    // =========================================
    // LOGIC LỌC + SORT + PHÂN TRANG
    // =========================================
    function getSelectedBrands() {
        const checked = Array.from(shopScope.querySelectorAll(".brand-checkbox:checked"));
        return checked.map(c => c.value);
    }

    function applyFiltersAndSort() {
        const maxPrice = Number(priceRange.value);
        const selectedBrands = getSelectedBrands();

        let filtered = products.filter(p => {
            // category theo prefix id
            if (currentCategory !== "all" && !p.id.startsWith(currentCategory)) return false;
            // price
            if (p.price > maxPrice) return false;
            // brand checkbox
            if (selectedBrands.length > 0 && !selectedBrands.includes(p.brand)) return false;
            return true;
        });

        // sort
        switch (currentSort) {
            case "popular":   // bán chạy: sold desc
                filtered.sort((a, b) => b.sold - a.sold);
                break;
            case "new":       // mới nhất: sold asc
                filtered.sort((a, b) => a.sold - b.sold);
                break;
            case "priceAsc":  // giá tăng dần
                filtered.sort((a, b) => a.price - b.price);
                break;
            case "priceDesc": // giá giảm dần
                filtered.sort((a, b) => b.price - a.price);
                break;
        }

        // LƯU DANH SÁCH ĐÃ LỌC + SORT
        currentFiltered = filtered;

        // TÍNH SỐ TRANG
        const totalPages = Math.max(1, Math.ceil(currentFiltered.length / ITEMS_PER_PAGE));
        if (currentPage > totalPages) currentPage = totalPages;

        renderCurrentPage();
    }

    function renderCurrentPage() {
        const start = (currentPage - 1) * ITEMS_PER_PAGE;
        const end = start + ITEMS_PER_PAGE;
        const pageItems = currentFiltered.slice(start, end);
        renderProducts(pageItems);

        // cập nhật trạng thái nút trang
        const pageButtons = shopScope.querySelectorAll(".page-number");
        pageButtons.forEach(btn => btn.classList.remove("active"));
        const activeBtn = Array.from(pageButtons).find(
            btn => Number(btn.dataset.page) === currentPage
        );
        if (activeBtn) activeBtn.classList.add("active");
    }

    // =========================================
    // TOAST MESSAGE (Có thể để global hoặc local)
    // =========================================
    const toastEl = document.getElementById("toast"); // Toast thường nằm ngoài .shop cũng được
    const toastMsgEl = document.getElementById("toastMessage");
    let toastTimeout = null;

    function showToast(message) {
        if (!toastEl || !toastMsgEl) return;
        toastMsgEl.textContent = message;
        toastEl.classList.add("show");
        if (toastTimeout) clearTimeout(toastTimeout);
        toastTimeout = setTimeout(() => {
            toastEl.classList.remove("show");
        }, 2000);
    }

    // =========================================
    // CÁC SỰ KIỆN (EVENTS)
    // =========================================

    // 1. Category Buttons
    shopScope.querySelectorAll(".cat-btn").forEach(btn => {
        btn.addEventListener("click", () => {
            shopScope.querySelectorAll(".cat-btn").forEach(b => b.classList.remove("active"));
            btn.classList.add("active");
            currentCategory = btn.dataset.category;
            applyFiltersAndSort();
        });
    });

    // 2. Sort Tabs
    shopScope.querySelectorAll(".sort-tab").forEach(tab => {
        tab.addEventListener("click", () => {
            shopScope.querySelectorAll(".sort-tab").forEach(t => t.classList.remove("active"));
            tab.classList.add("active");
            currentSort = tab.dataset.sort;
            applyFiltersAndSort();
        });
    });

    // 3. Price Range
    priceRange.addEventListener("input", () => {
        if (priceValue) priceValue.textContent = formatPrice(Number(priceRange.value));
        applyFiltersAndSort();
    });

    // 4. Brand Checkboxes
    shopScope.querySelectorAll(".brand-checkbox").forEach(cb => {
        cb.addEventListener("change", applyFiltersAndSort);
    });

    // 5. Event Delegation cho Card Sản Phẩm (Yêu thích, Giỏ hàng, Mua ngay)
    productGrid.addEventListener("click", (e) => {
        const target = e.target;

        // Bấm icon TIM
        if (target.closest(".product-wishlist-btn")) {
            const btn = target.closest(".product-wishlist-btn");
            const icon = btn.querySelector("i");
            btn.classList.toggle("active");

            // Đổi icon regular ↔ solid
            if (btn.classList.contains("active")) {
                icon.classList.remove("fa-regular");
                icon.classList.add("fa-solid");
            } else {
                icon.classList.remove("fa-solid");
                icon.classList.add("fa-regular");
            }

            const card = btn.closest(".product-card");
            const id = card?.dataset.id;
            if (btn.classList.contains("active")) {
                showToast(`Đã thêm sản phẩm ${id} vào danh sách yêu thích`);
            } else {
                showToast(`Đã bỏ yêu thích sản phẩm ${id}`);
            }
            return;
        }

        // Bấm THÊM GIỎ HÀNG
        if (target.closest(".product-overlay-btn.secondary")) {
            const card = target.closest(".product-card");
            const name = card?.querySelector(".product-name")?.textContent || "sản phẩm";
            showToast(`Đã thêm "${name}" vào giỏ hàng`);
            return;
        }

        // Bấm MUA NGAY
        if (target.closest(".product-overlay-btn.primary")) {
            const card = target.closest(".product-card");
            const name = card?.querySelector(".product-name")?.textContent || "sản phẩm";
            showToast(`Đặt mua "${name}" thành công!`);
            return;
        }
    });

    // 6. Pagination
    const paginationEl = shopScope.querySelector(".pagination");
    if (paginationEl) {
        paginationEl.addEventListener("click", (e) => {
            const btn = e.target.closest(".page-btn");
            if (!btn) return;

            const type = btn.dataset.page;
            const totalPages = Math.max(1, Math.ceil(currentFiltered.length / ITEMS_PER_PAGE));

            if (type === "prev") {
                if (currentPage > 1) {
                    currentPage--;
                    renderCurrentPage();
                }
            } else if (type === "next") {
                if (currentPage < totalPages) {
                    currentPage++;
                    renderCurrentPage();
                }
            } else {
                currentPage = Number(type);
                renderCurrentPage();
            }
        });
    }

    // =========================================
    // KHỞI CHẠY LẦN ĐẦU (INIT)
    // =========================================
    if (priceValue) priceValue.textContent = formatPrice(Number(priceRange.value));
    applyFiltersAndSort();
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