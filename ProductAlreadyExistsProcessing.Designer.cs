namespace Fixit
{
    partial class ProductAlreadyExistsProcessing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductAlreadyExistsProcessing));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lbInstructions = new System.Windows.Forms.Label();
            this.btnFixIT = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(16, 80);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(346, 197);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // lbInstructions
            // 
            this.lbInstructions.AutoSize = true;
            this.lbInstructions.Location = new System.Drawing.Point(13, 7);
            this.lbInstructions.Name = "lbInstructions";
            this.lbInstructions.Size = new System.Drawing.Size(359, 52);
            this.lbInstructions.TabIndex = 1;
            this.lbInstructions.Text = resources.GetString("lbInstructions.Text");
            this.lbInstructions.Click += new System.EventHandler(this.lbInstructions_Click);
            // 
            // btnFixIT
            // 
            this.btnFixIT.Location = new System.Drawing.Point(16, 288);
            this.btnFixIT.Name = "btnFixIT";
            this.btnFixIT.Size = new System.Drawing.Size(70, 21);
            this.btnFixIT.TabIndex = 2;
            this.btnFixIT.Text = "FixIT";
            this.btnFixIT.UseVisualStyleBackColor = true;
            this.btnFixIT.Click += new System.EventHandler(this.btnFixIT_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(102, 289);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(101, 20);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Visible = false;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // ProductAlreadyExistsProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 333);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnFixIT);
            this.Controls.Add(this.lbInstructions);
            this.Controls.Add(this.dataGridView1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductAlreadyExistsProcessing";
            this.Text = "Products To Be Removed";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lbInstructions;
        private System.Windows.Forms.Button btnFixIT;
        private System.Windows.Forms.Button btnSelectAll;

    }
}