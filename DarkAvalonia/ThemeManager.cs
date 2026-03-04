using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;

namespace DarkAvalonia
{
    public static class dark
    {
        public static Color bg0 = Color.FromRgb(37, 39, 44);
        public static Color bg = Color.FromRgb(47, 49, 54);
        public static Color bg1 = Color.FromRgb(54, 57, 63);
        public static Color fg = Color.FromRgb(210, 210, 210);

        // Lazy-initialized to avoid creating Avalonia objects before platform init
        private static IBrush _sb_bg;
        private static IBrush _sb_bg1;
        private static IBrush _sb_fg;

        public static IBrush sb_bg => _sb_bg ??= new SolidColorBrush(bg);
        public static IBrush sb_bg1 => _sb_bg1 ??= new SolidColorBrush(bg1);
        public static IBrush sb_fg => _sb_fg ??= new SolidColorBrush(fg);

        public static bool darkEnabled;

        /// <summary>
        /// Returns true if the app is currently rendering in dark mode.
        /// Checks the actual resolved theme variant, not just the setting.
        /// </summary>
        public static bool IsDarkTheme =>
            Application.Current?.ActualThemeVariant == ThemeVariant.Dark;

        public static void SetTheme(Application app, bool isDark)
        {
            darkEnabled = isDark;

            // When dark is enabled, force Dark theme (system detection is unreliable on Linux).
            // When dark is disabled, follow system theme (Default).
            app.RequestedThemeVariant = isDark ? ThemeVariant.Dark : ThemeVariant.Default;
        }

        /// <summary>
        /// Convert a light-mode pastel status color to an appropriate dark-mode equivalent.
        /// Preserves the hue/tint direction while shifting to a dark range that's visible
        /// against Fluent Dark backgrounds (~#1E1E1E to #2C2C2C).
        /// </summary>
        public static Color ToDarkVariant(Color c)
        {
            // Scale to ~30-90 range to be visible against dark backgrounds
            byte r = (byte)Math.Clamp(c.R * 0.35, c.R > 0 ? 25 : 0, 100);
            byte g = (byte)Math.Clamp(c.G * 0.35, c.G > 0 ? 25 : 0, 100);
            byte b = (byte)Math.Clamp(c.B * 0.35, c.B > 0 ? 25 : 0, 100);
            return Color.FromRgb(r, g, b);
        }

        /// <summary>
        /// Returns the appropriate status color for the current theme.
        /// In dark mode, converts the light-mode color to a darker variant.
        /// </summary>
        public static Color StatusColor(Color lightColor)
        {
            return IsDarkTheme ? ToDarkVariant(lightColor) : lightColor;
        }

        public static Color bgColor(Color c)
        {
            return darkEnabled ? bg : c;
        }

        public static Color bgColor1(Color c)
        {
            return darkEnabled ? bg1 : c;
        }

        public static IBrush bgBrush(IBrush b)
        {
            return darkEnabled ? sb_bg : b;
        }

        public static IBrush bgBrush1(IBrush b)
        {
            return darkEnabled ? sb_bg1 : b;
        }

        public static IBrush fgBrush(IBrush b)
        {
            return darkEnabled ? sb_fg : b;
        }

        public static Color Down(Color c)
        {
            if (!darkEnabled)
                return c;

            return Color.FromArgb(255, (byte)(c.R * 0.8), (byte)(c.G * 0.8), (byte)(c.B * 0.8));
        }

        public static bool IsUnix
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return ((p == 4) || (p == 6) || (p == 128));
            }
        }
    }
}
