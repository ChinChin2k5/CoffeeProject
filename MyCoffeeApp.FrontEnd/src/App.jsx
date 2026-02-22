import './index.css'; // Đảm bảo đã link file CSS gốc

function App() {
  return (
    // Dùng className thay cho class trong React nhé!
    <div className="flex min-h-screen items-center justify-center bg-[#FDF8F5]">
      
      <div className="w-full max-w-md rounded-2xl bg-white p-8 shadow-xl">
        {/* Khối Logo & Tên quán */}
        <div className="mb-8 text-center">
          <div className="mx-auto flex h-20 w-20 items-center justify-center rounded-full bg-[#3E2723] mb-4 shadow-lg">
            <span className="text-3xl text-white">☕</span>
          </div>
          <h1 className="text-3xl font-extrabold text-[#3E2723]">SPECIALZ Coffee</h1>
          <p className="text-sm font-medium text-gray-500 mt-2">Brewing moments of warmth</p>
        </div>

        {/* Form Đăng nhập giả lập */}
        <div className="space-y-4">
          <div>
            <label className="text-sm font-bold text-[#3E2723]">Username</label>
            <input 
              type="text" 
              placeholder="Nhập tên đăng nhập..." 
              className="mt-1 w-full rounded-xl border border-gray-300 p-3 outline-none focus:border-[#3E2723] focus:ring-1 focus:ring-[#3E2723]"
            />
          </div>

          <div>
            <label className="text-sm font-bold text-[#3E2723]">Password</label>
            <input 
              type="password" 
              placeholder="••••••••" 
              className="mt-1 w-full rounded-xl border border-gray-300 p-3 outline-none focus:border-[#3E2723] focus:ring-1 focus:ring-[#3E2723]"
            />
          </div>

          <button className="mt-6 w-full rounded-xl bg-[#3E2723] py-4 text-lg font-bold text-white shadow-md hover:bg-opacity-90 transition-all">
            Sign In
          </button>
        </div>
        
      </div>

    </div>
  );
}

export default App;