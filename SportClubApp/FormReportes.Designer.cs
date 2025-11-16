namespace SportClubApp
{
    partial class FormReportes
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
            dataGridReportes = new DataGridView();
            lblTitulo = new Label();  
            comboTipoReporte = new ComboBox();
            btnVolver = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridReportes).BeginInit();
            SuspendLayout();

            // dataGridReportes
            dataGridReportes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridReportes.Location = new Point(30, 100);  
            dataGridReportes.Name = "dataGridReportes";
            dataGridReportes.Size = new Size(740, 280);     
            dataGridReportes.TabIndex = 0;
            dataGridReportes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // lblTitulo (ANTES lblReportes)
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Candara", 15.75F, FontStyle.Bold);
            lblTitulo.Location = new Point(250, 20);         
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(300, 26);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Reportes del Sistema";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;

            // comboTipoReporte
            comboTipoReporte.DropDownStyle = ComboBoxStyle.DropDownList;
            comboTipoReporte.Font = new Font("Candara", 11.25F);
            comboTipoReporte.FormattingEnabled = true;
            comboTipoReporte.Items.AddRange(new object[] {
        "Cuotas que vencen hoy",
        "Socios con deudas",
        "Socios al día"});
            comboTipoReporte.Location = new Point(30, 60);   
            comboTipoReporte.Name = "comboTipoReporte";
            comboTipoReporte.Size = new Size(250, 26); 
            comboTipoReporte.TabIndex = 2;
            comboTipoReporte.SelectedIndexChanged += comboTipoReporte_SelectedIndexChanged;

            // btnVolver
            btnVolver.BackColor = Color.FromArgb(0, 192, 0);
            btnVolver.Font = new Font("Candara", 12F, FontStyle.Bold);
            btnVolver.ForeColor = Color.White;
            btnVolver.Location = new Point(650, 390);        
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(120, 40);
            btnVolver.TabIndex = 3;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;              

            // FormReportes
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnVolver);
            Controls.Add(comboTipoReporte);
            Controls.Add(lblTitulo);
            Controls.Add(dataGridReportes);
            Name = "FormReportes";
            Text = "Reportes del Sistema";
            ((System.ComponentModel.ISupportInitialize)dataGridReportes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridReportes;
        private Label lblTitulo;       
        private ComboBox comboTipoReporte;
        private Button btnVolver;
    }
}