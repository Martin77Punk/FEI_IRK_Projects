using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace FEI.IRK.HM.VZ
{
    public partial class VZForm : Form
    {

        VZTask1 Task1;
        VZTask2 Task2;
        
        public VZForm()
        {
            InitializeComponent();
            Task1 = new VZTask1();
            Task1.FrameUpdated += EventRefreshFrames;
            Task1.FoundObjectsUpdated += EventFoundObjectsUpdated;
            Task1.FlipHorizontal = CheckFlipHorizontal.Checked;
            Task1.FlipVertical = CheckFlipVertical.Checked;
            Task2 = new VZTask2();
            Task2.BitmapChanged += EventBitmapChanged;
            Task2.Task2StatusChanged += EventTask2StatusChanged;
        }

        private void EventTask2StatusChanged(bool Loaded, bool Processed)
        {
            VZTask2.Task2StatusChangedFunction Task2StatusChangedDelegate = new VZTask2.Task2StatusChangedFunction(Task2StatusChanged);
            object[] args = new object[2];
            args[0] = Loaded;
            args[1] = Processed;
            this.Invoke(Task2StatusChangedDelegate, args);
        }

        private void Task2StatusChanged(bool Loaded, bool Processed)
        {
            if (Loaded)
            {
                LabelImgLoaded.ForeColor = Color.Green;
                LabelImgLoaded.Text = "ÁNO";
            }
            else
            {
                LabelImgLoaded.ForeColor = Color.Red;
                LabelImgLoaded.Text = "NIE";
            }
            if (Processed)
            {
                LabelImgFinished.ForeColor = Color.Green;
                LabelImgFinished.Text = "ÁNO";
            }
            else
            {
                LabelImgFinished.ForeColor = Color.Red;
                LabelImgFinished.Text = "NIE";
            }
            if (Loaded && !Processed)
            {
                ImgShowRadio1.Enabled = true;
                ImgShowRadio2.Enabled = false;
                ImgShowRadio3.Enabled = false;
                ImgShowRadio4.Enabled = false;
                ImgShowRadio1.Checked = true;
            }
            if (Loaded && Processed)
            {
                ImgShowRadio1.Enabled = true;
                ImgShowRadio2.Enabled = true;
                ImgShowRadio3.Enabled = true;
                ImgShowRadio4.Enabled = true;
                ImgShowRadio4.Checked = true;
            }

            ButtonImgImport.Enabled = true;
            ButtonImgProcess.Enabled = true;
            NumericMinRadius.Enabled = true;
            NumericMaxRadius.Enabled = true;
            NumericMinThreshold.Enabled = true;
            NumericMinDistance.Enabled = true;
        }

        private void EventBitmapChanged(Bitmap bmp)
        {
            VZTask2.BitmapChangedFunction BitmapChangedDelegate = new VZTask2.BitmapChangedFunction(BitmapChanged);
            object[] args = new object[1];
            args[0] = bmp;
            this.Invoke(BitmapChangedDelegate, args);
        }

        private void BitmapChanged(Bitmap bmp)
        {
            Task2PicBox.Image = bmp;
            Task2PicBox.Invalidate();
        }

        private void EventFoundObjectsUpdated(string[] FoundObjects)
        {
            VZTask1.FoundObjectsFunction FoundObjectsDelegate = new VZTask1.FoundObjectsFunction(FoundObjectsUpdated);
            object[] args = new object[1];
            args[0] = FoundObjects;
            this.Invoke(FoundObjectsDelegate, args);
        }

        private void FoundObjectsUpdated(string[] FoundObjects)
        {
            FoundObjectsListBox.Items.Clear();
            foreach(string FoundObject in FoundObjects)
            {
                FoundObjectsListBox.Items.Add(FoundObject);
            }
        }

        private void EventRefreshFrames()
        {
            VZTask1.FrameUpdatedFunction RefreshDelegate = new VZTask1.FrameUpdatedFunction(RefreshFrames);
            this.Invoke(RefreshDelegate);
        }

        private void RefreshFrames()
        {
            ImageBoxMain.Image = Task1.MainFrame;
            ImageBoxHSV.Image = Task1.HsvFrame;
            if(CheckShowCanny.Checked)
            {
                ImageBoxRed.Image = Task1.RedCannyFrame;
                ImageBoxGreen.Image = Task1.GreenCannyFrame;
                ImageBoxBlue.Image = Task1.BlueCannyFrame;
                ImageBoxCyan.Image = Task1.CyanCannyFrame;
                ImageBoxMagenta.Image = Task1.MagentaCannyFrame;
                ImageBoxYellow.Image = Task1.YellowCannyFrame;
            }
            else
            {
                ImageBoxRed.Image = Task1.RedFrame;
                ImageBoxGreen.Image = Task1.GreenFrame;
                ImageBoxBlue.Image = Task1.BlueFrame;
                ImageBoxCyan.Image = Task1.CyanFrame;
                ImageBoxMagenta.Image = Task1.MagentaFrame;
                ImageBoxYellow.Image = Task1.YellowFrame;
            }
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            Task1.StartCapture();
        }

        private void ButtonPause_Click(object sender, EventArgs e)
        {
            Task1.PauseCapture();
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            Task1.StopCapture();
        }

        private void CheckFlipHorizontal_CheckedChanged(object sender, EventArgs e)
        {
            Task1.FlipHorizontal = CheckFlipHorizontal.Checked;
        }

        private void CheckFlipVertical_CheckedChanged(object sender, EventArgs e)
        {
            Task1.FlipVertical = CheckFlipVertical.Checked;
        }

        private void VZForm_Shown(object sender, EventArgs e)
        {
            RefreshFrames();
        }

        private void VZForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Task1.Dispose();
            Task2.Dispose();
        }

        private void NumericUpDownContrast_ValueChanged(object sender, EventArgs e)
        {
            Task1.Contrast = (double)NumericUpDownContrast.Value;
        }

        private void ButtonImgImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Filter = "Obrázky (*.bmp)|*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ButtonImgImport.Enabled = false;
                ButtonImgProcess.Enabled = false;
                NumericMinRadius.Enabled = false;
                NumericMaxRadius.Enabled = false;
                NumericMinThreshold.Enabled = false;
                NumericMinDistance.Enabled = false;
                ImgShowRadio1.Enabled = false;
                ImgShowRadio2.Enabled = false;
                ImgShowRadio3.Enabled = false;
                ImgShowRadio4.Enabled = false;
                Task2.InputFile = ofd.FileName;
            }
            ofd.Dispose();
        }

        private void ButtonImgProcess_Click(object sender, EventArgs e)
        {
            ButtonImgImport.Enabled = false;
            ButtonImgProcess.Enabled = false;
            NumericMinRadius.Enabled = false;
            NumericMaxRadius.Enabled = false;
            NumericMinThreshold.Enabled = false;
            NumericMinDistance.Enabled = false;
            ImgShowRadio1.Enabled = false;
            ImgShowRadio2.Enabled = false;
            ImgShowRadio3.Enabled = false;
            ImgShowRadio4.Enabled = false;
            Task2.StartProcessing();
        }

        private void NumericMinRadius_ValueChanged(object sender, EventArgs e)
        {
            Task2.MinRadius = (int)NumericMinRadius.Value;
        }

        private void NumericMaxRadius_ValueChanged(object sender, EventArgs e)
        {
            Task2.MaxRadius = (int)NumericMaxRadius.Value;
        }

        private void NumericMinThreshold_ValueChanged(object sender, EventArgs e)
        {
            Task2.MinThreshold = (int)NumericMinThreshold.Value;
        }

        private void NumericMinDistance_ValueChanged(object sender, EventArgs e)
        {
            Task2.MinDistance = (int)NumericMinDistance.Value;
        }

        private void ImgShowRadio1_CheckedChanged(object sender, EventArgs e)
        {
            Task2.ShowImage(1);
        }

        private void ImgShowRadio2_CheckedChanged(object sender, EventArgs e)
        {
            Task2.ShowImage(2);
        }

        private void ImgShowRadio3_CheckedChanged(object sender, EventArgs e)
        {
            Task2.ShowImage(3);
        }

        private void ImgShowRadio4_CheckedChanged(object sender, EventArgs e)
        {
            Task2.ShowImage(4);
        }

        
    }
}
