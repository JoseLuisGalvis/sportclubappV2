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
    public partial class FormRegistroNoSocio : Form
    {
        public FormRegistroNoSocio()
        {
            InitializeComponent();

            // Posicionamiento manual: centrado, pero un poco más abajo
            this.StartPosition = FormStartPosition.Manual;
            Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea; // Área de trabajo de la pantalla principal (excluye barra de tareas)
            int offsetVertical = 150; // Ajusta este valor para moverlo más abajo (en píxeles)

            int x = (screenBounds.Width - this.Width) / 2 + screenBounds.Left;
            int y = (screenBounds.Height - this.Height) / 2 + screenBounds.Top + offsetVertical;

            this.Location = new Point(x, y);

            // ===== Dark Mode =====
            // Aplicar tema al abrir
            ThemeManager.ApplyTheme(this);
            AplicarTema();

            // Suscribirse a cambios
            ThemeManager.ThemeChanged += (s, e) => AplicarTema();
            // ================================

            // Verificar conexión a BD al cargar el formulario
            if (!DBHelper.TestConnection())
            {
                MessageBox.Show(
                    "No se pudo conectar a la base de datos.\n\n" +
                    "Verifica que MySQL esté ejecutándose.",
                    "Error de Conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            // Configurar PasswordChar para los campos de contraseña
            txtPassword.PasswordChar = '*';
            txtConfirmarPassword.PasswordChar = '*';
        }

        // ===== AÑADIR ESTE MÉTODO AL FINAL DE LA CLASE =====
        private void AplicarTema()
        {
            if (ThemeManager.IsDarkMode)
            {
                this.BackColor = ThemeManager.DarkTheme.Background;
                this.ForeColor = ThemeManager.DarkTheme.Text;
                // Puedes añadir más controles específicos si es necesario
            }
            else
            {
                this.BackColor = ThemeManager.LightTheme.Background;
                this.ForeColor = ThemeManager.LightTheme.Text;
            }
        }
        // ====================================================

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // ========================================
            // VALIDACIONES BÁSICAS
            // ========================================
            
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido es obligatorio", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDni.Text))
            {
                MessageBox.Show("El DNI es obligatorio", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDni.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("El nombre de usuario es obligatorio", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("La contraseña es obligatoria", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (txtPassword.Text != txtConfirmarPassword.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmarPassword.Focus();
                return;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // ========================================
            // VERIFICAR DUPLICADOS
            // ========================================
            
            if (DBHelper.PersonaExistsByDni(txtDni.Text.Trim()))
            {
                MessageBox.Show("Ya existe una persona con ese DNI", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDni.Focus();
                return;
            }

            if (DBHelper.UsernameExists(txtUsername.Text.Trim()))
            {
                MessageBox.Show("Ese nombre de usuario ya está en uso", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsername.Focus();
                return;
            }

            // ========================================
            // REGISTRO EN BASE DE DATOS
            // ========================================

            try
            {
                // 1. Insertar en tabla PERSONA
                int personaId = DBHelper.InsertPersona(
                    txtNombre.Text.Trim(),
                    txtApellido.Text.Trim(),
                    txtDni.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                    "NoSocio"  // ⬅️ DIFERENCIA CLAVE con Socio
                );

                // 2. Insertar en tabla NOSOCIO
                DBHelper.InsertNoSocio(
                    personaId,
                    chkHabilitado.Checked,  // ⬅️ Valor del CheckBox
                    dtpFechaVisita.Value.Date
                );

                // 3. Crear USUARIO asociado
                DBHelper.InsertUsuarioConPersona(
                    txtUsername.Text.Trim(),
                    txtPassword.Text,
                    "NoSocio",  // ⬅️ Rol: NoSocio
                    personaId
                );

                // ========================================
                // MENSAJE DE ÉXITO
                // ========================================
                
                MessageBox.Show(
                    $"No Socio registrado exitosamente\n\n" +
                    $"Nombre: {txtNombre.Text} {txtApellido.Text}\n" +
                    $"DNI: {txtDni.Text}\n" +
                    $"Usuario: {txtUsername.Text}\n" +
                    $"Fecha de Visita: {dtpFechaVisita.Value.ToShortDateString()}",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar:\n{ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
