console.log("JS đã load thành công!");
// 1. ẨN / HIỆN KHỐI "GIAO HÀNG ĐẾN ĐỊA CHỈ KHÁC?"
const shipOtherCheckbox = document.getElementById("shipOther");
const shipOtherBlock = document.getElementById("shipping-other-block");

if (shipOtherCheckbox && shipOtherBlock) {
  shipOtherCheckbox.addEventListener("change", () => {
    // nếu được tick thì bỏ class hidden, không tick thì thêm lại
    shipOtherBlock.classList.toggle("hidden", !shipOtherCheckbox.checked);
  });
}

// 2. DEMO VOUCHER
const voucherInput = document.getElementById("voucher");
const voucherMsg = document.getElementById("voucherMsg");
const totalSpan = document.querySelector(".order-total");
const applyBtn = document.querySelector(".btn-apply");

if (voucherInput && voucherMsg && totalSpan && applyBtn) {
  applyBtn.addEventListener("click", () => {
    const code = voucherInput.value.trim().toUpperCase();

    if (!code) {
      voucherMsg.textContent = "Vui lòng nhập mã voucher.";
      voucherMsg.style.color = "#c53030";
      return;
    }

    if (code === "HAIR10") {
      voucherMsg.textContent = "Áp dụng mã HAIR10 thành công: giảm 10%.";
      voucherMsg.style.color = "green";
      // demo: 598.000 đ giảm 10% = 538.200 đ
      totalSpan.textContent = "538.200 đ";
    } else {
      voucherMsg.textContent = "Mã không hợp lệ hoặc đã hết hạn.";
      voucherMsg.style.color = "#c53030";
    }
  });
}

// 3. DEMO NÚT ĐẶT HÀNG
const orderBtn = document.querySelector(".btn-full");
if (orderBtn) {
  orderBtn.addEventListener("click", () => {
    alert("Đơn hàng của bạn đã được ghi nhận (demo).");
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