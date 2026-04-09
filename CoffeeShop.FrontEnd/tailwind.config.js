/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
      "./index.html",
      "./pages/**/*.html",
      "./src/**/*.{html,js}"
    ],
    theme: {
      extend: {
        colors: {
          coffee: {
            600: '#964b00',
            800: '#4b2c20',
          },
        },
      },
    },
    plugins: [],
  }