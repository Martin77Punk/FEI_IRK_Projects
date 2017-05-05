using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using FEI.IRK.HM.HMIvR;

namespace FEI.IRK.HM.HMIvR
{
    public class DeviceSensorsImpl : Java.Lang.Object, ISensorEventListener, IDeviceSensors
    {
        public event CompassDataReading CompassDataChanged;
        public event LightLevelReading LightLevelDataChanged;
        public event PressureReading PressureDataChanged;
        public event AccelerometerDataReading AccelerometerDataChanged;
        public event GyroscopeDataReading GyroscopeDataChanged;

        private SensorManager sensorManager;
        private Sensor sensorAccelerometer;
        private Sensor sensorGyroscope;
        private Sensor sensorCompass;
        private Sensor sensorLight;
        private Sensor sensorPressure;


        public DeviceSensorsImpl()
        {
            sensorManager = (SensorManager)Application.Context.GetSystemService(Context.SensorService);
            sensorAccelerometer = sensorManager.GetDefaultSensor(SensorType.Accelerometer);
            sensorGyroscope = sensorManager.GetDefaultSensor(SensorType.Gyroscope);
            sensorCompass = sensorManager.GetDefaultSensor(SensorType.Orientation);
            sensorLight = sensorManager.GetDefaultSensor(SensorType.Light);
            sensorPressure = sensorManager.GetDefaultSensor(SensorType.Pressure);
        }

        public void StartSensorsReading()
        {
            SensorDelay delay = SensorDelay.Ui;
            if (sensorAccelerometer != null) sensorManager.RegisterListener(this, sensorAccelerometer, delay);
            if (sensorGyroscope != null) sensorManager.RegisterListener(this, sensorGyroscope, delay);
            if (sensorCompass != null) sensorManager.RegisterListener(this, sensorCompass, delay);
            if (sensorLight != null) sensorManager.RegisterListener(this, sensorLight, delay);
            if (sensorPressure != null) sensorManager.RegisterListener(this, sensorPressure, delay);
        }

        public void StopSensorsReading()
        {
            if (sensorAccelerometer != null) sensorManager.UnregisterListener(this, sensorAccelerometer);
            if (sensorGyroscope != null) sensorManager.UnregisterListener(this, sensorGyroscope);
            if (sensorCompass != null) sensorManager.UnregisterListener(this, sensorCompass);
            if (sensorLight != null) sensorManager.UnregisterListener(this, sensorLight);
            if (sensorPressure != null) sensorManager.UnregisterListener(this, sensorPressure);
        }

        public SensorDevice GetSensorsType()
        {
            return SensorDevice.Android;
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            switch(e.Sensor.Type)
            {
                case SensorType.Accelerometer:
                    if (AccelerometerDataChanged != null)
                    {
                        AccelerometerDataChanged(e.Values[0], e.Values[1], e.Values[2]);
                    }
                    break;
                case SensorType.Gyroscope:
                    if (GyroscopeDataChanged != null)
                    {
                        GyroscopeDataChanged(e.Values[0], e.Values[1], e.Values[2]);
                    }
                    break;
                case SensorType.Orientation:
                    if (CompassDataChanged != null)
                    {
                        CompassDataChanged(e.Values[0]);
                    }
                    break;
                case SensorType.Light:
                    if (LightLevelDataChanged != null)
                    {
                        LightLevelDataChanged(e.Values[0]);
                    }
                    break;
                case SensorType.Pressure:
                    if (PressureDataChanged != null)
                    {
                        PressureDataChanged(e.Values[0]);
                    }
                    break;

            }
        }

    }
}