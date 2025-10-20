using System.Diagnostics;
using System.Drawing;

namespace SportClubApp
{
    public partial class Form1 : Form
    {
        // Variables para las imágenes
        private Image? imagenNadadorOriginal;
        private Image? imagenNadadorBlanca;

        // Variables para los menús dropdown
        private ContextMenuStrip menuRegistro;
        private ContextMenuStrip menuAcceder;

        // Variables para el zoom en pictureBox1
        private Size tamañoOriginalPictureBox;
        private Size tamañoZoomedPictureBox;
        private System.Windows.Forms.Timer timerZoom;
        private bool zoomingIn = false;

        public Form1()
        {
            InitializeComponent();
            InicializarMenuRegistro(); // Inicializar el menú dropdown
            InicializarMenuAcceder();
            CargarImagenes();

            // Aplicar tema inicial
            ThemeManager.ApplyTheme(this);
            AplicarTema();

            // Suscribirse a cambios de tema
            ThemeManager.ThemeChanged += (s, e) => AplicarTema();

            // Centrar la forma en la pantalla y maximizarla
            this.StartPosition = FormStartPosition.CenterScreen;

            // Inicializar zoom para pictureBox1
            InicializarZoomPictureBox();

        }

        // ============================================
        // MÉTODO: Inicializar zoom para pictureBox1
        // ============================================
        private void InicializarZoomPictureBox()
        {
            // Guardar tamaño original (asumiendo que se setea en designer)
            tamañoOriginalPictureBox = pictureBox1.Size;
            // Tamaño zoomed: aumenta un 20% (ajusta según necesites)
            tamañoZoomedPictureBox = new Size(
                (int)(tamañoOriginalPictureBox.Width * 1.2),
                (int)(tamañoOriginalPictureBox.Height * 1.2));

            // Eventos de mouse
            pictureBox1.MouseEnter += PictureBox1_MouseEnter;
            pictureBox1.MouseLeave += PictureBox1_MouseLeave;

            // Timer para animación suave (intervalo de 20ms para fluidez)
            timerZoom = new System.Windows.Forms.Timer();
            timerZoom.Interval = 20;
            timerZoom.Tick += TimerZoom_Tick;
        }

        // ============================================
        // EVENTO: Mouse entra en pictureBox1 (inicia zoom in)
        // ============================================
        private void PictureBox1_MouseEnter(object sender, EventArgs e)
        {
            zoomingIn = true;
            timerZoom.Start();
        }

        // ============================================
        // EVENTO: Mouse sale de pictureBox1 (inicia zoom out)
        // ============================================
        private void PictureBox1_MouseLeave(object sender, EventArgs e)
        {
            zoomingIn = false;
            timerZoom.Start();
        }

        // ============================================
        // EVENTO: Tick del timer para animar el zoom
        // ============================================
        private void TimerZoom_Tick(object sender, EventArgs e)
        {
            int step = 5; // Paso de animación (ajusta para velocidad)

            if (zoomingIn)
            {
                // Aumentar tamaño gradualmente
                if (pictureBox1.Width < tamañoZoomedPictureBox.Width)
                {
                    pictureBox1.Width += step;
                    pictureBox1.Height += step;
                    // Centrar si es necesario (ajusta location para que no se desplace)
                    pictureBox1.Left -= step / 2;
                    pictureBox1.Top -= step / 2;
                }
                else
                {
                    timerZoom.Stop();
                }
            }
            else
            {
                // Reducir tamaño gradualmente
                if (pictureBox1.Width > tamañoOriginalPictureBox.Width)
                {
                    pictureBox1.Width -= step;
                    pictureBox1.Height -= step;
                    // Restaurar location
                    pictureBox1.Left += step / 2;
                    pictureBox1.Top += step / 2;
                }
                else
                {
                    timerZoom.Stop();
                    // Asegurar tamaño exacto
                    pictureBox1.Size = tamañoOriginalPictureBox;
                }
            }
        }


        // ============================================
        // MÉTODO: Inicializar menú dropdown de Registro
        // ============================================
        private void InicializarMenuRegistro()
        {
            menuRegistro = new ContextMenuStrip();
            menuRegistro.BackColor = Color.FromArgb(30, 30, 30);
            menuRegistro.ForeColor = Color.White;
            menuRegistro.ShowImageMargin = false;
            menuRegistro.Font = new Font("Segoe UI", 10F);

            // Crear opciones del menú
            ToolStripMenuItem itemSocio = new ToolStripMenuItem("Registrar Socio");
            itemSocio.ForeColor = Color.White;
            itemSocio.Click += ItemSocio_Click;

            ToolStripMenuItem itemNoSocio = new ToolStripMenuItem("Registrar No Socio");
            itemNoSocio.ForeColor = Color.White;
            itemNoSocio.Click += ItemNoSocio_Click;

            // Agregar items al menú
            menuRegistro.Items.Add(itemSocio);
            menuRegistro.Items.Add(itemNoSocio);


            // Efectos hover para cada item
            foreach (ToolStripItem item in menuRegistro.Items)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    ToolStripMenuItem currentItem = menuItem;

                    currentItem.MouseEnter += (s, e) => {
                        currentItem.BackColor = Color.FromArgb(10, 30, 120);
                    };
                    currentItem.MouseLeave += (s, e) => {
                        currentItem.BackColor = Color.FromArgb(30, 30, 30);
                    };
                }
            }
        }

        // ============================================
        // MÉTODO: Inicializar menú dropdown de Acceder
        // ============================================
        private void InicializarMenuAcceder()
        {
            menuAcceder = new ContextMenuStrip();
            menuAcceder.BackColor = Color.FromArgb(30, 30, 30);
            menuAcceder.ForeColor = Color.White;
            menuAcceder.ShowImageMargin = false;
            menuAcceder.Font = new Font("Segoe UI", 10F);

            ToolStripSeparator separador = new ToolStripSeparator();

            ToolStripMenuItem itemAccesoAdmin = new ToolStripMenuItem("Acceso Administrador");
            itemAccesoAdmin.ForeColor = Color.FromArgb(255, 200, 100); // Color dorado
            itemAccesoAdmin.Click += ItemAccesoAdmin_Click;

            // Agregar items al menú
            menuAcceder.Items.Add(separador);
            menuAcceder.Items.Add(itemAccesoAdmin);

            // Efectos hover para cada item
            foreach (ToolStripItem item in menuAcceder.Items)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    ToolStripMenuItem currentItem = menuItem; 

                    currentItem.MouseEnter += (s, e) => {
                        currentItem.BackColor = Color.FromArgb(20, 40, 80);
                    };
                    currentItem.MouseLeave += (s, e) => {
                        currentItem.BackColor = Color.FromArgb(30, 30, 30);
                    };
                }
            }
        }


        // ============================================
        // MÉTODO: Cargar imágenes del nadador
        // ============================================
        private void CargarImagenes()
        {
            try
            {
                // Cargar imagen original
                string rutaOriginal = Path.Combine(Application.StartupPath, "Images", "natacion.png");
                if (File.Exists(rutaOriginal))
                {
                    imagenNadadorOriginal = Image.FromFile(rutaOriginal);
                    pictureBox1.Image = imagenNadadorOriginal;
                }

                // Cargar imagen blanca
                CargarImagenBlanca();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar imágenes: {ex.Message}", "Error");
            }
        }

        private void CargarImagenBlanca()
        {
            try
            {
                string rutaImagen = Path.Combine(Application.StartupPath, "Images", "natacion_white.png");

                Debug.WriteLine($"=== CARGANDO IMAGEN BLANCA ===");
                Debug.WriteLine($"Ruta: {rutaImagen}");
                Debug.WriteLine($"Existe: {File.Exists(rutaImagen)}");

                if (File.Exists(rutaImagen))
                {
                    // Liberar imagen anterior si existe
                    if (imagenNadadorBlanca != null && imagenNadadorBlanca != imagenNadadorOriginal)
                    {
                        imagenNadadorBlanca.Dispose();
                    }

                    imagenNadadorBlanca = Image.FromFile(rutaImagen);
                    Debug.WriteLine("✓ IMAGEN CARGADA CORRECTAMENTE");
                }
                else
                {
                    Debug.WriteLine("✗ IMAGEN NO ENCONTRADA");
                    imagenNadadorBlanca = imagenNadadorOriginal;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error");
                imagenNadadorBlanca = imagenNadadorOriginal;
            }
        }


        // ============================================
        // MÉTODO: Aplicar tema completo
        // ============================================
        private void AplicarTema()
        {
            if (ThemeManager.IsDarkMode)
            {
                this.BackColor = ThemeManager.DarkTheme.Background;
                lblAppName.ForeColor = ThemeManager.DarkTheme.Text;
                pnlHeader.BackColor = ThemeManager.DarkTheme.Header;
                btnModoOscuro.Text = "Modo Claro";

                if (imagenNadadorBlanca != null)
                    pictureBox1.Image = imagenNadadorBlanca;
            }
            else
            {
                this.BackColor = ThemeManager.LightTheme.Background;
                lblAppName.ForeColor = ThemeManager.LightTheme.Text;
                pnlHeader.BackColor = ThemeManager.LightTheme.Header;
                btnModoOscuro.Text = "Modo Oscuro";

                if (imagenNadadorOriginal != null)
                    pictureBox1.Image = imagenNadadorOriginal;
            }
        }

        // ============================================
        // EVENTO: Botón Modo Oscuro
        // ============================================
        private void btnModoOscuro_Click(object sender, EventArgs e)
        {
            ThemeManager.ToggleTheme();
        }


        // ============================================
        // EVENTO: Botón Acceder (Mostrar dropdown)
        // ============================================
        private void btnAcceder_Click(object sender, EventArgs e)
        {
            // Mostrar el menú justo debajo del botón
            Point ubicacion = btnAcceder.PointToScreen(new Point(0, btnAcceder.Height));
            menuAcceder.Show(ubicacion);
        }

        // ============================================
        // EVENTO: Botón Registro (Mostrar dropdown)
        // ============================================
        private void btnRegistro_Click(object sender, EventArgs e)
        {
            // Mostrar el menú justo debajo del botón
            Point ubicacion = btnRegistro.PointToScreen(new Point(0, btnRegistro.Height));
            menuRegistro.Show(ubicacion);
        }

        // ============================================
        // EVENTOS: Opciones del menú Registro
        // ============================================
        private void ItemSocio_Click(object sender, EventArgs e)
        {

            // Formulario Registro Socio:
            FormRegistroSocio formSocio = new FormRegistroSocio();
            formSocio.ShowDialog();
        }

        private void ItemNoSocio_Click(object sender, EventArgs e)
        {

            // Descomentar cuando crees el formulario:
            FormRegistroNoSocio formNoSocio = new FormRegistroNoSocio();
            formNoSocio.ShowDialog();
        }



        // ============================================
        // EVENTOS: Opciones del menú Acceder
        // ============================================

        private void ItemAccesoAdmin_Click(object sender, EventArgs e)
        {
            // Descomentar cuando crees el formulario:
            FormAccesoAdmin formAccesoAdmin = new FormAccesoAdmin();
            formAccesoAdmin.ShowDialog();
        }

        // ============================================
        // EVENTOS: PictureBox de Redes Sociales
        // ============================================
        private void pictureBoxInstagram_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = "https://www.instagram.com",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir Instagram: {ex.Message}");
            }
        }

        private void pictureBoxLinkedIn_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = "https://www.linkedin.com",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir LinkedIn: {ex.Message}");
            }
        }

        private void pictureBoxYoutube_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = "https://www.youtube.com",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir YouTube: {ex.Message}");
            }
        }
    }
}
