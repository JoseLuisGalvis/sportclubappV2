using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

namespace SportClubApp.Screens
{
    public partial class FormSeleccionUsuario : Form
    {
        public FormSeleccionUsuario()
        {
            InitializeComponent();
            this.Size = new Size(800, 600);
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(229, 84);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(314, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Selecciona el Tipo de Usuario";
            // 
            // FormSeleccionUsuario
            // 
            ClientSize = new Size(697, 424);
            Controls.Add(lblTitulo);
            Name = "FormSeleccionUsuario";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Selección de Usuario";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label? lblTitulo;
    }
}
