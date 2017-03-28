using FEI.IRK.HM.RMR.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEI.IRK.HM.RMR.App
{
    public partial class StartupForm : Form
    {

        private string LastDir = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
        private Boolean okPressed = false;

        public StartupForm()
        {
            InitializeComponent();
        }


        public Boolean OkPressed()
        {
            return okPressed;
        }

        public string GetSensorFile()
        {
            return PathToSensorFile.Text;
        }

        public string GetScanFile()
        {
            return PathToScanFile.Text;
        }

        private void CheckFiles()
        {
            string errText = String.Empty;
            string errTextSensor = String.Empty;
            string errTextScan = String.Empty;
            Boolean SensorOk = false;
            Boolean ScanOk = false;

            // Check sensor file
            if (!String.IsNullOrWhiteSpace(PathToSensorFile.Text))
            {
                if (System.IO.File.Exists(PathToSensorFile.Text))
                {
                    
                    if (RobotSensorHelper.CheckFile(PathToSensorFile.Text, out errTextSensor))
                    {
                        lblSensorFileStatus.Text = "V poriadku";
                        lblSensorFileStatus.ForeColor = Color.Green;
                        SensorOk = true;
                    }
                    else
                    {
                        lblSensorFileStatus.Text = "Chybný súbor";
                        lblSensorFileStatus.ForeColor = Color.Red;
                    }                    
                }
                else
                {
                    PathToSensorFile.Text = String.Empty;
                    lblSensorFileStatus.Text = "Súbor nevybraný";
                    lblSensorFileStatus.ForeColor = Color.Black;
                }
            }

            // Check scan file
            if (!String.IsNullOrWhiteSpace(PathToScanFile.Text))
            {
                if (System.IO.File.Exists(PathToScanFile.Text))
                {

                    if (RPLidarHelper.CheckFile(PathToScanFile.Text, out errTextScan))
                    {
                        lblScanFileStatus.Text = "V poriadku";
                        lblScanFileStatus.ForeColor = Color.Green;
                        ScanOk = true;
                    }
                    else
                    {
                        lblScanFileStatus.Text = "Chybný súbor";
                        lblScanFileStatus.ForeColor = Color.Red;
                    }
                }
                else
                {
                    PathToScanFile.Text = String.Empty;
                    lblScanFileStatus.Text = "Súbor nevybraný";
                    lblScanFileStatus.ForeColor = Color.Black;
                }
            }

            // Set error text
            errText = String.Format("{0}\r\n{1}", errTextSensor, errTextScan);
            errorText.Text = errText;

            // Update OK button
            if (SensorOk && ScanOk)
            {
                btnOk.Enabled = true;
            }
            else
            {
                btnOk.Enabled = false;
            }
        }

        private void btnChooseSensorFile_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = LastDir;
            openFileDialog.ShowDialog();
            if (!String.IsNullOrWhiteSpace(openFileDialog.FileName) && openFileDialog.CheckFileExists)
            {
                LastDir = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                PathToSensorFile.Text = openFileDialog.FileName;
                CheckFiles();
            }
        }

        private void btnChooseScanFile_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = LastDir;
            openFileDialog.ShowDialog();
            if (!String.IsNullOrWhiteSpace(openFileDialog.FileName) && openFileDialog.CheckFileExists)
            {
                LastDir = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                PathToScanFile.Text = openFileDialog.FileName;
                CheckFiles();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            okPressed = true;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
