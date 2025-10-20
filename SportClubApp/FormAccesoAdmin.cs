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
    public partial class FormAccesoAdmin : Form
    {
        public FormAccesoAdmin()
        {
            InitializeComponent();

            // ===== Dark Mode =====
            // Aplicar tema al abrir
            ThemeManager.ApplyTheme(this);
            AplicarTema();

            // Suscribirse a cambios
            ThemeManager.ThemeChanged += (s, e) => AplicarTema();
            // ================================

            // Configurar PasswordChar para los campos de contraseña
            txtAccesoPassAdmin.PasswordChar = '*';

            // Posicionamiento manual: centrado, pero un poco más abajo
            this.StartPosition = FormStartPosition.Manual;
            Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea; // Área de trabajo de la pantalla principal (excluye barra de tareas)
            int offsetVertical = 100; // Ajusta este valor para moverlo más abajo (en píxeles)

            int x = (screenBounds.Width - this.Width) / 2 + screenBounds.Left;
            int y = (screenBounds.Height - this.Height) / 2 + screenBounds.Top + offsetVertical;

            this.Location = new Point(x, y);
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
        // ====================================================

        private void btnAcceder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAccesoUserAdmin.Text) || string.IsNullOrEmpty(txtAccesoPassAdmin.Text))
            {
                MessageBox.Show("Ingrese username y password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DBHelper.ValidateUser(txtAccesoUserAdmin.Text, txtAccesoPassAdmin.Text))
            {
                string rol = DBHelper.GetUserRol(txtAccesoUserAdmin.Text);
                if (rol == "Administrador")
                {

                    // Cerrar el formulario de acceso
                    this.Close();

                    // Redirige al dashboard
                    FormDashboardAdmin dashboard = new FormDashboardAdmin();
                    dashboard.ShowDialog();
                    this.Hide(); // O Close() si no querés mantener el login abierto
                }
                else
                {
                    MessageBox.Show("Usuario no tiene rol de Administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Credenciales inválidas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
