using System;
using System.Drawing;
using System.Windows.Forms;
using SportClubApp.Data.Database;
using SportClubApp.Data.Repositories;

namespace SportClubApp
{
    public partial class FormDashboardAdmin : Form
    {
        public FormDashboardAdmin()
        {
            InitializeComponent();

            // ===== Dark Mode =====
            ThemeManager.ApplyTheme(this);
            AplicarTema();
            ThemeManager.ThemeChanged += (s, e) => AplicarTema();

            // ===== Posicionamiento inicial =====
            this.StartPosition = FormStartPosition.Manual;
            Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea;
            int offsetVertical = -80; // Posición base del Dashboard
            int x = (screenBounds.Width - this.Width) / 2 + screenBounds.Left;
            int y = (screenBounds.Height - this.Height) / 2 + screenBounds.Top + offsetVertical;
            this.Location = new Point(x, y);
        }

        // ===========================================================
        // MÉTODO REUTILIZABLE: Muestra un formulario 100 px debajo
        // ===========================================================
        private void MostrarFormularioDebajo(Form formularioNuevo, int offsetVertical = 100)
        {
            // Evitar duplicados del mismo tipo
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == formularioNuevo.GetType())
                {
                    form.BringToFront();
                    return;
                }
            }

            formularioNuevo.StartPosition = FormStartPosition.Manual;
            formularioNuevo.Location = new Point(this.Location.X, this.Location.Y + offsetVertical);
            formularioNuevo.Show();
        }

        // ===========================================================
        // BOTÓN: Gestión (abre FormCRUDAdmin)
        // ===========================================================
        private void BtnGestion_Click(object sender, EventArgs e)
        {
            var dbConnection = new DatabaseConnection(Config.ConnectionString);
            var usuarioRepo = new UsuarioRepository(dbConnection);

            var formCRUD = new FormCRUDAdmin(usuarioRepo);
            MostrarFormularioDebajo(formCRUD, 50); // 50 px debajo del Dashboard
        }

        // ===========================================================
        // BOTÓN: Carnet/Cuotas (abre FormMenuGestionCarnetCuotas)
        // ===========================================================
        private void BtnCarnet_Click(object sender, EventArgs e)
        {
            var formMenu = new FormMenuGestionCarnetCuotas();
            MostrarFormularioDebajo(formMenu, 100); // 100 px debajo del Dashboard
        }

        // ===========================================================
        // BOTÓN: Reportes (abre un formulario o mensaje)
        // ===========================================================
        private void BtnReportes_Click(object sender, EventArgs e)
        {
            // En lugar de un MessageBox, mostramos un FormReportes debajo
            var formReportes = new FormReportes();
            MostrarFormularioDebajo(formReportes, 100); // 100 px debajo del Dashboard
        }

        // ===========================================================
        // DARK MODE
        // ===========================================================
        private void AplicarTema()
        {
            if (ThemeManager.IsDarkMode)
            {
                this.BackColor = ThemeManager.DarkTheme.Background;
                this.ForeColor = ThemeManager.DarkTheme.Text;
            }
            else
            {
                this.BackColor = ThemeManager.LightTheme.Background;
                this.ForeColor = ThemeManager.LightTheme.Text;
            }
        }
    }
}

