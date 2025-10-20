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
    public partial class FormCRUDAdmin : Form
    {
        private DataTable administradoresData;

        public FormCRUDAdmin()
        {
            InitializeComponent();

            // Configuración inicial
            ConfigureDataGridView();
            LoadAdministradores();

            // ===== Dark Mode =====
            ThemeManager.ApplyTheme(this);
            AplicarTema();
            ThemeManager.ThemeChanged += (s, e) => AplicarTema();
            // ================================
        }

        private void ConfigureDataGridView()
        {
            // Configurar el DataGridView para que use las columnas que definiste
            dgvAdministradores.AutoGenerateColumns = false;
            dgvAdministradores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAdministradores.MultiSelect = false;
            dgvAdministradores.ReadOnly = true;

            // Configurar las columnas existentes con las propiedades de datos
            colId.DataPropertyName = "id";
            colUsername.DataPropertyName = "username";
            colFecha.DataPropertyName = "fecha_creacion";
            colEstado.DataPropertyName = "estado";
        }

        private void LoadAdministradores()
        {
            try
            {
                administradoresData = DBHelper.GetAdministradores();
                dgvAdministradores.DataSource = administradoresData;

                // Actualizar el label con el total
                label3.Text = $"Total: {administradoresData.Rows.Count} administradores";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar administradores: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // EVENTO: Botón Nuevo Administrador
        private void btnNuevoAdmin_Click(object sender, EventArgs e)
        {
            // Abrir formulario de registro de nuevo admin
            FormRegistroAdmin formRegistro = new FormRegistroAdmin();
            if (formRegistro.ShowDialog() == DialogResult.OK)
            {
                LoadAdministradores(); // Recargar lista después de agregar
            }
        }

        // EVENTO: Botón Editar Administrador
        private void btnEditarAdmin_Click(object sender, EventArgs e)
        {
            if (dgvAdministradores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un administrador para editar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener datos del administrador seleccionado
            DataGridViewRow selectedRow = dgvAdministradores.SelectedRows[0];
            int selectedId = Convert.ToInt32(selectedRow.Cells["colId"].Value);
            string username = selectedRow.Cells["colUsername"].Value.ToString();

            // Abrir formulario de edición
            FormEditarAdmin formEditar = new FormEditarAdmin(selectedId, username);
            if (formEditar.ShowDialog() == DialogResult.OK)
            {
                LoadAdministradores(); // Recargar lista después de editar
            }
        }

        // EVENTO: Botón Eliminar Administrador
        private void btnEliminarAdmin_Click(object sender, EventArgs e)
        {
            if (dgvAdministradores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un administrador para eliminar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener datos del administrador seleccionado
            DataGridViewRow selectedRow = dgvAdministradores.SelectedRows[0];
            int selectedId = Convert.ToInt32(selectedRow.Cells["colId"].Value);
            string username = selectedRow.Cells["colUsername"].Value.ToString();

            // Confirmación de seguridad
            DialogResult result = MessageBox.Show(
                $"¿Está seguro de que desea eliminar al administrador '{username}'?\n\nEsta acción no se puede deshacer.",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Verificar que no sea el último administrador
                    if (administradoresData.Rows.Count <= 1)
                    {
                        MessageBox.Show("No se puede eliminar el único administrador del sistema.", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Eliminar administrador
                    if (DBHelper.EliminarAdministrador(selectedId))
                    {
                        MessageBox.Show("Administrador eliminado exitosamente.", "Éxito",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAdministradores(); // Recargar lista
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el administrador.", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar administrador: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // EVENTO: Botón Cerrar
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // EVENTO: Búsqueda en tiempo real
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            // Filtrar administradores según el texto de búsqueda
            if (administradoresData != null)
            {
                string filter = txtBuscar.Text.Trim();
                if (!string.IsNullOrEmpty(filter))
                {
                    // Filtrar por ID o Username
                    administradoresData.DefaultView.RowFilter =
                        $"id LIKE '%{filter}%' OR username LIKE '%{filter}%'";
                }
                else
                {
                    administradoresData.DefaultView.RowFilter = "";
                }

                // Actualizar el contador con los resultados filtrados
                label3.Text = $"Total: {administradoresData.DefaultView.Count} administradores";
            }
        }

        // EVENTO: Doble click en una fila para editar
        private void dgvAdministradores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Asegurarse de que no sea el header
            {
                btnEditarAdmin_Click(sender, e);
            }
        }

        // APLICAR TEMA
        private void AplicarTema()
        {
            if (ThemeManager.IsDarkMode)
            {
                this.BackColor = ThemeManager.DarkTheme.Background;
                this.ForeColor = ThemeManager.DarkTheme.Text;

                // Aplicar tema a controles específicos
                dgvAdministradores.BackgroundColor = Color.FromArgb(45, 45, 45);
                dgvAdministradores.ForeColor = Color.White;
                dgvAdministradores.GridColor = Color.FromArgb(80, 80, 80);

                foreach (Control control in this.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        textBox.BackColor = Color.FromArgb(60, 60, 60);
                        textBox.ForeColor = Color.White;
                        textBox.BorderStyle = BorderStyle.FixedSingle;
                    }
                    else if (control is Button button)
                    {
                        button.BackColor = ThemeManager.DarkTheme.Header;
                        button.ForeColor = Color.White;
                        button.FlatStyle = FlatStyle.Flat;
                    }
                }
            }
            else
            {
                this.BackColor = ThemeManager.LightTheme.Background;
                this.ForeColor = ThemeManager.LightTheme.Text;

                dgvAdministradores.BackgroundColor = SystemColors.Window;
                dgvAdministradores.ForeColor = SystemColors.ControlText;
                dgvAdministradores.GridColor = SystemColors.Control;

                foreach (Control control in this.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        textBox.BackColor = SystemColors.Window;
                        textBox.ForeColor = SystemColors.WindowText;
                        textBox.BorderStyle = BorderStyle.FixedSingle;
                    }
                    else if (control is Button button)
                    {
                        button.BackColor = SystemColors.Control;
                        button.ForeColor = SystemColors.ControlText;
                        button.FlatStyle = FlatStyle.Standard;
                    }
                }
            }
        }
    }
}
