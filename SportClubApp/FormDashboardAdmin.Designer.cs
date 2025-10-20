namespace SportClubApp
{
    partial class FormDashboardAdmin
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
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnGestion = new Button();
            flowLayoutPanel2 = new FlowLayoutPanel();
            btnCarnet = new Button();
            flowLayoutPanel3 = new FlowLayoutPanel();
            btnReportes = new Button();
            lblGestion = new Label();
            lblCarnet = new Label();
            lblReportes = new Label();
            label1 = new Label();
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Bottom;
            flowLayoutPanel1.BackColor = Color.White;
            flowLayoutPanel1.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel1.Controls.Add(btnGestion);
            flowLayoutPanel1.FlowDirection = FlowDirection.BottomUp;
            flowLayoutPanel1.Location = new Point(55, 164);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(212, 200);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // btnGestion
            // 
            btnGestion.AutoSize = true;
            btnGestion.BackColor = Color.FromArgb(0, 192, 0);
            btnGestion.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGestion.ForeColor = Color.White;
            btnGestion.Location = new Point(25, 155);
            btnGestion.Margin = new Padding(25, 10, 10, 10);
            btnGestion.Name = "btnGestion";
            btnGestion.Size = new Size(163, 33);
            btnGestion.TabIndex = 0;
            btnGestion.Text = "Gestión de Registros";
            btnGestion.UseVisualStyleBackColor = false;
            btnGestion.Click += BtnGestion_Click;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Anchor = AnchorStyles.Bottom;
            flowLayoutPanel2.BackColor = Color.White;
            flowLayoutPanel2.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel2.Controls.Add(btnCarnet);
            flowLayoutPanel2.FlowDirection = FlowDirection.BottomUp;
            flowLayoutPanel2.Location = new Point(299, 165);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(212, 199);
            flowLayoutPanel2.TabIndex = 1;
            // 
            // btnCarnet
            // 
            btnCarnet.BackColor = Color.FromArgb(0, 192, 0);
            btnCarnet.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCarnet.ForeColor = Color.White;
            btnCarnet.Location = new Point(25, 154);
            btnCarnet.Margin = new Padding(25, 10, 10, 10);
            btnCarnet.Name = "btnCarnet";
            btnCarnet.Size = new Size(162, 33);
            btnCarnet.TabIndex = 0;
            btnCarnet.Text = "Carnet/Cuotas";
            btnCarnet.UseVisualStyleBackColor = false;
            btnCarnet.Click += BtnCarnet_Click;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.BackColor = Color.White;
            flowLayoutPanel3.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel3.Controls.Add(btnReportes);
            flowLayoutPanel3.FlowDirection = FlowDirection.BottomUp;
            flowLayoutPanel3.Location = new Point(543, 164);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(212, 200);
            flowLayoutPanel3.TabIndex = 2;
            // 
            // btnReportes
            // 
            btnReportes.BackColor = Color.FromArgb(0, 192, 0);
            btnReportes.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReportes.ForeColor = Color.White;
            btnReportes.Location = new Point(25, 159);
            btnReportes.Margin = new Padding(25, 10, 10, 10);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(158, 29);
            btnReportes.TabIndex = 0;
            btnReportes.Text = "Reportes";
            btnReportes.UseVisualStyleBackColor = false;
            btnReportes.Click += BtnReportes_Click;
            // 
            // lblGestion
            // 
            lblGestion.AutoSize = true;
            lblGestion.Font = new Font("Candara", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGestion.Location = new Point(95, 134);
            lblGestion.Name = "lblGestion";
            lblGestion.Size = new Size(134, 19);
            lblGestion.TabIndex = 0;
            lblGestion.Text = "Gestión Registros ";
            lblGestion.Click += lblGestion_Click;
            // 
            // lblCarnet
            // 
            lblCarnet.AutoSize = true;
            lblCarnet.Font = new Font("Candara", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCarnet.Location = new Point(299, 134);
            lblCarnet.Name = "lblCarnet";
            lblCarnet.Size = new Size(222, 19);
            lblCarnet.TabIndex = 3;
            lblCarnet.Text = "Cobro Cuotas y  Entrega Carnet";
            lblCarnet.Click += lblCarnet_Click;
            // 
            // lblReportes
            // 
            lblReportes.AutoSize = true;
            lblReportes.Font = new Font("Candara", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblReportes.Location = new Point(575, 134);
            lblReportes.Name = "lblReportes";
            lblReportes.Size = new Size(144, 19);
            lblReportes.TabIndex = 4;
            lblReportes.Text = "Reportes y Listados";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Candara", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(325, 57);
            label1.Name = "label1";
            label1.Size = new Size(164, 26);
            label1.TabIndex = 5;
            label1.Text = "Panel de Control";
            // 
            // FormDashboardAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(lblReportes);
            Controls.Add(lblCarnet);
            Controls.Add(lblGestion);
            Controls.Add(flowLayoutPanel3);
            Controls.Add(flowLayoutPanel2);
            Controls.Add(flowLayoutPanel1);
            Name = "FormDashboardAdmin";
            Text = "Panel de Control - Administrador";
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel3.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel2;
        private FlowLayoutPanel flowLayoutPanel3;
        private Label lblGestion;
        private Label lblCarnet;
        private Label lblReportes;
        private Button btnGestion;
        private Button btnCarnet;
        private Button btnReportes;
        private Label label1;
    }
}