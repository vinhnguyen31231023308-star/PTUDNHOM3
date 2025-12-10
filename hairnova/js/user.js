document.addEventListener("DOMContentLoaded", function () {
    setTimeout(() => {
        document.querySelectorAll('.hidden-up, .hidden-left').forEach(el => el.classList.add('show'));
    }, 100);

    loadDashboardData();
    renderOrders('all');
    const allButton = document.querySelector('.btn-filter');
    if (allButton) allButton.classList.add('active');
});

function openTab(tabName, element) {
    // 1. Ẩn tất cả nội dung tab
    var i, tabContent;
    tabContent = document.getElementsByClassName("tab-pane");
    for (i = 0; i < tabContent.length; i++) {
        tabContent[i].classList.remove("active");
    }

    // 2. Xóa trạng thái active của menu
    var tabLinks = document.getElementsByClassName("menu-item");
    for (i = 0; i < tabLinks.length; i++) {
        tabLinks[i].classList.remove("active");
    }

    // 3. Hiển thị tab được chọn
    document.getElementById(tabName).classList.add("active");

    // 4. Active menu tương ứng
    if (element) {
        element.classList.add("active");
    } else {
        if (tabName === 'tracking') tabLinks[1].classList.add("active");
        if (tabName === 'orders') tabLinks[2].classList.add("active");
        if (tabName === 'address') tabLinks[3].classList.add("active");
    }
}

function handleLogout() {
    if (confirm("Bạn có chắc chắn muốn đăng xuất không?")) {
        window.location.href = 'index.html';
    }
}

function closeAddressModal() {
    document.getElementById('address-modal').classList.remove('show');
}

function openEditAddressModal(addressId) {
    document.getElementById('modal-title').textContent = "Cập nhật địa chỉ";
    document.getElementById('address-modal').classList.add('show');
}

function openAddAddressModal() {
    document.getElementById('modal-title').textContent = "Thêm địa chỉ mới";
    document.getElementById('address-form').reset();
    document.getElementById('address-modal').classList.add('show');
}

// ==========================================
// DỮ LIỆU ĐƠN HÀNG (ĐÃ SỬA LỖI DẤU SAO)
// ==========================================
const ordersData = [
    {
        id: 'DH-40115',
        date: '29/11/2025',
        status: 'processing',
        statusText: 'Đang xử lý',
        total: '420.000đ',
        productName: 'Combo 2 chai Xịt Dưỡng Ẩm Chuyên Sâu',
        productDesc: '150ml • SL: 1',
        img: 'img/imgshop/pd2.jpg',
        payment: 'COD (Thanh toán khi nhận)',
        // SỬA: Dùng thẻ <strong> thay vì **
        logs: 'Đơn hàng của bạn đang được <strong>Đóng gói</strong> và chuẩn bị bàn giao cho đơn vị vận chuyển.'
    },
    {
        id: 'DH-39824',
        date: '20/11/2025',
        status: 'success',
        statusText: 'Giao thành công',
        total: '250.000đ',
        productName: 'Dầu Gội Tinh Chất Bưởi & Trà Xanh',
        productDesc: '500ml • SL: 1',
        img: 'img/imgshop/pd2.jpg',
        payment: 'Chuyển khoản',
        // SỬA: Dùng thẻ <strong> thay vì **
        logs: 'Đơn hàng đã được <strong>Giao thành công</strong> lúc 14:00 ngày 22/11/2025. Cảm ơn bạn!'
    },
    {
        id: 'DH-38701',
        date: '15/11/2025',
        status: 'cancelled',
        statusText: 'Đã hủy',
        total: '180.000đ',
        productName: 'Serum Phục Hồi Tóc Hư Tổn',
        productDesc: 'Giảm giá • SL: 1',
        img: 'img/imgshop/pd2.jpg',
        payment: 'Ví MoMo',
        // SỬA: Dùng thẻ <strong> thay vì **
        logs: 'Đơn hàng đã bị <strong>Hủy</strong> theo yêu cầu của khách hàng vào ngày 15/11/2025.'
    }
];

// ==========================================
// HÀM TẠO HTML THẺ ĐƠN HÀNG (DÙNG CHUNG)
// ==========================================
function createOrderCardHTML(order) {
    let statusClass = 'status-pending';
    let iconHTML = '<i class="fas fa-cube"></i>';
    let iconBg = '';

    if (order.status === 'success') {
        statusClass = 'status-success';
        iconHTML = '<i class="fas fa-check"></i>';
        iconBg = 'background-color: #e0f2f1; color: #0f5132;';
    } else if (order.status === 'cancelled') {
        statusClass = 'status-danger';
        iconHTML = '<i class="fas fa-times"></i>';
        iconBg = 'background-color: #ffebeb; color: #ff4444;';
    }

    return `
    <div class="order-card" style="display: flex; align-items: center; justify-content: space-between; padding: 20px; margin-bottom: 15px; border: 1px solid #f0f0f0; border-radius: 12px; background: white; box-shadow: 0 2px 5px rgba(0,0,0,0.02);">
        <div class="order-info" style="display:flex; align-items:center; gap:15px;">
            <div class="order-icon-box" style="width: 50px; height: 50px; flex-shrink: 0; font-size: 1.2rem; display:flex; align-items:center; justify-content:center; border-radius:12px; background:#f4f8ff; color:var(--bs-primary); ${iconBg}">
                ${iconHTML}
            </div>
            <div>
                <div style="font-weight: 700; font-size: 1rem; margin-bottom: 4px;">${order.productName}</div>
                <div style="font-size: 0.85rem; color: var(--text-gray);">${order.date} • ${order.total}</div>
            </div>
        </div>
        <div class="order-actions-wrapper" style="display: flex; align-items: center; gap: 20px;">
            <span class="status-badge ${statusClass}" style="padding: 8px 16px; font-size: 0.85rem; font-weight:600;">${order.statusText}</span>
            <button class="btn btn-outline" onclick="openOrderDetail('${order.id}')" 
                style="padding: 8px 20px; font-size: 0.85rem; border-radius: 50px; font-weight: 600; white-space: nowrap;">
                Chi tiết
            </button>
        </div>
    </div>
    `;
}

// ==========================================
// HÀM TẠO NÚT HÀNH ĐỘNG TÙY TRẠNG THÁI
// ==========================================
function getActionButtonsHTML(status) {
    let buttons = '';

    if (status === 'processing') {
        buttons += `<button type="button" class="btn btn-danger" style="margin-left: 10px;">Hủy đơn</button>`;
    }
    else if (status === 'success' || status === 'cancelled') {
        buttons += `<button type="button" class="btn btn-primary" style="margin-left: 10px;">Mua lại</button>`;
    }

    buttons += `<button type="button" class="btn btn-outline" onclick="closeOrderDetailModal()">Đóng</button>`;

    return buttons;
}

// ==========================================
// 3. XỬ LÝ MODAL CHI TIẾT (ĐÃ SỬA LỖI NÉT ĐỨT)
// ==========================================
function openOrderDetail(orderId) {
    const order = ordersData.find(o => o.id === orderId);
    if (!order) return;

    const modalBody = document.getElementById('detail-modal-body');
    const modalFooter = document.getElementById('order-detail-modal').querySelector('.modal-footer');
    if (!modalBody || !modalFooter) return;

    // 1. TẠO NỘI DUNG THANH HÀNH ĐỘNG DỰA TRÊN TRẠNG THÁI
    modalFooter.innerHTML = getActionButtonsHTML(order.status);

    // 2. TẠO NỘI DUNG MODAL
    modalBody.innerHTML = `
        <div style="background:#f9f9f9; padding:15px; border-radius:10px; margin-bottom:15px;">
            <p><strong>Mã đơn:</strong> ${order.id}</p>
            <p><strong>Ngày đặt:</strong> ${order.date}</p>
            <p><strong>Trạng thái:</strong> <span style="font-weight:700; color:var(--bs-primary)">${order.statusText}</span></p>
            <p><strong>Tổng tiền:</strong> <span style="font-weight:700; color:#dc3545;">${order.total}</span></p>
            <p><strong>Thanh toán:</strong> ${order.payment}</p>
        </div>
        
        <div style="margin-bottom:15px;">
            <p style="font-weight:bold; margin-bottom:10px; font-size: 1.05rem;">Sản phẩm đã mua:</p>
            <div style="display:flex; gap:15px; padding: 10px; border-bottom: 1px solid #eee;">
                <img src="${order.img}" style="width:70px; height:70px; object-fit:contain; border:1px solid #eee; border-radius:8px;">
                <div style="flex-grow: 1;">
                    <p style="font-weight: 500;">${order.productName}</p>
                    <p style="font-size:0.85rem; color:#666;">${order.productDesc}</p>
                </div>
                <div style="text-align: right; font-weight: 700; color:var(--bs-primary);">
                    ${order.total}
                </div>
            </div>
        </div>

        <div style="padding-top:15px;">
             <p style="font-weight:bold; margin-bottom:5px;">Lịch sử/Ghi chú hệ thống:</p>
             <p style="font-size:0.9rem; color:#555;">${order.logs}</p>
        </div>
    `;

    document.getElementById('order-detail-modal').classList.add('show');
}

function closeOrderDetailModal() {
    document.getElementById('order-detail-modal').classList.remove('show');
}

// ==========================================
// HÀM RENDER (KHÔNG ĐỔI)
// ==========================================
function loadDashboardData() {
    const container = document.getElementById('dashboard-recent-orders');
    if (!container) return;
    const recentOrders = ordersData.slice(0, 2);
    container.innerHTML = recentOrders.map(order => createOrderCardHTML(order)).join('');
}

function renderOrders(filterStatus = 'all') {
    const container = document.getElementById('order-list-container');
    if (!container) return;

    const filteredData = filterStatus === 'all'
        ? ordersData
        : ordersData.filter(order => order.status === filterStatus);

    if (filteredData.length === 0) {
        container.innerHTML = '<div class="text-center" style="padding:30px; color:#999;">Không có đơn hàng nào.</div>';
    } else {
        container.innerHTML = filteredData.map(order => createOrderCardHTML(order)).join('');
    }
}

function filterOrders(status, btn) {
    document.querySelectorAll('.btn-filter').forEach(b => b.classList.remove('active'));
    if (btn) btn.classList.add('active');
    renderOrders(status);
}
if (history.scrollRestoration) {
    history.scrollRestoration = 'manual';
} else {
    window.onbeforeunload = function () {
        window.scrollTo(0, 0);
    }
}