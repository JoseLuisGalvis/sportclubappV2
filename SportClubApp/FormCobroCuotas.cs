using SportClubApp.Data.Database;
using SportClubApp.Data.Interfaces;
using SportClubApp.Data.Repositories;
using SportClubApp.Models;
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
    public partial class FormCobroCuotas : Form
    {
        private readonly ICuotaRepository _cuotaRepository;
        private readonly ISocioRepository _socioRepository;

        public FormCobroCuotas()
        {
            InitializeComponent();

            var dbConnection = new DatabaseConnection(Config.ConnectionString);
            _cuotaRepository = new CuotaRepository(dbConnection);
            _socioRepository = new SocioRepository(dbConnection, new PersonaRepository(dbConnection));

            CargarMetodosPago();
            CargarCuotas();
        }

        private void CargarMetodosPago()
        {
            comboMetodoPago.Items.AddRange(new object[]
            {
                "Efectivo",
                "Tarjeta Débito",
                "Tarjeta Crédito",
                "Transferencia",
                "MercadoPago"
            });
            comboMetodoPago.SelectedIndex = 0;
        }

        private async void CargarCuotasPendientes()
        {
            try
            {
                var cuotas = await _cuotaRepository.ObtenerCuotasPendientesAsync();
                dataGridCuotas.Columns.Clear();
                dataGridCuotas.AutoGenerateColumns = true;
                dataGridCuotas.DataSource = cuotas;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cuotas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void CargarCuotas()
        {
            var cuotas = await _cuotaRepository.ObtenerCuotasImpagasAsync();
            dataGridCuotas.Columns.Clear();
            dataGridCuotas.AutoGenerateColumns = true;
            dataGridCuotas.DataSource = cuotas;

        }


        private async void btnMarcarPagada_Click(object sender, EventArgs e)
        {
            if (dataGridCuotas.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una cuota para marcar como pagada", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int cuotaId = (int)dataGridCuotas.CurrentRow.Cells["id"].Value;
                string metodoPagoStr = comboMetodoPago.SelectedItem.ToString();

                // Convertir string a enum MetodoPago
                MetodoPago metodoPago = Enum.Parse<MetodoPago>(metodoPagoStr.Replace(" ", ""));

                bool resultado = await _cuotaRepository.MarcarCuotaComoPagadaAsync(cuotaId, metodoPago);

                if (resultado)
                {
                    MessageBox.Show("Cuota marcada como pagada exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarCuotasPendientes(); // Refrescar lista
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar pago: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
