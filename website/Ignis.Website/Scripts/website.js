import 'prismjs'
import 'prismjs/components/prism-clike'
import 'prismjs/components/prism-csharp'
import 'prismjs/components/prism-cshtml'
import './theme'

window.OnPageLoad = () => {
  Prism.highlightAll()

  const interval = setInterval(() => {
    if (window.mermaid) {
      initMermaid()
      clearInterval(interval)
    }
  }, 100)
}

function initMermaid() {
  const isDarkMode = getTheme() === '2'
  mermaid.init(
    {
      darkMode: isDarkMode,
      theme: isDarkMode ? 'dark' : 'default',
    },
    document.querySelectorAll('.mermaid'),
  )
}
