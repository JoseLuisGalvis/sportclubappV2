using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SportClubApp.Data;
using SportClubApp.Data.Repositories;
using System.Data;
using SportClubApp.Data.Database;
using SportClubApp.Data.Interfaces;

namespace SportClubApp
{
    public partial class FormReportes : Form
    {
        private readonly ICuotaRepository _cuotaRepository;
        private readonly ISocioRepository _socioRepository;

        public FormReportes()
        {
            InitializeComponent();

            var dbConnection = new DatabaseConnection(Config.ConnectionString);
            _cuotaRepository = new CuotaRepository(dbConnection);
            _socioRepository = new SocioRepository(dbConnection, new PersonaRepository(dbConnection));

            comboTipoReporte.SelectedIndex = 0;

            // Inicializar lblTitulo con texto por defecto
            lblTitulo.Text = "Reportes del Sistema";  // ← ESTA LÍNEA NUEVA
        }

        private async void comboTipoReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboTipoReporte.SelectedItem == null) return;

            try
            {
                switch (comboTipoReporte.SelectedItem.ToString())
                {
                    case "Cuotas que vencen hoy":
                        await CargarCuotasQueVencenHoy();
                        break;
                    case "Socios con deudas":
                        await CargarSociosConDeudas();
                        break;
                    case "Socios al día":
                        await CargarSociosAlDia();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar reporte: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarCuotasQueVencenHoy()
        {
            var cuotas = await _cuotaRepository.ObtenerCuotasQueVencenHoyAsync();
            dataGridReportes.DataSource = cuotas;
            lblTitulo.Text = "Socios con cuotas que vencen HOY";
        }

        private async Task CargarSociosConDeudas()
        {
            // Usaremos una consulta directa para este reporte
            using var conn = new DatabaseConnection(Config.ConnectionString).GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT s.nroSocio, p.nombre, p.apellido, p.dni, 
                       COUNT(c.id) as cuotas_vencidas, 
                       SUM(c.monto) as deuda_total
                FROM socio s
                INNER JOIN persona p ON s.persona_id = p.id
                INNER JOIN cuota c ON s.nroSocio = c.socio_id
                WHERE c.estado IN ('Pendiente', 'Vencida') 
                AND c.fechaVencimiento < CURDATE()
                GROUP BY s.nroSocio";

            using var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn);
            using var adapter = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);

            dataGridReportes.DataSource = dt;
            lblTitulo.Text = "Socios con deudas";
        }

        private async Task CargarSociosAlDia()
        {
            // Socios sin cuotas vencidas
            using var conn = new DatabaseConnection(Config.ConnectionString).GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT s.nroSocio, p.nombre, p.apellido, p.dni, 
                       MAX(c.fechaVencimiento) as ultima_cuota_pagada
                FROM socio s
                INNER JOIN persona p ON s.persona_id = p.id
                LEFT JOIN cuota c ON s.nroSocio = c.socio_id
                WHERE s.estado_pago = 'Completado'
                GROUP BY s.nroSocio
                HAVING COUNT(CASE WHEN c.estado = 'Pendiente' AND c.fechaVencimiento < CURDATE() THEN 1 END) = 0";

            using var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn);
            using var adapter = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);

            dataGridReportes.DataSource = dt;
            lblTitulo.Text = "Socios al día";
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
