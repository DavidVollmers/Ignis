const defaultTheme = require('tailwindcss/defaultTheme')

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['**/*.{razor,html,cshtml,js}'],
  plugins: [require('@tailwindcss/typography')],
}
