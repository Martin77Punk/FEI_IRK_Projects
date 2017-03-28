using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    public class TimelineItem
    {

        private int frameNo;
        private double time;
        private double velocity;
        private double phi;
        private double positionX;
        private double positionY;
        private RobotSensorData sensorData;
        private RPLidarScan[] laserScans;


        /// <summary>
        /// Number of current frame
        /// </summary>
        public int FrameNo
        {
            get
            {
                return frameNo;
            }
        }


        /// <summary>
        /// Time for current frame
        /// </summary>
        public double Time
        {
            get
            {
                return time;
            }
        }


        /// <summary>
        /// Robot velocity in current frame
        /// </summary>
        public double Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }


        /// <summary>
        /// Robot angle in current frame
        /// </summary>
        public double Phi
        {
            get
            {
                return phi;
            }
            set
            {
                phi = value;
            }
        }
        

        /// <summary>
        /// Position on X axis of robot
        /// </summary>
        public double PositionX
        {
            get
            {
                return positionX;
            }
            set
            {
                positionX = value;
            }
        }


        /// <summary>
        /// Position on Y axis of robot
        /// </summary>
        public double PositionY
        {
            get
            {
                return positionY;
            }
            set
            {
                positionY = value;
            }
        }


        /// <summary>
        /// Robot Sensor data for current frame
        /// </summary>
        public RobotSensorData SensorData
        {
            get
            {
                return sensorData;
            }
            set
            {
                sensorData = value;
            }
        }


        /// <summary>
        /// RPLidar scanns for current frame
        /// </summary>
        public RPLidarScan[] LaserScans
        {
            get
            {
                return laserScans;
            }
            set
            {
                laserScans = value;
            }
        }



        /// <summary>
        /// Initiate empty Timeline frame
        /// </summary>
        public TimelineItem()
        {
            frameNo = 0;
            time = 0;
            velocity = 0;
            phi = 0;
            positionX = 0;
            positionY = 0;
            sensorData = null;
            laserScans = null;
        }


        /// <summary>
        /// Initiate Timeline frame with specified number and time
        /// </summary>
        /// <param name="FrameNo">Frame number</param>
        /// <param name="Time">Time</param>
        public TimelineItem(int FrameNo, double Time)
        {
            frameNo = FrameNo;
            time = Time;
            velocity = 0;
            phi = 0;
            positionX = 0;
            positionY = 0;
            sensorData = null;
            laserScans = null;
        }


    }
}
