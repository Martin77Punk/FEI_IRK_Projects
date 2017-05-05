using System;
using System.Collections.Generic;
using System.Text;

namespace FEI.IRK.HM.HMIvR
{
    public interface IDeviceSensors : IDisposable
    {
        void StartSensorsReading();
        void StopSensorsReading();
        SensorDevice GetSensorsType();
        event CompassDataReading CompassDataChanged;
        event LightLevelReading LightLevelDataChanged;
        event PressureReading PressureDataChanged;
        event AccelerometerDataReading AccelerometerDataChanged;
        event GyroscopeDataReading GyroscopeDataChanged;
    }

    public enum SensorDevice
    {
        Android,
        UWP
    }

    public delegate void CompassDataReading(double Azimuth);
    public delegate void LightLevelReading(double AmbientLight);
    public delegate void PressureReading(double AtmosfericPressure);
    public delegate void AccelerometerDataReading(double X, double Y, double Z);
    public delegate void GyroscopeDataReading(double X, double Y, double Z);
}
