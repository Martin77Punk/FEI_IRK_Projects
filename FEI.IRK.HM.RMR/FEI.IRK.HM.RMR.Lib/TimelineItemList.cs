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
        /// <returns></returns>
        public TimelineItem GetByTime(double Time)
        {
            return this.First(TLItem => TLItem.Time == Time);
        }


        /// <summary>
        /// Gets all time frames with available sensor data as array
        /// </summary>
        /// <returns></returns>
        public TimelineItem[] GetAllWithSensorData()
        {
            return this.Where(TLItem => TLItem.SensorData != null).ToArray();
        }


        /// <summary>
        /// Gets all time frames with available sensor data as array
        /// </summary>
        /// <returns></returns>
        public TimelineItem[] GetAllWithScanData()
        {
            return this.Where(TLItem => TLItem.LaserScans != null).ToArray();
        }



        public double GetLastItemTimeWithSensorData(int CurrentFrame)
        {
            TimelineItem LastSensorReading = null;
            try
            {
                LastSensorReading = this.Last(TLItem => TLItem.SensorData != null && TLItem.FrameNo <= CurrentFrame);
            }
            catch (Exception) { }
            if (LastSensorReading == null)
                return 0;
            else
                return LastSensorReading.Time;
        }



        public double GetLastItemTimeWithScanData(int CurrentFrame)
        {
            TimelineItem LastScanReading = null;
            try
            {
                LastScanReading = this.Last(TLItem => TLItem.LaserScans != null && TLItem.FrameNo <= CurrentFrame);
            }
            catch (Exception) { }
            if (LastScanReading == null)
                return 0;
            else
                return LastScanReading.Time;
        }



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


    }
}
