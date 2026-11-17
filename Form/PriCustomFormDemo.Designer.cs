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
            this.btnGravar = new System.Windows.Forms.Button();
            this.lblFicheiro = new System.Windows.Forms.Label();
            this.dtoDataUpload = new System.Windows.Forms.DateTimePicker();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.grbListagm = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cboFolha = new System.Windows.Forms.ComboBox();
            this.grbData.SuspendLayout();
            this.grbListagm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // grbData
            // 
            this.grbData.AutoSize = true;
            this.grbData.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.grbData.Controls.Add(this.btnGravar);
            this.grbData.Controls.Add(this.lblFicheiro);
            this.grbData.Controls.Add(this.dtoDataUpload);
            this.grbData.Controls.Add(this.btnBuscar);
            this.grbData.Dock = System.Windows.Forms.DockStyle.Top;
            this.grbData.Location = new System.Drawing.Point(0, 0);
            this.grbData.Name = "grbData";
            this.grbData.Size = new System.Drawing.Size(800, 84);
            this.grbData.TabIndex = 0;
            this.grbData.TabStop = false;
            this.grbData.Text = "Data";
            // 
            // btnGravar
            // 
            this.btnGravar.Location = new System.Drawing.Point(662, 32);
            this.btnGravar.Name = "btnGravar";
            this.btnGravar.Size = new System.Drawing.Size(132, 33);
            this.btnGravar.TabIndex = 3;
            this.btnGravar.Text = "Gravar";
            this.btnGravar.UseVisualStyleBackColor = true;
            this.btnGravar.Click += new System.EventHandler(this.btnGravar_Click);
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
            this.dtoDataUpload.Location = new System.Drawing.Point(6, 26);
            this.dtoDataUpload.Name = "dtoDataUpload";
            this.dtoDataUpload.Size = new System.Drawing.Size(205, 20);
            this.dtoDataUpload.TabIndex = 1;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(468, 32);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(147, 33);
            this.btnBuscar.TabIndex = 0;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // grbListagm
            // 
            this.grbListagm.AutoSize = true;
            this.grbListagm.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.grbListagm.Controls.Add(this.dataGridView1);
            this.grbListagm.Controls.Add(this.cboFolha);
            this.grbListagm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbListagm.Location = new System.Drawing.Point(0, 84);
            this.grbListagm.Name = "grbListagm";
            this.grbListagm.Size = new System.Drawing.Size(800, 366);
            this.grbListagm.TabIndex = 1;
            this.grbListagm.TabStop = false;
            this.grbListagm.Text = "Listagem";
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 46);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(788, 297);
            this.dataGridView1.TabIndex = 2;
            // 
            // cboFolha
            // 
            this.cboFolha.FormattingEnabled = true;
            this.cboFolha.Location = new System.Drawing.Point(647, 19);
            this.cboFolha.Name = "cboFolha";
            this.cboFolha.Size = new System.Drawing.Size(147, 21);
            this.cboFolha.TabIndex = 1;
            this.cboFolha.SelectedIndexChanged += new System.EventHandler(this.cboFolha_SelectedIndexChanged);
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
        private System.Windows.Forms.ComboBox cboFolha;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblFicheiro;
        private System.Windows.Forms.Button btnGravar;
    }
}