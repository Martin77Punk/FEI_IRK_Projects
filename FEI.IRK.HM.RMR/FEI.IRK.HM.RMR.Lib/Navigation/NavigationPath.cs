using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    /// <summary>
    /// Item describing a point in whole navigation path
    /// </summary>
    public class NavigationPath
    {
        /// <summary>
        /// Position X of navigation path item
        /// </summary>
        public double PositionX;
        /// <summary>
        /// Position Y of navigation path item
        /// </summary>
        public double PositionY;
        /// <summary>
        /// Distance of navigation path item
        /// </summary>
        public double Distance;
        /// <summary>
        /// Angle of navigation path item
        /// </summary>
        public double Angle;
    }
}
