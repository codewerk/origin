namespace Fixit
{
    partial class FeeslipsWithUnknownIns
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeeslipsWithUnknownIns));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lbInstructions = new System.Windows.Forms.Label();
            this.btnFixIT = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lblFeeslipsFound = new System.Windows.Forms.Label();
            this.SearchValue = new System.Windows.Forms.TextBox();
            this.btnSearchForFeeSlip = new System.Windows.Forms.Button();
            this.btnCopyGrid = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(16, 109);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(454, 197);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ModifyCheckAllButton);
            // 
            // lbInstructions
            // 
            this.lbInstructions.AutoSize = true;
            this.lbInstructions.Location = new System.Drawing.Point(13, 7);
            this.lbInstructions.Name = "lbInstructions";
            this.lbInstructions.Size = new System.Drawing.Size(457, 52);
            this.lbInstructions.TabIndex = 1;
            this.lbInstructions.Text = resources.GetString("lbInstructions.Text");
            // 
            // btnFixIT
            // 
            this.btnFixIT.Location = new System.Drawing.Point(16, 72);
            this.btnFixIT.Name = "btnFixIT";
            this.btnFixIT.Size = new System.Drawing.Size(70, 21);
            this.btnFixIT.TabIndex = 2;
            this.btnFixIT.Text = "FixIT";
            this.btnFixIT.UseVisualStyleBackColor = true;
            this.btnFixIT.Click += new System.EventHandler(this.btnFixIT_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(92, 72);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(101, 20);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.Text = "Check All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.CheckAllBoxes);
            // 
            // lblFeeslipsFound
            // 
            this.lblFeeslipsFound.AutoSize = true;
            this.lblFeeslipsFound.Location = new System.Drawing.Point(449, 75);
            this.lblFeeslipsFound.Name = "lblFeeslipsFound";
            this.lblFeeslipsFound.Size = new System.Drawing.Size(0, 13);
            this.lblFeeslipsFound.TabIndex = 16;
            // 
            // SearchValue
            // 
            this.SearchValue.Location = new System.Drawing.Point(366, 73);
            this.SearchValue.Name = "SearchValue";
            this.SearchValue.Size = new System.Drawing.Size(79, 20);
            this.SearchValue.TabIndex = 15;
            this.SearchValue.Text = "enter feeslip#";
            this.SearchValue.TextChanged += new System.EventHandler(this.SearchValue_TextChanged);
            this.SearchValue.Enter += new System.EventHandler(this.SearchValue_Enter);
            // 
            // btnSearchForFeeSlip
            // 
            this.btnSearchForFeeSlip.Location = new System.Drawing.Point(274, 72);
            this.btnSearchForFeeSlip.Name = "btnSearchForFeeSlip";
            this.btnSearchForFeeSlip.Size = new System.Drawing.Size(86, 21);
            this.btnSearchForFeeSlip.TabIndex = 14;
            this.btnSearchForFeeSlip.Text = "Feeslip Search";
            this.btnSearchForFeeSlip.UseVisualStyleBackColor = true;
            this.btnSearchForFeeSlip.Click += new System.EventHandler(this.btnSearchForFeeSlip_Click);
            // 
            // btnCopyGrid
            // 
            this.btnCopyGrid.Location = new System.Drawing.Point(198, 72);
            this.btnCopyGrid.Name = "btnCopyGrid";
            this.btnCopyGrid.Size = new System.Drawing.Size(70, 21);
            this.btnCopyGrid.TabIndex = 13;
            this.btnCopyGrid.Text = "Copy";
            this.btnCopyGrid.UseVisualStyleBackColor = true;
            this.btnCopyGrid.Click += new System.EventHandler(this.btnCopyGrid_Click);
            // 
            // FeeslipsWithUnknownIns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 333);
            this.Controls.Add(this.lblFeeslipsFound);
            this.Controls.Add(this.SearchValue);
            this.Controls.Add(this.btnSearchForFeeSlip);
            this.Controls.Add(this.btnCopyGrid);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnFixIT);
            this.Controls.Add(this.lbInstructions);
            this.Controls.Add(this.dataGridView1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FeeslipsWithUnknownIns";
            this.Text = "Fee Slips With Unknown Insurance";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lbInstructions;
        private System.Windows.Forms.Button btnFixIT;
        private System.Windows.Forms.Button btnSelectAll;
        public System.Windows.Forms.Label lblFeeslipsFound;
        private System.Windows.Forms.TextBox SearchValue;
        private System.Windows.Forms.Button btnSearchForFeeSlip;
        private System.Windows.Forms.Button btnCopyGrid;

    }
}