using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEI.IRK.HM.RMR.Lib
{
    public class LocalizationTimeline : TimelineBase
    {

        private LocalizationTimeline() { }

        public LocalizationTimeline(RobotSensorDataList SensorData, RPLidarMeasurementList ScanData) : base(SensorData, ScanData)
        {
            

        }

    }
}
