let isDarkMode = window.matchMedia('(prefers-color-scheme: dark)');

function updateTheme(theme) {
    theme = theme ?? window.localStorage.getItem('theme') ?? 'system';

    if (theme === 'dark' || (theme === 'system' && isDarkMode.matches)) {
        document.documentElement.classList.add('dark');
    } else if (theme === 'light' || (theme === 'system' && !isDarkMode.matches)) {
        document.documentElement.classList.remove('dark');
    }

    return theme;
}

function updateThemeWithoutTransitions(theme) {
    updateTheme(theme);
    document.documentElement.classList.add('[&_*]:!transition-none');
    window.setTimeout(() => {
        document.documentElement.classList.remove('[&_*]:!transition-none');
    }, 0);
}

document.documentElement.setAttribute('data-theme', updateTheme());

new MutationObserver(([{oldValue}]) => {
    let newValue = document.documentElement.getAttribute('data-theme');
    if (newValue !== oldValue) {
        try {
            window.localStorage.setItem('theme', newValue);
        } catch {
        }
        updateThemeWithoutTransitions(newValue);
    }
}).observe(document.documentElement, {attributeFilter: ['data-theme'], attributeOldValue: true});

isDarkMode.addEventListener('change', () => updateThemeWithoutTransitions());

window.setTheme = (theme) => {
    document.documentElement.setAttribute('data-theme', theme);
};
