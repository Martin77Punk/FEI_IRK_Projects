using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    public class RobotSensorData
    {

        private int timestamp;
        private short distance;
        private short angle;
        private RobotSensorData previous;


        /// <summary>
        /// Timestamp of current robot sensor reading
        /// </summary>
        public int Timestamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                timestamp = value;
            }
        }

        /// <summary>
        /// Travelled distance from the last robot sensor reading
        /// </summary>
        public short Distance
        {
            get
            {
                return distance;
            }
            set
            {
                distance = value;
            }
        }
        
        /// <summary>
        /// Angle update
        /// </summary>
        public short Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
            }
        }

        /// <summary>
        /// Link to the previous robot sensor reading
        /// </summary>
        public RobotSensorData Previous
        {
            get
            {
                return previous;
            }
            set
            {
                previous = value;
            }
        }


        /// <summary>
        /// Construct empty Robot sensor Data item
        /// </summary>
        public RobotSensorData()
        {
            timestamp = 0;
            distance = 0;
            angle = 0;
            previous = null;
        }


        /// <summary>
        /// Construct Robot sensor Data with specified values
        /// </summary>
        /// <param name="Timestamp">Timestamp of current robot sensor reading</param>
        /// <param name="Distance">Travelled distance from the last robot sensor reading</param>
        /// <param name="Angle">Angle update</param>
        public RobotSensorData(int Timestamp, short Distance, short Angle)
        {
            timestamp = Timestamp;
            distance = Distance;
            angle = Angle;
            previous = null;
        }


        /// <summary>
        /// Construct Robot sensor Data with specified values and with link to previous sensor readings
        /// </summary>
        /// <param name="Timestamp">Timestamp of current robot sensor reading</param>
        /// <param name="Distance">Travelled distance from the last robot sensor reading</param>
        /// <param name="Angle">Angle update</param>
        /// <param name="Previous">Link to the previous robot sensor reading</param>
        public RobotSensorData(int Timestamp, short Distance, short Angle, RobotSensorData Previous)
        {
            timestamp = Timestamp;
            distance = Distance;
            angle = Angle;
            previous = Previous;
        }


        /// <summary>
        /// Construct Robot sensor Data from Robot sensor file structure
        /// </summary>
        /// <param name="SensorStructure">Structure with Robot sensor data from file</param>
        public RobotSensorData(RobotSensorDataStruct SensorStructure)
        {
            timestamp = SensorStructure.Timestamp;
            distance = SensorStructure.Distance;
            angle = SensorStructure.Angle;
            previous = null;
        }


        /// <summary>
        /// Construct Robot sensor Data from Robot sensor file structure
        /// </summary>
        /// <param name="SensorStructure">Structure with Robot sensor data from file</param>
        /// <param name="Previous">Link to the previous robot sensor reading</param>
        public RobotSensorData(RobotSensorDataStruct SensorStructure, RobotSensorData Previous)
        {
            timestamp = SensorStructure.Timestamp;
            distance = SensorStructure.Distance;
            angle = SensorStructure.Angle;
            previous = Previous;
        }


    }
}
