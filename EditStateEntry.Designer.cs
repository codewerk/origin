namespace Fixit
{
    partial class EditStateEntry
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
            this.lblUserEntry = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblUserEntry
            // 
            this.lblUserEntry.AutoSize = true;
            this.lblUserEntry.Location = new System.Drawing.Point(8, 9);
            this.lblUserEntry.Name = "lblUserEntry";
            this.lblUserEntry.Size = new System.Drawing.Size(147, 13);
            this.lblUserEntry.TabIndex = 0;
            this.lblUserEntry.Text = "Enter feeslip#  then click Run";
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(11, 25);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(61, 25);
            this.btnRun.TabIndex = 2;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // EditStateEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 56);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.lblUserEntry);
            this.Name = "EditStateEntry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fee Slip - Edit Mode";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUserEntry;
        private System.Windows.Forms.Button btnRun;



    }
}