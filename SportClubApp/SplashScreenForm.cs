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
    public partial class SplashScreenForm : Form
    {
        private ProgressBar progressBar;
        private System.Windows.Forms.Timer timer;
        private int progress = 0;

        public SplashScreenForm()
        {
            InitializeComponent();
            ConfigurarSplashScreen();
        }

        private void ConfigurarSplashScreen()
        {
            // Configuración del formulario
            this.BackColor = Color.FromArgb(0, 84, 166);
            this.ShowInTaskbar = false;

            // Logo
            try
            {
                string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "logo_sportclub.png");
                if (File.Exists(logoPath))
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        Image = Image.FromFile(logoPath),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Size = new Size(400, 300),
                        Location = new Point(100, 50),
                        BackColor = Color.Transparent
                    };
                    this.Controls.Add(pictureBox);
                }
            }
            catch (Exception ex)
            {
                // Si falla cargar la imagen, continuar sin ella
                MessageBox.Show($"No se pudo cargar el logo: {ex.Message}");
            }

            // Progress Bar
            progressBar = new ProgressBar
            {
                Location = new Point(100, 370),
                Size = new Size(400, 20),
                Style = ProgressBarStyle.Continuous
            };
            this.Controls.Add(progressBar);

            // Label "Cargando..."
            Label lblCargando = new Label
            {
                Text = "Cargando SportClubApp...",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(220, 400)
            };
            this.Controls.Add(lblCargando);

            // Timer para animar la barra
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 30;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            progress += 2;
            progressBar.Value = Math.Min(progress, 100);

            if (progress >= 100)
            {
                timer.Stop();
                this.Close();
            }
        }
    }
}