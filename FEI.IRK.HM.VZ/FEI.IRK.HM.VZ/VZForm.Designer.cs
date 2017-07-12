namespace FEI.IRK.HM.VZ
{
    partial class VZForm
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
            this.components = new System.ComponentModel.Container();
            this.VZTabMain = new System.Windows.Forms.TabControl();
            this.Task1Tab = new System.Windows.Forms.TabPage();
            this.lblFoundPbjects = new System.Windows.Forms.Label();
            this.FoundObjectsListBox = new System.Windows.Forms.ListBox();
            this.NumericUpDownContrast = new System.Windows.Forms.NumericUpDown();
            this.lblContrast = new System.Windows.Forms.Label();
            this.CheckShowCanny = new System.Windows.Forms.CheckBox();
            this.VZTabScreens = new System.Windows.Forms.TabControl();
            this.Screen1Tab = new System.Windows.Forms.TabPage();
            this.ImageBoxMain = new Emgu.CV.UI.ImageBox();
            this.Screen2Tab = new System.Windows.Forms.TabPage();
            this.ImageBoxHSV = new Emgu.CV.UI.ImageBox();
            this.Screen3Tab = new System.Windows.Forms.TabPage();
            this.ImageBoxRed = new Emgu.CV.UI.ImageBox();
            this.Screen4Tab = new System.Windows.Forms.TabPage();
            this.ImageBoxGreen = new Emgu.CV.UI.ImageBox();
            this.Screen5Tab = new System.Windows.Forms.TabPage();
            this.ImageBoxBlue = new Emgu.CV.UI.ImageBox();
            this.Screen6Tab = new System.Windows.Forms.TabPage();
            this.ImageBoxCyan = new Emgu.CV.UI.ImageBox();
            this.Screen7Tab = new System.Windows.Forms.TabPage();
            this.ImageBoxMagenta = new Emgu.CV.UI.ImageBox();
            this.Screen8Tab = new System.Windows.Forms.TabPage();
            this.ImageBoxYellow = new Emgu.CV.UI.ImageBox();
            this.CheckFlipVertical = new System.Windows.Forms.CheckBox();
            this.CheckFlipHorizontal = new System.Windows.Forms.CheckBox();
            this.ButtonPause = new System.Windows.Forms.Button();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.Task2Tab = new System.Windows.Forms.TabPage();
            this.ImageTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.ImgShowRadio4 = new System.Windows.Forms.RadioButton();
            this.ImgShowRadio3 = new System.Windows.Forms.RadioButton();
            this.ImgShowRadio2 = new System.Windows.Forms.RadioButton();
            this.ImgShowRadio1 = new System.Windows.Forms.RadioButton();
            this.NumericMinThreshold = new System.Windows.Forms.NumericUpDown();
            this.lblMinThreshold = new System.Windows.Forms.Label();
            this.lblPx2 = new System.Windows.Forms.Label();
            this.lblPx1 = new System.Windows.Forms.Label();
            this.NumericMaxRadius = new System.Windows.Forms.NumericUpDown();
            this.lblMaxRadius = new System.Windows.Forms.Label();
            this.NumericMinRadius = new System.Windows.Forms.NumericUpDown();
            this.lblMinRadius = new System.Windows.Forms.Label();
            this.ButtonImgProcess = new System.Windows.Forms.Button();
            this.ButtonImgImport = new System.Windows.Forms.Button();
            this.LabelImgFinished = new System.Windows.Forms.Label();
            this.LabelImgLoaded = new System.Windows.Forms.Label();
            this.lblTask2CapFinished = new System.Windows.Forms.Label();
            this.lblTask2CapLoaded = new System.Windows.Forms.Label();
            this.Task2PicBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NumericMinDistance = new System.Windows.Forms.NumericUpDown();
            this.lblPx3 = new System.Windows.Forms.Label();
            this.ButtonStop = new System.Windows.Forms.Button();
            this.VZTabMain.SuspendLayout();
            this.Task1Tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownContrast)).BeginInit();
            this.VZTabScreens.SuspendLayout();
            this.Screen1Tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxMain)).BeginInit();
            this.Screen2Tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxHSV)).BeginInit();
            this.Screen3Tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxRed)).BeginInit();
            this.Screen4Tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxGreen)).BeginInit();
            this.Screen5Tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxBlue)).BeginInit();
            this.Screen6Tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxCyan)).BeginInit();
            this.Screen7Tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxMagenta)).BeginInit();
            this.Screen8Tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxYellow)).BeginInit();
            this.Task2Tab.SuspendLayout();
            this.ImageTypeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericMinThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericMaxRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericMinRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Task2PicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericMinDistance)).BeginInit();
            this.SuspendLayout();
            // 
            // VZTabMain
            // 
            this.VZTabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VZTabMain.Controls.Add(this.Task1Tab);
            this.VZTabMain.Controls.Add(this.Task2Tab);
            this.VZTabMain.Location = new System.Drawing.Point(13, 13);
            this.VZTabMain.Name = "VZTabMain";
            this.VZTabMain.SelectedIndex = 0;
            this.VZTabMain.Size = new System.Drawing.Size(1256, 699);
            this.VZTabMain.TabIndex = 0;
            // 
            // Task1Tab
            // 
            this.Task1Tab.Controls.Add(this.ButtonStop);
            this.Task1Tab.Controls.Add(this.lblFoundPbjects);
            this.Task1Tab.Controls.Add(this.FoundObjectsListBox);
            this.Task1Tab.Controls.Add(this.NumericUpDownContrast);
            this.Task1Tab.Controls.Add(this.lblContrast);
            this.Task1Tab.Controls.Add(this.CheckShowCanny);
            this.Task1Tab.Controls.Add(this.VZTabScreens);
            this.Task1Tab.Controls.Add(this.CheckFlipVertical);
            this.Task1Tab.Controls.Add(this.CheckFlipHorizontal);
            this.Task1Tab.Controls.Add(this.ButtonPause);
            this.Task1Tab.Controls.Add(this.ButtonStart);
            this.Task1Tab.Location = new System.Drawing.Point(4, 22);
            this.Task1Tab.Name = "Task1Tab";
            this.Task1Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Task1Tab.Size = new System.Drawing.Size(1248, 673);
            this.Task1Tab.TabIndex = 0;
            this.Task1Tab.Text = "Úloha 1";
            this.Task1Tab.UseVisualStyleBackColor = true;
            // 
            // lblFoundPbjects
            // 
            this.lblFoundPbjects.AutoSize = true;
            this.lblFoundPbjects.Location = new System.Drawing.Point(839, 254);
            this.lblFoundPbjects.Name = "lblFoundPbjects";
            this.lblFoundPbjects.Size = new System.Drawing.Size(87, 13);
            this.lblFoundPbjects.TabIndex = 9;
            this.lblFoundPbjects.Text = "Nájdené objekty:";
            // 
            // FoundObjectsListBox
            // 
            this.FoundObjectsListBox.FormattingEnabled = true;
            this.FoundObjectsListBox.Location = new System.Drawing.Point(839, 273);
            this.FoundObjectsListBox.Name = "FoundObjectsListBox";
            this.FoundObjectsListBox.Size = new System.Drawing.Size(403, 381);
            this.FoundObjectsListBox.TabIndex = 8;
            // 
            // NumericUpDownContrast
            // 
            this.NumericUpDownContrast.DecimalPlaces = 2;
            this.NumericUpDownContrast.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.NumericUpDownContrast.Location = new System.Drawing.Point(921, 176);
            this.NumericUpDownContrast.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.NumericUpDownContrast.Name = "NumericUpDownContrast";
            this.NumericUpDownContrast.Size = new System.Drawing.Size(103, 20);
            this.NumericUpDownContrast.TabIndex = 7;
            this.NumericUpDownContrast.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDownContrast.ValueChanged += new System.EventHandler(this.NumericUpDownContrast_ValueChanged);
            // 
            // lblContrast
            // 
            this.lblContrast.AutoSize = true;
            this.lblContrast.Location = new System.Drawing.Point(866, 178);
            this.lblContrast.Name = "lblContrast";
            this.lblContrast.Size = new System.Drawing.Size(49, 13);
            this.lblContrast.TabIndex = 6;
            this.lblContrast.Text = "Kontrast:";
            // 
            // CheckShowCanny
            // 
            this.CheckShowCanny.AutoSize = true;
            this.CheckShowCanny.Location = new System.Drawing.Point(869, 129);
            this.CheckShowCanny.Name = "CheckShowCanny";
            this.CheckShowCanny.Size = new System.Drawing.Size(179, 17);
            this.CheckShowCanny.TabIndex = 5;
            this.CheckShowCanny.Text = "Zobraziť hrany (len farebné tóny)";
            this.CheckShowCanny.UseVisualStyleBackColor = true;
            // 
            // VZTabScreens
            // 
            this.VZTabScreens.Controls.Add(this.Screen1Tab);
            this.VZTabScreens.Controls.Add(this.Screen2Tab);
            this.VZTabScreens.Controls.Add(this.Screen3Tab);
            this.VZTabScreens.Controls.Add(this.Screen4Tab);
            this.VZTabScreens.Controls.Add(this.Screen5Tab);
            this.VZTabScreens.Controls.Add(this.Screen6Tab);
            this.VZTabScreens.Controls.Add(this.Screen7Tab);
            this.VZTabScreens.Controls.Add(this.Screen8Tab);
            this.VZTabScreens.Location = new System.Drawing.Point(7, 7);
            this.VZTabScreens.Name = "VZTabScreens";
            this.VZTabScreens.SelectedIndex = 0;
            this.VZTabScreens.Size = new System.Drawing.Size(825, 651);
            this.VZTabScreens.TabIndex = 4;
            // 
            // Screen1Tab
            // 
            this.Screen1Tab.Controls.Add(this.ImageBoxMain);
            this.Screen1Tab.Location = new System.Drawing.Point(4, 22);
            this.Screen1Tab.Name = "Screen1Tab";
            this.Screen1Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Screen1Tab.Size = new System.Drawing.Size(817, 625);
            this.Screen1Tab.TabIndex = 0;
            this.Screen1Tab.Text = "Hlavné zobrazenie";
            this.Screen1Tab.UseVisualStyleBackColor = true;
            // 
            // ImageBoxMain
            // 
            this.ImageBoxMain.Location = new System.Drawing.Point(7, 10);
            this.ImageBoxMain.Name = "ImageBoxMain";
            this.ImageBoxMain.Size = new System.Drawing.Size(800, 600);
            this.ImageBoxMain.TabIndex = 2;
            this.ImageBoxMain.TabStop = false;
            // 
            // Screen2Tab
            // 
            this.Screen2Tab.Controls.Add(this.ImageBoxHSV);
            this.Screen2Tab.Location = new System.Drawing.Point(4, 22);
            this.Screen2Tab.Name = "Screen2Tab";
            this.Screen2Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Screen2Tab.Size = new System.Drawing.Size(817, 625);
            this.Screen2Tab.TabIndex = 1;
            this.Screen2Tab.Text = "HSV Model";
            this.Screen2Tab.UseVisualStyleBackColor = true;
            // 
            // ImageBoxHSV
            // 
            this.ImageBoxHSV.Location = new System.Drawing.Point(7, 10);
            this.ImageBoxHSV.Name = "ImageBoxHSV";
            this.ImageBoxHSV.Size = new System.Drawing.Size(800, 600);
            this.ImageBoxHSV.TabIndex = 2;
            this.ImageBoxHSV.TabStop = false;
            // 
            // Screen3Tab
            // 
            this.Screen3Tab.Controls.Add(this.ImageBoxRed);
            this.Screen3Tab.Location = new System.Drawing.Point(4, 22);
            this.Screen3Tab.Name = "Screen3Tab";
            this.Screen3Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Screen3Tab.Size = new System.Drawing.Size(817, 625);
            this.Screen3Tab.TabIndex = 2;
            this.Screen3Tab.Text = "Červená";
            this.Screen3Tab.UseVisualStyleBackColor = true;
            // 
            // ImageBoxRed
            // 
            this.ImageBoxRed.Location = new System.Drawing.Point(7, 10);
            this.ImageBoxRed.Name = "ImageBoxRed";
            this.ImageBoxRed.Size = new System.Drawing.Size(800, 600);
            this.ImageBoxRed.TabIndex = 2;
            this.ImageBoxRed.TabStop = false;
            // 
            // Screen4Tab
            // 
            this.Screen4Tab.Controls.Add(this.ImageBoxGreen);
            this.Screen4Tab.Location = new System.Drawing.Point(4, 22);
            this.Screen4Tab.Name = "Screen4Tab";
            this.Screen4Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Screen4Tab.Size = new System.Drawing.Size(817, 625);
            this.Screen4Tab.TabIndex = 3;
            this.Screen4Tab.Text = "Zelená";
            this.Screen4Tab.UseVisualStyleBackColor = true;
            // 
            // ImageBoxGreen
            // 
            this.ImageBoxGreen.Location = new System.Drawing.Point(7, 10);
            this.ImageBoxGreen.Name = "ImageBoxGreen";
            this.ImageBoxGreen.Size = new System.Drawing.Size(800, 600);
            this.ImageBoxGreen.TabIndex = 2;
            this.ImageBoxGreen.TabStop = false;
            // 
            // Screen5Tab
            // 
            this.Screen5Tab.Controls.Add(this.ImageBoxBlue);
            this.Screen5Tab.Location = new System.Drawing.Point(4, 22);
            this.Screen5Tab.Name = "Screen5Tab";
            this.Screen5Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Screen5Tab.Size = new System.Drawing.Size(817, 625);
            this.Screen5Tab.TabIndex = 4;
            this.Screen5Tab.Text = "Modrá";
            this.Screen5Tab.UseVisualStyleBackColor = true;
            // 
            // ImageBoxBlue
            // 
            this.ImageBoxBlue.Location = new System.Drawing.Point(7, 10);
            this.ImageBoxBlue.Name = "ImageBoxBlue";
            this.ImageBoxBlue.Size = new System.Drawing.Size(800, 600);
            this.ImageBoxBlue.TabIndex = 2;
            this.ImageBoxBlue.TabStop = false;
            // 
            // Screen6Tab
            // 
            this.Screen6Tab.Controls.Add(this.ImageBoxCyan);
            this.Screen6Tab.Location = new System.Drawing.Point(4, 22);
            this.Screen6Tab.Name = "Screen6Tab";
            this.Screen6Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Screen6Tab.Size = new System.Drawing.Size(817, 625);
            this.Screen6Tab.TabIndex = 5;
            this.Screen6Tab.Text = "Tyrkysová";
            this.Screen6Tab.UseVisualStyleBackColor = true;
            // 
            // ImageBoxCyan
            // 
            this.ImageBoxCyan.Location = new System.Drawing.Point(7, 10);
            this.ImageBoxCyan.Name = "ImageBoxCyan";
            this.ImageBoxCyan.Size = new System.Drawing.Size(800, 600);
            this.ImageBoxCyan.TabIndex = 2;
            this.ImageBoxCyan.TabStop = false;
            // 
            // Screen7Tab
            // 
            this.Screen7Tab.Controls.Add(this.ImageBoxMagenta);
            this.Screen7Tab.Location = new System.Drawing.Point(4, 22);
            this.Screen7Tab.Name = "Screen7Tab";
            this.Screen7Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Screen7Tab.Size = new System.Drawing.Size(817, 625);
            this.Screen7Tab.TabIndex = 6;
            this.Screen7Tab.Text = "Purpurová";
            this.Screen7Tab.UseVisualStyleBackColor = true;
            // 
            // ImageBoxMagenta
            // 
            this.ImageBoxMagenta.Location = new System.Drawing.Point(7, 10);
            this.ImageBoxMagenta.Name = "ImageBoxMagenta";
            this.ImageBoxMagenta.Size = new System.Drawing.Size(800, 600);
            this.ImageBoxMagenta.TabIndex = 2;
            this.ImageBoxMagenta.TabStop = false;
            // 
            // Screen8Tab
            // 
            this.Screen8Tab.Controls.Add(this.ImageBoxYellow);
            this.Screen8Tab.Location = new System.Drawing.Point(4, 22);
            this.Screen8Tab.Name = "Screen8Tab";
            this.Screen8Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Screen8Tab.Size = new System.Drawing.Size(817, 625);
            this.Screen8Tab.TabIndex = 7;
            this.Screen8Tab.Text = "Žltá";
            this.Screen8Tab.UseVisualStyleBackColor = true;
            // 
            // ImageBoxYellow
            // 
            this.ImageBoxYellow.Location = new System.Drawing.Point(7, 10);
            this.ImageBoxYellow.Name = "ImageBoxYellow";
            this.ImageBoxYellow.Size = new System.Drawing.Size(800, 600);
            this.ImageBoxYellow.TabIndex = 2;
            this.ImageBoxYellow.TabStop = false;
            // 
            // CheckFlipVertical
            // 
            this.CheckFlipVertical.AutoSize = true;
            this.CheckFlipVertical.Location = new System.Drawing.Point(1056, 82);
            this.CheckFlipVertical.Name = "CheckFlipVertical";
            this.CheckFlipVertical.Size = new System.Drawing.Size(115, 17);
            this.CheckFlipVertical.TabIndex = 3;
            this.CheckFlipVertical.Text = "Prevrátiť vertikálne";
            this.CheckFlipVertical.UseVisualStyleBackColor = true;
            this.CheckFlipVertical.CheckedChanged += new System.EventHandler(this.CheckFlipVertical_CheckedChanged);
            // 
            // CheckFlipHorizontal
            // 
            this.CheckFlipHorizontal.AutoSize = true;
            this.CheckFlipHorizontal.Checked = true;
            this.CheckFlipHorizontal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckFlipHorizontal.Location = new System.Drawing.Point(869, 82);
            this.CheckFlipHorizontal.Name = "CheckFlipHorizontal";
            this.CheckFlipHorizontal.Size = new System.Drawing.Size(126, 17);
            this.CheckFlipHorizontal.TabIndex = 2;
            this.CheckFlipHorizontal.Text = "Prevrátiť horizontálne";
            this.CheckFlipHorizontal.UseVisualStyleBackColor = true;
            this.CheckFlipHorizontal.CheckedChanged += new System.EventHandler(this.CheckFlipHorizontal_CheckedChanged);
            // 
            // ButtonPause
            // 
            this.ButtonPause.Location = new System.Drawing.Point(984, 39);
            this.ButtonPause.Name = "ButtonPause";
            this.ButtonPause.Size = new System.Drawing.Size(109, 23);
            this.ButtonPause.TabIndex = 1;
            this.ButtonPause.Text = "Pauza";
            this.ButtonPause.UseVisualStyleBackColor = true;
            this.ButtonPause.Click += new System.EventHandler(this.ButtonPause_Click);
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(869, 39);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(109, 23);
            this.ButtonStart.TabIndex = 0;
            this.ButtonStart.Text = "Štart";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // Task2Tab
            // 
            this.Task2Tab.Controls.Add(this.lblPx3);
            this.Task2Tab.Controls.Add(this.NumericMinDistance);
            this.Task2Tab.Controls.Add(this.label1);
            this.Task2Tab.Controls.Add(this.ImageTypeGroupBox);
            this.Task2Tab.Controls.Add(this.NumericMinThreshold);
            this.Task2Tab.Controls.Add(this.lblMinThreshold);
            this.Task2Tab.Controls.Add(this.lblPx2);
            this.Task2Tab.Controls.Add(this.lblPx1);
            this.Task2Tab.Controls.Add(this.NumericMaxRadius);
            this.Task2Tab.Controls.Add(this.lblMaxRadius);
            this.Task2Tab.Controls.Add(this.NumericMinRadius);
            this.Task2Tab.Controls.Add(this.lblMinRadius);
            this.Task2Tab.Controls.Add(this.ButtonImgProcess);
            this.Task2Tab.Controls.Add(this.ButtonImgImport);
            this.Task2Tab.Controls.Add(this.LabelImgFinished);
            this.Task2Tab.Controls.Add(this.LabelImgLoaded);
            this.Task2Tab.Controls.Add(this.lblTask2CapFinished);
            this.Task2Tab.Controls.Add(this.lblTask2CapLoaded);
            this.Task2Tab.Controls.Add(this.Task2PicBox);
            this.Task2Tab.Location = new System.Drawing.Point(4, 22);
            this.Task2Tab.Name = "Task2Tab";
            this.Task2Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Task2Tab.Size = new System.Drawing.Size(1248, 673);
            this.Task2Tab.TabIndex = 1;
            this.Task2Tab.Text = "Úloha 2";
            this.Task2Tab.UseVisualStyleBackColor = true;
            // 
            // ImageTypeGroupBox
            // 
            this.ImageTypeGroupBox.Controls.Add(this.ImgShowRadio4);
            this.ImageTypeGroupBox.Controls.Add(this.ImgShowRadio3);
            this.ImageTypeGroupBox.Controls.Add(this.ImgShowRadio2);
            this.ImageTypeGroupBox.Controls.Add(this.ImgShowRadio1);
            this.ImageTypeGroupBox.Location = new System.Drawing.Point(856, 296);
            this.ImageTypeGroupBox.Name = "ImageTypeGroupBox";
            this.ImageTypeGroupBox.Size = new System.Drawing.Size(361, 170);
            this.ImageTypeGroupBox.TabIndex = 15;
            this.ImageTypeGroupBox.TabStop = false;
            this.ImageTypeGroupBox.Text = "Zobraziť";
            // 
            // ImgShowRadio4
            // 
            this.ImgShowRadio4.AutoSize = true;
            this.ImgShowRadio4.Enabled = false;
            this.ImgShowRadio4.Location = new System.Drawing.Point(54, 115);
            this.ImgShowRadio4.Name = "ImgShowRadio4";
            this.ImgShowRadio4.Size = new System.Drawing.Size(99, 17);
            this.ImgShowRadio4.TabIndex = 3;
            this.ImgShowRadio4.TabStop = true;
            this.ImgShowRadio4.Text = "Finálny obrázok";
            this.ImgShowRadio4.UseVisualStyleBackColor = true;
            this.ImgShowRadio4.CheckedChanged += new System.EventHandler(this.ImgShowRadio4_CheckedChanged);
            // 
            // ImgShowRadio3
            // 
            this.ImgShowRadio3.AutoSize = true;
            this.ImgShowRadio3.Enabled = false;
            this.ImgShowRadio3.Location = new System.Drawing.Point(54, 91);
            this.ImgShowRadio3.Name = "ImgShowRadio3";
            this.ImgShowRadio3.Size = new System.Drawing.Size(108, 17);
            this.ImgShowRadio3.TabIndex = 2;
            this.ImgShowRadio3.TabStop = true;
            this.ImgShowRadio3.Text = "Nájdené kružnice";
            this.ImgShowRadio3.UseVisualStyleBackColor = true;
            this.ImgShowRadio3.CheckedChanged += new System.EventHandler(this.ImgShowRadio3_CheckedChanged);
            // 
            // ImgShowRadio2
            // 
            this.ImgShowRadio2.AutoSize = true;
            this.ImgShowRadio2.Enabled = false;
            this.ImgShowRadio2.Location = new System.Drawing.Point(54, 67);
            this.ImgShowRadio2.Name = "ImgShowRadio2";
            this.ImgShowRadio2.Size = new System.Drawing.Size(178, 17);
            this.ImgShowRadio2.TabIndex = 1;
            this.ImgShowRadio2.TabStop = true;
            this.ImgShowRadio2.Text = "Hrany pomocou Sobel operátora";
            this.ImgShowRadio2.UseVisualStyleBackColor = true;
            this.ImgShowRadio2.CheckedChanged += new System.EventHandler(this.ImgShowRadio2_CheckedChanged);
            // 
            // ImgShowRadio1
            // 
            this.ImgShowRadio1.AutoSize = true;
            this.ImgShowRadio1.Enabled = false;
            this.ImgShowRadio1.Location = new System.Drawing.Point(54, 43);
            this.ImgShowRadio1.Name = "ImgShowRadio1";
            this.ImgShowRadio1.Size = new System.Drawing.Size(124, 17);
            this.ImgShowRadio1.TabIndex = 0;
            this.ImgShowRadio1.TabStop = true;
            this.ImgShowRadio1.Text = "Importovaný obrázok";
            this.ImgShowRadio1.UseVisualStyleBackColor = true;
            this.ImgShowRadio1.CheckedChanged += new System.EventHandler(this.ImgShowRadio1_CheckedChanged);
            // 
            // NumericMinThreshold
            // 
            this.NumericMinThreshold.Location = new System.Drawing.Point(1041, 214);
            this.NumericMinThreshold.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.NumericMinThreshold.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.NumericMinThreshold.Name = "NumericMinThreshold";
            this.NumericMinThreshold.Size = new System.Drawing.Size(77, 20);
            this.NumericMinThreshold.TabIndex = 14;
            this.NumericMinThreshold.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.NumericMinThreshold.ValueChanged += new System.EventHandler(this.NumericMinThreshold_ValueChanged);
            // 
            // lblMinThreshold
            // 
            this.lblMinThreshold.AutoSize = true;
            this.lblMinThreshold.Location = new System.Drawing.Point(929, 216);
            this.lblMinThreshold.Name = "lblMinThreshold";
            this.lblMinThreshold.Size = new System.Drawing.Size(106, 13);
            this.lblMinThreshold.TabIndex = 13;
            this.lblMinThreshold.Text = "Minimálny Threshold:";
            // 
            // lblPx2
            // 
            this.lblPx2.AutoSize = true;
            this.lblPx2.Location = new System.Drawing.Point(1124, 175);
            this.lblPx2.Name = "lblPx2";
            this.lblPx2.Size = new System.Drawing.Size(18, 13);
            this.lblPx2.TabIndex = 12;
            this.lblPx2.Text = "px";
            // 
            // lblPx1
            // 
            this.lblPx1.AutoSize = true;
            this.lblPx1.Location = new System.Drawing.Point(1124, 149);
            this.lblPx1.Name = "lblPx1";
            this.lblPx1.Size = new System.Drawing.Size(18, 13);
            this.lblPx1.TabIndex = 11;
            this.lblPx1.Text = "px";
            // 
            // NumericMaxRadius
            // 
            this.NumericMaxRadius.Location = new System.Drawing.Point(1041, 173);
            this.NumericMaxRadius.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.NumericMaxRadius.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumericMaxRadius.Name = "NumericMaxRadius";
            this.NumericMaxRadius.Size = new System.Drawing.Size(77, 20);
            this.NumericMaxRadius.TabIndex = 10;
            this.NumericMaxRadius.Value = new decimal(new int[] {
            130,
            0,
            0,
            0});
            this.NumericMaxRadius.ValueChanged += new System.EventHandler(this.NumericMaxRadius_ValueChanged);
            // 
            // lblMaxRadius
            // 
            this.lblMaxRadius.AutoSize = true;
            this.lblMaxRadius.Location = new System.Drawing.Point(936, 175);
            this.lblMaxRadius.Name = "lblMaxRadius";
            this.lblMaxRadius.Size = new System.Drawing.Size(99, 13);
            this.lblMaxRadius.TabIndex = 9;
            this.lblMaxRadius.Text = "Maximálny polomer:";
            // 
            // NumericMinRadius
            // 
            this.NumericMinRadius.Location = new System.Drawing.Point(1041, 147);
            this.NumericMinRadius.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.NumericMinRadius.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumericMinRadius.Name = "NumericMinRadius";
            this.NumericMinRadius.Size = new System.Drawing.Size(77, 20);
            this.NumericMinRadius.TabIndex = 8;
            this.NumericMinRadius.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.NumericMinRadius.ValueChanged += new System.EventHandler(this.NumericMinRadius_ValueChanged);
            // 
            // lblMinRadius
            // 
            this.lblMinRadius.AutoSize = true;
            this.lblMinRadius.Location = new System.Drawing.Point(939, 149);
            this.lblMinRadius.Name = "lblMinRadius";
            this.lblMinRadius.Size = new System.Drawing.Size(96, 13);
            this.lblMinRadius.TabIndex = 7;
            this.lblMinRadius.Text = "Minimálny polomer:";
            // 
            // ButtonImgProcess
            // 
            this.ButtonImgProcess.Location = new System.Drawing.Point(1041, 91);
            this.ButtonImgProcess.Name = "ButtonImgProcess";
            this.ButtonImgProcess.Size = new System.Drawing.Size(176, 23);
            this.ButtonImgProcess.TabIndex = 6;
            this.ButtonImgProcess.Text = "Spusti spracovanie";
            this.ButtonImgProcess.UseVisualStyleBackColor = true;
            this.ButtonImgProcess.Click += new System.EventHandler(this.ButtonImgProcess_Click);
            // 
            // ButtonImgImport
            // 
            this.ButtonImgImport.Location = new System.Drawing.Point(856, 91);
            this.ButtonImgImport.Name = "ButtonImgImport";
            this.ButtonImgImport.Size = new System.Drawing.Size(179, 23);
            this.ButtonImgImport.TabIndex = 5;
            this.ButtonImgImport.Text = "Importuj obrázok";
            this.ButtonImgImport.UseVisualStyleBackColor = true;
            this.ButtonImgImport.Click += new System.EventHandler(this.ButtonImgImport_Click);
            // 
            // LabelImgFinished
            // 
            this.LabelImgFinished.AutoSize = true;
            this.LabelImgFinished.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LabelImgFinished.ForeColor = System.Drawing.Color.Red;
            this.LabelImgFinished.Location = new System.Drawing.Point(1169, 56);
            this.LabelImgFinished.Name = "LabelImgFinished";
            this.LabelImgFinished.Size = new System.Drawing.Size(28, 13);
            this.LabelImgFinished.TabIndex = 4;
            this.LabelImgFinished.Text = "NIE";
            // 
            // LabelImgLoaded
            // 
            this.LabelImgLoaded.AutoSize = true;
            this.LabelImgLoaded.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LabelImgLoaded.ForeColor = System.Drawing.Color.Red;
            this.LabelImgLoaded.Location = new System.Drawing.Point(984, 56);
            this.LabelImgLoaded.Name = "LabelImgLoaded";
            this.LabelImgLoaded.Size = new System.Drawing.Size(28, 13);
            this.LabelImgLoaded.TabIndex = 3;
            this.LabelImgLoaded.Text = "NIE";
            // 
            // lblTask2CapFinished
            // 
            this.lblTask2CapFinished.AutoSize = true;
            this.lblTask2CapFinished.Location = new System.Drawing.Point(1055, 56);
            this.lblTask2CapFinished.Name = "lblTask2CapFinished";
            this.lblTask2CapFinished.Size = new System.Drawing.Size(108, 13);
            this.lblTask2CapFinished.TabIndex = 2;
            this.lblTask2CapFinished.Text = "Obrázok spracovaný:";
            // 
            // lblTask2CapLoaded
            // 
            this.lblTask2CapLoaded.AutoSize = true;
            this.lblTask2CapLoaded.Location = new System.Drawing.Point(868, 56);
            this.lblTask2CapLoaded.Name = "lblTask2CapLoaded";
            this.lblTask2CapLoaded.Size = new System.Drawing.Size(110, 13);
            this.lblTask2CapLoaded.TabIndex = 1;
            this.lblTask2CapLoaded.Text = "Obrázok importovaný:";
            // 
            // Task2PicBox
            // 
            this.Task2PicBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Task2PicBox.Location = new System.Drawing.Point(17, 40);
            this.Task2PicBox.Name = "Task2PicBox";
            this.Task2PicBox.Size = new System.Drawing.Size(800, 600);
            this.Task2PicBox.TabIndex = 0;
            this.Task2PicBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(844, 240);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Minimálna vzdialenosť stredov kružníc:";
            // 
            // NumericMinDistance
            // 
            this.NumericMinDistance.Location = new System.Drawing.Point(1041, 240);
            this.NumericMinDistance.Name = "NumericMinDistance";
            this.NumericMinDistance.Size = new System.Drawing.Size(77, 20);
            this.NumericMinDistance.TabIndex = 17;
            this.NumericMinDistance.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.NumericMinDistance.ValueChanged += new System.EventHandler(this.NumericMinDistance_ValueChanged);
            // 
            // lblPx3
            // 
            this.lblPx3.AutoSize = true;
            this.lblPx3.Location = new System.Drawing.Point(1124, 242);
            this.lblPx3.Name = "lblPx3";
            this.lblPx3.Size = new System.Drawing.Size(18, 13);
            this.lblPx3.TabIndex = 18;
            this.lblPx3.Text = "px";
            // 
            // ButtonStop
            // 
            this.ButtonStop.Location = new System.Drawing.Point(1099, 39);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(109, 23);
            this.ButtonStop.TabIndex = 10;
            this.ButtonStop.Text = "Stop";
            this.ButtonStop.UseVisualStyleBackColor = true;
            this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // VZForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1281, 724);
            this.Controls.Add(this.VZTabMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VZForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vizuálne systémy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VZForm_FormClosing);
            this.Shown += new System.EventHandler(this.VZForm_Shown);
            this.VZTabMain.ResumeLayout(false);
            this.Task1Tab.ResumeLayout(false);
            this.Task1Tab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownContrast)).EndInit();
            this.VZTabScreens.ResumeLayout(false);
            this.Screen1Tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxMain)).EndInit();
            this.Screen2Tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxHSV)).EndInit();
            this.Screen3Tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxRed)).EndInit();
            this.Screen4Tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxGreen)).EndInit();
            this.Screen5Tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxBlue)).EndInit();
            this.Screen6Tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxCyan)).EndInit();
            this.Screen7Tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxMagenta)).EndInit();
            this.Screen8Tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBoxYellow)).EndInit();
            this.Task2Tab.ResumeLayout(false);
            this.Task2Tab.PerformLayout();
            this.ImageTypeGroupBox.ResumeLayout(false);
            this.ImageTypeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericMinThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericMaxRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericMinRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Task2PicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericMinDistance)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl VZTabMain;
        private System.Windows.Forms.TabPage Task1Tab;
        private System.Windows.Forms.TabPage Task2Tab;
        private System.Windows.Forms.Button ButtonPause;
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.TabControl VZTabScreens;
        private System.Windows.Forms.TabPage Screen1Tab;
        private System.Windows.Forms.TabPage Screen2Tab;
        private System.Windows.Forms.CheckBox CheckFlipVertical;
        private System.Windows.Forms.CheckBox CheckFlipHorizontal;
        private Emgu.CV.UI.ImageBox ImageBoxMain;
        private Emgu.CV.UI.ImageBox ImageBoxHSV;
        private System.Windows.Forms.TabPage Screen3Tab;
        private Emgu.CV.UI.ImageBox ImageBoxRed;
        private System.Windows.Forms.TabPage Screen4Tab;
        private Emgu.CV.UI.ImageBox ImageBoxGreen;
        private System.Windows.Forms.TabPage Screen5Tab;
        private System.Windows.Forms.TabPage Screen6Tab;
        private System.Windows.Forms.TabPage Screen7Tab;
        private System.Windows.Forms.TabPage Screen8Tab;
        private Emgu.CV.UI.ImageBox ImageBoxBlue;
        private Emgu.CV.UI.ImageBox ImageBoxCyan;
        private Emgu.CV.UI.ImageBox ImageBoxMagenta;
        private Emgu.CV.UI.ImageBox ImageBoxYellow;
        private System.Windows.Forms.CheckBox CheckShowCanny;
        private System.Windows.Forms.Label lblContrast;
        private System.Windows.Forms.NumericUpDown NumericUpDownContrast;
        private System.Windows.Forms.ListBox FoundObjectsListBox;
        private System.Windows.Forms.Label lblFoundPbjects;
        private System.Windows.Forms.PictureBox Task2PicBox;
        private System.Windows.Forms.Label lblTask2CapFinished;
        private System.Windows.Forms.Label lblTask2CapLoaded;
        private System.Windows.Forms.Label LabelImgFinished;
        private System.Windows.Forms.Label LabelImgLoaded;
        private System.Windows.Forms.Button ButtonImgProcess;
        private System.Windows.Forms.Button ButtonImgImport;
        private System.Windows.Forms.NumericUpDown NumericMaxRadius;
        private System.Windows.Forms.Label lblMaxRadius;
        private System.Windows.Forms.NumericUpDown NumericMinRadius;
        private System.Windows.Forms.Label lblMinRadius;
        private System.Windows.Forms.Label lblPx2;
        private System.Windows.Forms.Label lblPx1;
        private System.Windows.Forms.NumericUpDown NumericMinThreshold;
        private System.Windows.Forms.Label lblMinThreshold;
        private System.Windows.Forms.GroupBox ImageTypeGroupBox;
        private System.Windows.Forms.RadioButton ImgShowRadio4;
        private System.Windows.Forms.RadioButton ImgShowRadio3;
        private System.Windows.Forms.RadioButton ImgShowRadio2;
        private System.Windows.Forms.RadioButton ImgShowRadio1;
        private System.Windows.Forms.Label lblPx3;
        private System.Windows.Forms.NumericUpDown NumericMinDistance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ButtonStop;
    }
}

