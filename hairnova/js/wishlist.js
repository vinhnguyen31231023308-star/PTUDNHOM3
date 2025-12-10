  // TAB "BẠN CÓ CẦN THÊM / DEAL SỐC"
  const tabs = document.querySelectorAll(".recommend-tab");
  const panels = document.querySelectorAll(".recommend-panel");

  tabs.forEach((tab) => {
    tab.addEventListener("click", () => {
      const targetId = tab.dataset.target;

      tabs.forEach((t) => t.classList.remove("active"));
      panels.forEach((p) => p.classList.remove("active"));

      tab.classList.add("active");
      const panel = document.getElementById(targetId);
      if (panel) panel.classList.add("active");
    });
  });

  // SLIDER CHO TAB "BẠN CÓ CẦN THÊM?"
  const track = document.getElementById("moreTrack");
  const prevBtn = document.querySelector(".product-slider .prev");
  const nextBtn = document.querySelector(".product-slider .next");

  if (track && prevBtn && nextBtn) {
    const scrollAmount = 200; // px mỗi lần bấm

    prevBtn.addEventListener("click", () => {
      track.scrollBy({ left: -scrollAmount, behavior: "smooth" });
    });
    nextBtn.addEventListener("click", () => {
      track.scrollBy({ left: scrollAmount, behavior: "smooth" });
    });
  };

    // NÚT TRÁI TIM YÊU THÍCH CHO SẢN PHẨM GỢI Ý
  document.querySelectorAll(".product-wishlist-btn").forEach((btn) => {
    btn.addEventListener("click", () => {
      btn.classList.toggle("active");

      const icon = btn.querySelector("i");
      if (!icon) return;

      // đổi giữa tim rỗng và tim đầy
      if (btn.classList.contains("active")) {
        icon.classList.remove("fa-regular");
        icon.classList.add("fa-solid");
      } else {
        icon.classList.remove("fa-solid");
        icon.classList.add("fa-regular");
      }
    });
  });

// ================== DỮ LIỆU SẢN PHẨM GỢI Ý ==================
const PRODUCTS = {
  1: {
    id: 1,
    brand: "DOVE",
    name: "Dầu gội Dove Biotin Giảm Gãy Rụng/Hư Tổn",
    img: "img/imgwishlist/pd1.jpg",
    priceText: "195.000 đ",
  },
  2: {
    id: 2,
    brand: "BATISTE",
    name: "Dầu Gội Khô Batiste Dry Shampoo 200ml",
    img: "img/imgwishlist/pd2.jpg",
    priceText: "150.000 đ",
  },
  3: {
    id: 3,
    brand: "KELLA",
    name: "Dầu Gội Phủ Bạc Kella Nâu Tự Nhiên 150ml",
    img: "img/imgwishlist/pd3.jpg",
    priceText: "165.000 đ",
  },
  4: {
    id: 4,
    brand: "COCOON",
    name: "Nước Dưỡng Tóc Cocoon Tinh Dầu Bưởi 140ml",
    img: "img/imgwishlist/pd4.jpg",
    priceText: "110.000 đ",
  },
  5: {
    id: 5,
    brand: "LAVOX",
    name: "Dầu Gội Phủ Bạc Lavox Health & Live",
    img: "img/imgwishlist/pd5.jpg",
    priceText: "40.000 đ",
  },
  6: {
    id: 6,
    brand: "L'OREAL",
    name: "Dầu Gội L’Oreal Full Resist Purifying Shp",
    img: "img/imgwishlist/pd6.jpg",
    priceText: "225.000 đ",
  },
  7: {
    id: 7,
    brand: "L'OREAL",
    name: "Dầu Gội Bảo Vệ Màu Tóc L’Oreal 280ml",
    img: "img/imgwishlist/pd7.jpg",
    priceText: "130.000 đ",
  },
  8: {
    id: 8,
    brand: "TRESemmé",
    name: "Dầu Gội TRESemmé Detox Tóc Chắc Khỏe",
    img: "img/imgwishlist/pd8.jpg",
    priceText: "300.000 đ",
  },
};

// ================== WISHLIST ==================

// Cập nhật số lượng sản phẩm trong tiêu đề
function updateWishlistCount() {
  const rows = document.querySelectorAll(".wishlist-row");
  const countSpan = document.querySelector(".wishlist-count");
  if (countSpan) {
    countSpan.textContent = `(${rows.length} sản phẩm)`;
  }
}

// Xóa sản phẩm khi bấm nút X trên bảng
function initRemoveButtons() {
  document.addEventListener("click", function (e) {
    const removeBtn = e.target.closest(".btn-remove");
    if (!removeBtn) return;

    const row = removeBtn.closest(".wishlist-row");
    if (!row) return;

    const id = row.getAttribute("data-id");

    // Xóa dòng trong bảng
    row.remove();
    updateWishlistCount();

    // Tắt tim tương ứng trong khu gợi ý (nếu đang bật)
    if (id) {
      const card = document.querySelector(`.product-card[data-id="${id}"]`);
      if (card) {
        const heartBtn = card.querySelector(".product-wishlist-btn");
        const icon = heartBtn ? heartBtn.querySelector("i") : null;
        if (heartBtn && icon) {
          heartBtn.classList.remove("active");
          icon.classList.remove("fa-solid");
          icon.classList.add("fa-regular");
        }
      }
    }
  });
}

// Handler cho nút xem chi tiết / thêm giỏ hàng (demo)
function initActionButtons() {
  document.addEventListener("click", function (e) {
    if (e.target.classList.contains("btn-detail")) {
      e.preventDefault();
      alert("Đi tới trang chi tiết sản phẩm.");
    }

    if (e.target.classList.contains("btn-add-cart")) {
      e.preventDefault();
      if (e.target.disabled) return;
      alert("Thêm vào giỏ hàng.");
    }
  });
}

// Thêm / gỡ sản phẩm khỏi wishlist khi bấm tim trong gợi ý
function initHeartButtons() {
  const heartBtns = document.querySelectorAll(".product-wishlist-btn");

  heartBtns.forEach((btn) => {
    btn.addEventListener("click", function () {
      const card = btn.closest(".product-card");
      if (!card) return;

      const id = card.getAttribute("data-id");
      if (!id || !PRODUCTS[id]) return;

      const icon = btn.querySelector("i");
      const exists = document.querySelector(`.wishlist-row[data-id="${id}"]`);

      // Nếu đã có trong wishlist -> gỡ ra + tắt tim
      if (exists) {
        exists.remove();
        btn.classList.remove("active");
        if (icon) {
          icon.classList.remove("fa-solid");
          icon.classList.add("fa-regular");
        }
        updateWishlistCount();
        return;
      }

      // Chưa có -> bật tim + thêm dòng mới vào bảng
      btn.classList.add("active");
      if (icon) {
        icon.classList.remove("fa-regular");
        icon.classList.add("fa-solid");
      }

      const product = PRODUCTS[id];
      const today = new Date();
      const dateStr = today.toLocaleDateString("vi-VN");

      const row = document.createElement("div");
      row.className = "wishlist-row";
      row.setAttribute("data-id", product.id);

      row.innerHTML = `
        <div class="w-col w-col-product">
          <img src="${product.img}" alt="${product.name}" class="w-thumb">
          <div class="w-prod-info">
            <div class="w-prod-brand">${product.brand}</div>
            <a href="#" class="w-prod-name">${product.name}</a>
          </div>
        </div>
        <div class="w-col w-col-date">
          <span class="w-date">${dateStr}</span>
        </div>
        <div class="w-col w-col-status">
          <span class="w-status w-status-instock">Còn hàng</span>
        </div>
        <div class="w-col w-col-price">
          <span class="w-price">${product.priceText}</span>
        </div>
        <div class="w-col w-col-actions">
          <button class="btn-outline btn-detail">Xem chi tiết</button>
          <button class="btn-primary btn-add-cart">Thêm vào giỏ hàng</button>
          <button class="btn-icon btn-remove" aria-label="Xóa sản phẩm">
            <i class="fa-solid fa-xmark"></i>
          </button>
        </div>
      `;

      const table = document.querySelector(".wishlist-table");
      if (table) {
        table.appendChild(row);
        updateWishlistCount();
      }
    });
  });
}

// ================== KHỞI TẠO CHUNG ==================
document.addEventListener("DOMContentLoaded", function () {
  updateWishlistCount();
  initRemoveButtons();
  initActionButtons();
  initHeartButtons();

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