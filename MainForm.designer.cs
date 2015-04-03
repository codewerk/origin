namespace Fixit //omSupportBackup
{
   partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnCreateBakFile = new System.Windows.Forms.Button();
            this.txtbxExplanation = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.grpBoxSource = new System.Windows.Forms.GroupBox();
            this.cboBackupType = new System.Windows.Forms.ComboBox();
            this.cboDatabaseName = new System.Windows.Forms.ComboBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpboxBackupSet = new System.Windows.Forms.GroupBox();
            this.txtBxBackupSetName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.grpbxBackupDestination = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtbxDestination = new System.Windows.Forms.TextBox();
            this.btnIbal = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpbxCatchIt = new System.Windows.Forms.GroupBox();
            this.cboCaseCatchers = new System.Windows.Forms.ComboBox();
            this.btnFindCases = new System.Windows.Forms.Button();
            this.cboInsuranceChoices = new System.Windows.Forms.ComboBox();
            this.btnFixIt = new System.Windows.Forms.Button();
            this.grpbxFixIt = new System.Windows.Forms.GroupBox();
            this.grpbxBackup = new System.Windows.Forms.GroupBox();
            this.lblDBClientType = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.grpBoxSource.SuspendLayout();
            this.grpboxBackupSet.SuspendLayout();
            this.grpbxBackupDestination.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpbxCatchIt.SuspendLayout();
            this.grpbxFixIt.SuspendLayout();
            this.grpbxBackup.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCreateBakFile
            // 
            this.btnCreateBakFile.Location = new System.Drawing.Point(16, 19);
            this.btnCreateBakFile.Name = "btnCreateBakFile";
            this.btnCreateBakFile.Size = new System.Drawing.Size(57, 23);
            this.btnCreateBakFile.TabIndex = 0;
            this.btnCreateBakFile.Text = "OK";
            this.btnCreateBakFile.UseVisualStyleBackColor = true;
            this.btnCreateBakFile.Click += new System.EventHandler(this.btnCreateBakFile_Click);
            // 
            // txtbxExplanation
            // 
            this.txtbxExplanation.BackColor = System.Drawing.SystemColors.Control;
            this.txtbxExplanation.Location = new System.Drawing.Point(22, 3);
            this.txtbxExplanation.Multiline = true;
            this.txtbxExplanation.Name = "txtbxExplanation";
            this.txtbxExplanation.ReadOnly = true;
            this.txtbxExplanation.Size = new System.Drawing.Size(410, 40);
            this.txtbxExplanation.TabIndex = 1;
            this.txtbxExplanation.TabStop = false;
            this.txtbxExplanation.Text = "Creates a backup of the OfficeMate SQL Database in \r\nthe same directory, using AD" +
                "O information in the omate32.ini file.";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(122, 54);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(182, 22);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 2;
            // 
            // grpBoxSource
            // 
            this.grpBoxSource.Controls.Add(this.cboBackupType);
            this.grpBoxSource.Controls.Add(this.cboDatabaseName);
            this.grpBoxSource.Controls.Add(this.radioButton1);
            this.grpBoxSource.Controls.Add(this.label5);
            this.grpBoxSource.Controls.Add(this.label3);
            this.grpBoxSource.Controls.Add(this.label1);
            this.grpBoxSource.Location = new System.Drawing.Point(22, 61);
            this.grpBoxSource.Name = "grpBoxSource";
            this.grpBoxSource.Size = new System.Drawing.Size(465, 114);
            this.grpBoxSource.TabIndex = 3;
            this.grpBoxSource.TabStop = false;
            this.grpBoxSource.Text = "Source";
            // 
            // cboBackupType
            // 
            this.cboBackupType.Enabled = false;
            this.cboBackupType.FormattingEnabled = true;
            this.cboBackupType.Items.AddRange(new object[] {
            "Full"});
            this.cboBackupType.Location = new System.Drawing.Point(222, 46);
            this.cboBackupType.Name = "cboBackupType";
            this.cboBackupType.Size = new System.Drawing.Size(229, 21);
            this.cboBackupType.TabIndex = 7;
            // 
            // cboDatabaseName
            // 
            this.cboDatabaseName.Enabled = false;
            this.cboDatabaseName.FormattingEnabled = true;
            this.cboDatabaseName.Items.AddRange(new object[] {
            "-"});
            this.cboDatabaseName.Location = new System.Drawing.Point(222, 19);
            this.cboDatabaseName.Name = "cboDatabaseName";
            this.cboDatabaseName.Size = new System.Drawing.Size(229, 21);
            this.cboDatabaseName.TabIndex = 6;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(45, 91);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(71, 17);
            this.radioButton1.TabIndex = 5;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Database";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Backup component:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Backup Type:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Database:";
            // 
            // grpboxBackupSet
            // 
            this.grpboxBackupSet.Controls.Add(this.txtBxBackupSetName);
            this.grpboxBackupSet.Controls.Add(this.label9);
            this.grpboxBackupSet.Location = new System.Drawing.Point(22, 192);
            this.grpboxBackupSet.Name = "grpboxBackupSet";
            this.grpboxBackupSet.Size = new System.Drawing.Size(459, 61);
            this.grpboxBackupSet.TabIndex = 4;
            this.grpboxBackupSet.TabStop = false;
            this.grpboxBackupSet.Text = "Backup set";
            // 
            // txtBxBackupSetName
            // 
            this.txtBxBackupSetName.Location = new System.Drawing.Point(145, 20);
            this.txtBxBackupSetName.Name = "txtBxBackupSetName";
            this.txtBxBackupSetName.ReadOnly = true;
            this.txtBxBackupSetName.Size = new System.Drawing.Size(300, 20);
            this.txtBxBackupSetName.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Name:";
            // 
            // grpbxBackupDestination
            // 
            this.grpbxBackupDestination.Controls.Add(this.radioButton2);
            this.grpbxBackupDestination.Controls.Add(this.label2);
            this.grpbxBackupDestination.Controls.Add(this.txtbxDestination);
            this.grpbxBackupDestination.Location = new System.Drawing.Point(22, 272);
            this.grpbxBackupDestination.Name = "grpbxBackupDestination";
            this.grpbxBackupDestination.Size = new System.Drawing.Size(455, 107);
            this.grpbxBackupDestination.TabIndex = 5;
            this.grpbxBackupDestination.TabStop = false;
            this.grpbxBackupDestination.Text = "Destination";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(148, 23);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(46, 17);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Disk";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Back up to:";
            // 
            // txtbxDestination
            // 
            this.txtbxDestination.Location = new System.Drawing.Point(26, 54);
            this.txtbxDestination.Name = "txtbxDestination";
            this.txtbxDestination.ReadOnly = true;
            this.txtbxDestination.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtbxDestination.Size = new System.Drawing.Size(422, 20);
            this.txtbxDestination.TabIndex = 0;
            // 
            // btnIbal
            // 
            this.btnIbal.Location = new System.Drawing.Point(264, 16);
            this.btnIbal.Name = "btnIbal";
            this.btnIbal.Size = new System.Drawing.Size(168, 24);
            this.btnIbal.TabIndex = 5;
            this.btnIbal.Text = "Run iBal Only w/AR Report";
            this.btnIbal.UseVisualStyleBackColor = true;
            this.btnIbal.Visible = false;
            this.btnIbal.Click += new System.EventHandler(this.btnIbal_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(79, 19);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(57, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Progress";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(16, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(104, 28);
            this.panel1.TabIndex = 8;
            this.panel1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DisplayCatchFixPanelsWithoutBackup);
            // 
            // grpbxCatchIt
            // 
            this.grpbxCatchIt.Controls.Add(this.cboCaseCatchers);
            this.grpbxCatchIt.Controls.Add(this.btnFindCases);
            this.grpbxCatchIt.Location = new System.Drawing.Point(22, 496);
            this.grpbxCatchIt.Name = "grpbxCatchIt";
            this.grpbxCatchIt.Size = new System.Drawing.Size(455, 93);
            this.grpbxCatchIt.TabIndex = 6;
            this.grpbxCatchIt.TabStop = false;
            this.grpbxCatchIt.Text = "Find Cases";
            this.grpbxCatchIt.Visible = false;
            // 
            // cboCaseCatchers
            // 
            this.cboCaseCatchers.FormattingEnabled = true;
            this.cboCaseCatchers.Location = new System.Drawing.Point(6, 30);
            this.cboCaseCatchers.Name = "cboCaseCatchers";
            this.cboCaseCatchers.Size = new System.Drawing.Size(426, 21);
            this.cboCaseCatchers.TabIndex = 0;
            // 
            // btnFindCases
            // 
            this.btnFindCases.Location = new System.Drawing.Point(6, 57);
            this.btnFindCases.Name = "btnFindCases";
            this.btnFindCases.Size = new System.Drawing.Size(136, 30);
            this.btnFindCases.TabIndex = 1;
            this.btnFindCases.Text = "Find It";
            this.btnFindCases.UseVisualStyleBackColor = true;
            this.btnFindCases.Click += new System.EventHandler(this.NewSmallFixIt_FindItHandler);
            // 
            // cboInsuranceChoices
            // 
            this.cboInsuranceChoices.FormattingEnabled = true;
            this.cboInsuranceChoices.Location = new System.Drawing.Point(6, 29);
            this.cboInsuranceChoices.Name = "cboInsuranceChoices";
            this.cboInsuranceChoices.Size = new System.Drawing.Size(430, 21);
            this.cboInsuranceChoices.TabIndex = 2;
            // 
            // btnFixIt
            // 
            this.btnFixIt.Location = new System.Drawing.Point(6, 68);
            this.btnFixIt.Name = "btnFixIt";
            this.btnFixIt.Size = new System.Drawing.Size(136, 31);
            this.btnFixIt.TabIndex = 3;
            this.btnFixIt.Text = "Fix It";
            this.btnFixIt.UseVisualStyleBackColor = true;
            this.btnFixIt.Click += new System.EventHandler(this.NewSmallFixIt_FindItHandler);
            // 
            // grpbxFixIt
            // 
            this.grpbxFixIt.Controls.Add(this.cboInsuranceChoices);
            this.grpbxFixIt.Controls.Add(this.btnFixIt);
            this.grpbxFixIt.Location = new System.Drawing.Point(22, 613);
            this.grpbxFixIt.Name = "grpbxFixIt";
            this.grpbxFixIt.Size = new System.Drawing.Size(455, 105);
            this.grpbxFixIt.TabIndex = 9;
            this.grpbxFixIt.TabStop = false;
            this.grpbxFixIt.Text = "Run Solutions";
            this.grpbxFixIt.Visible = false;
            // 
            // grpbxBackup
            // 
            this.grpbxBackup.Controls.Add(this.lblDBClientType);
            this.grpbxBackup.Controls.Add(this.button1);
            this.grpbxBackup.Controls.Add(this.btnIbal);
            this.grpbxBackup.Controls.Add(this.panel1);
            this.grpbxBackup.Controls.Add(this.btnCreateBakFile);
            this.grpbxBackup.Controls.Add(this.progressBar);
            this.grpbxBackup.Controls.Add(this.btnCancel);
            this.grpbxBackup.Location = new System.Drawing.Point(22, 392);
            this.grpbxBackup.Name = "grpbxBackup";
            this.grpbxBackup.Size = new System.Drawing.Size(456, 88);
            this.grpbxBackup.TabIndex = 10;
            this.grpbxBackup.TabStop = false;
            this.grpbxBackup.Text = "Backup";
            // 
            // lblDBClientType
            // 
            this.lblDBClientType.AutoSize = true;
            this.lblDBClientType.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDBClientType.Location = new System.Drawing.Point(332, 58);
            this.lblDBClientType.Name = "lblDBClientType";
            this.lblDBClientType.Size = new System.Drawing.Size(76, 13);
            this.lblDBClientType.TabIndex = 10;
            this.lblDBClientType.Text = "DB Client Type";
            this.lblDBClientType.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(190, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 28);
            this.button1.TabIndex = 9;
            this.button1.Text = "get feeslip";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 744);
            this.Controls.Add(this.grpbxBackup);
            this.Controls.Add(this.grpbxFixIt);
            this.Controls.Add(this.grpbxCatchIt);
            this.Controls.Add(this.grpbxBackupDestination);
            this.Controls.Add(this.grpboxBackupSet);
            this.Controls.Add(this.grpBoxSource);
            this.Controls.Add(this.txtbxExplanation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Backup OfficeMate SQL Database";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RemoveDesktopFiles);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoveDesktopFiles);
            this.grpBoxSource.ResumeLayout(false);
            this.grpBoxSource.PerformLayout();
            this.grpboxBackupSet.ResumeLayout(false);
            this.grpboxBackupSet.PerformLayout();
            this.grpbxBackupDestination.ResumeLayout(false);
            this.grpbxBackupDestination.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpbxCatchIt.ResumeLayout(false);
            this.grpbxFixIt.ResumeLayout(false);
            this.grpbxBackup.ResumeLayout(false);
            this.grpbxBackup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateBakFile;
        private System.Windows.Forms.TextBox txtbxExplanation;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.GroupBox grpBoxSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox grpboxBackupSet;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboBackupType;
        private System.Windows.Forms.ComboBox cboDatabaseName;
        private System.Windows.Forms.TextBox txtBxBackupSetName;
        private System.Windows.Forms.GroupBox grpbxBackupDestination;
        private System.Windows.Forms.TextBox txtbxDestination;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grpbxCatchIt;
        private System.Windows.Forms.ComboBox cboCaseCatchers;
        private System.Windows.Forms.ComboBox cboInsuranceChoices;
        private System.Windows.Forms.Button btnFindCases;
        private System.Windows.Forms.Button btnFixIt;
        private System.Windows.Forms.GroupBox grpbxFixIt;
        private System.Windows.Forms.GroupBox grpbxBackup;
        private System.Windows.Forms.Button btnIbal;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblDBClientType;
    }
}

