// ==========================================
// 1. KHAI BÁO CÁC DOM ELEMENTS
// ==========================================
// Khối Form & Input
const loginForm = document.getElementById('loginForm');
const logoutForm = document.getElementById('logoutForm');
const emailLogin = document.getElementById('email');
const passwordLogin = document.getElementById('password');
const selectedRoleInput = document.getElementById('selected-role'); // Ẩn lấy role

// Khối Error Text
const blankEmailError = document.getElementById('blankEmailError');
const formatEmailError = document.getElementById('formatEmailError');
const blankPasswordError = document.getElementById('blankPasswordError');
const minimumPasswordError = document.getElementById('minimumPasswordError');

// Khối UI Role & Admin
const btnStaff = document.getElementById('btn-staff');
const btnManager = document.getElementById('btn-manager');
const logo = document.getElementById('shop-logo');
const adminModal = document.getElementById('admin-modal');
const btnLogin = document.getElementById('btn-login');

// ==========================================
// 2. XỬ LÝ GIAO DIỆN (UI LOGIC)
// ==========================================
// Đổi màu nút chọn Role
function selectRole(role) {
    selectedRoleInput.value = role.toLowerCase();
    const activeClass = "bg-gradient-to-r from-amber-600 to-amber-700 text-white shadow-lg scale-105".split(" ");
    const inactiveClass = "bg-amber-50 text-amber-700 hover:bg-amber-100 border border-amber-200".split(" ");

    if (role === 'staff') {
        btnStaff.classList.add(...activeClass); btnStaff.classList.remove(...inactiveClass);
        btnManager.classList.add(...inactiveClass); btnManager.classList.remove(...activeClass);
    } else {
        btnManager.classList.add(...activeClass); btnManager.classList.remove(...inactiveClass);
        btnStaff.classList.add(...inactiveClass); btnStaff.classList.remove(...activeClass);
    }
}

// Ẩn/Hiện mật khẩu
function togglePassword(inputId) {
    const input = document.getElementById(inputId);
    input.type = input.type === "password" ? "text" : "password";
}

// ==========================================
// 3. BACKDOOR ADMIN (NHẤN GIỮ LOGO 3 GIÂY)
// ==========================================
let pressTimer;

function startPress(e) {
    e.preventDefault(); 
    logo.classList.replace('scale-100', 'scale-95'); 
    pressTimer = setTimeout(() => {
        adminModal.classList.remove('hidden');
        adminModal.classList.add('flex');
    }, 3000);
}


function cancelPress() {
    clearTimeout(pressTimer);
    logo.classList.replace('scale-95', 'scale-100');
}

logo.addEventListener('mousedown', startPress);
logo.addEventListener('mouseup', cancelPress);
logo.addEventListener('mouseleave', cancelPress);
logo.addEventListener('touchstart', startPress, {passive: false});
logo.addEventListener('touchend', cancelPress);

function closeAdminModal() {
    adminModal.classList.add('hidden');
    adminModal.classList.remove('flex');
    // Sửa ID thành 'keyInput'
    document.getElementById('keyInput').value = ''; 
}
async function loginAdmin() {
    // Sửa ID thành 'keyInput'
    const keyInput = document.getElementById('keyInput').value; 
    
    try {
        const response = await fetch('http://localhost:5059/api/auth/verify-backdoor', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(keyInput)
        });

        if (response.ok) {
            window.location.href = '/Admin/admin.html'; 
        } else {
            alert('Còi báo động: Mã truy cập sai!');
            closeAdminModal(); 
        }
    } catch (error) {
        console.error(error);
    }
}

// ==========================================
// 4. XỬ LÝ VALIDATION VÀ GỌI API (FETCH)
// ==========================================
loginForm.addEventListener('submit', async function(event) {
    // Ngăn chặn việc tải lại trang
    event.preventDefault();

    // Lấy dữ liệu (Không trim password)
    const emailValue = emailLogin.value.trim();
    const passwordValue = passwordLogin.value; 
    const roleValue = selectedRoleInput.value;

    // Reset thông báo lỗi
    blankEmailError.classList.add('hidden');
    formatEmailError.classList.add('hidden');
    blankPasswordError.classList.add('hidden');
    minimumPasswordError.classList.add('hidden');

    let isValid = true; 
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    // Validate Email
    if (emailValue === '') {
        blankEmailError.classList.remove('hidden');
        isValid = false;
    } else if (!emailRegex.test(emailValue)) {
        formatEmailError.classList.remove('hidden');
        isValid = false;
    }

    // Validate Password
    if (passwordValue === '') {
        blankPasswordError.classList.remove('hidden');
        isValid = false;
    } else if (passwordValue.length < 8) {
        minimumPasswordError.classList.remove('hidden');
        isValid = false;
    }

    // Nếu mọi dữ liệu đã chuẩn xác -> Tiến hành Fetch API
    if (isValid) {
        try {
            btnLogin.textContent = "Đang Tải...";
            btnLogin.classList.add('opacity-50', 'cursor-not-allowed'); // Thêm hiệu ứng mờ của Tailwind
            // Hiển thị trạng thái loading (tuỳ chọn, em có thể thêm UI loading sau)
            console.log("Đang gửi yêu cầu đăng nhập...");

            // Gọi API bằng Fetch (Sửa URL theo cổng Backend của em)
            const response = await fetch('http://localhost:5059/api/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include',
                // Câu lệnh stringify biến đổi các string thành dạng mà trình duyệt đọc được
                body: JSON.stringify({
                    email: emailValue,
                    password: passwordValue,
                    role: roleValue
                })
            });

            // Parse dữ liệu từ Backend trả về
            const data = await response.json();

            // Kiểm tra HTTP Status Code
            if (!response.ok) {
                // Ví dụ Server trả về 401 Unauthorized hoặc 400 Bad Request
                throw new Error(data.message || 'Email hoặc mật khẩu không chính xác!');
            }

            // Đăng nhập thành công!
            console.log("Login thành công:", data);
            
            // 2. Chuyển hướng người dùng dựa theo role
            if (roleValue === 'admin') {
                window.location.href = '/Admin/admin.html';
            } else if (roleValue === 'manager') {
                window.location.href = '/Manager/manager.html';
            } else {
                window.location.href = '/Staff/staff.html';
            }

        } catch (error) {
            console.error("Lỗi đăng nhập:", error);
            // Hiển thị lỗi từ server cho người dùng thấy (em có thể tạo 1 thẻ <p> để hiện lỗi này)
            alert(error.message);
        } finally {
            btnLogin.textContent = "Đăng Nhập";
            btnLogin.classList.remove('opacity-50', 'cursor-not-allowed');
        }
    }
    async function handleLogout() {
        try {
            await fetch('http://localhost:5059/api/auth/logout', {
                method: 'POST',
                credentials: 'include'
            });
            window.location.replace('/');
        } catch (error) {
            console.error("Lỗi khi đăng xuất:", error);
            // Có lỗi thì vẫn cứ đá nó ra ngoài cho an toàn
            window.location.replace('/');
        }
    }
});
