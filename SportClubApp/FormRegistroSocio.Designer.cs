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
            button1 = new Button();
            button2 = new Button();
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            txtDNI = new TextBox();
            txtTelefono = new TextBox();
            txtEmail = new TextBox();
            dtpFechaAlta = new DateTimePicker();
            lblUsuario = new Label();
            txtUsuario = new TextBox();
            lblPasword = new Label();
            txtPassword = new TextBox();
            label2 = new Label();
            txtConfirmarPassword = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Candara", 15.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(362, 35);
            label1.Name = "label1";
            label1.Size = new Size(189, 26);
            label1.TabIndex = 0;
            label1.Text = "REGISTRO DE SOCIO";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNombre.Location = new Point(242, 95);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(53, 15);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre";
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblApellido.Location = new Point(242, 128);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(52, 15);
            lblApellido.TabIndex = 2;
            lblApellido.Text = "Apellido";
            // 
            // lblDNI
            // 
            lblDNI.AutoSize = true;
            lblDNI.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDNI.Location = new Point(242, 162);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(28, 15);
            lblDNI.TabIndex = 3;
            lblDNI.Text = "DNI";
            // 
            // lblTelefono
            // 
            lblTelefono.AutoSize = true;
            lblTelefono.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTelefono.Location = new Point(241, 196);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(56, 15);
            lblTelefono.TabIndex = 4;
            lblTelefono.Text = "Teléfono";
            // 
            // lblEMail
            // 
            lblEMail.AutoSize = true;
            lblEMail.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEMail.Location = new Point(241, 226);
            lblEMail.Name = "lblEMail";
            lblEMail.Size = new Size(37, 15);
            lblEMail.TabIndex = 5;
            lblEMail.Text = "EMail";
            // 
            // lblFechaAlta
            // 
            lblFechaAlta.AutoSize = true;
            lblFechaAlta.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFechaAlta.Location = new Point(242, 351);
            lblFechaAlta.Name = "lblFechaAlta";
            lblFechaAlta.Size = new Size(64, 15);
            lblFechaAlta.TabIndex = 6;
            lblFechaAlta.Text = "Fecha Alta";
            // 
            // chkAptoFisico
            // 
            chkAptoFisico.AutoSize = true;
            chkAptoFisico.Location = new Point(241, 382);
            chkAptoFisico.Name = "chkAptoFisico";
            chkAptoFisico.Size = new Size(98, 23);
            chkAptoFisico.TabIndex = 9;
            chkAptoFisico.Text = "Apto Físico";
            chkAptoFisico.UseVisualStyleBackColor = true;
            // 
            // chkHabilitado
            // 
            chkHabilitado.AutoSize = true;
            chkHabilitado.Location = new Point(544, 382);
            chkHabilitado.Name = "chkHabilitado";
            chkHabilitado.Size = new Size(96, 23);
            chkHabilitado.TabIndex = 10;
            chkHabilitado.Text = "Habilitado";
            chkHabilitado.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(0, 192, 0);
            button1.ForeColor = Color.White;
            button1.Location = new Point(242, 428);
            button1.Name = "button1";
            button1.Size = new Size(93, 32);
            button1.TabIndex = 11;
            button1.Text = "Guardar";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btnGuardar_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(0, 192, 0);
            button2.ForeColor = Color.White;
            button2.Location = new Point(554, 428);
            button2.Name = "button2";
            button2.Size = new Size(86, 32);
            button2.TabIndex = 12;
            button2.Text = "Cancelar";
            button2.UseVisualStyleBackColor = false;
            button2.Click += btnCancelar_Click;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(376, 86);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(263, 25);
            txtNombre.TabIndex = 13;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(376, 119);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(263, 25);
            txtApellido.TabIndex = 14;
            // 
            // txtDNI
            // 
            txtDNI.Location = new Point(376, 153);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(263, 25);
            txtDNI.TabIndex = 15;
            // 
            // txtTelefono
            // 
            txtTelefono.Location = new Point(376, 186);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(263, 25);
            txtTelefono.TabIndex = 16;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(378, 221);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(261, 25);
            txtEmail.TabIndex = 17;
            // 
            // dtpFechaAlta
            // 
            dtpFechaAlta.Location = new Point(378, 351);
            dtpFechaAlta.Name = "dtpFechaAlta";
            dtpFechaAlta.Size = new Size(261, 25);
            dtpFechaAlta.TabIndex = 18;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUsuario.Location = new Point(242, 257);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(49, 15);
            lblUsuario.TabIndex = 19;
            lblUsuario.Text = "Usuario";
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(378, 252);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(261, 25);
            txtUsuario.TabIndex = 20;
            // 
            // lblPasword
            // 
            lblPasword.AutoSize = true;
            lblPasword.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPasword.Location = new Point(242, 289);
            lblPasword.Name = "lblPasword";
            lblPasword.Size = new Size(69, 15);
            lblPasword.TabIndex = 21;
            lblPasword.Text = "Contraseña";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(378, 284);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(262, 25);
            txtPassword.TabIndex = 22;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(241, 322);
            label2.Name = "label2";
            label2.Size = new Size(128, 15);
            label2.TabIndex = 23;
            label2.Text = "Confirmar Contraseña";
            // 
            // txtConfirmarPassword
            // 
            txtConfirmarPassword.Location = new Point(378, 315);
            txtConfirmarPassword.Name = "txtConfirmarPassword";
            txtConfirmarPassword.Size = new Size(262, 25);
            txtConfirmarPassword.TabIndex = 24;
            // 
            // FormRegistroSocio
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 511);
            Controls.Add(txtConfirmarPassword);
            Controls.Add(label2);
            Controls.Add(txtPassword);
            Controls.Add(lblPasword);
            Controls.Add(txtUsuario);
            Controls.Add(lblUsuario);
            Controls.Add(dtpFechaAlta);
            Controls.Add(txtEmail);
            Controls.Add(txtTelefono);
            Controls.Add(txtDNI);
            Controls.Add(txtApellido);
            Controls.Add(txtNombre);
            Controls.Add(button2);
            Controls.Add(button1);
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
            Name = "FormRegistroSocio";
            Text = "Formulario Registro Socio";
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
        private Button button1;
        private Button button2;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtDNI;
        private TextBox txtTelefono;
        private TextBox txtEmail;
        private DateTimePicker dtpFechaAlta;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblPasword;
        private TextBox txtPassword;
        private Label label2;
        private TextBox txtConfirmarPassword;
    }
}