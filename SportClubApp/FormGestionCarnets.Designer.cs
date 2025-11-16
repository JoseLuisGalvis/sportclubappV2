namespace SportClubApp
{
    partial class FormGestionCarnets
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
            dataGridSocios = new DataGridView();
            btnGenerarCarnet = new Button();
            btnMarcarEntregado = new Button();
            btnVolver = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridSocios).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Candara", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(191, 88);
            label1.Name = "label1";
            label1.Size = new Size(185, 26);
            label1.TabIndex = 0;
            label1.Text = "Gestión de Carnets";
            // 
            // dataGridSocios
            // 
            dataGridSocios.BackgroundColor = SystemColors.ControlLight;
            dataGridSocios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridSocios.Location = new Point(107, 141);
            dataGridSocios.Name = "dataGridSocios";
            dataGridSocios.Size = new Size(369, 150);
            dataGridSocios.TabIndex = 1;
            // 
            // btnGenerarCarnet
            // 
            btnGenerarCarnet.BackColor = Color.FromArgb(0, 192, 0);
            btnGenerarCarnet.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGenerarCarnet.ForeColor = Color.White;
            btnGenerarCarnet.Location = new Point(107, 354);
            btnGenerarCarnet.Name = "btnGenerarCarnet";
            btnGenerarCarnet.Size = new Size(107, 44);
            btnGenerarCarnet.TabIndex = 2;
            btnGenerarCarnet.Text = "Generar Carnet";
            btnGenerarCarnet.UseVisualStyleBackColor = false;
            btnGenerarCarnet.Click += btnGenerarCarnet_Click;
            // 
            // btnMarcarEntregado
            // 
            btnMarcarEntregado.BackColor = Color.FromArgb(0, 192, 0);
            btnMarcarEntregado.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnMarcarEntregado.ForeColor = Color.White;
            btnMarcarEntregado.Location = new Point(235, 354);
            btnMarcarEntregado.Name = "btnMarcarEntregado";
            btnMarcarEntregado.Size = new Size(107, 44);
            btnMarcarEntregado.TabIndex = 3;
            btnMarcarEntregado.Text = "Marcar Entregado";
            btnMarcarEntregado.UseVisualStyleBackColor = false;
            btnMarcarEntregado.Click += btnMarcarEntregado_Click;
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(0, 192, 0);
            btnVolver.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnVolver.ForeColor = Color.White;
            btnVolver.Location = new Point(369, 354);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(107, 44);
            btnVolver.TabIndex = 4;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // FormGestionCarnets
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(575, 450);
            Controls.Add(btnVolver);
            Controls.Add(btnMarcarEntregado);
            Controls.Add(btnGenerarCarnet);
            Controls.Add(dataGridSocios);
            Controls.Add(label1);
            Name = "FormGestionCarnets";
            Text = "Form Gestión Carnets";
            ((System.ComponentModel.ISupportInitialize)dataGridSocios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private DataGridView dataGridSocios;
        private Button btnGenerarCarnet;
        private Button btnMarcarEntregado;
        private Button btnVolver;
    }
}