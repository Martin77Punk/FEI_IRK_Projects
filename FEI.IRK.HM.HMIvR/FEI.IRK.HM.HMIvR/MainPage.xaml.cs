using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FEI.IRK.HM.HMIvR
{
    public partial class MainPage : ContentPage
    {

        private HmiViewModel PageProperties;
        private IDeviceSensors Sensors;
        private bool IsUpside = true;


        public MainPage()
        {
            InitializeComponent();
            InitDataBinding();
            InitSensors();        
            
        }

        private void InitDataBinding()
        {
            PageProperties = new HmiViewModel();
            this.BindingContext = PageProperties;
        }

        private void InitSensors()
        {
            Sensors = DeviceSensors.Current;
            Sensors.CompassDataChanged += Sensors_CompassDataChanged;
            Sensors.LightLevelDataChanged += Sensors_LightLevelDataChanged;
            Sensors.PressureDataChanged += Sensors_PressureDataChanged;
            Sensors.AccelerometerDataChanged += Sensors_AccelerometerDataChanged;
            Sensors.GyroscopeDataChanged += Sensors_GyroscopeDataChanged;
        }

        private void Sensors_CompassDataChanged(double Azimuth)
        {
            if (!PageProperties.CompassVisible)
            {
                PageProperties.CompassVisible = true;
            }
            if (IsUpside)
            {
                PageProperties.CompassRotation = 360 - Azimuth;
            }
            else
            {
                PageProperties.CompassRotation = Azimuth;
            }
            
        }

        private void Sensors_LightLevelDataChanged(double AmbientLight)
        {
            if(!PageProperties.LightLevelVisible)
            {
                PageProperties.LightLevelVisible = true;
            }
            double DeviceMultiplier = 0;
            if (Sensors.GetSensorsType() == SensorDevice.Android)
            {
                // Android
                DeviceMultiplier = 100;
            }
            else
            {
                // UWP
                DeviceMultiplier = 200;
            }
            PageProperties.LightLevelValue = AmbientLight / DeviceMultiplier;
        }

        private void Sensors_PressureDataChanged(double AtmosfericPressure)
        {
            if (!PageProperties.PressureVisible)
            {
                PageProperties.PressureVisible = true;
            }
            PageProperties.PressureText = String.Format("Tlak: {0} hPa", AtmosfericPressure.ToString("F3"));
        }

        private void Sensors_AccelerometerDataChanged(double X, double Y, double Z)
        {            
            if (!PageProperties.AccelerometerVisible)
            {
                PageProperties.AccelerometerVisible = true;
            }

            double AccelX = 0;
            double AccelY = 0;
            double AccelPix = HmiViewModel.AccelerometerPix;
            string AccelImage;

            if (Sensors.GetSensorsType() == SensorDevice.Android)
            {
                // Android
                AccelX = 0.5 - (0.5 / 10 * X);
                AccelY = 0.5 + (0.5 / 10 * Y);
                if (Z > 0)
                {
                    AccelImage = "accel_green.png";
                    IsUpside = true;
                }
                else
                {
                    AccelImage = "accel_red.png";
                    IsUpside = false;
                }
            }
            else
            {
                // UWP
                AccelX = 0.5 + (0.5 * X);
                AccelY = 0.5 - (0.5 * Y);
                if (Z > 0)
                {
                    AccelImage = "accel_red.png";
                    IsUpside = false;
                }
                else
                {
                    AccelImage = "accel_green.png";
                    IsUpside = true;
                }
            }

            PageProperties.AccelerometerBounds = new Rectangle(AccelX, AccelY, AccelPix, AccelPix);
            if (((FileImageSource)PageProperties.AccelerometerImage).File != AccelImage)
            {
                PageProperties.AccelerometerImage = ImageSource.FromFile(AccelImage);
            }
        }

        private void Sensors_GyroscopeDataChanged(double X, double Y, double Z)
        {

            if (!PageProperties.GyroscopeVisible)
            {
                PageProperties.GyroscopeVisible = true;
            }

            double GyroX = 0;
            double GyroY = 0;
            double GyroPix = HmiViewModel.GyroscopePix;

            if (Sensors.GetSensorsType() == SensorDevice.Android)
            {
                // Android
                GyroX = 0.5 + (0.5 / 10 * Y);
                GyroY = 0.5 + (0.5 / 10 * X);
                if (!IsUpside)
                {
                    GyroX = 0.5 - (0.5 / 10 * Y);
                    GyroY = 0.5 - (0.5 / 10 * X);
                }
            }
            else
            {
                // UWP
                GyroX = 0.5 + (0.5 / 360 * Y);
                GyroY = 0.5 + (0.5 / 360 * X);
                if (!IsUpside)
                {
                    GyroX = 0.5 - (0.5 / 360 * Y);
                    GyroY = 0.5 - (0.5 / 360 * X);
                }
            }

            PageProperties.GyroscopeBounds = new Rectangle(GyroX, GyroY, GyroPix, GyroPix);
        }

        
        
    }
}
