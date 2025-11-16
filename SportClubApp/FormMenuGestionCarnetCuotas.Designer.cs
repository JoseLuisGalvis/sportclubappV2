namespace SportClubApp
{
    partial class FormMenuGestionCarnetCuotas
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
            btnGestionCarnets = new Button();
            btnVolver = new Button();
            lblGestion = new Label();
            btnCobro = new Button();
            SuspendLayout();
            // 
            // btnGestionCarnets
            // 
            btnGestionCarnets.BackColor = Color.FromArgb(0, 192, 0);
            btnGestionCarnets.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGestionCarnets.ForeColor = Color.White;
            btnGestionCarnets.Location = new Point(59, 298);
            btnGestionCarnets.Name = "btnGestionCarnets";
            btnGestionCarnets.Size = new Size(121, 36);
            btnGestionCarnets.TabIndex = 0;
            btnGestionCarnets.Text = "Gestión Carnets";
            btnGestionCarnets.UseVisualStyleBackColor = false;
            btnGestionCarnets.Click += btnGestionCarnets_Click;
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(0, 192, 0);
            btnVolver.Font = new Font("Candara", 11.25F, FontStyle.Bold);
            btnVolver.ForeColor = Color.White;
            btnVolver.Location = new Point(423, 298);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(121, 36);
            btnVolver.TabIndex = 2;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // lblGestion
            // 
            lblGestion.AutoSize = true;
            lblGestion.Font = new Font("Candara", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGestion.ForeColor = Color.Black;
            lblGestion.Location = new Point(178, 106);
            lblGestion.Name = "lblGestion";
            lblGestion.Size = new Size(236, 23);
            lblGestion.TabIndex = 3;
            lblGestion.Text = "Gestión de Carnets y Cuotas";
            lblGestion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnCobro
            // 
            btnCobro.BackColor = Color.FromArgb(0, 192, 0);
            btnCobro.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCobro.ForeColor = Color.White;
            btnCobro.Location = new Point(237, 298);
            btnCobro.Name = "btnCobro";
            btnCobro.Size = new Size(121, 36);
            btnCobro.TabIndex = 1;
            btnCobro.Text = "Cobro Cuotas";
            btnCobro.UseVisualStyleBackColor = false;
            btnCobro.Click += btnCobro_Click;
            // 
            // FormMenuGestionCarnetCuotas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(590, 450);
            Controls.Add(lblGestion);
            Controls.Add(btnVolver);
            Controls.Add(btnCobro);
            Controls.Add(btnGestionCarnets);
            Name = "FormMenuGestionCarnetCuotas";
            Text = "Menu Gestion Carnets Cuotas";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnGestionCarnets;
        private Button btnCobro;
        private Button btnVolver;
        private Label lblGestion;
    }
}