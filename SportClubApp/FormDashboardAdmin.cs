using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportClubApp
{
    public partial class FormDashboardAdmin : Form
    {
        public FormDashboardAdmin()
        {
            InitializeComponent();

            // ===== Dark Mode =====
            // Aplicar tema al abrir
            ThemeManager.ApplyTheme(this);
            AplicarTema();

            // Suscribirse a cambios
            ThemeManager.ThemeChanged += (s, e) => AplicarTema();
            // ================================

            // Posicionamiento manual: centrado, pero un poco más abajo
            this.StartPosition = FormStartPosition.Manual;
            Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea; // Área de trabajo de la pantalla principal (excluye barra de tareas)
            int offsetVertical = 200; // Ajusta este valor para moverlo más abajo (en píxeles)

            int x = (screenBounds.Width - this.Width) / 2 + screenBounds.Left;
            int y = (screenBounds.Height - this.Height) / 2 + screenBounds.Top + offsetVertical;

            this.Location = new Point(x, y);

        }

        private void BtnGestion_Click(object sender, EventArgs e)
        {
            FormCRUDAdmin formCRUD = new FormCRUDAdmin();
            formCRUD.ShowDialog();
        }

        private void BtnCarnet_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Gestionando carnets y cuotas. ¡Mantengamos las cuentas al día!", "Carnet/Cuotas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnReportes_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Generando reportes y listados. ¡Datos listos para analizar!", "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ===== Método para Aplicar Dark Mode =====
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

        private void lblGestion_Click(object sender, EventArgs e)
        {

        }

        private void lblCarnet_Click(object sender, EventArgs e)
        {

        }
        // ====================================================

    }
}
