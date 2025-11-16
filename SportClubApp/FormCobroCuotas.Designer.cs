namespace SportClubApp
{
    partial class FormCobroCuotas
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
            lblCuotas = new Label();
            dataGridCuotas = new DataGridView();
            comboMetodoPago = new ComboBox();
            btnMarcarPagada = new Button();
            btnVolver = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridCuotas).BeginInit();
            SuspendLayout();
            // 
            // lblCuotas
            // 
            lblCuotas.AutoSize = true;
            lblCuotas.Font = new Font("Candara", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCuotas.Location = new Point(172, 67);
            lblCuotas.Name = "lblCuotas";
            lblCuotas.Size = new Size(235, 23);
            lblCuotas.TabIndex = 0;
            lblCuotas.Text = "Cobro de Cuotas Mensuales";
            // 
            // dataGridCuotas
            // 
            dataGridCuotas.BackgroundColor = Color.FromArgb(224, 224, 224);
            dataGridCuotas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridCuotas.Location = new Point(162, 105);
            dataGridCuotas.Name = "dataGridCuotas";
            dataGridCuotas.Size = new Size(257, 150);
            dataGridCuotas.TabIndex = 1;
            // 
            // comboMetodoPago
            // 
            comboMetodoPago.FormattingEnabled = true;
            comboMetodoPago.Location = new Point(236, 261);
            comboMetodoPago.Name = "comboMetodoPago";
            comboMetodoPago.Size = new Size(121, 23);
            comboMetodoPago.TabIndex = 2;
            // 
            // btnMarcarPagada
            // 
            btnMarcarPagada.BackColor = Color.FromArgb(0, 192, 0);
            btnMarcarPagada.Font = new Font("Candara", 9.75F, FontStyle.Bold);
            btnMarcarPagada.ForeColor = Color.White;
            btnMarcarPagada.Location = new Point(162, 348);
            btnMarcarPagada.Name = "btnMarcarPagada";
            btnMarcarPagada.Size = new Size(117, 43);
            btnMarcarPagada.TabIndex = 3;
            btnMarcarPagada.Text = "Marcar como Pagada";
            btnMarcarPagada.UseVisualStyleBackColor = false;
            btnMarcarPagada.Click += btnMarcarPagada_Click;
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(0, 192, 0);
            btnVolver.Font = new Font("Candara", 9.75F, FontStyle.Bold);
            btnVolver.ForeColor = Color.White;
            btnVolver.Location = new Point(344, 348);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(75, 43);
            btnVolver.TabIndex = 4;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // FormCobroCuotas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(590, 450);
            Controls.Add(btnVolver);
            Controls.Add(btnMarcarPagada);
            Controls.Add(comboMetodoPago);
            Controls.Add(dataGridCuotas);
            Controls.Add(lblCuotas);
            Name = "FormCobroCuotas";
            Text = "Form Cobro Cuotas";
            ((System.ComponentModel.ISupportInitialize)dataGridCuotas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCuotas;
        private DataGridView dataGridCuotas;
        private ComboBox comboMetodoPago;
        private Button btnMarcarPagada;
        private Button btnVolver;
    }
}