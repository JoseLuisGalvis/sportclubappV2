using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SportClubApp
{
    public partial class FormMenuGestionCarnetCuotas : Form
    {
        public FormMenuGestionCarnetCuotas()
        {
            InitializeComponent();
        }

        // Método reutilizable para posicionar formularios uno debajo de otro
        private void MostrarFormularioDebajo(Form formularioNuevo, int offsetVertical = -90)
        {
            formularioNuevo.StartPosition = FormStartPosition.Manual;
            formularioNuevo.Location = new Point(this.Location.X, this.Location.Y + offsetVertical);
            formularioNuevo.Show();
        }

        private void btnGestionCarnets_Click(object sender, EventArgs e)
        {
            var formGestionCarnets = new FormGestionCarnets();
            MostrarFormularioDebajo(formGestionCarnets); // ✅ Usa el método común
        }

        private void btnCobro_Click(object sender, EventArgs e)
        {
            var formCobroCuotas = new FormCobroCuotas();
            MostrarFormularioDebajo(formCobroCuotas, 100); // ← 100px hacia ABAJO
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            // Buscar si hay un Dashboard abierto
            foreach (Form form in Application.OpenForms)
            {
                if (form is FormDashboardAdmin)
                {
                    form.BringToFront();
                    break;
                }
            }
            this.Close();
        }

    }
}
