using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    /// <summary>
    /// Class representing all robot trajectories till specified frame number
    /// </summary>
    public class RobotPathTimeItem
    {
        /// <summary>
        /// Current time Frame number
        /// </summary>
        public int FrameNo;
        /// <summary>
        /// Array of all Robot paths from start to current time frame
        /// </summary>
        public RobotPath[] Paths;

    }
}
