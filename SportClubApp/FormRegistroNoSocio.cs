using SportClubApp.Data.Database;
using SportClubApp.Data.Interfaces;
using SportClubApp.Models;

using Stripe.Checkout;

namespace SportClubApp
{
    public partial class FormRegistroNoSocio : Form
    {
        private readonly IDatabaseConnection _dbConnection;
        private readonly IPersonaRepository _personaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly INoSocioRepository _noSocioRepository;

        private byte[] fotoVisita = null;
        private bool guardandoDatos = false;
        private System.Diagnostics.Process procesoNavegador = null;
        private int personaIdRegistrada = 0;

        public FormRegistroNoSocio(
            IDatabaseConnection dbConnection,
            IPersonaRepository personaRepository,
            IUsuarioRepository usuarioRepository,
            INoSocioRepository noSocioRepository)
        {
            InitializeComponent();

            _dbConnection = dbConnection;
            _personaRepository = personaRepository;
            _usuarioRepository = usuarioRepository;
            _noSocioRepository = noSocioRepository;

            // ✅ MÍNIMA CONFIGURACIÓN (igual que Socio)
            chkHabilitado.Checked = true;
            chkHabilitado.Enabled = true;
        }

        // ✅ EVENTO FOTO - IDÉNTICO AL DE SOCIO
        private void btnSeleccionarFoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp|Todos los archivos|*.*";
                ofd.Title = "Seleccionar Foto para Pase Diario";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Image img = Image.FromFile(ofd.FileName);
                        pbFotoPreview.Image = img;

                        using (MemoryStream ms = new MemoryStream())
                        {
                            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            fotoVisita = ms.ToArray();
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

        // ✅ EVENTO PRINCIPAL - ESTRUCTURA IDÉNTICA A SOCIO
        private async void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (guardandoDatos)
            {
                MessageBox.Show("Ya se está procesando el registro. Por favor espere.",
                    "Procesando", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidarCampos())
                return;

            try
            {
                guardandoDatos = true;

                // VERIFICAR DUPLICADOS (igual que Socio)
                if (await _personaRepository.ExistePersonaPorDniAsync(txtDni.Text.Trim()))
                {
                    MessageBox.Show("Ya existe una persona con ese DNI", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDni.Focus();
                    return;
                }

                if (await _usuarioRepository.ExisteUsernameAsync(txtUsername.Text.Trim()))
                {
                    MessageBox.Show("Ese nombre de usuario ya está en uso", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Focus();
                    return;
                }

                // CREAR OBJETO (igual que Socio)
                var noSocio = new NoSocio(
                    txtNombre.Text.Trim(),
                    txtApellido.Text.Trim(),
                    txtDni.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim()
                )
                {
                    Habilitado = chkHabilitado.Checked,
                    FechaVisita = dtpFechaVisita.Value.Date,
                    FotoVisita = fotoVisita,
                    EstadoPago = EstadoPago.Pendiente,
                    MontoPago = 10000.00
                };

                // GUARDAR EN BBDD (igual que Socio)
                personaIdRegistrada = await _personaRepository.CrearPersonaAsync(noSocio);
                noSocio.Id = personaIdRegistrada;

                int idNoSocio = await _noSocioRepository.InsertarAsync(noSocio);
                noSocio.IdNoSocio = idNoSocio;

                // USUARIO (igual que Socio)
                var usuario = new Usuario
                {
                    Username = txtUsername.Text.Trim(),
                    Rol = Rol.NoSocio,
                    Activo = true,
                    PersonaId = personaIdRegistrada
                };
                // await _usuarioRepository.CrearUsuarioAsync(usuario);

                // MENSAJE ÉXITO (igual que Socio)
                MessageBox.Show(
                    $"✅ No Socio registrado exitosamente\n\n" +
                    $"Nombre: {txtNombre.Text} {txtApellido.Text}\n" +
                    $"DNI: {txtDni.Text}\n" +
                    $"Usuario: {txtUsername.Text}\n" +
                    $"Fecha Visita: {dtpFechaVisita.Value.ToShortDateString()}\n\n" +
                    $"Ahora procederemos al pago de la entrada diaria.",
                    "Registro Exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // REDIRIGIR A STRIPE (igual que Socio)
                await RedirigirAStripeNoSocioAsync(noSocio.IdNoSocio, noSocio.Nombre, noSocio.Apellido, noSocio.Email);
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

        // ✅ VALIDACIÓN - IDÉNTICA A SOCIO
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

            if (string.IsNullOrWhiteSpace(txtDni.Text))
            {
                MostrarValidacion("El DNI es obligatorio", txtDni);
                return false;
            }

            if (txtDni.Text.Trim().Length < 7 || txtDni.Text.Trim().Length > 8)
            {
                MostrarValidacion("El DNI debe tener entre 7 y 8 dígitos", txtDni);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MostrarValidacion("El nombre de usuario es obligatorio", txtUsername);
                return false;
            }

            if (fotoVisita == null)
            {
                var result = MessageBox.Show(
                    "⚠️ No has seleccionado una foto para el pase diario.\n\n¿Deseas continuar sin foto?",
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

        // ✅ STRIPE - IDÉNTICO A SOCIO
        private async Task RedirigirAStripeNoSocioAsync(int idNoSocio, string nombre, string apellido, string email)
        {
            try
            {
                Session session = StripePaymentHandler.CreateNoSocioPaymentSession(
                    idNoSocio,
                    $"{nombre} {apellido}",
                    email
                );

                var noSocio = await _noSocioRepository.ObtenerNoSocioPorIdAsync(idNoSocio);
                noSocio.StripeSessionId = session.Id;
                await _noSocioRepository.ActualizarNoSocioAsync(noSocio);

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

                VerificarPagoNoSocioPeriodicamente(idNoSocio, session.Id);
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

        // ✅ VERIFICACIÓN PAGO - IDÉNTICA A SOCIO
        private void VerificarPagoNoSocioPeriodicamente(int idNoSocio, string sessionId)
        {
            var timer = new System.Windows.Forms.Timer { Interval = 3000 };
            int intentos = 0;
            const int maxIntentos = 60;

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
                        await _noSocioRepository.MarcarPagoCompletadoAsync(idNoSocio, paymentIntentId);

                        if (procesoNavegador != null && !procesoNavegador.HasExited)
                        {
                            procesoNavegador.CloseMainWindow();
                            procesoNavegador = null;
                        }

                        if (this.IsHandleCreated)
                        {
                            this.Invoke(new Action(() =>
                            {
                                MessageBox.Show(
                                    "✅ ¡PAGO COMPLETADO EXITOSAMENTE!\n\n" +
                                    "Tu entrada diaria ha sido activada correctamente.\n" +
                                    "Ya puedes acceder al club por hoy.",
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
                        timer.Stop();
                        timer.Dispose();

                        if (this.IsHandleCreated)
                        {
                            this.Invoke(new Action(() =>
                            {
                                MessageBox.Show(
                                    "⏰ Tiempo de espera agotado.\n\n" +
                                    "Si realizaste el pago, verifica el estado en tu cuenta de Stripe.",
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}


