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
    public partial class ShowtimeForm : Form
    {

        // SETTING Constants:
        private readonly decimal RobotDiameterMin = 15;
        private readonly decimal RobotDiameterMax = 50;
        private readonly decimal RobotDiameterDef = 20;
        private readonly Boolean MapShowAll = false;
        private readonly decimal MapQuantisationMin = 5;
        private readonly decimal MapQuantisationMax = 50;
        private readonly decimal MapQuantisationDef = 10;

        // Local Private variables
        private Boolean DataSuccesfullyLoaded = false;
        private string DataLoadErrors = String.Empty;
        private RobotSensorDataList SensorData;
        private RPLidarMeasurementList ScanData;

        // Timelines
        private LocalizationTimeline TimelineLocalization;

        // Control
        private int CurrentFrame = 0;
        private Boolean TrackBarClicked = false;
        private Boolean ProgrammaticTimeNumericBoxChange = false;


        public ShowtimeForm(string SensorFile, string ScanFile)
        {
            InitializeComponent();
            InitializeRobotData(SensorFile, ScanFile);
        }

        private void InitializeRobotData(string SensorFile, string ScanFile)
        {
            string SensorDataErrors;
            string ScanDataErrors;

            // Load sensor & scan file
            SensorData = RobotSensorHelper.Deserialize(SensorFile, out SensorDataErrors);
            ScanData = RPLidarHelper.Deserialize(ScanFile, out ScanDataErrors);

            // Check for errors
            if (SensorData == null || ScanData == null)
            {
                if (SensorData != null && ScanData == null)
                {
                    DataLoadErrors = String.Format("{0}", ScanDataErrors);
                }
                else if (SensorData == null && ScanData != null)
                {
                    DataLoadErrors = String.Format("{0}", SensorDataErrors);
                }
                else
                {
                    DataLoadErrors = String.Format("{0}\r\n{1}", SensorDataErrors, ScanDataErrors);
                }
                DataSuccesfullyLoaded = false;
                return;
            } 
            else
            {
                DataSuccesfullyLoaded = true;
            }

            // Initialize Timelines
            TimelinesInit();
            
            // Initialize Form Components: TimeTrackBar
            TimeTrackBar.Minimum = 0;
            TimeTrackBar.Maximum = TimelineLocalization.GetTotalFrames();
            TimeTrackBar.SmallChange = 1;
            TimeTrackBar.LargeChange = TimelineLocalization.GetFramesCountInSecond();

            // Initialize Form Components: TimeNumericBox
            TimeNumericBox.Minimum = 0;
            TimeNumericBox.Maximum = TimelineLocalization.GetTotalTime();
            TimeNumericBox.DecimalPlaces = TimelineLocalization.GetDecimalPlacesForSeconds();
            TimeNumericBox.Increment = TimelineLocalization.GetMinimumSecondsIncrement();


            // Initialize Form Components: RobotDiameterNumericBox
            RobotDiameterNumericBox.Minimum = RobotDiameterMin;
            RobotDiameterNumericBox.Maximum = RobotDiameterMax;
            RobotDiameterNumericBox.Value = RobotDiameterDef;

            // Initialize Form Components: ShowMapCheckBox
            ShowMapCheckBox.Checked = MapShowAll;

            // Initialize Form Components: MapQuantisationNumBox
            MapQuantisationNumBox.Minimum = MapQuantisationMin;
            MapQuantisationNumBox.Maximum = MapQuantisationMax;
            MapQuantisationNumBox.Value = MapQuantisationDef;

            // Initialize PlayerTimer
            PlayerTimer.Interval = 1;
            int TimelineDecimalPlaces = TimelineLocalization.GetDecimalPlacesForSeconds();
            for (int i = 3; i > TimelineDecimalPlaces && i >= 0; i--)
            {
                PlayerTimer.Interval = PlayerTimer.Interval * 10;
            }

        }



        private void TimelinesInit()
        {
            // Initialize LocalizationTimeline
            TimelineLocalization = new LocalizationTimeline(SensorData, ScanData);
            TimelineLocalization.SubscribeComponents(RobotDiameterNumericBox, ShowMapCheckBox, MapQuantisationNumBox, Task1ImageBox, Task1FrameTextBox, Task1TimeTextBox, Task1PosXTextBox, Task1PosYTextBox, Task1AngleTextBox, Task1VelocityTextBox, Task1LastSensorTextBox, Task1SensorListBox, Task1LastScanTextBox, Task1ScanListBox);

        }


        private void TimelinesUpdateCurrentFrame()
        {
            TimelineLocalization.GoToFrame(CurrentFrame);
        }


        #region Media Control functions

        private void PlayerStart()
        {
            if (CurrentFrame == TimelineLocalization.GetTotalFrames())
            {
                PlayerGotoFrame(0);
            }
            btnMediaFirst.Enabled = false;
            btnMediaLast.Enabled = false;
            btnMediaNext.Enabled = false;
            btnMediaPrevious.Enabled = false;
            btnMediaPlay.Enabled = false;
            TimeNumericBox.ReadOnly = true;
            btnMediaStop.Enabled = true;            
            PlayerTimer.Start();
        }


        private void PlayerStop()
        {
            if (!PlayerTimer.Enabled) return;
            PlayerTimer.Stop();
            btnMediaFirst.Enabled = true;
            btnMediaLast.Enabled = true;
            btnMediaNext.Enabled = true;
            btnMediaPrevious.Enabled = true;
            btnMediaPlay.Enabled = true;
            TimeNumericBox.ReadOnly = false;
            btnMediaStop.Enabled = false;
        }


        private void PlayerNext(Boolean FromPlayTimer)
        {
            if (FromPlayTimer)
            {
                // Playing
                if (CurrentFrame == TimelineLocalization.GetTotalFrames())
                {
                    PlayerStop();
                    return;
                }
                CurrentFrame++;
            }
            else
            {
                // Manual - one second
                PlayerStop();
                double CurrentSecond = TimelineLocalization.FrameNo2Time(CurrentFrame);
                if (Math.Ceiling(CurrentSecond) == CurrentSecond)
                {
                    // +1 second
                    CurrentSecond = CurrentSecond + 1;
                } 
                else
                {
                    // Ceil second
                    CurrentSecond = Math.Ceiling(CurrentSecond);
                }
                CurrentFrame = TimelineLocalization.Time2FrameNo(CurrentSecond);                
                if (CurrentFrame > TimelineLocalization.GetTotalFrames()) CurrentFrame = TimelineLocalization.GetTotalFrames();
            }
            PlayerUpdateComponents();
            TimelinesUpdateCurrentFrame();
        }

        private void PlayerPrevious()
        {
            PlayerStop();
            double CurrentSecond = TimelineLocalization.FrameNo2Time(CurrentFrame);
            if (Math.Floor(CurrentSecond) == CurrentSecond)
            {
                // -1 second
                CurrentSecond = CurrentSecond - 1;
            }
            else
            {
                // Ceil second
                CurrentSecond = Math.Floor(CurrentSecond);
            }
            CurrentFrame = TimelineLocalization.Time2FrameNo(CurrentSecond);
            if (CurrentFrame < 0) CurrentFrame = 0;
            PlayerUpdateComponents();
            TimelinesUpdateCurrentFrame();
        }


        private void PlayerRewind()
        {
            PlayerStop();
            CurrentFrame = 0;
            PlayerUpdateComponents();
            TimelinesUpdateCurrentFrame();
        }


        private void PlayerFastForward()
        {
            PlayerStop();
            CurrentFrame = TimelineLocalization.GetTotalFrames();
            PlayerUpdateComponents();
            TimelinesUpdateCurrentFrame();
        }


        private void PlayerGotoSecond(double Second)
        {
            PlayerGotoFrame(TimelineLocalization.Time2FrameNo(Second));
        }


        private void PlayerGotoFrame(int FrameNo)
        {
            if (CurrentFrame == FrameNo) return;
            PlayerStop();
            CurrentFrame = FrameNo;
            PlayerUpdateComponents();
            TimelinesUpdateCurrentFrame();
        }


        private void PlayerGotoSensorData(int SensorItemIdx)
        {
            PlayerStop();
            CurrentFrame = TimelineLocalization.GetFrameNoWithNthSensorData(SensorItemIdx);
            PlayerUpdateComponents();
            TimelinesUpdateCurrentFrame();
        }


        private void PlayerGotoScanData(int ScanItemIdx)
        {
            PlayerStop();
            CurrentFrame = TimelineLocalization.GetFrameNoWithNthScanData(ScanItemIdx);
            PlayerUpdateComponents();
            TimelinesUpdateCurrentFrame();
        }


        private void PlayerUpdateComponents()
        {
            ProgrammaticTimeNumericBoxChange = true;
            TimeTrackBar.Value = CurrentFrame;
            TimeNumericBox.Value = (decimal)TimelineLocalization.FrameNo2Time(CurrentFrame);
            ProgrammaticTimeNumericBoxChange = false;
        }

        #endregion
        

        #region Form Events

        private void ShowtimeForm_Shown(object sender, EventArgs e)
        {
            if (!DataSuccesfullyLoaded)
            {
                MessageBox.Show(DataLoadErrors, "[I-RMR] Riadenie mobilných robotov (Martin Heteš)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        #endregion


        #region Media Buttons Events

        private void btnMediaPlay_Click(object sender, EventArgs e)
        {
            PlayerStart();
        }

        private void btnMediaStop_Click(object sender, EventArgs e)
        {
            PlayerStop();
        }

        private void btnMediaPrevious_Click(object sender, EventArgs e)
        {
            PlayerPrevious();
        }

        private void btnMediaNext_Click(object sender, EventArgs e)
        {
            PlayerNext(false);
        }

        private void btnMediaFirst_Click(object sender, EventArgs e)
        {
            PlayerRewind();
        }

        private void btnMediaLast_Click(object sender, EventArgs e)
        {
            PlayerFastForward();
        }

        #endregion


        #region Time manipulation components events

        private void TimeTrackBar_Scroll(object sender, EventArgs e)
        {
            if (TrackBarClicked)
            {
                // Update only TimeNumericBox
                TimeNumericBox.Value = (decimal)TimelineLocalization.FrameNo2Time(TimeTrackBar.Value);
                //System.Diagnostics.Debug.WriteLine("TimeTrackBar - Set TimeNumericBox: " + TimeTrackBar.Value.ToString());
            }
            
        }

        private void TimeTrackBar_MouseDown(object sender, MouseEventArgs e)
        {
            PlayerStop();
            TrackBarClicked = true;
        }

        private void TimeTrackBar_MouseUp(object sender, MouseEventArgs e)
        {
            TrackBarClicked = false;
            TimeNumericBox.Value = (decimal)TimelineLocalization.FrameNo2Time(TimeTrackBar.Value);
            PlayerGotoFrame(TimeTrackBar.Value);
            //System.Diagnostics.Debug.WriteLine("TimeTrackBar - Set Time: " + TimeTrackBar.Value.ToString());
        }

        private void TimeNumericBox_ValueChanged(object sender, EventArgs e)
        {
            if (!TrackBarClicked && !ProgrammaticTimeNumericBoxChange)
            {
                PlayerStop();
                TimeTrackBar.Value = TimelineLocalization.Time2FrameNo((double)TimeNumericBox.Value);
                PlayerGotoSecond((double)TimeNumericBox.Value);
                //System.Diagnostics.Debug.WriteLine("TimeNumericBox - Set Time: " + TimeNumericBox.Value.ToString());
            }
            
        }


        private void PlayerTimer_Tick(object sender, EventArgs e)
        {
            PlayerNext(true);
        }



        #endregion


        #region Task1 Events

        private void Task1SensorListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlayerGotoSensorData(Task1SensorListBox.SelectedIndex);
        }

        private void Task1ScanListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlayerGotoScanData(Task1ScanListBox.SelectedIndex);
        }

        #endregion

    }
}
