const loginForm = document.getElementById('loginForm');
const emailLogin = document.getElementById('email');
const passwordLogin = document.getElementById('password');
const blankEmailError = document.getElementById('blankEmailError');
const formatEmailError = document.getElementById('formatEmailError');
const blankPasswordError = document.getElementById('blankPasswordError');
const minimumPasswordError = document.getElementById('minimumPasswordError')
//Biến e chính là event trong JavaScript
loginForm.addEventListener('submit', (e) => {
    //Ngăn chặn việc tải lại trang
    e.preventDefault();
    //Lấy dữ liệu người dụng nhập vào. Dùng hàm trim() để cắt khoảng trắng
    const emailValue = emailLogin.value.trim();
    const passwordValue = passwordLogin.value.trim();
    blankEmailError.classList.add('hidden');
    formatEmailError.classList.add('hidden');
    blankPasswordError.classList.add('hidden');
    minimumPasswordError.classList.add('hidden');
    let isValid = true; //Đây là biến kiểm tra (Giả sử ban đầu dữ liệu đúng)
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (emailValue === '') {
        blankEmailError.classList.remove('hidden');
        isValid = false;
    }
    else if (!emailRegex.test(emailValue)) {
        formatEmailError.classList.remove('hidden');
        isValid = false;
    }
    if (passwordValue === '') {
        blankPasswordError.classList.remove('hidden');
        isValid = false;
    }
    else if (passwordValue.length < 8) {
        minimumPasswordError.classList.remove('hidden');
        isValid = false;
    }
});
