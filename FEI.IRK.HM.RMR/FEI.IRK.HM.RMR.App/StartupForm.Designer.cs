namespace FEI.IRK.HM.RMR.App
{
    partial class StartupForm
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
            this.groupFiles = new System.Windows.Forms.GroupBox();
            this.PathToSensorFile = new System.Windows.Forms.TextBox();
            this.btnChooseSensorFile = new System.Windows.Forms.Button();
            this.lblChooseSensorFile = new System.Windows.Forms.Label();
            this.lblChooseScanFile = new System.Windows.Forms.Label();
            this.btnChooseScanFile = new System.Windows.Forms.Button();
            this.PathToScanFile = new System.Windows.Forms.TextBox();
            this.lblIsSensorFileOk = new System.Windows.Forms.Label();
            this.lblIsScanFileOk = new System.Windows.Forms.Label();
            this.lblSensorFileStatus = new System.Windows.Forms.Label();
            this.lblScanFileStatus = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.errorText = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupFiles
            // 
            this.groupFiles.Controls.Add(this.errorText);
            this.groupFiles.Controls.Add(this.lblScanFileStatus);
            this.groupFiles.Controls.Add(this.lblSensorFileStatus);
            this.groupFiles.Controls.Add(this.lblIsScanFileOk);
            this.groupFiles.Controls.Add(this.lblIsSensorFileOk);
            this.groupFiles.Controls.Add(this.lblChooseScanFile);
            this.groupFiles.Controls.Add(this.btnChooseScanFile);
            this.groupFiles.Controls.Add(this.PathToScanFile);
            this.groupFiles.Controls.Add(this.lblChooseSensorFile);
            this.groupFiles.Controls.Add(this.btnChooseSensorFile);
            this.groupFiles.Controls.Add(this.PathToSensorFile);
            this.groupFiles.Location = new System.Drawing.Point(13, 13);
            this.groupFiles.Name = "groupFiles";
            this.groupFiles.Size = new System.Drawing.Size(817, 366);
            this.groupFiles.TabIndex = 0;
            this.groupFiles.TabStop = false;
            this.groupFiles.Text = "Výber offline súborov robota";
            // 
            // PathToSensorFile
            // 
            this.PathToSensorFile.Location = new System.Drawing.Point(26, 49);
            this.PathToSensorFile.Name = "PathToSensorFile";
            this.PathToSensorFile.ReadOnly = true;
            this.PathToSensorFile.Size = new System.Drawing.Size(730, 20);
            this.PathToSensorFile.TabIndex = 0;
            // 
            // btnChooseSensorFile
            // 
            this.btnChooseSensorFile.Location = new System.Drawing.Point(762, 49);
            this.btnChooseSensorFile.Name = "btnChooseSensorFile";
            this.btnChooseSensorFile.Size = new System.Drawing.Size(35, 20);
            this.btnChooseSensorFile.TabIndex = 1;
            this.btnChooseSensorFile.Text = "...";
            this.btnChooseSensorFile.UseVisualStyleBackColor = true;
            this.btnChooseSensorFile.Click += new System.EventHandler(this.btnChooseSensorFile_Click);
            // 
            // lblChooseSensorFile
            // 
            this.lblChooseSensorFile.AutoSize = true;
            this.lblChooseSensorFile.Location = new System.Drawing.Point(26, 30);
            this.lblChooseSensorFile.Name = "lblChooseSensorFile";
            this.lblChooseSensorFile.Size = new System.Drawing.Size(231, 13);
            this.lblChooseSensorFile.TabIndex = 2;
            this.lblChooseSensorFile.Text = "Vyber súbor senzorických údajov iRobotCreate:";
            // 
            // lblChooseScanFile
            // 
            this.lblChooseScanFile.AutoSize = true;
            this.lblChooseScanFile.Location = new System.Drawing.Point(26, 97);
            this.lblChooseScanFile.Name = "lblChooseScanFile";
            this.lblChooseScanFile.Size = new System.Drawing.Size(285, 13);
            this.lblChooseScanFile.TabIndex = 5;
            this.lblChooseScanFile.Text = "Vyber súbor laserových meraní prekážok RPLidar skenera:";
            // 
            // btnChooseScanFile
            // 
            this.btnChooseScanFile.Location = new System.Drawing.Point(762, 116);
            this.btnChooseScanFile.Name = "btnChooseScanFile";
            this.btnChooseScanFile.Size = new System.Drawing.Size(35, 20);
            this.btnChooseScanFile.TabIndex = 4;
            this.btnChooseScanFile.Text = "...";
            this.btnChooseScanFile.UseVisualStyleBackColor = true;
            this.btnChooseScanFile.Click += new System.EventHandler(this.btnChooseScanFile_Click);
            // 
            // PathToScanFile
            // 
            this.PathToScanFile.Location = new System.Drawing.Point(26, 116);
            this.PathToScanFile.Name = "PathToScanFile";
            this.PathToScanFile.ReadOnly = true;
            this.PathToScanFile.Size = new System.Drawing.Size(730, 20);
            this.PathToScanFile.TabIndex = 3;
            // 
            // lblIsSensorFileOk
            // 
            this.lblIsSensorFileOk.AutoSize = true;
            this.lblIsSensorFileOk.Location = new System.Drawing.Point(26, 174);
            this.lblIsSensorFileOk.Name = "lblIsSensorFileOk";
            this.lblIsSensorFileOk.Size = new System.Drawing.Size(131, 13);
            this.lblIsSensorFileOk.TabIndex = 6;
            this.lblIsSensorFileOk.Text = "Údaje zo senzorov robota:";
            // 
            // lblIsScanFileOk
            // 
            this.lblIsScanFileOk.AutoSize = true;
            this.lblIsScanFileOk.Location = new System.Drawing.Point(414, 174);
            this.lblIsScanFileOk.Name = "lblIsScanFileOk";
            this.lblIsScanFileOk.Size = new System.Drawing.Size(142, 13);
            this.lblIsScanFileOk.TabIndex = 7;
            this.lblIsScanFileOk.Text = "Údaje z laserového skenera:";
            // 
            // lblSensorFileStatus
            // 
            this.lblSensorFileStatus.AutoSize = true;
            this.lblSensorFileStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblSensorFileStatus.Location = new System.Drawing.Point(163, 174);
            this.lblSensorFileStatus.Name = "lblSensorFileStatus";
            this.lblSensorFileStatus.Size = new System.Drawing.Size(102, 13);
            this.lblSensorFileStatus.TabIndex = 8;
            this.lblSensorFileStatus.Text = "Súbor nevybraný";
            // 
            // lblScanFileStatus
            // 
            this.lblScanFileStatus.AutoSize = true;
            this.lblScanFileStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblScanFileStatus.Location = new System.Drawing.Point(562, 174);
            this.lblScanFileStatus.Name = "lblScanFileStatus";
            this.lblScanFileStatus.Size = new System.Drawing.Size(102, 13);
            this.lblScanFileStatus.TabIndex = 9;
            this.lblScanFileStatus.Text = "Súbor nevybraný";
            // 
            // errorText
            // 
            this.errorText.Location = new System.Drawing.Point(26, 206);
            this.errorText.Multiline = true;
            this.errorText.Name = "errorText";
            this.errorText.ReadOnly = true;
            this.errorText.Size = new System.Drawing.Size(771, 138);
            this.errorText.TabIndex = 10;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(755, 385);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Zatvoriť";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(674, 385);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 424);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[I-RMR] Riadenie mobilných robotov (Martin Heteš)";
            this.groupFiles.ResumeLayout(false);
            this.groupFiles.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupFiles;
        private System.Windows.Forms.Button btnChooseSensorFile;
        private System.Windows.Forms.TextBox PathToSensorFile;
        private System.Windows.Forms.Label lblChooseScanFile;
        private System.Windows.Forms.Button btnChooseScanFile;
        private System.Windows.Forms.TextBox PathToScanFile;
        private System.Windows.Forms.Label lblChooseSensorFile;
        private System.Windows.Forms.Label lblScanFileStatus;
        private System.Windows.Forms.Label lblSensorFileStatus;
        private System.Windows.Forms.Label lblIsScanFileOk;
        private System.Windows.Forms.Label lblIsSensorFileOk;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox errorText;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOk;
    }
}

