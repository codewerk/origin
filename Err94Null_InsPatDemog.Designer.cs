namespace Fixit
{
    partial class Err94Null_InsPatDemog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtbxPatientID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLookupInsuranceWithPatID = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.lblInsuranceList = new System.Windows.Forms.Label();
            this.lblInsurancePlanList = new System.Windows.Forms.Label();
            this.btnAssignPlanToCarrier = new System.Windows.Forms.Button();
            this.lbInstruction = new System.Windows.Forms.Label();
            this.lblCarrierFound = new System.Windows.Forms.Label();
            this.lblInsuranceCarrierFound_No = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle20;
            this.dataGridView1.Location = new System.Drawing.Point(314, 179);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle21;
            this.dataGridView1.Size = new System.Drawing.Size(164, 31);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.TabStop = false;
            this.dataGridView1.Visible = false;
            // 
            // txtbxPatientID
            // 
            this.txtbxPatientID.Location = new System.Drawing.Point(15, 114);
            this.txtbxPatientID.Name = "txtbxPatientID";
            this.txtbxPatientID.Size = new System.Drawing.Size(130, 20);
            this.txtbxPatientID.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter Patient ID, then TAB key";
            // 
            // btnLookupInsuranceWithPatID
            // 
            this.btnLookupInsuranceWithPatID.Location = new System.Drawing.Point(151, 302);
            this.btnLookupInsuranceWithPatID.Name = "btnLookupInsuranceWithPatID";
            this.btnLookupInsuranceWithPatID.Size = new System.Drawing.Size(70, 22);
            this.btnLookupInsuranceWithPatID.TabIndex = 1;
            this.btnLookupInsuranceWithPatID.Text = "Search";
            this.btnLookupInsuranceWithPatID.UseVisualStyleBackColor = true;
            this.btnLookupInsuranceWithPatID.Click += new System.EventHandler(this.btnLookupInsuranceWithPatID_Click);
            this.btnLookupInsuranceWithPatID.Enter += new System.EventHandler(this.btnLookupInsuranceWithPatID_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle23.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle23.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle23.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle23.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle23;
            this.dataGridView2.Location = new System.Drawing.Point(16, 151);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle24.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle24.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle24.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle24;
            this.dataGridView2.RowHeadersWidth = 4;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(130, 31);
            this.dataGridView2.TabIndex = 4;
            this.dataGridView2.TabStop = false;
            // 
            // lblInsuranceList
            // 
            this.lblInsuranceList.AutoSize = true;
            this.lblInsuranceList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInsuranceList.Location = new System.Drawing.Point(311, 148);
            this.lblInsuranceList.Name = "lblInsuranceList";
            this.lblInsuranceList.Size = new System.Drawing.Size(92, 13);
            this.lblInsuranceList.TabIndex = 5;
            this.lblInsuranceList.Text = "Insurance Carriers";
            this.lblInsuranceList.Visible = false;
            // 
            // lblInsurancePlanList
            // 
            this.lblInsurancePlanList.AutoSize = true;
            this.lblInsurancePlanList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInsurancePlanList.Location = new System.Drawing.Point(311, 163);
            this.lblInsurancePlanList.Name = "lblInsurancePlanList";
            this.lblInsurancePlanList.Size = new System.Drawing.Size(83, 13);
            this.lblInsurancePlanList.TabIndex = 6;
            this.lblInsurancePlanList.Text = "Insurance Plans";
            this.lblInsurancePlanList.Visible = false;
            // 
            // btnAssignPlanToCarrier
            // 
            this.btnAssignPlanToCarrier.Location = new System.Drawing.Point(151, 151);
            this.btnAssignPlanToCarrier.Name = "btnAssignPlanToCarrier";
            this.btnAssignPlanToCarrier.Size = new System.Drawing.Size(119, 42);
            this.btnAssignPlanToCarrier.TabIndex = 7;
            this.btnAssignPlanToCarrier.Text = "Assign plan to carrier";
            this.btnAssignPlanToCarrier.UseVisualStyleBackColor = true;
            this.btnAssignPlanToCarrier.Click += new System.EventHandler(this.btnAssignPlanToCarrier_Click);
            // 
            // lbInstruction
            // 
            this.lbInstruction.AutoSize = true;
            this.lbInstruction.Location = new System.Drawing.Point(13, 7);
            this.lbInstruction.Name = "lbInstruction";
            this.lbInstruction.Size = new System.Drawing.Size(160, 65);
            this.lbInstruction.TabIndex = 8;
            this.lbInstruction.Text = "1) Enter the patient ID\r\n2) Click the Search button\r\n3) Select a plan\r\n4) Click t" +
                "he Assign plan buton\r\n5) Click the red X to exit the form";
            // 
            // lblCarrierFound
            // 
            this.lblCarrierFound.AutoSize = true;
            this.lblCarrierFound.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCarrierFound.Location = new System.Drawing.Point(318, 133);
            this.lblCarrierFound.Name = "lblCarrierFound";
            this.lblCarrierFound.Size = new System.Drawing.Size(85, 15);
            this.lblCarrierFound.TabIndex = 9;
            this.lblCarrierFound.Text = "carrierfound";
            this.lblCarrierFound.Visible = false;
            // 
            // lblInsuranceCarrierFound_No
            // 
            this.lblInsuranceCarrierFound_No.AutoSize = true;
            this.lblInsuranceCarrierFound_No.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInsuranceCarrierFound_No.Location = new System.Drawing.Point(311, 230);
            this.lblInsuranceCarrierFound_No.Name = "lblInsuranceCarrierFound_No";
            this.lblInsuranceCarrierFound_No.Size = new System.Drawing.Size(134, 13);
            this.lblInsuranceCarrierFound_No.TabIndex = 10;
            this.lblInsuranceCarrierFound_No.Text = "InsuranceCarrierFound_No";
            this.lblInsuranceCarrierFound_No.Visible = false;
            // 
            // Err94Null_InsPatDemog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 287);
            this.Controls.Add(this.lblInsuranceCarrierFound_No);
            this.Controls.Add(this.lblCarrierFound);
            this.Controls.Add(this.lbInstruction);
            this.Controls.Add(this.btnAssignPlanToCarrier);
            this.Controls.Add(this.lblInsurancePlanList);
            this.Controls.Add(this.lblInsuranceList);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.btnLookupInsuranceWithPatID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbxPatientID);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Err94Null_InsPatDemog";
            this.Text = "Carrier missing plan for patient";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtbxPatientID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLookupInsuranceWithPatID;
        public System.Windows.Forms.DataGridView dataGridView1;
        public System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label lblInsuranceList;
        private System.Windows.Forms.Label lblInsurancePlanList;
        private System.Windows.Forms.Button btnAssignPlanToCarrier;
        private System.Windows.Forms.Label lbInstruction;
        private System.Windows.Forms.Label lblCarrierFound;
        private System.Windows.Forms.Label lblInsuranceCarrierFound_No;
    }
}