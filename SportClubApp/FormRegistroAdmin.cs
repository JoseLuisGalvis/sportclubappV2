using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SportClubApp
{
    public partial class FormRegistroAdmin : Form
    {
        public FormRegistroAdmin()
        {
            InitializeComponent();

            // =====  Dark Mode ===== 
            // Aplicar tema al abrir
            ThemeManager.ApplyTheme(this);
            AplicarTema();

            // Suscribirse a cambios
            ThemeManager.ThemeChanged += (s, e) => AplicarTema();
            // ================================

            // Configurar PasswordChar para los campos de contraseña
            txtPasswordAdmin.PasswordChar = '*';
            txtConfPassAdmin.PasswordChar = '*';

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

        private void btnRegistrar_ClickAdmin(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserAdmin.Text) || string.IsNullOrEmpty(txtPasswordAdmin.Text) || string.IsNullOrEmpty(txtConfPassAdmin.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtPasswordAdmin.Text != txtConfPassAdmin.Text)
            {
                MessageBox.Show("Las passwords no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DBHelper.UsernameExists(txtUserAdmin.Text))
            {
                MessageBox.Show("Username ya existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DBHelper.InsertUsuario(txtUserAdmin.Text, txtPasswordAdmin.Text, "Administrador");
                MessageBox.Show("Administrador registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // O redirige a login si lo preferís
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
