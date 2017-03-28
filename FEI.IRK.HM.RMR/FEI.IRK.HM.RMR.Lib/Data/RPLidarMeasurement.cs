using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    public class RPLidarMeasurement
    {

        private int timestamp;
        private RPLidarScanList scans;
        private RPLidarMeasurement previous;


        /// <summary>
        /// Timestamp in microseconds of current scans measurement
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
        /// Scans count available for current measurement
        /// </summary>
        public int ScanCount
        {
            get
            {
                return scans.Count;
            }
        }


        /// <summary>
        /// List of RPLidar Scans data
        /// </summary>
        public RPLidarScanList Scans
        {
            get
            {
                return scans;
            }
        }


        /// <summary>
        /// Link to the previous measurement
        /// </summary>
        public RPLidarMeasurement Previous
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
        /// Create new RPLidar measurement object with default values
        /// </summary>
        public RPLidarMeasurement()
        {
            timestamp = 0;
            scans = new RPLidarScanList();
        }


        /// <summary>
        /// Create new RPLidar measurement object with specified timestamp and empty scans list
        /// </summary>
        /// <param name="TimeStamp">Timestamp in microseconds of current scans measurement</param>
        public RPLidarMeasurement(int TimeStamp)
        {
            timestamp = TimeStamp;
            scans = new RPLidarScanList();
        }


        /// <summary>
        /// Create new RPLidar measurement object with specified timestamp and scans list
        /// </summary>
        /// <param name="TimeStamp">Timestamp in microseconds of current scans measurement</param>
        /// <param name="Scans">List of RPLidar Scans data</param>
        public RPLidarMeasurement(int TimeStamp, RPLidarScanList Scans)
        {
            timestamp = TimeStamp;
            scans = (Scans == null) ? new RPLidarScanList() : Scans;
        }


        /// <summary>
        /// Create new RPLidar measurement object with specified timestamp, scans list and previous measurement
        /// </summary>
        /// <param name="TimeStamp">Timestamp in microseconds of current scans measurement</param>
        /// <param name="Scans">List of RPLidar Scans data</param>
        /// <param name="PreviousMeasurement">Previous RPLidarMeasurement item</param>
        public RPLidarMeasurement(int TimeStamp, RPLidarScanList Scans, RPLidarMeasurement PreviousMeasurement)
        {
            timestamp = TimeStamp;
            scans = (Scans == null) ? new RPLidarScanList() : Scans;
            previous = PreviousMeasurement;
        }


    }
}
