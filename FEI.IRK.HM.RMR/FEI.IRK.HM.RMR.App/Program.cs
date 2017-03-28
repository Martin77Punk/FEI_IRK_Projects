using FEI.IRK.HM.RMR.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FEI.IRK.HM.RMR.App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check Command line arguments
            string[] CmdLineArgs = Environment.GetCommandLineArgs();
            if (CmdLineArgs.Length >= 3)
            {
                string SensorFile = CmdLineArgs[1];
                string ScanFile = CmdLineArgs[2];
                if (CheckFiles(SensorFile, ScanFile))
                {
                    Application.Run(new ShowtimeForm(SensorFile, ScanFile));
                    return;
                }
            }

            // Basic execution
            Boolean OkPressed = true;
            while (OkPressed)
            {
                StartupForm startupForm = new StartupForm();
                startupForm.ShowDialog();
                OkPressed = startupForm.OkPressed();
                if (OkPressed)
                {
                    string SensorFile = startupForm.GetSensorFile();
                    string ScanFile = startupForm.GetScanFile();
                    startupForm.Dispose();
                    if (CheckFiles(SensorFile, ScanFile))
                    {
                        Application.Run(new ShowtimeForm(SensorFile, ScanFile));
                        return;
                    }
                } else
                {
                    startupForm.Dispose();
                }
            }
        }


        /// <summary>
        /// Checks Sensor and Scan file for validity and display MessageBox with error in case of problem
        /// </summary>
        /// <param name="SensorFile">Full path to Sensor file</param>
        /// <param name="ScanFile">Full path to Scan file</param>
        /// <returns>TRUE if both files has been verified succesfully, otherwise FALSE</returns>
        static Boolean CheckFiles(string SensorFile, string ScanFile)
        {
            string errText = String.Empty;
            string errTextSensor = String.Empty;
            string errTextScan = String.Empty;
            Boolean SensorOk = false;
            Boolean ScanOk = false;

            // Check sensor file
            if (!String.IsNullOrWhiteSpace(SensorFile))
            {
                if (System.IO.File.Exists(SensorFile))
                {
                    if (RobotSensorHelper.CheckFile(SensorFile, out errTextSensor))
                    {
                        errTextSensor = String.Empty;
                        SensorOk = true;
                    }
                }
                else
                {
                    errTextSensor = String.Format("Súbor senzorových dát '{0}' neexistuje - zlá cesta!", SensorFile);
                }
            }

            // Check scan file
            if (!String.IsNullOrWhiteSpace(ScanFile))
            {
                if (System.IO.File.Exists(ScanFile))
                {
                    if (RPLidarHelper.CheckFile(ScanFile, out errTextScan))
                    {
                        errTextScan = String.Empty;
                        ScanOk = true;
                    }
                }
                else
                {
                    errTextScan = String.Format("Súbor sken dát '{0}' neexistuje - zlá cesta!", ScanFile);
                }
            }

            // Display error text
            errText = String.Format("{0}\r\n{1}", errTextSensor, errTextScan);

            // Update OK button
            if (SensorOk && ScanOk)
            {
                return true;
            }
            else
            {
                MessageBox.Show(errText, "[I-RMR] Riadenie mobilných robotov (Martin Heteš)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }


    }
}
