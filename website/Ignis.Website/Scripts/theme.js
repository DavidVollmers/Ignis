let isDarkMode = window.matchMedia('(prefers-color-scheme: dark)')

function updateTheme(theme) {
  theme = theme ?? window.localStorage.getItem('theme') ?? '0'

  if (theme === '2' || (theme === '0' && isDarkMode.matches)) {
    document.documentElement.classList.add('dark')
  } else if (theme === '1' || (theme === '0' && !isDarkMode.matches)) {
    document.documentElement.classList.remove('dark')
  }

  return theme
}

function updateThemeWithoutTransitions(theme) {
  document.documentElement.classList.add('[&_*]:!transition-none')
  window.setTimeout(() => {
    updateTheme(theme)
    window.setTimeout(() => {
      document.documentElement.classList.remove('[&_*]:!transition-none')

      mermaid.init(
        {
          darkMode: theme === '2',
          theme: theme === '2' ? 'dark' : 'default',
        },
        document.querySelectorAll('.mermaid'),
      )
    }, 15)
  }, 15)
}

document.documentElement.setAttribute('data-theme', updateTheme())

new MutationObserver(([{ oldValue }]) => {
  let newValue = document.documentElement.getAttribute('data-theme')
  if (newValue !== oldValue) {
    try {
      window.localStorage.setItem('theme', newValue)
    } catch {}
    updateThemeWithoutTransitions(newValue)
  }
}).observe(document.documentElement, { attributeFilter: ['data-theme'], attributeOldValue: true })

isDarkMode.addEventListener('change', () => updateThemeWithoutTransitions())

window.setTheme = (theme) => {
  document.documentElement.setAttribute('data-theme', theme)
}

window.getTheme = () => {
  const theme = document.documentElement.getAttribute('data-theme')
  if (theme === '0') {
    return isDarkMode.matches ? '2' : '1'
  }
  return theme
}
