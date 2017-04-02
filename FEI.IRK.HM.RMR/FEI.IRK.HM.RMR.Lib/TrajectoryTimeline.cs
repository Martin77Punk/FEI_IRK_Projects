using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEI.IRK.HM.RMR.Lib
{
    public class TrajectoryTimeline : TimelineBase
    {

        #region Private variables

        private Color ColorBackground = Color.AliceBlue;
        private Brush BrushTrajectory = Brushes.OrangeRed;
        private Brush BrushRobot = Brushes.DarkSlateBlue;
        private Brush BrushObstacle = Brushes.DarkOliveGreen;

        private int ZeroX = 0;
        private int ZeroY = 0;
        private double TransformMultiplier = 0;
        private double MaxCoordX = 0;
        private double MaxCoordY = 0;

        #endregion


        #region Constructors

        /// <summary>
        /// Disabled
        /// </summary>
        private TrajectoryTimeline() { }

        /// <summary>
        /// Initialize Trajectory Timeline with supplied Sensor and Laser scanner data
        /// </summary>
        /// <param name="SensorData">Robot sensor data</param>
        /// <param name="ScanData">RPLidar Laser scanner data</param>
        public TrajectoryTimeline(RobotSensorDataList SensorData, RPLidarMeasurementList ScanData) : base(SensorData, ScanData)
        {
            SetDrawingBounds();
        }

        #endregion


        #region Private paint functions

        /// <summary>
        /// Function indicating that the contents of the image was changed and whether to invalidate PictureBox
        /// </summary>
        /// <returns>TRUE if PictureBox should be invalidated and repainted</returns>
        protected override Boolean ShouldInvalidatePictureBoxInNewFrame(int PreviousFrameNo, int NewFrameNo)
        {
            return true;
        }

        /// <summary>
        /// Function for painting on the PictureBox
        /// </summary>
        /// <param name="FrameNo">Number of the frame to paint</param>
        /// <param name="g">GDI Graphics object used for painting</param>
        /// <param name="PictureBoxMaxX">PictureBox width</param>
        /// <param name="PictureBoxMaxY">PictureBox height</param>
        protected override void PaintFrameMap(int FrameNo, Graphics g, int PictureBoxMaxX, int PictureBoxMaxY)
        {
            // Initialize bounds and clear background
            SetDrawingBounds();
            TransformSetPicureBoxSizes(PictureBoxMaxX, PictureBoxMaxY);
            g.Clear(ColorBackground);
            // Paint Obstacles
            //if (ObstacleList[FrameNo].Obstacles != null)
            //{
            //    foreach (Obstacle SingleObstacle in ObstacleList[FrameNo].Obstacles)
            //    {
            //        g.FillRectangle(BrushObstacle, TransformX(SingleObstacle.PositionX), TransformY(SingleObstacle.PositionY), 1, 1);
            //    }
            //}
            //if (NavigateClicked)
            //{
            //    for (int i = 1; i < NavigationList.Count; i++)
            //    {
            //        g.DrawLine(PenRobotNavigation, TransformX(NavigationList[i - 1].PositionX), TransformY(NavigationList[i - 1].PositionY), TransformX(NavigationList[i].PositionX), TransformY(NavigationList[i].PositionY));
            //    }
            //}
            //// Paint Robot
            //double RobotRadius = (double)RobotDiameter / 2;
            //int RobotTopX = TransformX(-RobotRadius);
            //int RobotTopY = TransformY(RobotRadius);
            //int robotSize = (int)((double)RobotDiameter / TransformMultiplier);
            //g.DrawEllipse(PenRobot, RobotTopX, RobotTopY, robotSize, robotSize);
            //g.FillPie(BrushRobot, RobotTopX, RobotTopY, robotSize, robotSize, 15, -30);
        }

        /// <summary>
        /// Reset drawing bounds - this should be executed usually at start and at robot size change
        /// </summary>
        private void SetDrawingBounds()
        {
            //// Get Min and Max obstacle position
            //double ObstacleMinX = Math.Floor(ObstacleList.Min(OList => (OList.Obstacles != null) ? OList.Obstacles.Min(OArray => OArray.PositionX) : 0));
            //double ObstacleMaxX = Math.Ceiling(ObstacleList.Max(OList => (OList.Obstacles != null) ? OList.Obstacles.Max(OArray => OArray.PositionX) : 0));
            //double ObstacleMinY = Math.Floor(ObstacleList.Min(OList => (OList.Obstacles != null) ? OList.Obstacles.Min(OArray => OArray.PositionY) : 0));
            //double ObstacleMaxY = Math.Ceiling(ObstacleList.Max(OList => (OList.Obstacles != null) ? OList.Obstacles.Max(OArray => OArray.PositionY) : 0));

            //// Get totals track length for obstacles on X and Y axis
            //double ObstacleTrackXLength = Math.Ceiling(Math.Abs(ObstacleMaxX - ObstacleMinX));
            //double ObstacleTrackYLength = Math.Ceiling(Math.Abs(ObstacleMaxY - ObstacleMinY));

            //// Extend axis bounds with robot diameter and 5% of track length from each side
            //ObstacleMinX = Math.Floor(ObstacleMinX - (double)RobotDiameter - 0.05 * ObstacleTrackXLength);
            //ObstacleMaxX = Math.Ceiling(ObstacleMaxX + (double)RobotDiameter + 0.05 * ObstacleTrackXLength);
            //ObstacleMinY = Math.Floor(ObstacleMinY - (double)RobotDiameter - 0.05 * ObstacleTrackYLength);
            //ObstacleMaxY = Math.Ceiling(ObstacleMaxY + (double)RobotDiameter + 0.05 * ObstacleTrackYLength);

            //// Get Max coords for each axis
            //MaxObstacleCoordX = 0;
            //MaxObstacleCoordY = 0;
            //if (Math.Abs(ObstacleMinX) > MaxObstacleCoordX) MaxObstacleCoordX = Math.Abs(ObstacleMinX);
            //if (Math.Abs(ObstacleMaxX) > MaxObstacleCoordX) MaxObstacleCoordX = Math.Abs(ObstacleMaxX);
            //if (Math.Abs(ObstacleMinY) > MaxObstacleCoordY) MaxObstacleCoordY = Math.Abs(ObstacleMinY);
            //if (Math.Abs(ObstacleMaxY) > MaxObstacleCoordY) MaxObstacleCoordY = Math.Abs(ObstacleMaxY);
        }

        /// <summary>
        /// Reset transform multiplier used for drawing
        /// </summary>
        /// <param name="PictureBoxMaxX">Maximum width of PictureBox area</param>
        /// <param name="PictureBoxMaxY">Maximum height of PictureBox area</param>
        private void TransformSetPicureBoxSizes(int PictureBoxMaxX, int PictureBoxMaxY)
        {
            ZeroX = (PictureBoxMaxX / 2);
            ZeroY = (PictureBoxMaxY / 2);
            if ((MaxCoordX / ZeroX) < (MaxCoordY / ZeroY))
            {
                TransformMultiplier = MaxCoordY / ZeroY; // MaxRobotCoordX / ZeroX;
            }
            else
            {
                TransformMultiplier = MaxCoordX / ZeroX; //MaxRobotCoordY / ZeroY;
            }

        }

        /// <summary>
        /// Transform X coordinate of robot space to X coordinate of PictureBox
        /// </summary>
        /// <param name="ValueX">Robot space X coordinate</param>
        /// <returns>PictureBox X coordinate</returns>
        private int TransformX(double ValueX)
        {
            if (ValueX > 0)
            {
                // Top screen
                return (int)(ZeroX + Math.Abs(ValueX / TransformMultiplier));
            }
            else
            {
                // Bottom screen
                return (int)(ZeroX - Math.Abs(ValueX / TransformMultiplier));
            }
        }

        /// <summary>
        /// Transform Y coordinate of robot space to Y coordinate of PictureBox
        /// </summary>
        /// <param name="ValueY">Robot space Y coordinate</param>
        /// <returns>PictureBox Y coordinate</returns>
        private int TransformY(double ValueY)
        {
            if (ValueY > 0)
            {
                // Right on screen
                return (int)(ZeroY - Math.Abs(ValueY / TransformMultiplier));
            }
            else
            {
                // Left on screen
                return (int)(ZeroY + Math.Abs(ValueY / TransformMultiplier));
            }
        }


        #endregion

    }
}
