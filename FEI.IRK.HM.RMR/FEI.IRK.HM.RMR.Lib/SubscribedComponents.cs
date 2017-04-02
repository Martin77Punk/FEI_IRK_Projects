using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEI.IRK.HM.RMR.Lib
{
    public class SubscribedComponents
    {

        #region Private variables

        private NumericUpDown robotDiameterNumericBox;
        private NumericUpDown tangentBugDetourNumericBox;
        private CheckBox showMapCheckBox;
        private CheckBox displayRobotTrackCheckBox;
        private NumericUpDown mapQuantisationNumBox;
        private PictureBox imageBox;
        private TextBox frameTextBox;
        private TextBox secondsTextBox;
        private TextBox positionXTextBox;
        private TextBox positionYTextBox;
        private TextBox angleTextBox;
        private TextBox velocityTextBox;
        private TextBox lastSensorTimeTextBox;
        private ListBox sensorListBox;
        private TextBox lastScanTimeTextBox;
        private ListBox scanListBox;
        private TextBox lastDataTimeTextBox;
        private ListBox dataListBox;
        private TextBox navigationText;
        private string timeFormat;
        private string distanceFormat;

        #endregion


        #region Public accessors

        /// <summary>
        /// Numeric Box component with Robot Diameter
        /// </summary>
        public NumericUpDown RobotDiameterNumericBox
        {
            get
            {
                return robotDiameterNumericBox;
            }
        }

        /// <summary>
        /// Numeric Box component with Detour length for Tangent bug algoritm
        /// </summary>
        public NumericUpDown TangentBugDetourNumericBox
        {
            get
            {
                return tangentBugDetourNumericBox;
            }
        }

        /// <summary>
        /// Check Box component with Show Map setting
        /// </summary>
        public CheckBox ShowMapCheckBox
        {
            get
            {
                return showMapCheckBox;
            }
        }

        /// <summary>
        /// Check Box component with Display Robot track setting
        /// </summary>
        public CheckBox DisplayRobotTrackCheckBox
        {
            get
            {
                return displayRobotTrackCheckBox;
            }
        }

        /// <summary>
        /// Numeric Box component with Map Quantisation setting
        /// </summary>
        public NumericUpDown MapQuantisationNumBox
        {
            get
            {
                return mapQuantisationNumBox;
            }
        }
        
        /// <summary>
        /// Picture Box component for drawing
        /// </summary>
        public PictureBox ImageBox
        {
            get
            {
                return imageBox;
            }
        }
        
        /// <summary>
        /// Text Box component for setting current Frame number
        /// </summary>
        public TextBox FrameTextBox
        {
            get
            {
                return FrameTextBox;
            }
        }
        
        /// <summary>
        /// Text Box component for setting current Elapsed Time
        /// </summary>
        public TextBox SecondsTextBox
        {
            get
            {
                return secondsTextBox;
            }
        }
        
        /// <summary>
        /// Text Box component for setting current X position coordinate of the robot
        /// </summary>
        public TextBox PositionXTextBox
        {
            get
            {
                return positionXTextBox;
            }
        }
        
        /// <summary>
        /// Text Box component for setting current Y position coordinate of the robot
        /// </summary>
        public TextBox PositionYTextBox
        {
            get
            {
                return positionYTextBox;
            }
        }
        
        /// <summary>
        /// Text Box component for setting current Angle of the robot
        /// </summary>
        public TextBox AngleTextBox
        {
            get
            {
                return angleTextBox;
            }
        }
        
        /// <summary>
        /// Text Box component for setting current Velocity of the robot
        /// </summary>
        public TextBox VelocityTextBox
        {
            get
            {
                return velocityTextBox;
            }
        }
        
        /// <summary>
        /// Text Box component for setting elapsed time from last sensor data
        /// </summary>
        public TextBox LastSensorTimeTextBox
        {
            get
            {
                return lastSensorTimeTextBox;
            }
        }
        
        /// <summary>
        /// List Box component with populated Sensor data list
        /// </summary>
        public ListBox SensorListBox
        {
            get
            {
                return sensorListBox;
            }
        }
        
        /// <summary>
        /// Text Box component for setting elapsed time from last laser scan data
        /// </summary>
        public TextBox LastScanTimeTextBox
        {
            get
            {
                return lastScanTimeTextBox;
            }
        }
        
        /// <summary>
        /// List Box component with populated Scan data list
        /// </summary>
        public ListBox ScanListBox
        {
            get
            {
                return scanListBox;
            }
        }

        /// <summary>
        /// Text Box component for setting elapsed time from last sensor/scan data
        /// </summary>
        public TextBox LastDataTimeTextBox
        {
            get
            {
                return lastDataTimeTextBox;
            }
        }

        /// <summary>
        /// List Box component where Sensor and Scan data list will be populated
        /// </summary>
        public ListBox DataListBox
        {
            get
            {
                return dataListBox;
            }
        }

        /// <summary>
        /// Text Box component for setting navigation text
        /// </summary>
        public TextBox NavigationText
        {
            get
            {
                return navigationText;
            }
        }

        #endregion


        #region Public functions

        /// <summary>
        /// Update Frame number in designated Windorms component
        /// </summary>
        /// <param name="FrameNo">Current Frame number</param>
        public void SetFrame(int FrameNo)
        {
            if (frameTextBox != null)
            {
                frameTextBox.Text = FrameNo.ToString();
            }
        }

        /// <summary>
        /// Update Time in designated Windorms component
        /// </summary>
        /// <param name="Seconds">Current time in seconds</param>
        public void SetSeconds(double Seconds)
        {
            if (secondsTextBox != null)
            {
                secondsTextBox.Text = Seconds.ToString(timeFormat);
            }
        }

        /// <summary>
        /// Update Robot's X position in designated Windorms component
        /// </summary>
        /// <param name="PositionX">Coordinate X of robot position</param>
        public void SetPositionX(double PositionX)
        {
            if (PositionXTextBox != null)
            {
                PositionXTextBox.Text = PositionX.ToString(distanceFormat);
            }
        }

        /// <summary>
        /// Update Robot's Y position in designated Windorms component
        /// </summary>
        /// <param name="PositionY">Coordinate Y of robot position</param>
        public void SetPositionY(double PositionY)
        {
            if (PositionYTextBox != null)
            {
                PositionYTextBox.Text = PositionY.ToString(distanceFormat);
            }
        }

        /// <summary>
        /// Update Robot's Velocity in designated Windorms component
        /// </summary>
        /// <param name="Velocity">Robot's Velocity value</param>
        public void SetVelocity(double Velocity)
        {
            if (VelocityTextBox != null)
            {
                VelocityTextBox.Text = Velocity.ToString(distanceFormat);
            }
        }

        /// <summary>
        /// Update Robot's Angle in designated Windorms component
        /// </summary>
        /// <param name="Angle">Robot's Angle value</param>
        public void SetAngle(double Angle)
        {
            if (AngleTextBox != null)
            {
                int angle = (int)Angle;
                if (angle < 0)
                {
                    while (angle < 0) angle = angle + 360;
                }
                if (angle > 360)
                {
                    while (angle > 360) angle = angle - 360;
                }
                AngleTextBox.Text = angle.ToString();
            }
        }

        /// <summary>
        /// Update Last sensor time difference in designated Windorms component
        /// </summary>
        /// <param name="Seconds">Time in seconds where last robot's sensor data occured</param>
        public void SetLastSensorSeconds(double Seconds)
        {
            if (lastSensorTimeTextBox != null)
            {
                lastSensorTimeTextBox.Text = Seconds.ToString(timeFormat);
            }
        }

        /// <summary>
        /// Update Last scan time difference in designated Windorms component
        /// </summary>
        /// <param name="Seconds">Time in seconds where last robot's scan data occured</param>
        public void SetLastScanSeconds(double Seconds)
        {
            if (lastScanTimeTextBox != null)
            {
                lastScanTimeTextBox.Text = Seconds.ToString(timeFormat);
            }
        }

        /// <summary>
        /// Update Last sensor/scan time difference in designated Windorms component
        /// </summary>
        /// <param name="Seconds">Time in seconds where last robot's sensor/scan data occured</param>
        public void SetLastDataSeconds(double Seconds)
        {
            if (lastDataTimeTextBox != null)
            {
                lastDataTimeTextBox.Text = Seconds.ToString(timeFormat);
            }
        }

        /// <summary>
        /// Update Navigation Hint text in designated Windorms component
        /// </summary>
        /// <param name="NavigationHint">Navigation Hint text</param>
        public void SetNavigationHint(string NavigationHint)
        {
            if (navigationText != null)
            {
                navigationText.Text = NavigationHint;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Initiate Subscribed Components class for maintaining data in subscribed Winforms components
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
        /// <param name="TimeFormat">DOUBLE type format for serializing Time as String</param>
        /// <param name="DistanceFormat">DOUBLE type format for serializing Distance/Position as String</param>
        public SubscribedComponents(NumericUpDown RobotDiameterNumericBox, NumericUpDown TangentBugDetourNumericBox, CheckBox ShowMapCheckBox, CheckBox DisplayRobotTrackCheckBox, NumericUpDown MapQuantisationNumBox, PictureBox ImageBox, TextBox FrameTextBox, TextBox SecondsTextBox, TextBox PositionXTextBox, TextBox PositionYTextBox, TextBox AngleTextBox, TextBox VelocityTextBox, TextBox LastSensorTimeTextBox, ListBox SensorListBox, TextBox LastScanTimeTextBox, ListBox ScanListBox, TextBox LastDataTimeTextBox, ListBox DataListBox, TextBox NavigationText, string TimeFormat, string DistanceFormat)
        {
            this.robotDiameterNumericBox = RobotDiameterNumericBox;
            this.tangentBugDetourNumericBox = TangentBugDetourNumericBox;
            this.showMapCheckBox = ShowMapCheckBox;
            this.displayRobotTrackCheckBox = DisplayRobotTrackCheckBox;
            this.mapQuantisationNumBox = MapQuantisationNumBox;
            this.imageBox = ImageBox;
            this.frameTextBox = FrameTextBox;
            this.secondsTextBox = SecondsTextBox;
            this.positionXTextBox = PositionXTextBox;
            this.positionYTextBox = PositionYTextBox;
            this.angleTextBox = AngleTextBox;
            this.velocityTextBox = VelocityTextBox;
            this.lastSensorTimeTextBox = LastSensorTimeTextBox;
            this.sensorListBox = SensorListBox;
            this.lastScanTimeTextBox = LastScanTimeTextBox;
            this.scanListBox = ScanListBox;
            this.lastDataTimeTextBox = LastDataTimeTextBox;
            this.dataListBox = DataListBox;
            this.navigationText = NavigationText;
            this.timeFormat = TimeFormat;
            this.distanceFormat = DistanceFormat;
        }

        #endregion

    }
}
