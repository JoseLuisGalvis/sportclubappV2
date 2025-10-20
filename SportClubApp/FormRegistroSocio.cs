using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stripe.Checkout;

namespace SportClubApp
{
    public partial class FormRegistroSocio : Form
    {
        private int personaIdRegistrada = 0;
        private System.Diagnostics.Process procesoNavegador; // ← VARIABLE NUEVA PARA CERRAR NAVEGADOR

        public FormRegistroSocio()
        {
            InitializeComponent();

            // Posicionamiento manual: centrado, pero un poco más abajo
            this.StartPosition = FormStartPosition.Manual;
            Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea;
            int offsetVertical = 150;

            int x = (screenBounds.Width - this.Width) / 2 + screenBounds.Left;
            int y = (screenBounds.Height - this.Height) / 2 + screenBounds.Top + offsetVertical;

            this.Location = new Point(x, y);

            // ===== Dark Mode =====
            ThemeManager.ApplyTheme(this);
            AplicarTema();
            ThemeManager.ThemeChanged += (s, e) => AplicarTema();
            // ================================

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

            txtPassword.PasswordChar = '*';
            txtConfirmarPassword.PasswordChar = '*';
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MessageBox.Show("El DNI es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDNI.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("El nombre de usuario es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("La contraseña es obligatoria", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (txtPassword.Text != txtConfirmarPassword.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmarPassword.Focus();
                return;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // Verificar DNI duplicado
            if (DBHelper.PersonaExistsByDni(txtDNI.Text.Trim()))
            {
                MessageBox.Show("Ya existe una persona con ese DNI", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDNI.Focus();
                return;
            }

            // Verificar username duplicado
            if (DBHelper.UsernameExists(txtUsuario.Text.Trim()))
            {
                MessageBox.Show("Ese nombre de usuario ya está en uso", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsuario.Focus();
                return;
            }

            try
            {
                // 1. Insertar Persona
                int personaId = DBHelper.InsertPersona(
                    txtNombre.Text.Trim(),
                    txtApellido.Text.Trim(),
                    txtDNI.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                    "Socio"
                );

                // 2. Generar carnet
                string carnet = $"SOC-{personaId:D5}";

                // 3. Insertar Socio (con estado_pago = 'Pendiente')
                DBHelper.InsertSocio(
                    personaId,
                    dtpFechaAlta.Value.Date,
                    chkHabilitado.Checked,
                    chkAptoFisico.Checked,
                    carnet
                );

                // 4. Crear usuario asociado
                DBHelper.InsertUsuarioConPersona(
                    txtUsuario.Text.Trim(),
                    txtPassword.Text,
                    "Socio",
                    personaId
                );

                // Guardar el personaId para usarlo después
                personaIdRegistrada = personaId;

                MessageBox.Show(
                    $"Socio registrado exitosamente\n\n" +
                    $"Nombre: {txtNombre.Text} {txtApellido.Text}\n" +
                    $"DNI: {txtDNI.Text}\n" +
                    $"Usuario: {txtUsuario.Text}\n" +
                    $"Carnet: {carnet}\n" +
                    $"Fecha Alta: {dtpFechaAlta.Value.ToShortDateString()}\n\n" +
                    $"Procederemos al pago de membresía.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // ========== REDIRIGIR A STRIPE ==========
                RedirigirAStripe(personaId, txtNombre.Text.Trim(), txtApellido.Text.Trim(), txtEmail.Text.Trim());
                // =============================================
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== MÉTODO: Redirigir a Stripe ==========
        private void RedirigirAStripe(int personaId, string nombre, string apellido, string email)
        {
            try
            {
                // Crear sesión de pago en Stripe
                Session session = StripePaymentHandler.CreatePaymentSession(
                    personaId,
                    $"{nombre} {apellido}",
                    email
                );

                // Guardar el sessionId en la BD (estado_pago = 'Pendiente')
                DBHelper.UpdateSocioStripeSession(personaId, session.Id);

                // Abrir el navegador con la URL de pago de Stripe
                string paymentUrl = session.Url;

                try
                {
                    // ABRIR Y GUARDAR REFERENCIA DEL PROCESO - FORMA MEJORADA
                    procesoNavegador = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = paymentUrl,
                        UseShellExecute = true  // ← Esto es clave
                    });
                }
                catch
                {
                    MessageBox.Show(
                        $"No se pudo abrir el navegador.\n\nAbre este link manualmente:\n{paymentUrl}",
                        "Abrir Navegador",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                // Mostrar verificación de pago
                MostrarFormularioEsperaConfirmacion(personaId, session.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar el pago:\n{ex.Message}", "Error de Pago", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== MÉTODO: Cerrar ventana del navegador de Stripe ==========
        private void CerrarVentanaStripe()
        {
            try
            {
                // OPCIÓN 1: Cerrar el proceso específico que abrimos
                if (procesoNavegador != null && !procesoNavegador.HasExited)
                {
                    procesoNavegador.CloseMainWindow();

                    // Esperar un poco y si no se cierra, forzar
                    if (!procesoNavegador.WaitForExit(1000))
                    {
                        procesoNavegador.Kill();
                    }
                    procesoNavegador.Dispose();
                    procesoNavegador = null;
                }

                // OPCIÓN 2: Buscar y cerrar ventanas de Stripe por título
                var procesos = System.Diagnostics.Process.GetProcesses();
                foreach (var proceso in procesos)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(proceso.MainWindowTitle) &&
                            (proceso.MainWindowTitle.IndexOf("stripe", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             proceso.MainWindowTitle.IndexOf("checkout", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             proceso.MainWindowTitle.IndexOf("pago", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             proceso.MainWindowTitle.IndexOf("payment", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            proceso.CloseMainWindow();

                            // Esperar y forzar si es necesario
                            if (!proceso.WaitForExit(500))
                            {
                                proceso.Kill();
                            }
                        }
                    }
                    catch
                    {
                        // Ignorar errores en procesos individuales
                    }
                }

                // OPCIÓN 3: Cerrar navegadores recientes
                CerrarNavegadoresRecientes();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cerrando navegador: {ex.Message}");
            }
        }

        // ========== MÉTODO AUXILIAR: Cerrar navegadores recientes ==========
        private void CerrarNavegadoresRecientes()
        {
            try
            {
                // Buscar procesos de navegador activos
                string[] navegadores = { "chrome", "msedge", "firefox", "iexplore", "opera" };

                foreach (string nav in navegadores)
                {
                    var procesos = System.Diagnostics.Process.GetProcessesByName(nav);
                    foreach (var proc in procesos)
                    {
                        try
                        {
                            // Solo cerrar si la ventana es visible (tiene interfaz)
                            if (proc.MainWindowHandle != IntPtr.Zero)
                            {
                                proc.CloseMainWindow();
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                        catch
                        {
                            // Continuar con el siguiente
                        }
                    }
                }
            }
            catch
            {
                // Fallo silencioso
            }
        }

        // ========== MÉTODO: Verificar confirmación de pago ==========
        private void MostrarFormularioEsperaConfirmacion(int personaId, string sessionId)
        {
            // Crear un Timer que verifique cada 2 segundos si el pago se completó
            System.Windows.Forms.Timer timerVerificacion = new System.Windows.Forms.Timer();
            timerVerificacion.Interval = 2000; // 2 segundos

            timerVerificacion.Tick += (sender, e) =>
            {
                try
                {
                    // Verificar si el pago fue exitoso
                    if (StripePaymentHandler.IsPaymentSuccessful(sessionId))
                    {
                        timerVerificacion.Stop();

                        // Obtener el PaymentIntentId
                        string paymentIntentId = StripePaymentHandler.GetPaymentIntentId(sessionId);

                        // Marcar el pago como completado en BD
                        DBHelper.MarkPaymentAsCompleted(personaId, paymentIntentId);

                        // ← PRIMERO CERRAR EL NAVEGADOR DE STRIPE ←
                        CerrarVentanaStripe();

                        // ← LUEGO MOSTRAR EL MENSAJE ←
                        MessageBox.Show(
                            "✅ ¡PAGO EXITOSO!\n\n" +
                            "Tu membresía ha sido activada correctamente.\n" +
                            "Ya puedes disfrutar de todos los beneficios del club.\n\n" +
                            "💰 Pago procesado: ARS 100,000.00\n" +
                            "📅 Membresía activa por 1 año",
                            "Pago Completado - Club Deportivo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        timerVerificacion.Dispose();

                        // Cerrar el formulario después de mostrar el mensaje
                        this.Invoke(new Action(() => this.Close()));
                    }
                }
                catch (Exception ex)
                {
                    // Log silencioso, no interrumpir al usuario
                    System.Diagnostics.Debug.WriteLine($"Error verificando pago: {ex.Message}");
                }
            };

            timerVerificacion.Start();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
        }
    }
}