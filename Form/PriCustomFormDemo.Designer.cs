namespace ExtensibilityNduku.Form
{
    partial class PriCustomFormDemo
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
            this.grbData = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboFolha = new System.Windows.Forms.ComboBox();
            this.btnGravar = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.lblFicheiro = new System.Windows.Forms.Label();
            this.dtoDataUpload = new System.Windows.Forms.DateTimePicker();
            this.grbListagm = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.grbData.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grbListagm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // grbData
            // 
            this.grbData.AutoSize = true;
            this.grbData.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.grbData.Controls.Add(this.groupBox1);
            this.grbData.Controls.Add(this.lblFicheiro);
            this.grbData.Controls.Add(this.dtoDataUpload);
            this.grbData.Dock = System.Windows.Forms.DockStyle.Top;
            this.grbData.Location = new System.Drawing.Point(0, 0);
            this.grbData.Name = "grbData";
            this.grbData.Size = new System.Drawing.Size(800, 75);
            this.grbData.TabIndex = 0;
            this.grbData.TabStop = false;
            this.grbData.Text = "Data";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox1.Controls.Add(this.cboFolha);
            this.groupBox1.Controls.Add(this.btnGravar);
            this.groupBox1.Controls.Add(this.btnBuscar);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(309, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(488, 56);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // cboFolha
            // 
            this.cboFolha.Dock = System.Windows.Forms.DockStyle.Right;
            this.cboFolha.FormattingEnabled = true;
            this.cboFolha.Location = new System.Drawing.Point(330, 16);
            this.cboFolha.Name = "cboFolha";
            this.cboFolha.Size = new System.Drawing.Size(155, 21);
            this.cboFolha.TabIndex = 2;
            this.cboFolha.SelectedIndexChanged += new System.EventHandler(this.cboFolha_SelectedIndexChanged_1);
            // 
            // btnGravar
            // 
            this.btnGravar.Location = new System.Drawing.Point(155, 16);
            this.btnGravar.Name = "btnGravar";
            this.btnGravar.Size = new System.Drawing.Size(132, 33);
            this.btnGravar.TabIndex = 3;
            this.btnGravar.Text = "Gravar";
            this.btnGravar.UseVisualStyleBackColor = true;
            this.btnGravar.Click += new System.EventHandler(this.btnGravar_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBuscar.Location = new System.Drawing.Point(3, 16);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(128, 37);
            this.btnBuscar.TabIndex = 0;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // lblFicheiro
            // 
            this.lblFicheiro.AutoSize = true;
            this.lblFicheiro.Location = new System.Drawing.Point(267, 32);
            this.lblFicheiro.Name = "lblFicheiro";
            this.lblFicheiro.Size = new System.Drawing.Size(0, 13);
            this.lblFicheiro.TabIndex = 2;
            // 
            // dtoDataUpload
            // 
            this.dtoDataUpload.Location = new System.Drawing.Point(6, 36);
            this.dtoDataUpload.Name = "dtoDataUpload";
            this.dtoDataUpload.Size = new System.Drawing.Size(205, 20);
            this.dtoDataUpload.TabIndex = 1;
            // 
            // grbListagm
            // 
            this.grbListagm.AutoSize = true;
            this.grbListagm.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.grbListagm.Controls.Add(this.dataGridView1);
            this.grbListagm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbListagm.Location = new System.Drawing.Point(0, 75);
            this.grbListagm.Name = "grbListagm";
            this.grbListagm.Size = new System.Drawing.Size(800, 375);
            this.grbListagm.TabIndex = 1;
            this.grbListagm.TabStop = false;
            this.grbListagm.Text = "Listagem";
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 16);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(794, 356);
            this.dataGridView1.TabIndex = 2;
            // 
            // PriCustomFormDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.Controls.Add(this.grbListagm);
            this.Controls.Add(this.grbData);
            this.Name = "PriCustomFormDemo";
            this.Size = new System.Drawing.Size(800, 450);
            this.Text = "TDU_MovitelDesempenho";
            this.grbData.ResumeLayout(false);
            this.grbData.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.grbListagm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grbData;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.GroupBox grbListagm;
        private System.Windows.Forms.DateTimePicker dtoDataUpload;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblFicheiro;
        private System.Windows.Forms.Button btnGravar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboFolha;
    }
}