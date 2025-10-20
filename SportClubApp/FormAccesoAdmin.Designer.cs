namespace SportClubApp
{
    partial class FormAccesoAdmin
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
            lblAccesoAdmin = new Label();
            lblUserAccesoAdmin = new Label();
            txtAccesoUserAdmin = new TextBox();
            lblAccesoPassAdmin = new Label();
            txtAccesoPassAdmin = new TextBox();
            btnAccessoLogin = new Button();
            btnAccesoCancelar = new Button();
            SuspendLayout();
            // 
            // lblAccesoAdmin
            // 
            lblAccesoAdmin.AutoSize = true;
            lblAccesoAdmin.Font = new Font("Candara", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAccesoAdmin.Location = new Point(247, 94);
            lblAccesoAdmin.Name = "lblAccesoAdmin";
            lblAccesoAdmin.Size = new Size(331, 26);
            lblAccesoAdmin.TabIndex = 0;
            lblAccesoAdmin.Text = "INICIAR SESIÓN - ADMINISTRADOR";
            lblAccesoAdmin.Click += btnAcceder_Click;
            // 
            // lblUserAccesoAdmin
            // 
            lblUserAccesoAdmin.AutoSize = true;
            lblUserAccesoAdmin.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUserAccesoAdmin.Location = new Point(271, 153);
            lblUserAccesoAdmin.Name = "lblUserAccesoAdmin";
            lblUserAccesoAdmin.Size = new Size(49, 15);
            lblUserAccesoAdmin.TabIndex = 1;
            lblUserAccesoAdmin.Text = "Usuario";
            // 
            // txtAccesoUserAdmin
            // 
            txtAccesoUserAdmin.Location = new Point(350, 150);
            txtAccesoUserAdmin.Name = "txtAccesoUserAdmin";
            txtAccesoUserAdmin.Size = new Size(154, 23);
            txtAccesoUserAdmin.TabIndex = 2;
            // 
            // lblAccesoPassAdmin
            // 
            lblAccesoPassAdmin.AutoSize = true;
            lblAccesoPassAdmin.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAccesoPassAdmin.Location = new Point(271, 195);
            lblAccesoPassAdmin.Name = "lblAccesoPassAdmin";
            lblAccesoPassAdmin.Size = new Size(69, 15);
            lblAccesoPassAdmin.TabIndex = 3;
            lblAccesoPassAdmin.Text = "Contraseña";
            // 
            // txtAccesoPassAdmin
            // 
            txtAccesoPassAdmin.Location = new Point(350, 192);
            txtAccesoPassAdmin.Name = "txtAccesoPassAdmin";
            txtAccesoPassAdmin.Size = new Size(154, 23);
            txtAccesoPassAdmin.TabIndex = 4;
            // 
            // btnAccessoLogin
            // 
            btnAccessoLogin.BackColor = Color.FromArgb(0, 192, 0);
            btnAccessoLogin.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAccessoLogin.ForeColor = Color.White;
            btnAccessoLogin.Location = new Point(278, 267);
            btnAccessoLogin.Name = "btnAccessoLogin";
            btnAccessoLogin.Size = new Size(108, 38);
            btnAccessoLogin.TabIndex = 5;
            btnAccessoLogin.Text = "Iniciar Sesión";
            btnAccessoLogin.UseVisualStyleBackColor = false;
            btnAccessoLogin.Click += btnAcceder_Click;
            // 
            // btnAccesoCancelar
            // 
            btnAccesoCancelar.BackColor = Color.FromArgb(0, 192, 0);
            btnAccesoCancelar.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAccesoCancelar.ForeColor = Color.White;
            btnAccesoCancelar.Location = new Point(396, 267);
            btnAccesoCancelar.Name = "btnAccesoCancelar";
            btnAccesoCancelar.Size = new Size(108, 38);
            btnAccesoCancelar.TabIndex = 6;
            btnAccesoCancelar.Text = "Cancelar";
            btnAccesoCancelar.UseVisualStyleBackColor = false;
            btnAccesoCancelar.Click += btnCancelar_Click;
            // 
            // FormAccesoAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnAccesoCancelar);
            Controls.Add(btnAccessoLogin);
            Controls.Add(txtAccesoPassAdmin);
            Controls.Add(lblAccesoPassAdmin);
            Controls.Add(txtAccesoUserAdmin);
            Controls.Add(lblUserAccesoAdmin);
            Controls.Add(lblAccesoAdmin);
            Name = "FormAccesoAdmin";
            Text = "Formulario Acceso Administrador";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblAccesoAdmin;
        private Label lblUserAccesoAdmin;
        private TextBox txtAccesoUserAdmin;
        private Label lblAccesoPassAdmin;
        private TextBox txtAccesoPassAdmin;
        private Button btnAccessoLogin;
        private Button btnAccesoCancelar;
    }
}