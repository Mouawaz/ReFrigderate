using Microsoft.JSInterop;

namespace BlazorApp1.Services
{
    public interface IThemeState
    {
        bool IsDarkMode { get; set; }
        event Action OnThemeChange;
        Task InitializeThemeAsync();
        Task SetThemeAsync(bool isDark);
    }

    public class ThemeState : IThemeState
    {
        private readonly IJSRuntime _jsRuntime;
        private bool _isDarkMode;
        private bool _isInitialized;

        public event Action OnThemeChange;

        public ThemeState(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                if (_isDarkMode != value)
                {
                    _isDarkMode = value;
                    OnThemeChange?.Invoke();
                }
            }
        }

        public async Task InitializeThemeAsync()
        {
            if (_isInitialized) return;

            try
            {
                var savedTheme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme");
                if (string.IsNullOrEmpty(savedTheme))
                {
                    var prefersDark = await _jsRuntime.InvokeAsync<bool>("eval", 
                        "window.matchMedia('(prefers-color-scheme: dark)').matches");
                    await SetThemeAsync(prefersDark);
                }
                else
                {
                    await SetThemeAsync(savedTheme == "dark");
                }

                _isInitialized = true;
            }
            catch
            {
                _isDarkMode = false;
            }
        }

        public async Task SetThemeAsync(bool isDark)
        {
            try
            {
                IsDarkMode = isDark;
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", isDark ? "dark" : "light");
        

                await _jsRuntime.InvokeVoidAsync("document.documentElement.classList.remove", "light", "dark");
                await _jsRuntime.InvokeVoidAsync("document.documentElement.classList.add", isDark ? "dark" : "light");
            }
            catch
            {
                // Handle prerendering scenario
            }
        }
    }
}