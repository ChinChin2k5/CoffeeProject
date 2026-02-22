import { useState } from 'react';
import './index.css';

function App() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const handleLogin = async () => {
    try {
      const response = await fetch('http://localhost:5269/swagger/index.html', {
        method: 'POST',
        headers: {
          'Content-Type': 'application.json',
        },
        body: JSON.stringify({
          username: username,
          password: password
        })
      });
    }
    const data = await response.json();
    if (response.ok) {
      alert("Bao Cao: " + data.message);
    }
    else {
      alert("No roi cac chau oi chay di: " + data.message);
    }
    
  }
  return (
    //Kích hoạt flex + toàn màn hình h-screen + căn giữa chiều dọc chiều ngang
    <div className = "flex h-screen items-center justify-center flex-col">
      <div className = "w-16 h-16 flex rounded-full bg-[#3E2723] mb-6 items-center justify-center" >
          <span className="text-4xl text-white">☕</span>
      </div>
      <div className="mb-8 text-center">
          <h1 className="text-3xl font-bold">SPECIALZ Coffee</h1>
          <p className="mt-2 text-sm text-gray-500">Những khoảnh khắc ấm áp đang được vun đắp.</p>
      </div>
      <div className = "w-full max-w-md rounded-2xl bg-white p-8 shadow-xl">
        {/* Tao div de tu dong dan cach, thay vi chui vao de margin-bottom */}
        <div className="space-y-4">
          {/*Khi nguoi dung bam vao username, no se nhay thang vao username */} 
          <label htmlFor="user" className ="text-xm font-bold text-[#3E2723]">Tài Khoản</label>
          <input id="user" type="text" placeholder="Nhập Tên Đăng Nhập... " className="mt-1 w-full rounded-xl border border-gray-300 p-3" ></input>
          <label htmlFor="password" className ="text-xm font-bold text-[#3E2723]">Mật Khẩu</label>
          <input id="password" type="password" placeholder="Nhập Mật Khẩu..." className="mt-1 w-full rounded-xl border border-gray-300 p-3"></input>
          <h1 className ="text-sm font-bold text-[#3E2723] text-right cursor-pointer hover:text-amber-800">Quên Mật Khẩu ?</h1>
          <div className="flex items-center justify-center">
            <button className = "w-full bg-[#3E2723] border border-gray-300 rounded-xl p-3 shadow-md text-white transition-all hover:bg-[#3E2723]/80 hover:scale-[1.02] cursor-pointer">Đăng Nhập</button>
          </div>
        </div>
      </div>
    </div>
  );
}
export default App;