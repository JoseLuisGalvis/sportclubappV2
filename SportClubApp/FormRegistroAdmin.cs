using SportClubApp.Data.Interfaces;
using SportClubApp.Models;

namespace SportClubApp
{
    public partial class FormRegistroAdmin : Form
    {
        private readonly IUsuarioRepository _usuarioRepository;

        // ✅ NUEVO CONSTRUCTOR con DI
        public FormRegistroAdmin(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            InitializeComponent();

            // ===== Dark Mode =====
            ThemeManager.ApplyTheme(this);
            AplicarTema();
            ThemeManager.ThemeChanged += (s, e) => AplicarTema();
            // ================================

            // Configurar PasswordChar para los campos de contraseña
            txtPasswordAdmin.PasswordChar = '*';
            txtConfPassAdmin.PasswordChar = '*';
        }

        // ✅ CONSTRUCTOR TEMPORAL para diseñador
        public FormRegistroAdmin() : this(null)
        {
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

        // ✅ MÉTODO MIGRADO - ASINCRONO
        private async void btnRegistrar_ClickAdmin(object sender, EventArgs e)
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

            try
            {
                if (_usuarioRepository == null)
                {
                    MessageBox.Show("Error: Repository no inicializado", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ NUEVA FORMA - Verificar si el username existe
                bool usernameExiste = await _usuarioRepository.ExisteUsernameAsync(txtUserAdmin.Text);
                if (usernameExiste)
                {
                    MessageBox.Show("Username ya existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ NUEVA FORMA - Crear usuario administrador
                var nuevoUsuario = new Usuario(txtUserAdmin.Text, txtPasswordAdmin.Text, Rol.Administrador);
                await _usuarioRepository.CrearUsuarioAsync(nuevoUsuario, txtPasswordAdmin.Text);

                MessageBox.Show("Administrador registrado exitosamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}