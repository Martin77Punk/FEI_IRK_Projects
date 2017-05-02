using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEI.IRK.HM.RMR.Lib
{
    public abstract class TimelineBase
    {

        #region Private constants & Variables

        /// <summary>
        /// Sensor data deserialized from file
        /// </summary>
        private RobotSensorDataList SensorDataForTimeline;

        /// <summary>
        /// Laser scanner data deserialized from file
        /// </summary>
        private RPLidarMeasurementList ScanDataForTimeline;

        /// <summary>
        /// Windows forms components subscribed to current timeline
        /// </summary>
        private SubscribedComponents WinFormComponents;

        /// <summary>
        /// Current Frame number of current timeline
        /// </summary>
        private int CurrentFrameNo = 0;
        
        /// <summary>
        /// Factor indicating amount of ONE SECOND in Timestamp value
        /// </summary>
        private static readonly decimal Timestamp2TimeFactor = 1000000;   // microseconds

        /// <summary>
        /// Factor indicating amount of frames in ONE SECOND
        /// </summary>
        private static readonly decimal TimelineRateFactor = 100;         // 100 per second
        
        /// <summary>
        /// DOUBLE type format for serializing Time as String
        /// </summary>
        private static readonly string TimeFormat = "F2";

        /// <summary>
        /// DOUBLE type format for serializing Distance/Position as String
        /// </summary>
        private static readonly string DistanceFormat = "F3";

        /// <summary>
        /// Timeline Data with time frames
        /// </summary>
        private TimelineItemList TimelineData;

        #endregion


        #region Protected Accessors

        /// <summary>
        /// Timeline Items list
        /// </summary>
        protected TimelineItemList TimelineItems
        {
            get
            {
                return TimelineData;
            }
        }

        /// <summary>
        /// Robot's diameter
        /// </summary>
        protected decimal RobotDiameter
        {
            get
            {
                if (WinFormComponents != null && WinFormComponents.RobotDiameterNumericBox != null)
                {
                    return WinFormComponents.RobotDiameterNumericBox.Value;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Detour length for Tangent Bug algoritm
        /// </summary>
        protected decimal TangentBugDetour
        {
            get
            {
                if (WinFormComponents != null && WinFormComponents.TangentBugDetourNumericBox != null)
                {
                    return WinFormComponents.TangentBugDetourNumericBox.Value;
                } 
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// If TRUE display Whole map with all obstacles
        /// </summary>
        protected Boolean WholeObstacleMap
        {
            get
            {
                if (WinFormComponents != null && WinFormComponents.ShowMapCheckBox != null)
                {
                    return WinFormComponents.ShowMapCheckBox.Checked;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// If TRUE display robot track
        /// </summary>
        protected Boolean ShowRobotTrack
        {
            get
            {
                if (WinFormComponents != null && WinFormComponents.DisplayRobotTrackCheckBox != null)
                {
                    return WinFormComponents.DisplayRobotTrackCheckBox.Checked;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Map quantisation rate for flood algoritm
        /// </summary>
        protected decimal MapQuantisationRate
        {
            get
            {
                if (WinFormComponents != null && WinFormComponents.MapQuantisationNumBox != null)
                {
                    return WinFormComponents.MapQuantisationNumBox.Value;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Navigation Hint text
        /// </summary>
        protected string NavigationHint
        {
            get
            {
                if (WinFormComponents != null && WinFormComponents.NavigationText != null)
                {
                    return WinFormComponents.NavigationText.Text;
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                if (WinFormComponents != null)
                {
                    WinFormComponents.SetNavigationHint(value);
                }
            }
        }

        /// <summary>
        /// DOUBLE type format for serializing Distance/Position as String
        /// </summary>
        protected string DoubleDistanceFormat
        {
            get
            {
                return DistanceFormat;
            }
        }

        #endregion


        #region Public Accessors

        /// <summary>
        /// Total frames available in current Timeline
        /// </summary>
        public int TotalFrames
        {
            get
            {
                return TimelineData.Last().FrameNo;
            }
        }

        /// <summary>
        /// Total time available in current Timeline
        /// </summary>
        public decimal TotalTime
        {
            get
            {
                return (decimal)TimelineData.Last().Time;
            }
        }

        /// <summary>
        /// Frame rate per second
        /// </summary>
        public int FramesCountInSecond
        {
            get
            {
                return (int)TimelineRateFactor;
            }
        }

        /// <summary>
        /// Decimal places in each second
        /// </summary>
        public int DecimalPlacesForSeconds
        {
            get
            {
                int DecimalPlaces = 0;
                decimal TimeFactor = TimelineRateFactor;
                while ((TimeFactor % 10) == 0)
                {
                    DecimalPlaces++;
                    TimeFactor = TimeFactor / 10;
                }
                return DecimalPlaces;
            }
        }

        /// <summary>
        /// Minimum increment for time
        /// </summary>
        public decimal MinimumSecondsIncrement
        {
            get
            {
                return (decimal)(1 / TimelineRateFactor);
            }
        }

        /// <summary>
        /// Current Frame Number
        /// </summary>
        public int CurrentFrameNumber
        {
            get
            {
                return CurrentFrameNo;
            }
        }

        /// <summary>
        /// Gets Timeline Item by index of sensor data
        /// </summary>
        /// <param name="SensorReadingIdx">Index of sensor data</param>
        /// <returns>Timeline Item frame which contains sensor data with supplied index</returns>
        public int GetFrameNoWithNthSensorData(int SensorReadingIdx)
        {
            return TimelineData.GetFrameNoWithNthSensorData(SensorReadingIdx);
        }

        /// <summary>
        /// Gets Timeline Item by index of scan data
        /// </summary>
        /// <param name="SensorReadingIdx">Index of scan data</param>
        /// <returns>Timeline Item frame which contains scan data with supplied index</returns>
        public int GetFrameNoWithNthScanData(int SensorReadingIdx)
        {
            return TimelineData.GetFrameNoWithNthScanData(SensorReadingIdx);
        }

        /// <summary>
        /// Gets Timeline Item by index of sensor/scan data
        /// </summary>
        /// <param name="DataReadingIdx">Index of sensor/scan data</param>
        /// <returns>Timeline Item frame which contains sensor/scan data with supplied index</returns>
        public int GetFrameNoWithNthAnyData(int DataReadingIdx)
        {
            return TimelineData.GetFrameNoWithNthAnyData(DataReadingIdx);
        }


        #endregion


        #region Constructors

        /// <summary>
        /// Disabled
        /// </summary>
        protected TimelineBase() { }

        /// <summary>
        /// Initialize timeline with supplied Sensor and Laser scanner data
        /// </summary>
        /// <param name="SensorData">Robot sensor data</param>
        /// <param name="ScanData">RPLidar Laser scanner data</param>
        public TimelineBase(RobotSensorDataList SensorData, RPLidarMeasurementList ScanData)
        {
            SensorDataForTimeline = SensorData;
            ScanDataForTimeline = ScanData;
            InitTimeline();
        }

        #endregion


        #region Public functions
        
        /// <summary>
        /// Subscribe Winforms application's components to Timeline
        /// </summary>
        /// <param name="RobotDiameterNumericBox">Numeric Box component with Robot Diameter</param>
        /// <param name="TangentBugDetourNumericBox">Numeric Box component with Detour length for Tangent bug algoritm</param>
        /// <param name="ShowMapCheckBox">Check Box component with Show Map setting</param>
        /// <param name="DisplayRobotTrackCheckBox">Check Box component with Display Robot track setting</param>
        /// <param name="MapQuantisationNumBox">Numeric Box component with Map Quantisation setting</param>
        /// <param name="ImageBox">Picture Box component for drawing</param>
        /// <param name="FrameTextBox">Text Box component for setting current Frame number</param>
        /// <param name="SecondsTextBox">Text Box component for setting current Elapsed Time</param>
        /// <param name="PositionXTextBox">Text Box component for setting current X position coordinate of the robot</param>
        /// <param name="PositionYTextBox">Text Box component for setting current Y position coordinate of the robot</param>
        /// <param name="AngleTextBox">Text Box component for setting current Angle of the robot</param>
        /// <param name="VelocityTextBox">Text Box component for setting current Velocity of the robot</param>
        /// <param name="LastSensorTimeTextBox">Text Box component for setting elapsed time from last sensor data</param>
        /// <param name="SensorListBox">List Box component where Sensor data list will be populated</param>
        /// <param name="LastScanTimeTextBox">Text Box component for setting elapsed time from last laser scan data</param>
        /// <param name="ScanListBox">List Box component where Scan data list will be populated</param>
        /// <param name="LastDataTimeTextBox">Text Box component for setting elapsed time from last sensor/scan data</param>
        /// <param name="DataListBox">List Box component where Sensor and Scan data list will be populated</param>
        /// <param name="NavigationText">Text Box component for setting navigation text</param>
        public void SubscribeComponents(NumericUpDown RobotDiameterNumericBox, NumericUpDown TangentBugDetourNumericBox, CheckBox ShowMapCheckBox, CheckBox DisplayRobotTrackCheckBox, NumericUpDown MapQuantisationNumBox, PictureBox ImageBox, TextBox FrameTextBox, TextBox SecondsTextBox, TextBox PositionXTextBox, TextBox PositionYTextBox, TextBox AngleTextBox, TextBox VelocityTextBox, TextBox LastSensorTimeTextBox, ListBox SensorListBox, TextBox LastScanTimeTextBox, ListBox ScanListBox, TextBox LastDataTimeTextBox, ListBox DataListBox, TextBox NavigationText)
        {
            // Save subscribed components into local variable
            WinFormComponents = new SubscribedComponents(RobotDiameterNumericBox, TangentBugDetourNumericBox, ShowMapCheckBox, DisplayRobotTrackCheckBox, MapQuantisationNumBox, ImageBox, FrameTextBox, SecondsTextBox, PositionXTextBox, PositionYTextBox, AngleTextBox, VelocityTextBox, LastSensorTimeTextBox, SensorListBox, LastScanTimeTextBox, ScanListBox, LastDataTimeTextBox, DataListBox, NavigationText, TimeFormat, DistanceFormat);

            // Publish Sensor data to SensorListBox
            if (SensorListBox != null)
            {
                SensorListBox.Items.Clear();
                TimelineItem[] ItemsWithSensorData = TimelineData.GetAllWithSensorData();
                if (ItemsWithSensorData != null && ItemsWithSensorData.Length > 0)
                {
                    foreach (TimelineItem ItemWithSensorData in ItemsWithSensorData)
                    {
                        SensorListBox.Items.Add(String.Format("Senzor:\tT = [{0}] s\tD = [{1}] mm\tA = [{2}]°", ItemWithSensorData.Time.ToString(TimeFormat), ItemWithSensorData.SensorData.Distance.ToString(DistanceFormat), ItemWithSensorData.SensorData.Angle.ToString()));
                    }
                }
            }
            // Publish Scan data to ScanListBox
            if (ScanListBox != null)
            {
                ScanListBox.Items.Clear();
                TimelineItem[] ItemsWithScanData = TimelineData.GetAllWithScanData();
                if (ItemsWithScanData != null && ItemsWithScanData.Length > 0)
                {
                    foreach (TimelineItem ItemWithScanData in ItemsWithScanData)
                    {
                        ScanListBox.Items.Add(String.Format("Laser:\tT = [{0}] s\tP = [{1}]", ItemWithScanData.Time.ToString(TimeFormat), ItemWithScanData.LaserScans.Length.ToString()));
                    }
                }
            }
            // Publish Sensor/Scan data to DataListBox
            if (DataListBox != null)
            {
                DataListBox.Items.Clear();
                TimelineItem[] ItemsWithData = TimelineData.GetAllWithAnyData();
                if (ItemsWithData != null && ItemsWithData.Length > 0)
                {
                    foreach (TimelineItem ItemWithData in ItemsWithData)
                    {
                        if (ItemWithData.SensorData != null && ItemWithData.LaserScans != null)
                        {
                            DataListBox.Items.Add(String.Format("Oboje:\tT = [{0}] s\tD = [{1}] mm\tA = [{2}]°\tP = [{3}]", ItemWithData.Time.ToString(TimeFormat), ItemWithData.SensorData.Distance.ToString(DistanceFormat), ItemWithData.SensorData.Angle.ToString(), ItemWithData.LaserScans.Length.ToString()));
                        }
                        else if (ItemWithData.SensorData != null && ItemWithData.LaserScans == null)
                        {
                            DataListBox.Items.Add(String.Format("Senzor:\tT = [{0}] s\tD = [{1}] mm\tA = [{2}]°", ItemWithData.Time.ToString(TimeFormat), ItemWithData.SensorData.Distance.ToString(DistanceFormat), ItemWithData.SensorData.Angle.ToString()));
                        }
                        else if (ItemWithData.SensorData == null && ItemWithData.LaserScans != null)
                        {
                            DataListBox.Items.Add(String.Format("Laser:\tT = [{0}] s\tP = [{1}]", ItemWithData.Time.ToString(TimeFormat), ItemWithData.LaserScans.Length.ToString()));
                        }
                    }
                }
            }
            // Subscribe to PictureBox Paint event
            if (ImageBox != null)
            {
                ImageBox.Paint += PaintImageBox;
            }
            // Subscribe to RobotDiameterNumericBox Value changed event
            if (RobotDiameterNumericBox != null)
            {
                RobotDiameterNumericBox.ValueChanged += RobotSettings_ValueChanged;
            }
            // Subscribe to ShowMapCheckBox Value changed event
            if (ShowMapCheckBox != null)
            {
                ShowMapCheckBox.CheckedChanged += RobotSettings_ValueChanged;
            }
            // Subscribe to DisplayRobotTrackCheckBox Value changed event
            if (DisplayRobotTrackCheckBox != null)
            {
                DisplayRobotTrackCheckBox.CheckedChanged += RobotSettings_ValueChanged;
            }
            // Subscribe to MapQuantisationNumBox Value changed event
            if (MapQuantisationNumBox != null)
            {
                MapQuantisationNumBox.ValueChanged += RobotSettings_ValueChanged;
            }
        }

        
        /// <summary>
        /// Unsubscribe Winforms components from current Timeline
        /// </summary>
        public void UnsubscribeComponents()
        {
            // Unpublish SensorListBox
            if (WinFormComponents.SensorListBox != null)
            {
                WinFormComponents.SensorListBox.Items.Clear();
            }
            // Unpublish ScanListBox
            if (WinFormComponents.ScanListBox != null)
            {
                WinFormComponents.ScanListBox.Items.Clear();
            }
            // Unpublish DataListBox
            if (WinFormComponents.DataListBox != null)
            {
                WinFormComponents.DataListBox.Items.Clear();
            }
            // Unsubscribe ImageBox
            if (WinFormComponents.ImageBox != null)
            {
                WinFormComponents.ImageBox.Paint -= PaintImageBox;
            }
            // Unsubscribe to RobotDiameterNumericBox Value changed event
            if (WinFormComponents.RobotDiameterNumericBox != null)
            {
                WinFormComponents.RobotDiameterNumericBox.ValueChanged -= RobotSettings_ValueChanged;
            }
            // Unsubscribe to ShowMapCheckBox Value changed event
            if (WinFormComponents.ShowMapCheckBox != null)
            {
                WinFormComponents.ShowMapCheckBox.CheckedChanged -= RobotSettings_ValueChanged;
            }
            // Unsubscribe to DisplayRobotTrackCheckBox Value changed event
            if (WinFormComponents.DisplayRobotTrackCheckBox != null)
            {
                WinFormComponents.DisplayRobotTrackCheckBox.CheckedChanged -= RobotSettings_ValueChanged;
            }
            // Unsubscribe to MapQuantisationNumBox Value changed event
            if (WinFormComponents.MapQuantisationNumBox != null)
            {
                WinFormComponents.MapQuantisationNumBox.ValueChanged -= RobotSettings_ValueChanged;
            }
            // Free WinFormsComponents
            WinFormComponents = null;
        }
                
        /// <summary>
        /// Go to specified time in current timeline
        /// </summary>
        /// <param name="second">Timeline time</param>
        public void GoToSecond(double second)
        {
            GoToFrame(Time2FrameNo(second));
        }

        /// <summary>
        /// Go to specified frame in current timeline
        /// </summary>
        /// <param name="FrameNo">Frame number in current timeline</param>
        public void GoToFrame(int FrameNo)
        {
            // Save Previous FrameNo in this function
            int PreviousFrameNo = CurrentFrameNo;

            // Set CurrentFrameNo
            CurrentFrameNo = FrameNo;

            // Check for subscribed components
            if (WinFormComponents == null)
                return;

            // Get timeline item
            TimelineItem TimeItem = TimelineData[FrameNo];

            // Update data in components
            WinFormComponents.SetFrame(TimeItem.FrameNo);
            WinFormComponents.SetSeconds(TimeItem.Time);
            WinFormComponents.SetPositionX(TimeItem.PositionX);
            WinFormComponents.SetPositionY(TimeItem.PositionY);
            WinFormComponents.SetVelocity(TimeItem.Velocity);
            WinFormComponents.SetAngle(TimeItem.Phi);
            WinFormComponents.SetLastSensorSeconds(TimeItem.Time - TimelineData.GetLastItemTimeWithSensorData(TimeItem.FrameNo));
            WinFormComponents.SetLastScanSeconds(TimeItem.Time - TimelineData.GetLastItemTimeWithScanData(TimeItem.FrameNo));
            WinFormComponents.SetLastDataSeconds(TimeItem.Time - TimelineData.GetLastItemTimeWithAnyData(TimeItem.FrameNo));

            // Check if ImageBox is subscribed and Start Drawing
            if (WinFormComponents.ImageBox != null)
            {
                if (ShouldInvalidatePictureBoxInNewFrame(PreviousFrameNo, FrameNo))
                {
                    WinFormComponents.ImageBox.Invalidate();
                }                
            }
            
        }
        
        /// <summary>
        /// Get time for specified frame number in timeline
        /// </summary>
        /// <param name="FrameNo">Timeline Frame number</param>
        /// <returns></returns>
        public double FrameNo2Time(int FrameNo)
        {
            return (double)((double)FrameNo / (double)TimelineRateFactor);
        }

        /// <summary>
        /// Get Frame number for specified time in timeline
        /// </summary>
        /// <param name="Time">Timeline time</param>
        /// <returns></returns>
        public int Time2FrameNo(double Time)
        {
            return (int)(Time * (double)TimelineRateFactor);
        }


        #endregion


        #region Protected functions


        /// <summary>
        /// Function indicating that the contents of the image was changed and whether to invalidate PictureBox
        /// </summary>
        /// <returns>TRUE if PictureBox should be invalidated and repainted</returns>
        protected abstract Boolean ShouldInvalidatePictureBoxInNewFrame(int PreviousFrameNo, int NewFrameNo);

        /// <summary>
        /// Function for painting on the PictureBox - this function is meant to be overriden
        /// </summary>
        /// <param name="FrameNo">Number of the frame to paint</param>
        /// <param name="g">GDI Graphics object used for painting</param>
        /// <param name="PictureBoxMaxX">PictureBox width</param>
        /// <param name="PictureBoxMaxY">PictureBox height</param>
        protected abstract void PaintFrameMap(int FrameNo, Graphics g, int PictureBoxMaxX, int PictureBoxMaxY);

        /// <summary>
        /// Invalidate Picture Box contents and forces to repaint
        /// </summary>
        protected void RefreshImageBox()
        {
            if (WinFormComponents != null && WinFormComponents.ImageBox != null)
            {
                WinFormComponents.ImageBox.Invalidate();
            }
        }
        
        /// <summary>
        /// Get Frame number in timeline for specified Timestamp in microseconds
        /// </summary>
        /// <param name="Timestamp">Unix Timestamp in microseconds</param>
        /// <returns></returns>
        protected int Timestamp2FrameNo(int Timestamp)
        {
            return (int)Math.Round((double)Timestamp * 1 / (double)Timestamp2TimeFactor * (double)TimelineRateFactor);
        }

        /// <summary>
        /// Get time in timeline for specified Timestamp in microseconds
        /// </summary>
        /// <param name="Timestamp">Unix Timestamp in microseconds</param>
        /// <returns></returns>
        protected double Timestamp2Time(int Timestamp)
        {
            return Math.Round((double)Timestamp * 1 / (double)Timestamp2TimeFactor * (double)TimelineRateFactor) / (double)TimelineRateFactor;
        }


        #endregion
        

        #region Private functions

        /// <summary>
        /// Paint event handler for PictureBox
        /// </summary>
        /// <param name="sender">Picture box to paint</param>
        /// <param name="e">Paint event arguments</param>
        private void PaintImageBox(object sender, PaintEventArgs e)
        {
            PaintFrameMap(CurrentFrameNo, e.Graphics, ((PictureBox)sender).ClientSize.Width, ((PictureBox)sender).ClientSize.Height);
        }

        /// <summary>
        /// Event handler for Settings components which will invalidate current ImageBox
        /// </summary>
        /// <param name="sender">Component with changed value</param>
        /// <param name="e">Event arguments for changed component</param>
        private void RobotSettings_ValueChanged(object sender, EventArgs e)
        {
            if (WinFormComponents.ImageBox != null)
            {
                WinFormComponents.ImageBox.Invalidate();
            }
        }

        /// <summary>
        /// Initialize timeline
        /// </summary>
        private void InitTimeline()
        {
            // Get maximum timestamp from both files + Transform to seconds with ceil number
            int MaxTimestamp = (SensorDataForTimeline.Max(t => t.Timestamp) < ScanDataForTimeline.Max(t => t.Timestamp)) ? ScanDataForTimeline.Max(t => t.Timestamp) : SensorDataForTimeline.Max(t => t.Timestamp);
            int TimelineMaxSeconds = (int)Math.Ceiling(MaxTimestamp * 1 / Timestamp2TimeFactor);
            // Total number of frames
            int FramesNo = Time2FrameNo(TimelineMaxSeconds);
            // Initialize timeline with all frames
            TimelineData = new TimelineItemList();
            for (int i = 0; i <= FramesNo; i++)
            {
                TimelineItem frame = new TimelineItem(i, FrameNo2Time(i));
                TimelineData.Add(frame);
            }
            // Add robot sensor data to required frames + update Velocity and Phi on the rest
            int LastFrameNo = -1;
            double LastPhi = TimelineData[0].Phi;
            double LastPositionX = TimelineData[0].PositionX;
            double LastPositionY = TimelineData[0].PositionY;
            foreach (RobotSensorData SensorData in SensorDataForTimeline)
            {
                int FrameNo = Timestamp2FrameNo(SensorData.Timestamp);
                TimelineData[FrameNo].SensorData = SensorData;
                LastFrameNo++;
                // Update velocity
                double Velocity = 0;
                double PositionX = LastPositionX;
                double PositionY = LastPositionY;
                if(SensorData.Distance != 0)
                {
                    Velocity = SensorData.Distance / (TimelineData[FrameNo].Time - TimelineData[LastFrameNo].Time);
                    PositionX = LastPositionX + SensorData.Distance * Math.Cos(LastPhi / 180 * Math.PI);
                    PositionY = LastPositionY + SensorData.Distance * Math.Sin(LastPhi / 180 * Math.PI);
                }
                TimelineData[FrameNo].PositionX = PositionX;
                TimelineData[FrameNo].PositionY = PositionY;
                // Get old Phi and update velocity + Phi on all frames from previous reading
                for (int i = LastFrameNo; i <= FrameNo; i++)
                {
                    TimelineData[i].Velocity = Velocity;
                    TimelineData[i].Phi = LastPhi;
                }
                // Update Phi on current frame
                if (SensorData.Angle != 0)
                {
                    TimelineData[FrameNo].Phi += SensorData.Angle;
                }
                LastPhi = TimelineData[FrameNo].Phi;
                LastPositionX = PositionX;
                LastPositionY = PositionY;
                LastFrameNo = FrameNo;
            }
            // Update velocity + Phi from last reading till the last frame
            for (int i = LastFrameNo + 1; i <= FramesNo; i++)
            {
                TimelineData[i].Velocity = 0;
                TimelineData[i].Phi = LastPhi;
                TimelineData[i].PositionX = LastPositionX;
                TimelineData[i].PositionY = LastPositionY;
            }
            // Approximate Robot positions on TimelineItems without Sensor data
            LastFrameNo = 0;
            TimelineItem[] ItemsWithSensorData = TimelineData.GetAllWithSensorData();
            for (int i = 0; i < ItemsWithSensorData.Length; i++)
            {
                double PositionX = ItemsWithSensorData[i].PositionX;
                double PositionY = ItemsWithSensorData[i].PositionY;
                LastPositionX = TimelineData[LastFrameNo].PositionX;
                LastPositionY = TimelineData[LastFrameNo].PositionY;
                double DifferenceX = PositionX - LastPositionX;
                double DifferenceY = PositionY - LastPositionY;
                if (ItemsWithSensorData[i].SensorData.Distance != 0)
                {
                    double angle = Math.Atan2(DifferenceY, DifferenceX) * 180 / Math.PI;
                    double distance = Math.Sqrt(DifferenceX * DifferenceX + DifferenceY * DifferenceY);
                    int frames = ItemsWithSensorData[i].FrameNo - LastFrameNo; //  - 1

                    int frame = 1;
                    for (int j = LastFrameNo + 1; j < ItemsWithSensorData[i].FrameNo; j++)
                    {
                        TimelineData[j].PositionX = LastPositionX + (distance / frames * frame) * Math.Cos(angle / 180 * Math.PI);
                        TimelineData[j].PositionY = LastPositionY + (distance / frames * frame) * Math.Sin(angle / 180 * Math.PI);
                        frame++;
                    }
                }
                else
                {
                    for (int j = LastFrameNo + 1; j < ItemsWithSensorData[i].FrameNo; j++)
                    {
                        TimelineData[j].PositionX = LastPositionX;
                        TimelineData[j].PositionY = LastPositionY;
                    }
                }
                LastFrameNo = ItemsWithSensorData[i].FrameNo;
            }
            // Add RPLidar scan data to required frames
            foreach (RPLidarMeasurement ScanMeasurement in ScanDataForTimeline)
            {
                int FrameNo = Timestamp2FrameNo(ScanMeasurement.Timestamp);
                TimelineData[FrameNo].LaserScans = ScanMeasurement.Scans.ToArray();
            }
        }

        #endregion

    }
}
