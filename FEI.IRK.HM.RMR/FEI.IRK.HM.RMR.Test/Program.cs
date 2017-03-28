using FEI.IRK.HM.RMR.Lib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            RPLidarMeasurementList mList = RPLidarHelper.Deserialize("C:\\Coding\\RPLidar.txt");
            RobotSensorDataList sList = RobotSensorHelper.Deserialize("C:\\Coding\\iRobotCreate.dat");

            double rplMax = mList.Max(ml => ml.Scans.Max(mli => mli.Distance));
            double cs = Math.Cos(90 / 180 * Math.PI);

            double x = Math.Ceiling(2.1);
            LocalizationTimeline t = new LocalizationTimeline(sList, mList);
            


        }
    }
}
