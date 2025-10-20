﻿using System;
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
    public partial class FormEditarAdmin : Form
    {
        private int adminId;
        private string currentUsername;

        // Constructor que recibe datos del admin a editar
        public FormEditarAdmin(int id, string username)
        {
            InitializeComponent();

            this.adminId = id;
            this.currentUsername = username;

            // Cargar datos existentes
            CargarDatosAdministrador();

            // Aplicar tema
            ThemeManager.ApplyTheme(this);
            AplicarTema();
            ThemeManager.ThemeChanged += (s, e) => AplicarTema();
        }

        private void CargarDatosAdministrador()
        {
            txtUserAdmin.Text = currentUsername;
        }

        // EVENTO: Botón Guardar
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                GuardarCambios();
            }
        }

        private bool ValidarDatos()
        {
            // Validar usuario
            if (string.IsNullOrWhiteSpace(txtUserAdmin.Text))
            {
                MessageBox.Show("El nombre de usuario es obligatorio.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserAdmin.Focus();
                return false;
            }

            // Validar si el username cambió y ya existe
            if (txtUserAdmin.Text != currentUsername && DBHelper.UsernameExists(txtUserAdmin.Text))
            {
                MessageBox.Show("El nombre de usuario ya existe. Elija otro.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserAdmin.Focus();
                return false;
            }

            // Validar contraseñas si se están cambiando
            if (!string.IsNullOrWhiteSpace(txtNuevaPassword.Text))
            {
                if (txtNuevaPassword.Text != txtConfPassword.Text)
                {
                    MessageBox.Show("Las contraseñas no coinciden.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNuevaPassword.Focus();
                    return false;
                }

                if (txtNuevaPassword.Text.Length < 6)
                {
                    MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNuevaPassword.Focus();
                    return false;
                }
            }

            return true;
        }

        private void GuardarCambios()
        {
            try
            {
                bool success = DBHelper.ActualizarAdministrador(
                    adminId,
                    txtUserAdmin.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtNuevaPassword.Text) ? null : txtNuevaPassword.Text
                );

                if (success)
                {
                    MessageBox.Show("Administrador actualizado exitosamente.", "Éxito",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el administrador.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // EVENTO: Botón Cancelar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // EVENTO: Tecla Enter en campos de texto
        private void txtUserAdmin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnGuardar_Click(sender, e);
            }
        }

        private void txtNuevaPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnGuardar_Click(sender, e);
            }
        }

        // APLICAR TEMA
        private void AplicarTema()
        {
            if (ThemeManager.IsDarkMode)
            {
                this.BackColor = ThemeManager.DarkTheme.Background;
                this.ForeColor = ThemeManager.DarkTheme.Text;
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
