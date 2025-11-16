using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SportClubApp.Data.Database;
using SportClubApp.Data.Interfaces;
using SportClubApp.Data.Repositories;

using SportClubApp.Services;

namespace SportClubApp
{
    public partial class FormGestionCarnets : Form
    {
        private readonly ISocioRepository _socioRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly CarnetPDFService _carnetPDFService;
        private readonly string _logoPath;

        public FormGestionCarnets()
        {
            InitializeComponent();

            var dbConnection = new DatabaseConnection(Config.ConnectionString);
            _personaRepository = new PersonaRepository(dbConnection);
            _socioRepository = new SocioRepository(dbConnection, _personaRepository);
            _carnetPDFService = new CarnetPDFService();

            // Ruta del logo
            _logoPath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "logo_sportclub.png");

            CargarSociosConPagoCompletado();
        }

        private async void CargarSociosConPagoCompletado()
        {
            try
            {
                var socios = await _socioRepository.ObtenerSociosConPagoCompletado();

                // Verificar si hay socios
                if (socios == null || !socios.Any())
                {
                    MessageBox.Show("No hay socios con pago completado para mostrar", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Limpiar el DataGridView
                    dataGridSocios.DataSource = null;
                    dataGridSocios.Columns.Clear();
                    return;
                }

                // En lugar de usar DataSource directamente, crear columnas manualmente
                dataGridSocios.AutoGenerateColumns = false;
                dataGridSocios.Columns.Clear();

                // Crear columnas manualmente
                dataGridSocios.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    Name = "NroSocio",
                    HeaderText = "N° Socio",
                    DataPropertyName = "NroSocio",
                    Width = 80
                });

                dataGridSocios.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    Name = "NombreCompleto",
                    HeaderText = "Nombre",
                    DataPropertyName = "NombreCompleto",
                    Width = 200
                });

                dataGridSocios.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    Name = "DNI",
                    HeaderText = "DNI",
                    DataPropertyName = "DNI",
                    Width = 100
                });

                dataGridSocios.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    Name = "FechaAlta",
                    HeaderText = "Fecha Alta",
                    DataPropertyName = "FechaAlta",
                    Width = 100
                });

                dataGridSocios.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    Name = "EstadoPago",
                    HeaderText = "Estado Pago",
                    DataPropertyName = "EstadoPago",
                    Width = 100
                });

                // Asignar datos
                dataGridSocios.DataSource = socios;

                // Mostrar información en el título
                this.Text = $"Gestión de Carnets - {socios.Count} socios con pago completado";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar socios: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnGenerarCarnet_Click(object sender, EventArgs e)
        {
            if (dataGridSocios.CurrentRow == null || dataGridSocios.CurrentRow.Index < 0 || dataGridSocios.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Seleccione un socio para generar el carnet", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Mostrar indicador de progreso
                btnGenerarCarnet.Enabled = false;
                btnGenerarCarnet.Text = "Generando...";
                Cursor = Cursors.WaitCursor;

                // Obtener el número de socio de manera segura
                var selectedRow = dataGridSocios.CurrentRow;
                int nroSocio;

                if (selectedRow.Cells["NroSocio"].Value != null)
                {
                    nroSocio = Convert.ToInt32(selectedRow.Cells["NroSocio"].Value);
                }
                else
                {
                    MessageBox.Show("No se pudo obtener el número de socio", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Obtener el nombre para mostrar en mensajes
                string nombreSocio = selectedRow.Cells["NombreCompleto"].Value?.ToString() ?? "N/A";

                // Obtener datos completos del socio
                var socio = await _socioRepository.ObtenerSocioParaCarnetAsync(nroSocio);

                if (socio == null)
                {
                    MessageBox.Show("No se encontraron datos del socio seleccionado", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Generar código de carnet si no existe
                if (string.IsNullOrEmpty(socio.CarnetCodigo))
                {
                    socio.CarnetCodigo = $"SCG5-{socio.NroSocio:000000}";
                }

                // Verificar si el logo existe
                string logoPath = File.Exists(_logoPath) ? _logoPath : null;

                // Generar carnet PDF
                string pdfPath = _carnetPDFService.GenerarCarnet(socio, logoPath);

                // Marcar como entregado en la base de datos
                bool exito = await _socioRepository.MarcarCarnetEntregadoAsync(nroSocio, socio.CarnetCodigo);

                if (exito)
                {
                    string mensaje = $"✅ Carnet generado exitosamente!\n\n" +
                                   $"Socio: {nombreSocio}\n" +
                                   $"N° Socio: {nroSocio}\n" +
                                   $"Archivo: {Path.GetFileName(pdfPath)}\n" +
                                   $"Ubicación: {Path.GetDirectoryName(pdfPath)}";

                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Abrir el PDF automáticamente
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = pdfPath,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"No se pudo abrir el PDF: {ex.Message}");
                        // No es crítico, solo informar
                        MessageBox.Show($"Carnet generado pero no se pudo abrir automáticamente.\nUbicación: {pdfPath}",
                            "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Error al marcar carnet como entregado en la base de datos", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al generar carnet: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Restaurar estado del botón
                btnGenerarCarnet.Enabled = true;
                btnGenerarCarnet.Text = "Generar Carnet";
                Cursor = Cursors.Default;
            }
        }

        private async void btnMarcarEntregado_Click(object sender, EventArgs e)
        {
            if (dataGridSocios.CurrentRow == null || dataGridSocios.CurrentRow.Index < 0 || dataGridSocios.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Seleccione un socio para marcar como entregado", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Bloquear boton
                btnMarcarEntregado.Enabled = false;
                Cursor = Cursors.WaitCursor;

                var selectedRow = dataGridSocios.CurrentRow;

                // Obtener Nro de socio
                int nroSocio = Convert.ToInt32(selectedRow.Cells["NroSocio"].Value);

                // Obtener datos completos (necesitamos CarnetCodigo)
                var socio = await _socioRepository.ObtenerSocioParaCarnetAsync(nroSocio);

                if (socio == null)
                {
                    MessageBox.Show("No se encontraron datos del socio seleccionado.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Generar código si aún no existe
                string codigoCarnet = !string.IsNullOrEmpty(socio.CarnetCodigo)
                    ? socio.CarnetCodigo
                    : $"SCG5-{nroSocio:000000}";

                // CONFIRMACIÓN
                string nombreSocio = selectedRow.Cells["NombreCompleto"].Value?.ToString() ?? "N/A";
                var confirmResult = MessageBox.Show(
                    $"¿Confirmas que el carnet del socio:\n\n{nombreSocio}\nN° {nroSocio}\n\nHa sido ENTREGADO?",
                    "Confirmar entrega",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmResult != DialogResult.Yes)
                    return;

                // Ejecutar actualización
                bool exito = await _socioRepository.MarcarCarnetEntregadoAsync(nroSocio, codigoCarnet);

                if (exito)
                {
                    MessageBox.Show(
                        $"El carnet fue marcado como ENTREGADO correctamente.\n\n" +
                        $"Socio: {nombreSocio}\nN° {nroSocio}\nCódigo: {codigoCarnet}",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // Recargar la tabla
                    CargarSociosConPagoCompletado();
                }
                else
                {
                    MessageBox.Show("Ocurrió un error al marcar el carnet como entregado.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al marcar carnet como entregado: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnMarcarEntregado.Enabled = true;
                Cursor = Cursors.Default;
            }
        }


        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            // Recargar la lista de socios
            CargarSociosConPagoCompletado();
        }

        private void dataGridSocios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Generar carnet al hacer doble clic en una fila
            if (e.RowIndex >= 0 && !dataGridSocios.Rows[e.RowIndex].IsNewRow)
            {
                btnGenerarCarnet_Click(sender, e);
            }
        }

        // Método para mostrar información del socio seleccionado (sin usar label)
        private void MostrarInfoSocioSeleccionado()
        {
            if (dataGridSocios.CurrentRow != null && dataGridSocios.CurrentRow.Index >= 0 && !dataGridSocios.CurrentRow.IsNewRow)
            {
                var selectedRow = dataGridSocios.CurrentRow;
                string nombre = selectedRow.Cells["NombreCompleto"].Value?.ToString() ?? "N/A";
                string nroSocio = selectedRow.Cells["NroSocio"].Value?.ToString() ?? "N/A";

                // Podemos mostrar en el status strip o en un tooltip si lo agregamos después
                Console.WriteLine($"Socio seleccionado: {nombre} (N° {nroSocio})");
            }
        }
    }
}