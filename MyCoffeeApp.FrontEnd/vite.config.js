import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite' // BƯỚC 1: Gọi plugin Tailwind V4 tới

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
    tailwindcss(), // BƯỚC 2: Cắm phích điện Tailwind vào hệ thống!
  ],
})
