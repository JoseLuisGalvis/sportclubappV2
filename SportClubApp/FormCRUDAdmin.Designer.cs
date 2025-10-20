namespace SportClubApp
{
    partial class FormCRUDAdmin
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
            label2 = new Label();
            txtBuscar = new TextBox();
            dgvAdministradores = new DataGridView();
            label3 = new Label();
            btnNuevoAdmin = new Button();
            btnEditarAdmin = new Button();
            btnEliminarAdmin = new Button();
            btnCerrar = new Button();
            colId = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colUsername = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvAdministradores).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Candara", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.Location = new Point(173, 28);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(311, 26);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "GESTIÓN DE ADMINISTRADORES";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Candara", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(173, 76);
            label2.Name = "label2";
            label2.Size = new Size(47, 15);
            label2.TabIndex = 1;
            label2.Text = "Buscar:";
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(242, 73);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(231, 23);
            txtBuscar.TabIndex = 2;
            txtBuscar.TextChanged += this.txtBuscar_TextChanged;
            // 
            // dgvAdministradores
            // 
            dgvAdministradores.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAdministradores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAdministradores.Columns.AddRange(new DataGridViewColumn[] { colId, colEstado, colFecha, colUsername });
            dgvAdministradores.Location = new Point(59, 185);
            dgvAdministradores.MultiSelect = false;
            dgvAdministradores.Name = "dgvAdministradores";
            dgvAdministradores.ReadOnly = true;
            dgvAdministradores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAdministradores.Size = new Size(600, 299);
            dgvAdministradores.TabIndex = 3;
            dgvAdministradores.CellDoubleClick += this.dgvAdministradores_CellDoubleClick;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Candara", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(226, 156);
            label3.Name = "label3";
            label3.Size = new Size(231, 26);
            label3.TabIndex = 4;
            label3.Text = "Total: 0 administradores";
            // 
            // btnNuevoAdmin
            // 
            btnNuevoAdmin.Location = new Point(266, 114);
            btnNuevoAdmin.Name = "btnNuevoAdmin";
            btnNuevoAdmin.Size = new Size(141, 23);
            btnNuevoAdmin.TabIndex = 5;
            btnNuevoAdmin.Text = "Nuevo Administrador";
            btnNuevoAdmin.UseVisualStyleBackColor = true;
            btnNuevoAdmin.Click += this.btnNuevoAdmin_Click;
            // 
            // btnEditarAdmin
            // 
            btnEditarAdmin.Location = new Point(173, 444);
            btnEditarAdmin.Name = "btnEditarAdmin";
            btnEditarAdmin.Size = new Size(75, 23);
            btnEditarAdmin.TabIndex = 6;
            btnEditarAdmin.Text = "Editar";
            btnEditarAdmin.UseVisualStyleBackColor = true;
            btnEditarAdmin.Click += this.btnEditarAdmin_Click;
            // 
            // btnEliminarAdmin
            // 
            btnEliminarAdmin.Location = new Point(288, 444);
            btnEliminarAdmin.Name = "btnEliminarAdmin";
            btnEliminarAdmin.Size = new Size(75, 23);
            btnEliminarAdmin.TabIndex = 7;
            btnEliminarAdmin.Text = "Eliminar";
            btnEliminarAdmin.UseVisualStyleBackColor = true;
            btnEliminarAdmin.Click += this.btnEliminarAdmin_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(398, 444);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(75, 23);
            btnCerrar.TabIndex = 8;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += this.btnCerrar_Click;
            // 
            // colId
            // 
            colId.HeaderText = "ID";
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Width = 50;
            // 
            // colEstado
            // 
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            colEstado.Width = 80;
            // 
            // colFecha
            // 
            colFecha.HeaderText = "Fecha Creación";
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            colFecha.Width = 120;
            // 
            // colUsername
            // 
            colUsername.HeaderText = "Usuario";
            colUsername.Name = "colUsername";
            colUsername.ReadOnly = true;
            colUsername.Width = 150;
            // 
            // FormCRUDAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 496);
            Controls.Add(btnCerrar);
            Controls.Add(btnEliminarAdmin);
            Controls.Add(btnEditarAdmin);
            Controls.Add(btnNuevoAdmin);
            Controls.Add(label3);
            Controls.Add(dgvAdministradores);
            Controls.Add(txtBuscar);
            Controls.Add(label2);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "FormCRUDAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestión de Administradores";
            ((System.ComponentModel.ISupportInitialize)dgvAdministradores).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label label2;
        private TextBox txtBuscar;
        private DataGridView dgvAdministradores;
        private Label label3;
        private Button btnNuevoAdmin;
        private Button btnEditarAdmin;
        private Button btnEliminarAdmin;
        private Button btnCerrar;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colUsername;
    
    }
}