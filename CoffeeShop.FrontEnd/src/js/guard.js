// 1. Lấy dữ liệu và ép TẤT CẢ về chữ thường để triệt tiêu lỗi phân biệt Hoa/Thường
const rawRole = localStorage.getItem("userRole");
const safeRole = rawRole ? rawRole.toLowerCase() : null; 
const currentPath = window.location.pathname.toLowerCase(); 

//alert("Đang đứng ở: " + currentPath + "\nThẻ đang cầm: " + safeRole);

// 2. Bao trọn ổ mọi đường dẫn có thể là Sảnh Lễ Tân
const isAtRoot = currentPath === '/' || currentPath === '/index.html' || currentPath === '/login.html';

if (safeRole === null) {
    // KHÔNG CÓ THẺ: Đang ở phòng khác thì đuổi ra Lễ Tân
    if (!isAtRoot) {
        window.location.href = '/';
    }
} else {
    // ĐÃ CÓ THẺ
    if (isAtRoot) {
        // Đang ở Lễ Tân -> Bấm thang máy cho lên thẳng phòng (VIP Pass)
        if (safeRole === "admin") window.location.href = '/Admin/admin.html';
        else if (safeRole === "manager") window.location.href = '/Manager/manager.html';
        else if (safeRole === "staff") window.location.href = '/Staff/staff.html';
        else {
            // Thẻ giả mạo -> Xé thẻ, đá ra cửa
            localStorage.removeItem("userRole");
            window.location.href = '/';
        }
    } else {
        // Đang lảng vảng ở các tầng -> Kiểm tra lệch pha
        if (currentPath.includes('admin') && safeRole !== "admin") window.location.href = '/';
        else if (currentPath.includes('manager') && safeRole !== "manager") window.location.href = '/';
        else if (currentPath.includes('staff') && safeRole !== "staff") window.location.href = '/';
    }
}