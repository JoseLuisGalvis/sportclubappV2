using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClubApp
{
    public static class ThemeManager
    {
        public static bool IsDarkMode { get; private set; } = false;

        // Evento para notificar cuando cambia el tema
        public static event EventHandler? ThemeChanged;

        // Colores del tema oscuro
        public static class DarkTheme
        {
            public static Color Background = Color.FromArgb(20, 20, 20);
            public static Color Header = Color.FromArgb(10, 10, 10);
            public static Color Text = Color.White;
            public static Color MenuBackground = Color.FromArgb(30, 30, 30);
            public static Color MenuHover = Color.FromArgb(10, 30, 120);
        }

        // Colores del tema claro
        public static class LightTheme
        {
            public static Color Background = Color.White;
            public static Color Header = Color.Navy;
            public static Color Text = Color.Black;
            public static Color MenuBackground = Color.FromArgb(240, 240, 240);
            public static Color MenuHover = Color.FromArgb(200, 220, 255);
        }

        // Método para cambiar el tema
        public static void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            ThemeChanged?.Invoke(null, EventArgs.Empty);
        }

        // Método para aplicar tema a un formulario
        public static void ApplyTheme(Form form)
        {
            if (IsDarkMode)
            {
                form.BackColor = DarkTheme.Background;
                form.ForeColor = DarkTheme.Text;
            }
            else
            {
                form.BackColor = LightTheme.Background;
                form.ForeColor = LightTheme.Text;
            }
        }
    }
}