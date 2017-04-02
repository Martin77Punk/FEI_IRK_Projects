using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    /// <summary>
    /// Collection of all obstacles in each available time frame
    /// </summary>
    public class ObstacleItemList : List<ObstacleItem>
    {
        /// <summary>
        /// Initialize Collection of obstacles from available timeline data
        /// </summary>
        /// <param name="TimelineData">Prepared Timeline data with all robot positions, sensor readings and obstacles</param>
        /// <param name="FollowRobot">If enabled, obstacle list will be created relative to each robot position</param>
        public ObstacleItemList(TimelineItemList TimelineData, Boolean FollowRobot)
        {
            Obstacle[] LastObstacles = null;
            for (int i = 0; i < TimelineData.Count; i++)
            {
                if (TimelineData[i].LaserScans != null)
                {
                    // Generate list of obstacles
                    List<Obstacle> ObstacleList = new List<Obstacle>();
                    foreach (RPLidarScan SingleScan in TimelineData[i].LaserScans)
                    {
                        if (SingleScan.Distance != 0)
                        {
                            double ObstacleDistance = (1.5281 * SingleScan.Distance + 1.9913) * 10; // cm to mm
                            Obstacle CurObstacle = new Obstacle();
                            if (FollowRobot)
                            {
                                CurObstacle.PositionX = TimelineData[i].PositionX + ObstacleDistance * Math.Cos((TimelineData[i].Phi - SingleScan.Angle) / 180 * Math.PI);
                                CurObstacle.PositionY = TimelineData[i].PositionY + ObstacleDistance * Math.Sin((TimelineData[i].Phi - SingleScan.Angle) / 180 * Math.PI);
                            }
                            else
                            {
                                CurObstacle.PositionX = ObstacleDistance * Math.Cos(-SingleScan.Angle / 180 * Math.PI);
                                CurObstacle.PositionY = ObstacleDistance * Math.Sin(-SingleScan.Angle / 180 * Math.PI);
                            }                            
                            ObstacleList.Add(CurObstacle);
                        }
                    }
                    // add item with new list of obstacles
                    ObstacleItem item = new ObstacleItem();
                    item.FrameNo = i;
                    item.Obstacles = ObstacleList.ToArray();
                    Add(item);
                    // save link to last obstacles list
                    LastObstacles = item.Obstacles;
                }
                else
                {
                    // add last obstacles
                    ObstacleItem item = new ObstacleItem();
                    item.FrameNo = i;
                    item.Obstacles = LastObstacles;
                    Add(item);
                }
            }
        }
    }
}
