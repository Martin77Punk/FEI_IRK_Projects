using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    public class RPLidarScan
    {

        private double distance;
        private double angle;

        /// <summary>
        /// Distance to the nearest obstacle at given angle
        /// </summary>
        public double Distance
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
        /// Angle of the current measurement scan
        /// </summary>
        public double Angle
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
        /// Constructor for creating empty RPLidar scan object
        /// </summary>
        public RPLidarScan()
        {
            distance = 0;
            angle = 0;
        }


        /// <summary>
        /// Constructor for creating RPLidar scan object with specified data
        /// </summary>
        /// <param name="Distance">Distance to the nearest obstacle at given angle</param>
        /// <param name="Angle">Angle of the current measurement scan</param>
        public RPLidarScan(double Distance, double Angle)
        {
            distance = Distance;
            angle = Angle;
        }


    }
}
