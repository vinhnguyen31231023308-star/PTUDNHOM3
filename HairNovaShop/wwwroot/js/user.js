document.addEventListener("DOMContentLoaded", function () {
    setTimeout(() => {
        document.querySelectorAll('.hidden-up, .hidden-left').forEach(el => el.classList.add('show'));
    }, 100);

    loadDashboardData();
    renderOrders('all');
    const allButton = document.querySelector('.btn-filter');
    if (allButton) allButton.classList.add('active');

    // Add enter key handler for tracking search
    const trackingInput = document.getElementById('tracking-order-code');
    if (trackingInput) {
        trackingInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                searchTracking();
            }
        });
    }
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
        if (tabName === 'profile') tabLinks[4].classList.add("active");
    }
}

function handleLogout() {
    if (confirm("Bạn có chắc chắn muốn đăng xuất không?")) {
        window.location.href = '/Account/Logout';
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
    const form = document.getElementById('address-form');
    if (form) form.reset();
    document.getElementById('address-modal').classList.add('show');
}

// ==========================================
// LOAD ORDERS FROM SERVER
// ==========================================
let ordersData = [];

async function loadOrdersFromServer() {
    try {
        const response = await fetch('/Account/GetOrders');
        const result = await response.json();
        if (result.success && result.orders) {
            ordersData = result.orders.map(order => ({
                id: order.orderCode,
                orderId: order.id,
                date: order.createdAt,
                status: mapStatusToFilter(order.status),
                statusText: order.statusName,
                total: formatPrice(order.total),
                productName: order.firstProductName || 'Đơn hàng',
                productDesc: order.itemsCount ? `${order.itemsCount} sản phẩm` : '',
                img: order.firstProductImage || '/images/placeholder.png',
                payment: order.paymentMethod,
                logs: getStatusLog(order.status, order.createdAt)
            }));
            return true;
        }
    } catch (error) {
        console.error('Error loading orders:', error);
    }
    return false;
}

function mapStatusToFilter(status) {
    if (status === 'pending' || status === 'confirmed') return 'processing';
    if (status === 'completed') return 'success';
    if (status === 'cancelled') return 'cancelled';
    return 'processing';
}

function formatPrice(price) {
    return new Intl.NumberFormat('vi-VN').format(price) + ' đ';
}

function getStatusLog(status, date) {
    const statusMessages = {
        'pending': `Đơn hàng của bạn đang được <strong>Chờ xác nhận</strong> và sẽ được xử lý sớm nhất.`,
        'confirmed': `Đơn hàng đã được <strong>Xác nhận</strong> và đang được chuẩn bị.`,
        'shipping': `Đơn hàng đang được <strong>Vận chuyển</strong> đến địa chỉ của bạn.`,
        'completed': `Đơn hàng đã được <strong>Giao thành công</strong>. Cảm ơn bạn đã mua sắm!`,
        'cancelled': `Đơn hàng đã bị <strong>Hủy</strong>.`
    };
    return statusMessages[status] || 'Đơn hàng đang được xử lý.';
}

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
    <div class="order-card">
        <div class="order-info">
            <div class="order-icon-box" style="${iconBg}">
                ${iconHTML}
            </div>
            <div>
                <div style="font-weight: 700; font-size: 1rem; margin-bottom: 4px;">${order.productName}</div>
                <div style="font-size: 0.85rem; color: var(--text-gray);">${order.date} • ${order.total}</div>
            </div>
        </div>
        <div class="order-actions-wrapper">
            <span class="status-badge ${statusClass}">${order.statusText}</span>
            <button class="btn btn-outline" onclick="openOrderDetail('${order.orderId || order.id}')" 
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

    if (status === 'processing' || status === 'pending' || status === 'confirmed') {
        buttons += `<button type="button" class="btn btn-danger" onclick="handleCancelOrder()" style="margin-left: 10px;">Hủy đơn</button>`;
    }
    else if (status === 'success' || status === 'completed' || status === 'cancelled') {
        buttons += `<button type="button" class="btn btn-primary" onclick="handleReorder()" style="margin-left: 10px;">Mua lại</button>`;
    }

    buttons += `<button type="button" class="btn btn-outline" onclick="closeOrderDetailModal()">Đóng</button>`;

    return buttons;
}

// ==========================================
// XỬ LÝ MODAL CHI TIẾT ĐƠN HÀNG
// ==========================================
async function openOrderDetail(orderId) {
    try {
        const response = await fetch(`/Account/GetOrderDetails?id=${orderId}`);
        const result = await response.json();
        
        if (!result.success) {
            alert(result.message || 'Không thể tải chi tiết đơn hàng');
            return;
        }

        const order = result.order;
        const modalBody = document.getElementById('detail-modal-body');
        const modalFooter = document.getElementById('order-detail-modal').querySelector('.modal-footer');
        if (!modalBody || !modalFooter) return;

        // Tạo nội dung thanh hành động
        modalFooter.innerHTML = getActionButtonsHTML(order.status);

        // Tạo nội dung modal
        let itemsHTML = '';
        if (order.items && order.items.length > 0) {
            itemsHTML = order.items.map(item => `
                <div style="display:flex; gap:15px; padding: 10px; border-bottom: 1px solid #eee;">
                    <img src="${item.productImage}" style="width:70px; height:70px; object-fit:contain; border:1px solid #eee; border-radius:8px;" onerror="this.src='/images/placeholder.png'">
                    <div style="flex-grow: 1;">
                        <p style="font-weight: 500;">${item.productName}</p>
                        <p style="font-size:0.85rem; color:#666;">${item.capacity ? item.capacity + ' • ' : ''}SL: ${item.quantity}</p>
                    </div>
                    <div style="text-align: right; font-weight: 700; color:var(--bs-primary);">
                        ${formatPrice(item.total)}
                    </div>
                </div>
            `).join('');
        }

        modalBody.innerHTML = `
            <div style="background:#f9f9f9; padding:15px; border-radius:10px; margin-bottom:15px;">
                <p><strong>Mã đơn:</strong> ${order.orderCode}</p>
                <p><strong>Ngày đặt:</strong> ${order.createdAt}</p>
                <p><strong>Trạng thái:</strong> <span style="font-weight:700; color:var(--bs-primary)">${order.statusName}</span></p>
                <p><strong>Tổng tiền:</strong> <span style="font-weight:700; color:#dc3545;">${formatPrice(order.total)}</span></p>
                <p><strong>Thanh toán:</strong> ${order.paymentMethod}</p>
                <p><strong>Địa chỉ giao hàng:</strong> ${order.shippingAddress}</p>
            </div>
            
            <div style="margin-bottom:15px;">
                <p style="font-weight:bold; margin-bottom:10px; font-size: 1.05rem;">Sản phẩm đã mua:</p>
                ${itemsHTML || '<p style="color:#999;">Không có sản phẩm</p>'}
            </div>

            <div style="padding-top:15px;">
                 <p style="font-weight:bold; margin-bottom:5px;">Lịch sử/Ghi chú hệ thống:</p>
                 <p style="font-size:0.9rem; color:#555;">${getStatusLog(order.status, order.createdAt)}</p>
            </div>
        `;

        document.getElementById('order-detail-modal').classList.add('show');
    } catch (error) {
        console.error('Error loading order details:', error);
        alert('Có lỗi xảy ra khi tải chi tiết đơn hàng');
    }
}

function closeOrderDetailModal() {
    document.getElementById('order-detail-modal').classList.remove('show');
}

function handleCancelOrder() {
    if (confirm('Bạn có chắc chắn muốn hủy đơn hàng này không?')) {
        alert('Chức năng hủy đơn hàng sẽ được triển khai sau.');
    }
}

function handleReorder() {
    alert('Chức năng mua lại sẽ được triển khai sau.');
}

// ==========================================
// HÀM RENDER
// ==========================================
async function loadDashboardData() {
    const container = document.getElementById('dashboard-recent-orders');
    if (!container) return;
    
    await loadOrdersFromServer();
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

// ==========================================
// TRACKING ORDER
// ==========================================
async function searchTracking() {
    const input = document.querySelector('.tracking-search input');
    const orderCode = input?.value.trim();
    
    if (!orderCode) {
        alert('Vui lòng nhập mã đơn hàng');
        return;
    }

    try {
        const response = await fetch(`/Account/GetOrderByCode?orderCode=${encodeURIComponent(orderCode)}`);
        const result = await response.json();
        
        if (!result.success) {
            alert(result.message || 'Không tìm thấy đơn hàng');
            return;
        }

        const order = result.order;
        displayTrackingResult(order);
    } catch (error) {
        console.error('Error searching order:', error);
        alert('Có lỗi xảy ra khi tìm kiếm đơn hàng');
    }
}

function displayTrackingResult(order) {
    // Update tracking display with order data
    const trackingContainer = document.querySelector('#tracking .card-box');
    if (!trackingContainer) return;

    // Calculate progress percentage
    let progress = 0;
    if (order.status === 'completed') progress = 100;
    else if (order.status === 'shipping') progress = 75;
    else if (order.status === 'confirmed') progress = 50;
    else if (order.status === 'pending') progress = 25;

    const statusName = order.statusName;
    const statusClass = order.status === 'completed' ? 'status-success' : 
                       order.status === 'cancelled' ? 'status-danger' : 'status-pending';

    // Update timeline steps
    const steps = trackingContainer.querySelectorAll('.step-item');
    steps.forEach((step, index) => {
        step.classList.remove('active');
        if (order.status === 'pending' && index === 0) step.classList.add('active');
        else if (order.status === 'confirmed' && index <= 1) step.classList.add('active');
        else if (order.status === 'shipping' && index <= 2) step.classList.add('active');
        else if (order.status === 'completed' && index <= 3) step.classList.add('active');
    });

    // Update progress bar
    const progressBar = trackingContainer.querySelector('.progress-bar-fill');
    if (progressBar) progressBar.style.width = progress + '%';

    // Update order info
    const orderInfo = trackingContainer.querySelector('h3');
    if (orderInfo) orderInfo.textContent = `Mã đơn: #${order.orderCode}`;
    
    const statusBadge = trackingContainer.querySelector('.status-badge');
    if (statusBadge) {
        statusBadge.textContent = statusName;
        statusBadge.className = `status-badge ${statusClass}`;
    }
}

// ==========================================
// UPDATE PROFILE
// ==========================================
async function updateProfile() {
    const fullName = document.getElementById('profile-fullname')?.value;
    const phone = document.getElementById('profile-phone')?.value;

    if (!fullName || !phone) {
        alert('Vui lòng điền đầy đủ thông tin');
        return;
    }

    try {
        const formData = new FormData();
        formData.append('fullName', fullName);
        formData.append('phone', phone);

        const response = await fetch('/Account/UpdateProfile', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
            }
        });

        const result = await response.json();
        if (result.success) {
            alert(result.message || 'Cập nhật thông tin thành công!');
            location.reload();
        } else {
            alert(result.message || 'Có lỗi xảy ra');
        }
    } catch (error) {
        console.error('Error updating profile:', error);
        alert('Có lỗi xảy ra khi cập nhật thông tin');
    }
}

// ==========================================
// CHANGE PASSWORD
// ==========================================
async function changePassword() {
    const currentPassword = document.getElementById('current-password')?.value;
    const newPassword = document.getElementById('new-password')?.value;
    const confirmPassword = document.getElementById('confirm-password')?.value;

    if (!currentPassword || !newPassword || !confirmPassword) {
        alert('Vui lòng điền đầy đủ thông tin');
        return;
    }

    if (newPassword !== confirmPassword) {
        alert('Mật khẩu mới và xác nhận mật khẩu không khớp');
        return;
    }

    try {
        const formData = new FormData();
        formData.append('currentPassword', currentPassword);
        formData.append('newPassword', newPassword);

        const response = await fetch('/Account/ChangePassword', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
            }
        });

        const result = await response.json();
        if (result.success) {
            alert(result.message || 'Đổi mật khẩu thành công!');
            document.getElementById('current-password').value = '';
            document.getElementById('new-password').value = '';
            document.getElementById('confirm-password').value = '';
        } else {
            alert(result.message || 'Có lỗi xảy ra');
        }
    } catch (error) {
        console.error('Error changing password:', error);
        alert('Có lỗi xảy ra khi đổi mật khẩu');
    }
}

if (history.scrollRestoration) {
    history.scrollRestoration = 'manual';
} else {
    window.onbeforeunload = function () {
        window.scrollTo(0, 0);
    }
}
