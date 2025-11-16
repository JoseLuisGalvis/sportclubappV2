using SportClubApp.Data.Database;
using SportClubApp.Data.Interfaces;
using SportClubApp.Models;
using Stripe.Checkout;

using System.Runtime.InteropServices;

namespace SportClubApp
{
    public partial class FormRegistroSocio : Form
    {
        private readonly IDatabaseConnection _dbConnection;
        private readonly IPersonaRepository _personaRepository;
        private readonly ISocioRepository _socioRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        private int personaIdRegistrada = 0;
        private System.Diagnostics.Process procesoNavegador;

        private byte[] fotoCarnet = null;
        private bool guardandoDatos = false;

        // ================================================================
        // ✅ WIN32 API (los mantenemos por si acaso, pero no los usamos)
        // ================================================================
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        private const uint WM_CLOSE = 0x0010;

        public FormRegistroSocio(
            IDatabaseConnection dbConnection,
            IPersonaRepository personaRepository,
            ISocioRepository socioRepository,
            IUsuarioRepository usuarioRepository)
        {
            InitializeComponent();

            _dbConnection = dbConnection;
            _personaRepository = personaRepository;
            _socioRepository = socioRepository;
            _usuarioRepository = usuarioRepository;

            this.StartPosition = FormStartPosition.Manual;
            Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea;
            int offsetVertical = 150;

            int x = (screenBounds.Width - this.Width) / 2 + screenBounds.Left;
            int y = (screenBounds.Height - this.Height) / 2 + screenBounds.Top + offsetVertical;
            this.Location = new Point(x, y);

            ThemeManager.ApplyTheme(this);
            AplicarTema();
            ThemeManager.ThemeChanged += (s, e) => AplicarTema();

            if (!_dbConnection.TestConnection())
            {
                MessageBox.Show(
                    "No se pudo conectar a la base de datos.\n\nVerifica que MySQL esté ejecutándose.",
                    "Error de Conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void AplicarTema()
        {
            this.BackColor = ThemeManager.IsDarkMode ? ThemeManager.DarkTheme.Background : ThemeManager.LightTheme.Background;
            this.ForeColor = ThemeManager.IsDarkMode ? ThemeManager.DarkTheme.Text : ThemeManager.LightTheme.Text;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ================================================================
        // EVENTO PRINCIPAL: Guardar socio
        // ================================================================
        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (guardandoDatos)
            {
                MessageBox.Show("Ya se está procesando el registro. Por favor espere.",
                    "Procesando", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidarCampos())
            {
                return;
            }

            try
            {
                guardandoDatos = true;

                // VERIFICAR DUPLICADOS
                if (await _personaRepository.ExistePersonaPorDniAsync(txtDNI.Text.Trim()))
                {
                    MessageBox.Show("Ya existe una persona con ese DNI", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDNI.Focus();
                    return;
                }

                if (await _usuarioRepository.ExisteUsernameAsync(txtUsuario.Text.Trim()))
                {
                    MessageBox.Show("Ese nombre de usuario ya está en uso", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsuario.Focus();
                    return;
                }

                // CREAR OBJETO SOCIO
                var socio = new Socio(
                    txtNombre.Text.Trim(),
                    txtApellido.Text.Trim(),
                    txtDNI.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim()
                )
                {
                    FechaAlta = dtpFechaAlta.Value.Date,
                    Habilitado = chkHabilitado.Checked,
                    AptoFisico = chkAptoFisico.Checked,
                    FotoCarnet = fotoCarnet
                };

                // GUARDAR EN BASE DE DATOS

                // 1️⃣ Crear Persona
                personaIdRegistrada = await _personaRepository.CrearPersonaAsync(socio);
                socio.Id = personaIdRegistrada;

                // 2️⃣ Crear Socio (ESTO GENERA EL nroSocio)
                int nroSocioGenerado = await _socioRepository.CrearSocioAsync(socio);
                socio.NroSocio = nroSocioGenerado;

                // 3️⃣ Generar Carnet DESPUÉS de tener el nroSocio
                socio.GenerarCarnet();

                // 4️⃣ Actualizar el socio con el carnet generado
                await _socioRepository.ActualizarSocioAsync(socio);

                // 5️⃣ Crear Usuario
                var usuario = new Usuario
                {
                    Username = txtUsuario.Text.Trim(),
                    Rol = Rol.Socio,
                    Activo = true,
                    PersonaId = personaIdRegistrada
                };

                // MENSAJE DE ÉXITO
                MessageBox.Show(
                    $"✅ Socio registrado exitosamente\n\n" +
                    $"Número de Socio: {socio.NroSocio}\n" +
                    $"Nombre: {txtNombre.Text} {txtApellido.Text}\n" +
                    $"DNI: {txtDNI.Text}\n" +
                    $"Usuario: {txtUsuario.Text}\n" +
                    $"Carnet: {socio.Carnet}\n" +
                    $"Fecha Alta: {dtpFechaAlta.Value.ToShortDateString()}\n\n" +
                    $"Ahora procederemos al pago de membresía.",
                    "Registro Exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // REDIRIGIR A STRIPE
                await RedirigirAStripeAsync(socio.NroSocio, socio.Nombre, socio.Apellido, socio.Email);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al guardar:\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                guardandoDatos = false;
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MostrarValidacion("El nombre es obligatorio", txtNombre);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MostrarValidacion("El apellido es obligatorio", txtApellido);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MostrarValidacion("El DNI es obligatorio", txtDNI);
                return false;
            }

            if (txtDNI.Text.Trim().Length < 7 || txtDNI.Text.Trim().Length > 8)
            {
                MostrarValidacion("El DNI debe tener entre 7 y 8 dígitos", txtDNI);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MostrarValidacion("El nombre de usuario es obligatorio", txtUsuario);
                return false;
            }

            if (fotoCarnet == null)
            {
                this.Focus();

                var result = MessageBox.Show(
                    "⚠️ No has seleccionado una foto para el carnet.\n\n¿Deseas continuar sin foto?",
                    "Foto no seleccionada",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    btnSeleccionarFoto.Focus();
                    return false;
                }
            }

            return true;
        }

        private void MostrarValidacion(string mensaje, Control control)
        {
            MessageBox.Show(mensaje, "⚠️ Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
        }

        // ================================================================
        // REDIRIGIR A STRIPE PARA EL PAGO
        // ================================================================
        private async Task RedirigirAStripeAsync(int nroSocio, string nombre, string apellido, string email)
        {
            try
            {
                Session session = StripePaymentHandler.CreatePaymentSession(
                    nroSocio,
                    $"{nombre} {apellido}",
                    email
                );

                var socio = await _socioRepository.ObtenerSocioPorNroAsync(nroSocio);
                socio.StripeSessionId = session.Id;
                await _socioRepository.ActualizarSocioAsync(socio);

                string paymentUrl = session.Url;

                try
                {
                    procesoNavegador = System.Diagnostics.Process.Start(
                        new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = paymentUrl,
                            UseShellExecute = true
                        }
                    );
                }
                catch
                {
                    MessageBox.Show(
                        $"No se pudo abrir el navegador automáticamente.\n\n" +
                        $"Abre este link manualmente:\n\n{paymentUrl}",
                        "Abrir Navegador",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                // VERIFICAR PAGO CON TIMER SIMPLIFICADO
                VerificarPagoPeriodicamente(nroSocio, session.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"❌ Error al procesar el pago:\n\n{ex.Message}",
                    "Error de Pago",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ================================================================
        // ✅ VERIFICAR PAGO PERIÓDICAMENTE - VERSIÓN SIMPLIFICADA
        // ================================================================
        private void VerificarPagoPeriodicamente(int nroSocio, string sessionId)
        {
            var timer = new System.Windows.Forms.Timer
            {
                Interval = 3000 // Verificar cada 3 segundos
            };

            int intentos = 0;
            const int maxIntentos = 60; // 3 minutos máximo

            timer.Tick += async (s, e) =>
            {
                intentos++;

                try
                {
                    if (StripePaymentHandler.IsPaymentSuccessful(sessionId))
                    {
                        timer.Stop();
                        timer.Dispose();

                        string paymentIntentId = StripePaymentHandler.GetPaymentIntentId(sessionId);
                        await _socioRepository.MarcarPagoCompletadoAsync(nroSocio, paymentIntentId);

                        // ✅ SOLUCIÓN MÍNIMA - Cerrar solo el proceso que abrimos
                        if (procesoNavegador != null && !procesoNavegador.HasExited)
                        {
                            procesoNavegador.CloseMainWindow();
                            procesoNavegador = null;
                        }

                        // Mostrar mensaje de éxito
                        if (this.IsHandleCreated)
                        {
                            this.Invoke(new Action(() =>
                            {
                                MessageBox.Show(
                                    "✅ ¡PAGO COMPLETADO EXITOSAMENTE!\n\n" +
                                    "Tu membresía ha sido activada correctamente.\n" +
                                    "Ya puedes disfrutar de todos los beneficios del club.",
                                    "Pago Exitoso",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information
                                );
                                this.Close();
                            }));
                        }
                    }
                    else if (intentos >= maxIntentos)
                    {
                        // Tiempo agotado
                        timer.Stop();
                        timer.Dispose();

                        if (this.IsHandleCreated)
                        {
                            this.Invoke(new Action(() =>
                            {
                                MessageBox.Show(
                                    "⏰ Tiempo de espera agotado.\n\n" +
                                    "Si realizaste el pago, verifica el estado en tu cuenta de Stripe.\n" +
                                    "Puedes cerrar manualmente la ventana del navegador.",
                                    "Tiempo Agotado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error verificando pago: {ex.Message}");
                }
            };

            timer.Start();
        }

        // ================================================================
        // ✅ ELIMINAMOS TODOS LOS MÉTODOS COMPLEJOS DE CERRAR VENTANAS
        // ================================================================

        private void btnSeleccionarFoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp|Todos los archivos|*.*";
                ofd.Title = "Seleccionar Foto para Carnet";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Image img = Image.FromFile(ofd.FileName);
                        pbFotoPreview.Image = img;

                        using (MemoryStream ms = new MemoryStream())
                        {
                            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            fotoCarnet = ms.ToArray();
                        }

                        MessageBox.Show("Foto cargada correctamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar la imagen:\n{ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}