using SportClubApp.Data.Interfaces;

namespace SportClubApp
{
    public partial class FormAccesoAdmin : Form
    {
        private readonly IUsuarioRepository _usuarioRepository;

        // ✅ NUEVO CONSTRUCTOR con Dependency Injection
        public FormAccesoAdmin(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            InitializeComponent();

            // ===== Dark Mode =====
            ThemeManager.ApplyTheme(this);
            AplicarTema();
            ThemeManager.ThemeChanged += (s, e) => AplicarTema();
            // ================================

            // Configurar PasswordChar para los campos de contraseña
            txtAccesoPassAdmin.PasswordChar = '*';

            // Posicionamiento manual
            this.StartPosition = FormStartPosition.Manual;
            Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea;
            int offsetVertical = -110;

            int x = (screenBounds.Width - this.Width) / 2 + screenBounds.Left;
            int y = (screenBounds.Height - this.Height) / 2 + screenBounds.Top + offsetVertical;

            this.Location = new Point(x, y);
        }

        // ✅ CONSTRUCTOR SIN PARÁMETROS para el Diseñador (TEMPORAL)
        public FormAccesoAdmin() : this(null)
        {
            // Este constructor es necesario para que el Diseñador de Forms funcione
            // En runtime se usará el constructor con DI
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

        // ✅ MÉTODO ACTUALIZADO - ASINCRONO
        private async void btnAcceder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAccesoUserAdmin.Text) || string.IsNullOrEmpty(txtAccesoPassAdmin.Text))
            {
                MessageBox.Show("Ingrese username y password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // ✅ NUEVA FORMA - Validación asíncrona
                bool valido = await _usuarioRepository.ValidarCredencialesAsync(
                    txtAccesoUserAdmin.Text,
                    txtAccesoPassAdmin.Text
                );

                if (valido)
                {
                    var usuario = await _usuarioRepository.ObtenerUsuarioPorUsernameAsync(txtAccesoUserAdmin.Text);

                    if (usuario != null && usuario.Rol == Models.Rol.Administrador)
                    {
                        // Cerrar el formulario de acceso
                        this.Close();

                        // 
                        var dashboard = new FormDashboardAdmin();
                        dashboard.ShowDialog();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Usuario no tiene rol de Administrador.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Credenciales inválidas.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error al validar credenciales: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            // Método sin cambios
        }
    }
}