using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;

namespace FEI.IRK.HM.HMIvR
{
    public class DeviceSensorsImpl : IDeviceSensors
    {
        public event CompassDataReading CompassDataChanged;
        public event LightLevelReading LightLevelDataChanged;
        public event PressureReading PressureDataChanged;
        public event AccelerometerDataReading AccelerometerDataChanged;
        public event GyroscopeDataReading GyroscopeDataChanged;


        private Accelerometer accelerometer;
        private Gyrometer gyrometer;
        private Compass compass;
        private LightSensor lightsensor;
        private Barometer barometer;

        private static readonly uint BaseReportInterval = 50;


        public DeviceSensorsImpl()
        {
            // Get Accelerometer
            try
            {
                accelerometer = Accelerometer.GetDefault();
            }
            catch
            {
                accelerometer = null;
            }
            // Get Gyrometer
            try
            {
                gyrometer = Gyrometer.GetDefault();
            }
            catch
            {
                gyrometer = null;
            }
            // Get Compass
            try
            {
                compass = Compass.GetDefault();
            }
            catch
            {
                compass = null;
            }
            // Get LightSensor
            try
            {
                lightsensor = LightSensor.GetDefault();
            }
            catch
            {
                lightsensor = null;
            }
            // Get Barometer
            try
            {
                barometer = Barometer.GetDefault();
            }
            catch
            {
                barometer = null;
            }
        }


        public void StartSensorsReading()
        {
            if (accelerometer != null)
            {
                accelerometer.ReportInterval = (accelerometer.MinimumReportInterval > BaseReportInterval) ? accelerometer.MinimumReportInterval : BaseReportInterval;
                accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            }
            if (gyrometer != null)
            {
                gyrometer.ReportInterval = (gyrometer.MinimumReportInterval > BaseReportInterval) ? gyrometer.MinimumReportInterval : BaseReportInterval;
                gyrometer.ReadingChanged += Gyrometer_ReadingChanged;
            }
            if (compass != null)
            {
                compass.ReportInterval = (compass.MinimumReportInterval > BaseReportInterval) ? compass.MinimumReportInterval : BaseReportInterval;
                compass.ReadingChanged += Compass_ReadingChanged;
            }
            if (lightsensor != null)
            {
                lightsensor.ReportInterval = (lightsensor.MinimumReportInterval > BaseReportInterval) ? lightsensor.MinimumReportInterval : BaseReportInterval;
                lightsensor.ReadingChanged += Lightsensor_ReadingChanged;
            }
            if (barometer != null)
            {
                barometer.ReportInterval = (barometer.MinimumReportInterval > BaseReportInterval) ? barometer.MinimumReportInterval : BaseReportInterval;
                barometer.ReadingChanged += Barometer_ReadingChanged;
            }
        }

        public void StopSensorsReading()
        {
            if (accelerometer != null)
            {
                accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            }
            if (gyrometer != null)
            {
                gyrometer.ReadingChanged -= Gyrometer_ReadingChanged;
            }
            if (compass != null)
            {
                compass.ReadingChanged -= Compass_ReadingChanged;
            }
            if (lightsensor != null)
            {
                lightsensor.ReadingChanged -= Lightsensor_ReadingChanged;
            }
            if (barometer != null)
            {
                barometer.ReadingChanged -= Barometer_ReadingChanged;
            }
        }

        public SensorDevice GetSensorsType()
        {
            return SensorDevice.UWP;
        }

        private void Accelerometer_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            if (AccelerometerDataChanged != null)
            {
                AccelerometerDataChanged(args.Reading.AccelerationX, args.Reading.AccelerationY, args.Reading.AccelerationZ);
            }
        }

        private void Gyrometer_ReadingChanged(Gyrometer sender, GyrometerReadingChangedEventArgs args)
        {
            if (GyroscopeDataChanged != null)
            {
                GyroscopeDataChanged(args.Reading.AngularVelocityX, args.Reading.AngularVelocityY, args.Reading.AngularVelocityZ);
            }
        }

        private void Compass_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
            if (CompassDataChanged != null && args.Reading.HeadingTrueNorth != null && args.Reading.HeadingTrueNorth.HasValue)
            {
                CompassDataChanged(args.Reading.HeadingTrueNorth.Value);
            }
        }
        
        private void Lightsensor_ReadingChanged(LightSensor sender, LightSensorReadingChangedEventArgs args)
        {
            if (LightLevelDataChanged != null)
            {
                LightLevelDataChanged(args.Reading.IlluminanceInLux);
            }
        }

        private void Barometer_ReadingChanged(Barometer sender, BarometerReadingChangedEventArgs args)
        {
            if (PressureDataChanged != null)
            {
                PressureDataChanged(args.Reading.StationPressureInHectopascals);
            }
        }


        private bool disposed = false;

        ~DeviceSensorsImpl()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose only
                }

                disposed = true;
            }
        }

    }
}
