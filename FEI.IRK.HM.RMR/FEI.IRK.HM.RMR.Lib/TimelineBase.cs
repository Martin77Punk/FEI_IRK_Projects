using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEI.IRK.HM.RMR.Lib
{
    public abstract class TimelineBase
    {

        private RobotSensorDataList SensorDataForTimeline;
        private RPLidarMeasurementList ScanDataForTimeline;
        private SubscribedComponents WinFormComponents;

        /// <summary>
        /// Factor indicating amount of ONE SECOND in Timestamp value
        /// </summary>
        protected static readonly decimal Timestamp2TimeFactor = 1000000;   // microseconds

        /// <summary>
        /// Factor indicating amount of frames in ONE SECOND
        /// </summary>
        protected static readonly decimal TimelineRateFactor = 100;         // 100 per second


        protected static readonly string TimeFormat = "F2";


        protected static readonly string DistanceFormat = "F3";

        /// <summary>
        /// Timeline Data with time frames
        /// </summary>
        protected TimelineItemList TimelineData;


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



        public int GetTotalFrames()
        {
            return TimelineData.Last().FrameNo;
        }


        public decimal GetTotalTime()
        {
            return (decimal)TimelineData.Last().Time;
        }


        public int GetFramesCountInSecond()
        {
            return (int)TimelineRateFactor;
        }


        public int GetDecimalPlacesForSeconds()
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


        public decimal GetMinimumSecondsIncrement()
        {
            return (decimal)(1 / TimelineRateFactor);
        }


        public void SubscribeComponents(NumericUpDown RobotDiameterNumericBox, CheckBox ShowMapCheckBox, NumericUpDown MapQuantisationNumBox, PictureBox ImageBox, TextBox FrameTextBox, TextBox SecondsTextBox, TextBox PositionXTextBox, TextBox PositionYTextBox, TextBox AngleTextBox, TextBox VelocityTextBox, TextBox LastSensorTimeTextBox, ListBox SensorListBox, TextBox LastScanTimeTextBox, ListBox ScanListBox)
        {
            // Save subscribed components into local variable
            WinFormComponents = new SubscribedComponents(RobotDiameterNumericBox, ShowMapCheckBox, MapQuantisationNumBox, ImageBox, FrameTextBox, SecondsTextBox, PositionXTextBox, PositionYTextBox, AngleTextBox, VelocityTextBox, LastSensorTimeTextBox, SensorListBox, LastScanTimeTextBox, ScanListBox, TimeFormat, DistanceFormat);

            // Publish Sensor data to SensorListBox
            if (SensorListBox != null)
            {
                SensorListBox.Items.Clear();
                TimelineItem[] ItemsWithSensorData = TimelineData.GetAllWithSensorData();
                if (ItemsWithSensorData != null && ItemsWithSensorData.Length > 0)
                {
                    foreach (TimelineItem ItemWithSensorData in ItemsWithSensorData)
                    {
                        SensorListBox.Items.Add(String.Format("T = [{0}] s\tD = [{1}] mm\tA = [{2}]°", ItemWithSensorData.Time.ToString(TimeFormat), ItemWithSensorData.SensorData.Distance.ToString(DistanceFormat), ItemWithSensorData.SensorData.Angle.ToString()));
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
                        ScanListBox.Items.Add(String.Format("T = [{0}] s\tP = [{1}]", ItemWithScanData.Time.ToString(TimeFormat), ItemWithScanData.LaserScans.Length.ToString()));
                    }
                }
            }


        }


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
            // Free WinFormsComponents
            WinFormComponents = null;
        }



        public int GetFrameNoWithNthSensorData(int SensorReadingIdx)
        {
            return TimelineData.GetFrameNoWithNthSensorData(SensorReadingIdx);
        }


        public int GetFrameNoWithNthScanData(int SensorReadingIdx)
        {
            return TimelineData.GetFrameNoWithNthScanData(SensorReadingIdx);
        }


        public void GoToSecond(double second)
        {
            GoToFrame(Time2FrameNo(second));
        }


        public void GoToFrame(int FrameNo)
        {
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

            // Check if ImageBox is subscribed
            if (WinFormComponents.ImageBox == null)
                return;

            // Start Drawing

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
                //LastPhi = TimelineData[LastFrameNo].Phi;
                double PositionX = LastPositionX;
                double PositionY = LastPositionY;
                if(SensorData.Distance != 0)
                {
                    Velocity = SensorData.Distance / (TimelineData[FrameNo].Time - TimelineData[LastFrameNo].Time);
                    PositionX = LastPositionX + SensorData.Distance * Math.Cos(LastPhi / 180 * Math.PI);
                    PositionY = LastPositionY + SensorData.Distance * Math.Sin(LastPhi / 180 * Math.PI);
                    //PositionX = LastPositionX + (TimelineData[FrameNo].Time - TimelineData[LastFrameNo].Time) * Velocity * Math.Cos(LastPhi / 180 * Math.PI);
                    //PositionY = LastPositionY + (TimelineData[FrameNo].Time - TimelineData[LastFrameNo].Time) * Velocity * SensorData.Distance * Math.Sin(LastPhi / 180 * Math.PI);
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
                    double angle = Math.Atan(DifferenceX / DifferenceY) * 180 / Math.PI;
                    double distance = Math.Sqrt(DifferenceX * DifferenceX + DifferenceY * DifferenceY);
                    int frames = ItemsWithSensorData[i].FrameNo - LastFrameNo; //  - 1

                    int frame = 1;
                    for (int j = LastFrameNo + 1; j < ItemsWithSensorData[i].FrameNo; j++)
                    {
                        TimelineData[j].PositionX = LastPositionX + (distance / frames * frame) * Math.Sin(angle / 180 * Math.PI);
                        TimelineData[j].PositionY = LastPositionY + (distance / frames * frame) * Math.Cos(angle / 180 * Math.PI);
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


            //#if DEBUG
            //            LastPhi = TimelineData[0].Phi;
            //            LastPositionX = TimelineData[0].PositionX;
            //            LastPositionY = TimelineData[0].PositionY;
            //            for (int i = 1; i <= FramesNo; i++)
            //            {
            //                if (TimelineData[i].SensorData != null)
            //                {
            //                    // Item without sensor data
            //                    double AllowedDifference = 1.5;
            //                    double DifferenceX = Math.Abs(TimelineData[i].PositionX - TimelineData[i - 1].PositionX);
            //                    double DifferenceY = Math.Abs(TimelineData[i].PositionY - TimelineData[i - 1].PositionY);
            //                    System.Diagnostics.Debug.Assert(DifferenceX < AllowedDifference && DifferenceY < AllowedDifference, String.Format("Approximated: [{0}; {1}], Real: [{2}; {3}], i = {4}", TimelineData[i - 1].PositionX, TimelineData[i - 1].PositionY, TimelineData[i].PositionX, TimelineData[i].PositionY, i));
            //                }
            //                LastPhi = TimelineData[i].Phi;
            //                LastPositionX = TimelineData[i].PositionX;
            //                LastPositionY = TimelineData[i].PositionY;
            //            }
            //#endif

           

            // Add RPLidar scan data to required frames
            foreach (RPLidarMeasurement ScanMeasurement in ScanDataForTimeline)
            {
                int FrameNo = Timestamp2FrameNo(ScanMeasurement.Timestamp);
                TimelineData[FrameNo].LaserScans = ScanMeasurement.Scans.ToArray();
            }


        }

    }
}
