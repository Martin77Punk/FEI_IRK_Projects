using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    /// <summary>
    /// Collection of all obstacles found at the current time frame
    /// </summary>
    public class ObstacleItem
    {
        /// <summary>
        /// Current Frame number
        /// </summary>
        public int FrameNo;

        public bool HasScanData;
        /// <summary>
        /// Collection of all obstacles found at the current time frame
        /// </summary>
        public Obstacle[] Obstacles;
    }
}
