// ==========================================
// 1. BIẾN TOÀN CỤC LƯU TRỮ TRẠNG THÁI
// ==========================================
let cart = []; // Balo đựng đồ khách gọi
const VAT_RATE = 0.08; // Thuế 8%

// Hàm format tiền tệ VNĐ cho đẹp
const formatMoney = (money) => {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(money);
};

// ==========================================
// 2. KÉO DỮ LIỆU TỪ BACKEND VÀ VẼ MENU
// ==========================================
async function loadProducts() {
    try {
        const response = await fetch('http://localhost:5059/api/product', {
            method: 'GET',
            credentials: 'include'
        });

        if (!response.ok) throw new Error("Không thể tải danh sách sản phẩm!");

        const result = await response.json();
        const products = result.data; 
        
        const container = document.getElementById('product-container');
        container.innerHTML = ''; 

        products.forEach(p => {
            const cardHtml = `
                <div onclick="addToCart(${p.id}, '${p.name}', ${p.price})" class="bg-white p-4 rounded-xl shadow-sm border border-gray-100 cursor-pointer hover:shadow-md hover:border-amber-400 transition transform hover:-translate-y-1">
                    <div class="bg-amber-50 h-32 rounded-lg mb-4 flex items-center justify-center overflow-hidden">
                        ${p.image ? `<img src="${p.image}" class="w-full h-full object-cover"/>` : `<span>☕️</span>`}
                    </div>
                    <h3 class="font-bold text-gray-800 line-clamp-1">${p.name}</h3>
                    <p class="text-xs text-gray-400 mb-1">${p.categoryName}</p>
                    <p class="text-amber-600 font-bold mt-1">${formatMoney(p.price)}</p>
                </div>
            `;
            container.innerHTML += cardHtml;
        });
    } catch (error) {
        console.error("Lỗi tải Menu:", error);
    }
}

// ==========================================
// 3. XỬ LÝ GIỎ HÀNG (CART LOGIC)
// ==========================================
function addToCart(id, name, price) {
    const existingItem = cart.find(item => item.id === id);
    
    if (existingItem) {
        existingItem.quantity += 1;
    } else {
        cart.push({ id, name, price, quantity: 1 });
    }
    
    renderCart(); // Gọi hàm vẽ lại giỏ hàng
}

function renderCart() {
    const cartContainer = document.getElementById('cart-items');
    const btnShowQR = document.getElementById('btnShowQR');
    
    // Nếu giỏ hàng trống thì hiển thị icon rỗng
    if (cart.length === 0) {
        cartContainer.innerHTML = `
            <div class="flex flex-col items-center justify-center h-full text-gray-400 mt-10">
                <svg class="w-12 h-12 mb-2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z"></path></svg>
                <p>Giỏ hàng đang trống</p>
            </div>
        `;
        btnShowQR.disabled = true; // Khóa nút thanh toán
    } else {
        cartContainer.innerHTML = ''; // Dọn rác để vẽ danh sách
        btnShowQR.disabled = false; // Mở khóa nút thanh toán
    }

    let subtotal = 0;
    let totalItems = 0;

    // Vẽ từng món trong giỏ
    cart.forEach(item => {
        const itemTotal = item.price * item.quantity;
        subtotal += itemTotal;
        totalItems += item.quantity;

        cartContainer.innerHTML += `
            <div class="flex justify-between items-center bg-gray-50 p-3 rounded-lg border border-gray-200">
                <div>
                    <h4 class="font-bold text-gray-800">${item.name}</h4>
                    <p class="text-sm text-gray-500">${formatMoney(item.price)} x ${item.quantity}</p>
                </div>
                <div class="font-bold text-gray-800">${formatMoney(itemTotal)}</div>
            </div>
        `;
    });

    // Tính tiền
    const tax = subtotal * VAT_RATE;
    const total = subtotal + tax;

    // Bắn số liệu lên HTML
    document.getElementById('cart-count').innerText = `${totalItems} món`;
    document.getElementById('subtotal').innerText = formatMoney(subtotal);
    document.getElementById('tax').innerText = formatMoney(tax);
    document.getElementById('total').innerText = formatMoney(total);
    document.getElementById('qr-total').innerText = formatMoney(total);
}

// Xử lý nút Hủy Đơn (Xóa sạch giỏ hàng)
document.getElementById('btnVoidOrder').addEventListener('click', () => {
    if (cart.length > 0 && confirm('Sếp có chắc muốn hủy đơn hiện tại không?')) {
        cart = []; // Trút ngược balo
        renderCart(); // Vẽ lại giao diện
    }
});

// Chạy khởi tạo khi mở web
document.addEventListener("DOMContentLoaded", () => {
    loadProducts();
    renderCart(); // Chạy nháp 1 lần để set giao diện giỏ hàng trống
});