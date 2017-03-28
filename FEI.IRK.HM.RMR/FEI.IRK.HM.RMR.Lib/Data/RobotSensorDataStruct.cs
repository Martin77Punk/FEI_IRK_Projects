using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    public struct RobotSensorDataStruct
    {
        public int Timestamp;
        public byte WheelpdropCaster;          //0-1
        public byte WheelpdropLeft;            //0-1
        public byte WheelpdropRight;           //0-1
        public byte BumpLeft;                  //0-1
        public byte BumpRight;                 //0-1
        public byte Wall;                      //0-1
        public byte CliffLeft;                 //0-1
        public byte CliffFrontLeft;            //0-1
        public byte CliffFrontRight;           //0-1
        public byte CliffRight;                //0-1
        public byte VirtualWall;               //0-1
        public byte LSD0overcurrent;           //0-1
        public byte LSD1overcurrent;           //0-1
        public byte LSD2overcurrent;           //0-1
        public byte RightWheelovercurrent;     //0-1
        public byte LeftWheelovercurrent;      //0-1
        public byte IRbyte;                    //0-255
        public byte PlayPressed;               //0-1
        public byte AdvancePressed;            //0-1
        public short Distance;                 //-32768-32767 mm
        public short Angle;                    //-32768-32767 degree
        public byte ChargingState;             //0-5
        public ushort Voltage;                 //0-65535 mV
        public short Current;                  //-32768-32767
        public sbyte BatteryTemperature;       //-128 -127
        public ushort BatteryCharge;           //0-65535 mAh
        public ushort BatteryCapacity;         //0-65535 mAh
        public ushort WallSignal;              //0-4095
        public ushort CliffLeftSignal;         //0-4095
        public ushort CliffFrontLeftSignal;    //0-4095
        public ushort CliffFrontRightSignal;   //0-4095
        public ushort CliffRightSignal;        //0-4095
        public byte CargoBayDigitalInput0;     //0-1
        public byte CargoBayDigitalInput1;     //0-1
        public byte CargoBayDigitalInput2;     //0-1
        public byte CargoBayDigitalInput3;     //0-1
        public byte DeviceDetect_BaudRateChange;//0-1
        public ushort CargoBayAnalogSignal;    //0-1023
        public byte InternalCharger;           //0-1
        public byte HomaBaseCharger;           //0-1
        public byte OImode;                    //0-3
        public byte SongNumber;                //0-15
        public byte SongPlaying;               //0-1
        public byte NumberOfStreamPackets;     //0-43
        public short RequestedVelocity;        //-500-500 mm/s
        public short RequestedRadius;          //-32768-32767 mm
        public short RequestedRightVelocity;   //-500-500 mm/s
        public short RequestedLeftVelocity;    //-500-500mm/s
    }
}
