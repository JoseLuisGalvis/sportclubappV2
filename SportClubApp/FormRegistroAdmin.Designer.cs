namespace SportClubApp
{
    partial class FormRegistroAdmin
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
            lblRegistroAdmin = new Label();
            lblUserAdmin = new Label();
            txtUserAdmin = new TextBox();
            lblPasswordAdmin = new Label();
            txtPasswordAdmin = new TextBox();
            lblConfPasswordAdmin = new Label();
            txtConfPassAdmin = new TextBox();
            btnRegistrarAdmin = new Button();
            btnCancelarAdmin = new Button();
            SuspendLayout();
            // 
            // lblRegistroAdmin
            // 
            lblRegistroAdmin.AutoSize = true;
            lblRegistroAdmin.Font = new Font("Candara", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRegistroAdmin.Location = new Point(285, 81);
            lblRegistroAdmin.Name = "lblRegistroAdmin";
            lblRegistroAdmin.Size = new Size(226, 26);
            lblRegistroAdmin.TabIndex = 0;
            lblRegistroAdmin.Text = "Registro Administrador";
            // 
            // lblUserAdmin
            // 
            lblUserAdmin.AutoSize = true;
            lblUserAdmin.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUserAdmin.Location = new Point(237, 139);
            lblUserAdmin.Name = "lblUserAdmin";
            lblUserAdmin.Size = new Size(49, 15);
            lblUserAdmin.TabIndex = 1;
            lblUserAdmin.Text = "Usuario";
            // 
            // txtUserAdmin
            // 
            txtUserAdmin.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtUserAdmin.Location = new Point(371, 139);
            txtUserAdmin.Name = "txtUserAdmin";
            txtUserAdmin.Size = new Size(169, 23);
            txtUserAdmin.TabIndex = 2;
            // 
            // lblPasswordAdmin
            // 
            lblPasswordAdmin.AutoSize = true;
            lblPasswordAdmin.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPasswordAdmin.Location = new Point(236, 184);
            lblPasswordAdmin.Name = "lblPasswordAdmin";
            lblPasswordAdmin.Size = new Size(69, 15);
            lblPasswordAdmin.TabIndex = 3;
            lblPasswordAdmin.Text = "Contraseña";
            // 
            // txtPasswordAdmin
            // 
            txtPasswordAdmin.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtPasswordAdmin.Location = new Point(371, 184);
            txtPasswordAdmin.Name = "txtPasswordAdmin";
            txtPasswordAdmin.Size = new Size(168, 23);
            txtPasswordAdmin.TabIndex = 4;
            // 
            // lblConfPasswordAdmin
            // 
            lblConfPasswordAdmin.AutoSize = true;
            lblConfPasswordAdmin.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblConfPasswordAdmin.Location = new Point(233, 229);
            lblConfPasswordAdmin.Name = "lblConfPasswordAdmin";
            lblConfPasswordAdmin.Size = new Size(128, 15);
            lblConfPasswordAdmin.TabIndex = 5;
            lblConfPasswordAdmin.Text = "Confirmar Contraseña";
            // 
            // txtConfPassAdmin
            // 
            txtConfPassAdmin.Location = new Point(371, 226);
            txtConfPassAdmin.Name = "txtConfPassAdmin";
            txtConfPassAdmin.Size = new Size(169, 23);
            txtConfPassAdmin.TabIndex = 6;
            // 
            // btnRegistrarAdmin
            // 
            btnRegistrarAdmin.BackColor = Color.FromArgb(0, 192, 0);
            btnRegistrarAdmin.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRegistrarAdmin.ForeColor = Color.White;
            btnRegistrarAdmin.Location = new Point(243, 279);
            btnRegistrarAdmin.Name = "btnRegistrarAdmin";
            btnRegistrarAdmin.Size = new Size(110, 42);
            btnRegistrarAdmin.TabIndex = 7;
            btnRegistrarAdmin.Text = "Registrar Administrador";
            btnRegistrarAdmin.UseVisualStyleBackColor = false;
            btnRegistrarAdmin.Click += btnRegistrar_ClickAdmin;
            // 
            // btnCancelarAdmin
            // 
            btnCancelarAdmin.BackColor = Color.FromArgb(0, 192, 0);
            btnCancelarAdmin.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelarAdmin.ForeColor = Color.White;
            btnCancelarAdmin.Location = new Point(430, 280);
            btnCancelarAdmin.Name = "btnCancelarAdmin";
            btnCancelarAdmin.Size = new Size(110, 41);
            btnCancelarAdmin.TabIndex = 8;
            btnCancelarAdmin.Text = "Cancelar";
            btnCancelarAdmin.UseVisualStyleBackColor = false;
            btnCancelarAdmin.Click += btnCancelar_Click;
            // 
            // FormRegistroAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCancelarAdmin);
            Controls.Add(btnRegistrarAdmin);
            Controls.Add(txtConfPassAdmin);
            Controls.Add(lblConfPasswordAdmin);
            Controls.Add(txtPasswordAdmin);
            Controls.Add(lblPasswordAdmin);
            Controls.Add(txtUserAdmin);
            Controls.Add(lblUserAdmin);
            Controls.Add(lblRegistroAdmin);
            Name = "FormRegistroAdmin";
            Text = "Formulario Acceso Administrador";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRegistroAdmin;
        private Label lblUserAdmin;
        private TextBox txtUserAdmin;
        private Label lblPasswordAdmin;
        private TextBox txtPasswordAdmin;
        private Label lblConfPasswordAdmin;
        private TextBox txtConfPassAdmin;
        private Button btnRegistrarAdmin;
        private Button btnCancelarAdmin;
    }
}