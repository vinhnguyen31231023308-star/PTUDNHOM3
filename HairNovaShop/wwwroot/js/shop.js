document.addEventListener('DOMContentLoaded', function () {
    const shopScope = document.querySelector('.shop');
    if (!shopScope) return;

    const priceRange = shopScope.querySelector("#priceRange");
    const priceValue = shopScope.querySelector("#priceValue");
    const sortTabs = shopScope.querySelectorAll(".sort-tab");
    const brandCheckboxes = shopScope.querySelectorAll(".brand-checkbox");

    // Format price
    function formatPrice(v) {
        return new Intl.NumberFormat('vi-VN').format(v) + " đ";
    }

    // Update price display
    if (priceRange && priceValue) {
        priceRange.addEventListener("input", () => {
            priceValue.textContent = formatPrice(Number(priceRange.value));
        });
    }

    // Sort tabs - redirect with sort parameter
    sortTabs.forEach(tab => {
        tab.addEventListener("click", function() {
            const sort = this.dataset.sort;
            const url = new URL(window.location.href);
            url.searchParams.set('sort', sort);
            url.searchParams.delete('page'); // Reset to page 1 when sorting
            window.location.href = url.toString();
        });
    });

    // Brand checkboxes - redirect with brand parameter
    brandCheckboxes.forEach(cb => {
        cb.addEventListener("change", function() {
            const url = new URL(window.location.href);
            if (this.checked) {
                url.searchParams.set('brand', this.value);
            } else {
                url.searchParams.delete('brand');
            }
            url.searchParams.delete('page'); // Reset to page 1 when filtering
            window.location.href = url.toString();
        });
    });

    // Price range - redirect with maxPrice parameter
    if (priceRange) {
        let priceTimeout = null;
        priceRange.addEventListener("input", function() {
            clearTimeout(priceTimeout);
            priceTimeout = setTimeout(() => {
                const url = new URL(window.location.href);
                url.searchParams.set('maxPrice', this.value);
                url.searchParams.delete('page'); // Reset to page 1 when filtering
                window.location.href = url.toString();
            }, 500); // Debounce 500ms
        });
    }

    // Wishlist toggle
    window.toggleWishlist = function(productId, btn) {
        const icon = btn.querySelector("i");
        btn.classList.toggle("active");

        if (btn.classList.contains("active")) {
            icon.classList.remove("fa-regular");
            icon.classList.add("fa-solid");
            showToast("Đã thêm sản phẩm vào danh sách yêu thích");
        } else {
            icon.classList.remove("fa-solid");
            icon.classList.add("fa-regular");
            showToast("Đã bỏ yêu thích sản phẩm");
        }
    };

    // Add to cart
    window.addToCart = function(productId) {
        // TODO: Implement cart functionality
        showToast("Đã thêm sản phẩm vào giỏ hàng");
    };

    // Buy now
    window.buyNow = function(productId) {
        // TODO: Implement buy now functionality
        showToast("Đặt mua sản phẩm thành công!");
    };

    // Toast message
    const toastEl = document.getElementById("toast");
    const toastMsgEl = document.getElementById("toastMessage");
    let toastTimeout = null;

    window.showToast = function(message) {
        if (!toastEl || !toastMsgEl) return;
        toastMsgEl.textContent = message;
        toastEl.classList.add("show");
        if (toastTimeout) clearTimeout(toastTimeout);
        toastTimeout = setTimeout(() => {
            toastEl.classList.remove("show");
        }, 2000);
    };
});
