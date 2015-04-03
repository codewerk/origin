namespace Fixit
{
    partial class PunctuationCharacter
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnFixIT = new System.Windows.Forms.Button();
            this.lbInstructions = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnCopyGrid = new System.Windows.Forms.Button();
            this.lblFeeslipsFound = new System.Windows.Forms.Label();
            this.SearchValue = new System.Windows.Forms.TextBox();
            this.btnSearchForFeeSlip = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(367, 289);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(70, 21);
            this.btnSelectAll.TabIndex = 7;
            this.btnSelectAll.Text = "Check All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Visible = false;
            // 
            // btnFixIT
            // 
            this.btnFixIT.Location = new System.Drawing.Point(12, 292);
            this.btnFixIT.Name = "btnFixIT";
            this.btnFixIT.Size = new System.Drawing.Size(70, 21);
            this.btnFixIT.TabIndex = 6;
            this.btnFixIT.Text = "FixIT";
            this.btnFixIT.UseVisualStyleBackColor = true;
            this.btnFixIT.Visible = false;
            // 
            // lbInstructions
            // 
            this.lbInstructions.AutoSize = true;
            this.lbInstructions.Location = new System.Drawing.Point(9, 11);
            this.lbInstructions.Name = "lbInstructions";
            this.lbInstructions.Size = new System.Drawing.Size(316, 26);
            this.lbInstructions.TabIndex = 5;
            this.lbInstructions.Text = "1) Click the Copy button to copy the list into another document.\r\n2) Go to the Pa" +
                "tient demographic to remove the illegal characters.";
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridView1.Location = new System.Drawing.Point(12, 84);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridView1.Size = new System.Drawing.Size(456, 145);
            this.dataGridView1.TabIndex = 4;
            // 
            // btnCopyGrid
            // 
            this.btnCopyGrid.Location = new System.Drawing.Point(12, 57);
            this.btnCopyGrid.Name = "btnCopyGrid";
            this.btnCopyGrid.Size = new System.Drawing.Size(70, 21);
            this.btnCopyGrid.TabIndex = 8;
            this.btnCopyGrid.Text = "Copy";
            this.btnCopyGrid.UseVisualStyleBackColor = true;
            this.btnCopyGrid.Click += new System.EventHandler(this.btnCopyGrid_Click);
            // 
            // lblFeeslipsFound
            // 
            this.lblFeeslipsFound.AutoSize = true;
            this.lblFeeslipsFound.Location = new System.Drawing.Point(413, 61);
            this.lblFeeslipsFound.Name = "lblFeeslipsFound";
            this.lblFeeslipsFound.Size = new System.Drawing.Size(0, 13);
            this.lblFeeslipsFound.TabIndex = 19;
            // 
            // SearchValue
            // 
            this.SearchValue.Location = new System.Drawing.Point(279, 58);
            this.SearchValue.Name = "SearchValue";
            this.SearchValue.Size = new System.Drawing.Size(128, 20);
            this.SearchValue.TabIndex = 18;
            this.SearchValue.Text = "enter patient# then TAB";
            this.SearchValue.TextChanged += new System.EventHandler(this.SearchValue_TextChanged);
            this.SearchValue.Enter += new System.EventHandler(this.SearchValue_Enter);
            // 
            // btnSearchForFeeSlip
            // 
            this.btnSearchForFeeSlip.Location = new System.Drawing.Point(494, 60);
            this.btnSearchForFeeSlip.Name = "btnSearchForFeeSlip";
            this.btnSearchForFeeSlip.Size = new System.Drawing.Size(35, 21);
            this.btnSearchForFeeSlip.TabIndex = 17;
            this.btnSearchForFeeSlip.Text = "Feeslip Search";
            this.btnSearchForFeeSlip.UseVisualStyleBackColor = true;
            this.btnSearchForFeeSlip.Visible = false;
            // 
            // PunctuationCharacter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 256);
            this.Controls.Add(this.lblFeeslipsFound);
            this.Controls.Add(this.SearchValue);
            this.Controls.Add(this.btnSearchForFeeSlip);
            this.Controls.Add(this.btnCopyGrid);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnFixIT);
            this.Controls.Add(this.lbInstructions);
            this.Controls.Add(this.dataGridView1);
            this.Name = "PunctuationCharacter";
            this.Text = "Illegal Characters";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnFixIT;
        private System.Windows.Forms.Label lbInstructions;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnCopyGrid;
        public System.Windows.Forms.Label lblFeeslipsFound;
        private System.Windows.Forms.TextBox SearchValue;
        private System.Windows.Forms.Button btnSearchForFeeSlip;
    }
}