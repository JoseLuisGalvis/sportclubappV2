namespace SportClubApp
{
    partial class FormRegistroSocio
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
            label1 = new Label();
            lblNombre = new Label();
            lblApellido = new Label();
            lblDNI = new Label();
            lblTelefono = new Label();
            lblEMail = new Label();
            lblFechaAlta = new Label();
            chkAptoFisico = new CheckBox();
            chkHabilitado = new CheckBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            txtDNI = new TextBox();
            txtTelefono = new TextBox();
            txtEmail = new TextBox();
            dtpFechaAlta = new DateTimePicker();
            lblUsuario = new Label();
            txtUsuario = new TextBox();
            pbFotoPreview = new PictureBox();
            btnSeleccionarFoto = new Button();
            lblFoto = new Label();
            ((System.ComponentModel.ISupportInitialize)pbFotoPreview).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Candara", 15.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(161, 56);
            label1.Name = "label1";
            label1.Size = new Size(189, 26);
            label1.TabIndex = 0;
            label1.Text = "REGISTRO DE SOCIO";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNombre.Location = new Point(41, 116);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(53, 15);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre";
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblApellido.Location = new Point(41, 149);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(52, 15);
            lblApellido.TabIndex = 2;
            lblApellido.Text = "Apellido";
            // 
            // lblDNI
            // 
            lblDNI.AutoSize = true;
            lblDNI.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDNI.Location = new Point(41, 183);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(28, 15);
            lblDNI.TabIndex = 3;
            lblDNI.Text = "DNI";
            // 
            // lblTelefono
            // 
            lblTelefono.AutoSize = true;
            lblTelefono.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTelefono.Location = new Point(40, 217);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(56, 15);
            lblTelefono.TabIndex = 4;
            lblTelefono.Text = "Teléfono";
            // 
            // lblEMail
            // 
            lblEMail.AutoSize = true;
            lblEMail.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEMail.Location = new Point(40, 247);
            lblEMail.Name = "lblEMail";
            lblEMail.Size = new Size(37, 15);
            lblEMail.TabIndex = 5;
            lblEMail.Text = "EMail";
            // 
            // lblFechaAlta
            // 
            lblFechaAlta.AutoSize = true;
            lblFechaAlta.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFechaAlta.Location = new Point(41, 304);
            lblFechaAlta.Name = "lblFechaAlta";
            lblFechaAlta.Size = new Size(64, 15);
            lblFechaAlta.TabIndex = 6;
            lblFechaAlta.Text = "Fecha Alta";
            // 
            // chkAptoFisico
            // 
            chkAptoFisico.AutoSize = true;
            chkAptoFisico.Location = new Point(40, 335);
            chkAptoFisico.Name = "chkAptoFisico";
            chkAptoFisico.Size = new Size(98, 23);
            chkAptoFisico.TabIndex = 9;
            chkAptoFisico.Text = "Apto Físico";
            chkAptoFisico.UseVisualStyleBackColor = true;
            // 
            // chkHabilitado
            // 
            chkHabilitado.AutoSize = true;
            chkHabilitado.Location = new Point(343, 335);
            chkHabilitado.Name = "chkHabilitado";
            chkHabilitado.Size = new Size(96, 23);
            chkHabilitado.TabIndex = 10;
            chkHabilitado.Text = "Habilitado";
            chkHabilitado.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(0, 192, 0);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(41, 381);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(93, 32);
            btnGuardar.TabIndex = 11;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(0, 192, 0);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(353, 381);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(86, 32);
            btnCancelar.TabIndex = 12;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(175, 107);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(263, 25);
            txtNombre.TabIndex = 13;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(175, 140);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(263, 25);
            txtApellido.TabIndex = 14;
            // 
            // txtDNI
            // 
            txtDNI.Location = new Point(175, 174);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(263, 25);
            txtDNI.TabIndex = 15;
            // 
            // txtTelefono
            // 
            txtTelefono.Location = new Point(175, 207);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(263, 25);
            txtTelefono.TabIndex = 16;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(177, 242);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(261, 25);
            txtEmail.TabIndex = 17;
            // 
            // dtpFechaAlta
            // 
            dtpFechaAlta.Location = new Point(177, 304);
            dtpFechaAlta.Name = "dtpFechaAlta";
            dtpFechaAlta.Size = new Size(261, 25);
            dtpFechaAlta.TabIndex = 18;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUsuario.Location = new Point(41, 278);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(49, 15);
            lblUsuario.TabIndex = 19;
            lblUsuario.Text = "Usuario";
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(177, 273);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(261, 25);
            txtUsuario.TabIndex = 20;
            // 
            // pbFotoPreview
            // 
            pbFotoPreview.BackColor = SystemColors.ControlLight;
            pbFotoPreview.BorderStyle = BorderStyle.FixedSingle;
            pbFotoPreview.Location = new Point(494, 111);
            pbFotoPreview.Name = "pbFotoPreview";
            pbFotoPreview.Size = new Size(150, 168);
            pbFotoPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pbFotoPreview.TabIndex = 25;
            pbFotoPreview.TabStop = false;
            // 
            // btnSeleccionarFoto
            // 
            btnSeleccionarFoto.BackColor = Color.LimeGreen;
            btnSeleccionarFoto.ForeColor = Color.White;
            btnSeleccionarFoto.Location = new Point(494, 293);
            btnSeleccionarFoto.Name = "btnSeleccionarFoto";
            btnSeleccionarFoto.Size = new Size(150, 35);
            btnSeleccionarFoto.TabIndex = 26;
            btnSeleccionarFoto.Text = "📷 Seleccionar Foto";
            btnSeleccionarFoto.UseVisualStyleBackColor = false;
            btnSeleccionarFoto.Click += btnSeleccionarFoto_Click;
            // 
            // label3
            // 
            lblFoto.AutoSize = true;
            lblFoto.Font = new Font("Candara", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFoto.Location = new Point(516, 79);
            lblFoto.Name = "label3";
            lblFoto.Size = new Size(114, 19);
            lblFoto.TabIndex = 27;
            lblFoto.Text = "Foto del Carnet";
            // 
            // FormRegistroSocio
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(681, 511);
            Controls.Add(lblFoto);
            Controls.Add(btnSeleccionarFoto);
            Controls.Add(pbFotoPreview);
            Controls.Add(txtUsuario);
            Controls.Add(lblUsuario);
            Controls.Add(dtpFechaAlta);
            Controls.Add(txtEmail);
            Controls.Add(txtTelefono);
            Controls.Add(txtDNI);
            Controls.Add(txtApellido);
            Controls.Add(txtNombre);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(chkHabilitado);
            Controls.Add(chkAptoFisico);
            Controls.Add(lblFechaAlta);
            Controls.Add(lblEMail);
            Controls.Add(lblTelefono);
            Controls.Add(lblDNI);
            Controls.Add(lblApellido);
            Controls.Add(lblNombre);
            Controls.Add(label1);
            Font = new Font("Segoe UI", 10F, FontStyle.Bold | FontStyle.Italic);
            Location = new Point(660, 80);
            Name = "FormRegistroSocio";
            Text = "Formulario Registro Socio";
            ((System.ComponentModel.ISupportInitialize)pbFotoPreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label lblNombre;
        private Label lblApellido;
        private Label lblDNI;
        private Label lblTelefono;
        private Label lblEMail;
        private Label lblFechaAlta;
        private CheckBox chkAptoFisico;
        private CheckBox chkHabilitado;
        private Button btnGuardar;
        private Button btnCancelar;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtDNI;
        private TextBox txtTelefono;
        private TextBox txtEmail;
        private DateTimePicker dtpFechaAlta;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private PictureBox pbFotoPreview;
        private Button btnSeleccionarFoto;
        private Label lblFoto;
    }
}