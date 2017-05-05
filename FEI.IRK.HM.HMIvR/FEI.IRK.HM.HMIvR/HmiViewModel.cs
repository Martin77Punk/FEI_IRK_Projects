using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace FEI.IRK.HM.HMIvR
{
    public class HmiViewModel : INotifyPropertyChanged
    {

        public static readonly double AccelerometerPix = 40;
        public static readonly double GyroscopePix = 60;

        private bool _CompassVisible = false;
        private double _CompassRotation = 0;

        private bool _AccelerometerVisible = false;
        private ImageSource _AccelerometerImage = ImageSource.FromFile("accel_green.png");
        private Rectangle _AccelerometerBounds = new Rectangle(0.5, 0.5, AccelerometerPix, AccelerometerPix);

        private bool _GyroscopeVisible = false;
        private Rectangle _GyroscopeBounds = new Rectangle(0.5, 0.5, GyroscopePix, GyroscopePix);

        private bool _LightLevelVisible = false;
        private double _LightLevelValue = 0.5;

        private bool _PressureVisible = false;
        private string _PressureText = "";

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }



        public double CompassRotation
        {
            get
            {
                return _CompassRotation;
            }
            set
            {
                _CompassRotation = value;
                OnPropertyChanged();
            }
        }

        public bool CompassVisible
        {
            get
            {
                return _CompassVisible;
            }
            set
            {
                _CompassVisible = value;
                OnPropertyChanged();
            }
        }


        public bool AccelerometerVisible
        {
            get
            {
                return _AccelerometerVisible;
            }
            set
            {
                _AccelerometerVisible = value;
                OnPropertyChanged();
            }
        }

        public ImageSource AccelerometerImage
        {
            get
            {
                return _AccelerometerImage;
            }
            set
            {
                _AccelerometerImage = value;
                OnPropertyChanged();
            }
        }

        public Rectangle AccelerometerBounds
        {
            get
            {
                return _AccelerometerBounds;
            }
            set
            {
                _AccelerometerBounds = value;
                OnPropertyChanged();
            }
        }


        public bool GyroscopeVisible
        {
            get
            {
                return _GyroscopeVisible;
            }
            set
            {
                _GyroscopeVisible = value;
                OnPropertyChanged();
            }
        }


        public Rectangle GyroscopeBounds
        {
            get
            {
                return _GyroscopeBounds;
            }
            set
            {
                _GyroscopeBounds = value;
                OnPropertyChanged();
            }
        }


        public bool LightLevelVisible
        {
            get
            {
                return _LightLevelVisible;
            }
            set
            {
                _LightLevelVisible = value;
                OnPropertyChanged();
            }
        }


        public double LightLevelValue
        {
            get
            {
                return _LightLevelValue;
            }
            set
            {
                _LightLevelValue = value;
                OnPropertyChanged();
            }
        }


        public bool PressureVisible
        {
            get
            {
                return _PressureVisible;
            }
            set
            {
                _PressureVisible = value;
                OnPropertyChanged();
            }
        }


        public string PressureText
        {
            get
            {
                return _PressureText;
            }
            set
            {
                _PressureText = value;
                OnPropertyChanged();
            }
        }

    }
}
