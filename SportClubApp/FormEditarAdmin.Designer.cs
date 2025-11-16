namespace SportClubApp
{
    partial class FormEditarAdmin
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
            lblTitulo = new Label();
            lblUserAdmin = new Label();
            txtUserAdmin = new TextBox();
            lblNuevaPassword = new Label();
            lblConfPassword = new Label();
            txtNuevaPassword = new TextBox();
            txtConfPassword = new TextBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Candara", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.Location = new Point(200, 80);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(202, 26);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Editar Administrador";
            lblTitulo.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblUserAdmin
            // 
            lblUserAdmin.AutoSize = true;
            lblUserAdmin.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUserAdmin.Location = new Point(167, 138);
            lblUserAdmin.Name = "lblUserAdmin";
            lblUserAdmin.Size = new Size(52, 15);
            lblUserAdmin.TabIndex = 1;
            lblUserAdmin.Text = "Usuario:";
            // 
            // txtUserAdmin
            // 
            txtUserAdmin.Location = new Point(230, 135);
            txtUserAdmin.Name = "txtUserAdmin";
            txtUserAdmin.Size = new Size(201, 23);
            txtUserAdmin.TabIndex = 2;
            // 
            // lblNuevaPassword
            // 
            lblNuevaPassword.AutoSize = true;
            lblNuevaPassword.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNuevaPassword.Location = new Point(112, 194);
            lblNuevaPassword.Name = "lblNuevaPassword";
            lblNuevaPassword.Size = new Size(110, 15);
            lblNuevaPassword.TabIndex = 3;
            lblNuevaPassword.Text = "Nueva Contraseña:";
            // 
            // lblConfPassword
            // 
            lblConfPassword.AutoSize = true;
            lblConfPassword.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblConfPassword.Location = new Point(92, 248);
            lblConfPassword.Name = "lblConfPassword";
            lblConfPassword.Size = new Size(131, 15);
            lblConfPassword.TabIndex = 4;
            lblConfPassword.Text = "Confirmar Contraseña:";
            // 
            // txtNuevaPassword
            // 
            txtNuevaPassword.Location = new Point(230, 191);
            txtNuevaPassword.Name = "txtNuevaPassword";
            txtNuevaPassword.Size = new Size(201, 23);
            txtNuevaPassword.TabIndex = 5;
            // 
            // txtConfPassword
            // 
            txtConfPassword.Location = new Point(230, 245);
            txtConfPassword.Name = "txtConfPassword";
            txtConfPassword.Size = new Size(201, 23);
            txtConfPassword.TabIndex = 6;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(230, 316);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(75, 23);
            btnGuardar.TabIndex = 7;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(355, 316);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(75, 23);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // FormEditarAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(560, 461);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(txtConfPassword);
            Controls.Add(txtNuevaPassword);
            Controls.Add(lblConfPassword);
            Controls.Add(lblNuevaPassword);
            Controls.Add(txtUserAdmin);
            Controls.Add(lblUserAdmin);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "FormEditarAdmin";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Editar Administrador";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblUserAdmin;
        private TextBox txtUserAdmin;
        private Label lblNuevaPassword;
        private Label lblConfPassword;
        private TextBox txtNuevaPassword;
        private TextBox txtConfPassword;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}