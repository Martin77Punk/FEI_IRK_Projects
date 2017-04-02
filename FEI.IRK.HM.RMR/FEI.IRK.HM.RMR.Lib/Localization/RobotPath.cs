using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    /// <summary>
    /// Specify 2 points as a single line in robot trajectory
    /// </summary>
    public class RobotPath
    {
        /// <summary>
        /// Robot's initial position in path
        /// </summary>
        public TimelineItem Position1;
        /// <summary>
        /// Robot's destination position in path
        /// </summary>
        public TimelineItem Position2;
    }
}
