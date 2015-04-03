namespace Fixit
{
    partial class SendStatementCheckBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendStatementCheckBox));
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnFixIT = new System.Windows.Forms.Button();
            this.lbInstructions = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnCopyGrid = new System.Windows.Forms.Button();
            this.SearchValue = new System.Windows.Forms.TextBox();
            this.btnSearchForPatient = new System.Windows.Forms.Button();
            this.lblFeeslipsFound = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(88, 97);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(101, 20);
            this.btnSelectAll.TabIndex = 7;
            this.btnSelectAll.Text = "Check All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.CheckAllBoxes);
            // 
            // btnFixIT
            // 
            this.btnFixIT.Location = new System.Drawing.Point(12, 96);
            this.btnFixIT.Name = "btnFixIT";
            this.btnFixIT.Size = new System.Drawing.Size(70, 21);
            this.btnFixIT.TabIndex = 6;
            this.btnFixIT.Text = "FixIT";
            this.btnFixIT.UseVisualStyleBackColor = true;
            this.btnFixIT.Click += new System.EventHandler(this.btnFixIT_Click);
            // 
            // lbInstructions
            // 
            this.lbInstructions.AutoSize = true;
            this.lbInstructions.Location = new System.Drawing.Point(9, 9);
            this.lbInstructions.Name = "lbInstructions";
            this.lbInstructions.Size = new System.Drawing.Size(361, 65);
            this.lbInstructions.TabIndex = 5;
            this.lbInstructions.Text = resources.GetString("lbInstructions.Text");
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 123);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(501, 182);
            this.dataGridView1.TabIndex = 4;
            // 
            // btnCopyGrid
            // 
            this.btnCopyGrid.Location = new System.Drawing.Point(195, 96);
            this.btnCopyGrid.Name = "btnCopyGrid";
            this.btnCopyGrid.Size = new System.Drawing.Size(70, 21);
            this.btnCopyGrid.TabIndex = 10;
            this.btnCopyGrid.Text = "Copy";
            this.btnCopyGrid.UseVisualStyleBackColor = true;
            this.btnCopyGrid.Click += new System.EventHandler(this.btnCopyGrid_Click);
            // 
            // SearchValue
            // 
            this.SearchValue.Location = new System.Drawing.Point(363, 96);
            this.SearchValue.Name = "SearchValue";
            this.SearchValue.Size = new System.Drawing.Size(79, 20);
            this.SearchValue.TabIndex = 16;
            this.SearchValue.Text = "enter patient#";
            this.SearchValue.TextChanged += new System.EventHandler(this.SearchValue_TextChanged);
            this.SearchValue.Enter += new System.EventHandler(this.SearchValue_Enter);
            // 
            // btnSearchForPatient
            // 
            this.btnSearchForPatient.Location = new System.Drawing.Point(271, 95);
            this.btnSearchForPatient.Name = "btnSearchForPatient";
            this.btnSearchForPatient.Size = new System.Drawing.Size(86, 21);
            this.btnSearchForPatient.TabIndex = 17;
            this.btnSearchForPatient.Text = "Patient Search";
            this.btnSearchForPatient.UseVisualStyleBackColor = true;
            this.btnSearchForPatient.Click += new System.EventHandler(this.btnSearchForPatient_Click);
            // 
            // lblFeeslipsFound
            // 
            this.lblFeeslipsFound.AutoSize = true;
            this.lblFeeslipsFound.Location = new System.Drawing.Point(448, 99);
            this.lblFeeslipsFound.Name = "lblFeeslipsFound";
            this.lblFeeslipsFound.Size = new System.Drawing.Size(0, 13);
            this.lblFeeslipsFound.TabIndex = 18;
            // 
            // SendStatementCheckBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(518, 332);
            this.Controls.Add(this.lblFeeslipsFound);
            this.Controls.Add(this.btnSearchForPatient);
            this.Controls.Add(this.SearchValue);
            this.Controls.Add(this.btnCopyGrid);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnFixIT);
            this.Controls.Add(this.lbInstructions);
            this.Controls.Add(this.dataGridView1);
            this.Name = "SendStatementCheckBox";
            this.Text = "Send Statement CheckBox";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectAll;
        public System.Windows.Forms.DataGridView dataGridView1;
        public System.Windows.Forms.Label lbInstructions;
        private System.Windows.Forms.TextBox SearchValue;
        private System.Windows.Forms.Button btnSearchForPatient;
        public System.Windows.Forms.Label lblFeeslipsFound;
        public System.Windows.Forms.Button btnFixIT;
        public System.Windows.Forms.Button btnCopyGrid;
    }
}