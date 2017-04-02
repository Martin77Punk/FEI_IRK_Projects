using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    public class TimelineItemList : List<TimelineItem>
    {

        /// <summary>
        /// Gets single time frame from Timeline identified by its time
        /// </summary>
        /// <param name="Time">Time of the searched time frame</param>
        /// <returns>Timeline item with specific Time</returns>
        public TimelineItem GetByTime(double Time)
        {
            return this.First(TLItem => TLItem.Time == Time);
        }
        
        /// <summary>
        /// Gets all time frames with available sensor data as array
        /// </summary>
        /// <returns>All timeline items with sensor data</returns>
        public TimelineItem[] GetAllWithSensorData()
        {
            return this.Where(TLItem => TLItem.SensorData != null).ToArray();
        }

        /// <summary>
        /// Gets all time frames with available scan data as array
        /// </summary>
        /// <returns>All timeline items with scan data</returns>
        public TimelineItem[] GetAllWithScanData()
        {
            return this.Where(TLItem => TLItem.LaserScans != null).ToArray();
        }

        /// <summary>
        /// Gets all time frames with available sensor or scan data as array
        /// </summary>
        /// <returns>All timeline items with sensor or scan data</returns>
        public TimelineItem[] GetAllWithAnyData()
        {
            return this.Where(TLItem => TLItem.SensorData != null || TLItem.LaserScans != null).ToArray();
        }

        /// <summary>
        /// Gets Last Timeline item time with sensor data for specific frame
        /// </summary>
        /// <param name="CurrentFrame">Current Frame number</param>
        /// <returns>Timeline item time with last sensor data for current frame</returns>
        public double GetLastItemTimeWithSensorData(int CurrentFrame)
        {
            TimelineItem LastSensorReading = null;
            LastSensorReading = (this.Count(TLItem => TLItem.SensorData != null && TLItem.FrameNo <= CurrentFrame) > 0) ? this.Last(TLItem => TLItem.SensorData != null && TLItem.FrameNo <= CurrentFrame) : null;
            if (LastSensorReading == null)
                return 0;
            else
                return LastSensorReading.Time;
        }

        /// <summary>
        /// Gets Last Timeline item time with scan data for specific frame
        /// </summary>
        /// <param name="CurrentFrame">Current Frame number</param>
        /// <returns>Timeline item time with last scan data for current frame</returns>
        public double GetLastItemTimeWithScanData(int CurrentFrame)
        {
            TimelineItem LastScanReading = null;
            LastScanReading = (this.Count(TLItem => TLItem.LaserScans != null && TLItem.FrameNo <= CurrentFrame) > 0) ? this.Last(TLItem => TLItem.LaserScans != null && TLItem.FrameNo <= CurrentFrame) : null;
            if (LastScanReading == null)
                return 0;
            else
                return LastScanReading.Time;
        }

        /// <summary>
        /// Gets Last Timeline item time with sensor or scan data for specific frame
        /// </summary>
        /// <param name="CurrentFrame">Current Frame number</param>
        /// <returns>Timeline item time with last sensor or scan data for current frame</returns>
        public double GetLastItemTimeWithAnyData(int CurrentFrame)
        {
            TimelineItem LastData = null;
            LastData = (this.Count(TLItem => (TLItem.SensorData != null || TLItem.LaserScans != null) && TLItem.FrameNo <= CurrentFrame) > 0) ? this.Last(TLItem => (TLItem.SensorData != null || TLItem.LaserScans != null) && TLItem.FrameNo <= CurrentFrame) : null;
            if (LastData == null)
                return 0;
            else
                return LastData.Time;
        }

        /// <summary>
        /// Gets Frame number of Timeline item containing Sensor data specified by its index
        /// </summary>
        /// <param name="DataIdx">Sensor data index</param>
        /// <returns>Frame number with Sensor data</returns>
        public int GetFrameNoWithNthSensorData(int DataIdx)
        {
            TimelineItem[] Items = GetAllWithSensorData();
            if (Items.Length <= DataIdx)
            {
                return 0;
            }
            else
            {
                return Items[DataIdx].FrameNo;
            }
        }

        /// <summary>
        /// Gets Frame number of Timeline item containing Scan data specified by its index
        /// </summary>
        /// <param name="DataIdx">Scan data index</param>
        /// <returns>Frame number with Scan data</returns>
        public int GetFrameNoWithNthScanData(int DataIdx)
        {
            TimelineItem[] Items = GetAllWithScanData();
            if (Items.Length <= DataIdx)
            {
                return 0;
            }
            else
            {
                return Items[DataIdx].FrameNo;
            }
        }

        /// <summary>
        /// Gets Frame number of Timeline item containing Sensor or Scan data specified by its index
        /// </summary>
        /// <param name="DataIdx">Data index</param>
        /// <returns>Frame number with Sensor/Scan data</returns>
        public int GetFrameNoWithNthAnyData(int DataIdx)
        {
            TimelineItem[] Items = GetAllWithAnyData();
            if (Items.Length <= DataIdx)
            {
                return 0;
            }
            else
            {
                return Items[DataIdx].FrameNo;
            }
        }

    }
}
