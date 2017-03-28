using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEI.IRK.HM.RMR.Lib
{
    class SubscribedComponents
    {
        private NumericUpDown robotDiameterNumericBox;
        private CheckBox showMapCheckBox;
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

        private string timeFormat;
        private string distanceFormat;


        /// <summary>
        /// Winforms component with
        /// </summary>
        public NumericUpDown RobotDiameterNumericBox
        {
            get
            {
                return robotDiameterNumericBox;
            }
        }



        public CheckBox ShowMapCheckBox
        {
            get
            {
                return showMapCheckBox;
            }
        }



        public NumericUpDown MapQuantisationNumBox
        {
            get
            {
                return mapQuantisationNumBox;
            }
        }



        public PictureBox ImageBox
        {
            get
            {
                return imageBox;
            }
        }




        public TextBox FrameTextBox
        {
            get
            {
                return FrameTextBox;
            }
        }



        public TextBox SecondsTextBox
        {
            get
            {
                return secondsTextBox;
            }
        }



        public TextBox PositionXTextBox
        {
            get
            {
                return positionXTextBox;
            }
        }



        public TextBox PositionYTextBox
        {
            get
            {
                return positionYTextBox;
            }
        }



        public TextBox AngleTextBox
        {
            get
            {
                return angleTextBox;
            }
        }



        public TextBox VelocityTextBox
        {
            get
            {
                return velocityTextBox;
            }
        }



        public TextBox LastSensorTimeTextBox
        {
            get
            {
                return lastSensorTimeTextBox;
            }
        }



        public ListBox SensorListBox
        {
            get
            {
                return sensorListBox;
            }
        }



        public TextBox LastScanTimeTextBox
        {
            get
            {
                return lastScanTimeTextBox;
            }
        }



        public ListBox ScanListBox
        {
            get
            {
                return scanListBox;
            }
        }



        public void SetFrame(int FrameNo)
        {
            if (frameTextBox != null)
            {
                frameTextBox.Text = FrameNo.ToString();
            }
        }


        public void SetSeconds(double Seconds)
        {
            if (secondsTextBox != null)
            {
                secondsTextBox.Text = Seconds.ToString(timeFormat);
            }
        }


        public void SetPositionX(double PositionX)
        {
            if (PositionXTextBox != null)
            {
                PositionXTextBox.Text = PositionX.ToString(distanceFormat);
            }
        }


        public void SetPositionY(double PositionY)
        {
            if (PositionYTextBox != null)
            {
                PositionYTextBox.Text = PositionY.ToString(distanceFormat);
            }
        }


        public void SetVelocity(double Velocity)
        {
            if (VelocityTextBox != null)
            {
                VelocityTextBox.Text = Velocity.ToString(distanceFormat);
            }
        }


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


        public void SetLastSensorSeconds(double Seconds)
        {
            if (lastSensorTimeTextBox != null)
            {
                lastSensorTimeTextBox.Text = Seconds.ToString(timeFormat);
            }
        }


        public void SetLastScanSeconds(double Seconds)
        {
            if (lastScanTimeTextBox != null)
            {
                lastScanTimeTextBox.Text = Seconds.ToString(timeFormat);
            }
        }


        public SubscribedComponents(NumericUpDown RobotDiameterNumericBox, CheckBox ShowMapCheckBox, NumericUpDown MapQuantisationNumBox, PictureBox ImageBox, TextBox FrameTextBox, TextBox SecondsTextBox, TextBox PositionXTextBox, TextBox PositionYTextBox, TextBox AngleTextBox, TextBox VelocityTextBox, TextBox LastSensorTimeTextBox, ListBox SensorListBox, TextBox LastScanTimeTextBox, ListBox ScanListBox, string TimeFormat, string DistanceFormat)
        {
            this.robotDiameterNumericBox = RobotDiameterNumericBox;
            this.showMapCheckBox = ShowMapCheckBox;
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

            this.timeFormat = TimeFormat;
            this.distanceFormat = DistanceFormat;
        }

    }
}
