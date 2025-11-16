namespace SportClubApp
{
    partial class FormRegistroNoSocio
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTituloNoSocio = new Label();
            lblNombre = new Label();
            lblApellido = new Label();
            lblDni = new Label();
            lblTelefono = new Label();
            lblEmail = new Label();
            lblUsername = new Label();
            lblFechaVisita = new Label();
            dtpFechaVisita = new DateTimePicker();
            btnRegistrar = new Button();
            btnCancelar = new Button();
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            txtDni = new TextBox();
            txtTelefono = new TextBox();
            txtEmail = new TextBox();
            txtUsername = new TextBox();
            chkHabilitado = new CheckBox();
            lblFoto = new Label();
            btnSeleccionarFoto = new Button();
            pbFotoPreview = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pbFotoPreview).BeginInit();
            SuspendLayout();
            // 
            // lblTituloNoSocio
            // 
            lblTituloNoSocio.AutoSize = true;
            lblTituloNoSocio.Font = new Font("Candara", 15.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTituloNoSocio.Location = new Point(199, 52);
            lblTituloNoSocio.Name = "lblTituloNoSocio";
            lblTituloNoSocio.Size = new Size(221, 26);
            lblTituloNoSocio.TabIndex = 0;
            lblTituloNoSocio.Text = "REGISTRO DE NO SOCIO";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNombre.Location = new Point(48, 119);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(53, 15);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre";
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblApellido.Location = new Point(48, 148);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(52, 15);
            lblApellido.TabIndex = 2;
            lblApellido.Text = "Apellido";
            // 
            // lblDni
            // 
            lblDni.AutoSize = true;
            lblDni.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDni.Location = new Point(48, 176);
            lblDni.Name = "lblDni";
            lblDni.Size = new Size(28, 15);
            lblDni.TabIndex = 3;
            lblDni.Text = "DNI";
            // 
            // lblTelefono
            // 
            lblTelefono.AutoSize = true;
            lblTelefono.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTelefono.Location = new Point(48, 205);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(56, 15);
            lblTelefono.TabIndex = 4;
            lblTelefono.Text = "Teléfono";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEmail.Location = new Point(47, 232);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(37, 15);
            lblEmail.TabIndex = 5;
            lblEmail.Text = "Email";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUsername.Location = new Point(47, 260);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(49, 15);
            lblUsername.TabIndex = 6;
            lblUsername.Text = "Usuario";
            // 
            // lblFechaVisita
            // 
            lblFechaVisita.AutoSize = true;
            lblFechaVisita.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFechaVisita.Location = new Point(47, 291);
            lblFechaVisita.Name = "lblFechaVisita";
            lblFechaVisita.Size = new Size(88, 15);
            lblFechaVisita.TabIndex = 9;
            lblFechaVisita.Text = "Fecha de Visita";
            // 
            // dtpFechaVisita
            // 
            dtpFechaVisita.Location = new Point(193, 285);
            dtpFechaVisita.Name = "dtpFechaVisita";
            dtpFechaVisita.Size = new Size(227, 23);
            dtpFechaVisita.TabIndex = 10;
            // 
            // btnRegistrar
            // 
            btnRegistrar.BackColor = Color.FromArgb(0, 192, 0);
            btnRegistrar.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRegistrar.ForeColor = Color.White;
            btnRegistrar.Location = new Point(92, 367);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(83, 35);
            btnRegistrar.TabIndex = 11;
            btnRegistrar.Text = "Guardar";
            btnRegistrar.UseVisualStyleBackColor = false;
            btnRegistrar.Click += btnRegistrar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(0, 192, 0);
            btnCancelar.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(338, 367);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(83, 35);
            btnCancelar.TabIndex = 12;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(193, 116);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(227, 23);
            txtNombre.TabIndex = 13;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(193, 145);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(227, 23);
            txtApellido.TabIndex = 14;
            // 
            // txtDni
            // 
            txtDni.Location = new Point(193, 173);
            txtDni.Name = "txtDni";
            txtDni.Size = new Size(227, 23);
            txtDni.TabIndex = 15;
            // 
            // txtTelefono
            // 
            txtTelefono.Location = new Point(194, 202);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(226, 23);
            txtTelefono.TabIndex = 16;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(194, 229);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(227, 23);
            txtEmail.TabIndex = 17;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(194, 256);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(227, 23);
            txtUsername.TabIndex = 18;
            // 
            // chkHabilitado
            // 
            chkHabilitado.AutoSize = true;
            chkHabilitado.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkHabilitado.Location = new Point(50, 330);
            chkHabilitado.Name = "chkHabilitado";
            chkHabilitado.Size = new Size(82, 19);
            chkHabilitado.TabIndex = 21;
            chkHabilitado.Text = "Habilitado";
            chkHabilitado.UseVisualStyleBackColor = true;
            // 
            // lblFoto
            // 
            lblFoto.AutoSize = true;
            lblFoto.Font = new Font("Candara", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFoto.Location = new Point(475, 85);
            lblFoto.Name = "lblFoto";
            lblFoto.Size = new Size(144, 19);
            lblFoto.TabIndex = 29;
            lblFoto.Text = "Foto del Pase Diario";
            // 
            // btnSeleccionarFoto
            // 
            btnSeleccionarFoto.BackColor = Color.LimeGreen;
            btnSeleccionarFoto.ForeColor = Color.White;
            btnSeleccionarFoto.Location = new Point(475, 297);
            btnSeleccionarFoto.Name = "btnSeleccionarFoto";
            btnSeleccionarFoto.Size = new Size(150, 35);
            btnSeleccionarFoto.TabIndex = 28;
            btnSeleccionarFoto.Text = "📷 Seleccionar Foto";
            btnSeleccionarFoto.UseVisualStyleBackColor = false;
            btnSeleccionarFoto.Click += btnSeleccionarFoto_Click;
            // 
            // pbFotoPreview
            // 
            pbFotoPreview.BackColor = SystemColors.ControlLight;
            pbFotoPreview.BorderStyle = BorderStyle.FixedSingle;
            pbFotoPreview.Location = new Point(475, 116);
            pbFotoPreview.Name = "pbFotoPreview";
            pbFotoPreview.Size = new Size(150, 168);
            pbFotoPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pbFotoPreview.TabIndex = 30;
            pbFotoPreview.TabStop = false;
            // 
            // FormRegistroNoSocio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(664, 511);
            Controls.Add(pbFotoPreview);
            Controls.Add(lblFoto);
            Controls.Add(btnSeleccionarFoto);
            Controls.Add(chkHabilitado);
            Controls.Add(txtUsername);
            Controls.Add(txtEmail);
            Controls.Add(txtTelefono);
            Controls.Add(txtDni);
            Controls.Add(txtApellido);
            Controls.Add(txtNombre);
            Controls.Add(btnCancelar);
            Controls.Add(btnRegistrar);
            Controls.Add(dtpFechaVisita);
            Controls.Add(lblFechaVisita);
            Controls.Add(lblUsername);
            Controls.Add(lblEmail);
            Controls.Add(lblTelefono);
            Controls.Add(lblDni);
            Controls.Add(lblApellido);
            Controls.Add(lblNombre);
            Controls.Add(lblTituloNoSocio);
            Name = "FormRegistroNoSocio";
            Text = "Formulario Registro No Socio";
            ((System.ComponentModel.ISupportInitialize)pbFotoPreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTituloNoSocio;
        private Label lblNombre;
        private Label lblApellido;
        private Label lblDni;
        private Label lblTelefono;
        private Label lblEmail;
        private Label lblUsername;
        private Label lblFechaVisita;
        private DateTimePicker dtpFechaVisita;
        private Button btnRegistrar;
        private Button btnCancelar;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtDni;
        private TextBox txtTelefono;
        private TextBox txtEmail;
        private TextBox txtUsername;
        private CheckBox chkHabilitado;
        private Label lblFoto;
        private PictureBox pbFotoPreview;
        private Button btnSeleccionarFoto;
    }
}