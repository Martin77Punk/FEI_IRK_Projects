using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEI.IRK.HM.RMR.Lib
{
    public class MappingTimeline : TimelineBase
    {

        #region Private variables

        private Color ColorBackground = Color.AliceBlue;
        private Pen PenRobot = new Pen(Color.DarkSlateBlue, 1);
        private Pen PenRobotTrack = new Pen(Color.OrangeRed, 1);
        private Brush BrushRobot = Brushes.DarkSlateBlue;
        private Brush BrushObstacle = Brushes.DarkOliveGreen;

        private int ZeroX = 0;
        private int ZeroY = 0;
        private double TransformMultiplier = 0;
        private double MaxCoordX = 0;
        private double MaxCoordY = 0;

        private ObstacleItemList ObstacleList;
        private RobotPathTimeLine PathTimeLine;

        #endregion


        #region Constructors

        /// <summary>
        /// Disabled
        /// </summary>
        private MappingTimeline() { }

        /// <summary>
        /// Initialize Mapping Timeline with supplied Sensor and Laser scanner data
        /// </summary>
        /// <param name="SensorData">Robot sensor data</param>
        /// <param name="ScanData">RPLidar Laser scanner data</param>
        public MappingTimeline(RobotSensorDataList SensorData, RPLidarMeasurementList ScanData) : base(SensorData, ScanData)
        {
            PathTimeLine = new RobotPathTimeLine(TimelineItems);
            ObstacleList = new ObstacleItemList(TimelineItems, true);
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
            // Invalidate in case of different PosX, PosY, Phi, LastScanTime
            double PrevX = TimelineItems[PreviousFrameNo].PositionX;
            double PrevY = TimelineItems[PreviousFrameNo].PositionY;
            double PrevPhi = TimelineItems[PreviousFrameNo].Phi;
            double PrevScanTime = TimelineItems.GetLastItemTimeWithScanData(PreviousFrameNo);
            double NewX = TimelineItems[NewFrameNo].PositionX;
            double NewY = TimelineItems[NewFrameNo].PositionY;
            double NewPhi = TimelineItems[NewFrameNo].Phi;
            double NewScanTime = TimelineItems.GetLastItemTimeWithScanData(NewFrameNo);
            if (PrevX != NewX || PrevY != NewY || PrevPhi != NewPhi || PrevScanTime != NewScanTime)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            if (WholeObstacleMap)
            {
                for (int f = 0; f <= FrameNo; f++)
                {
                    if (ObstacleList[f].Obstacles != null)
                    {
                        foreach (Obstacle SingleObstacle in ObstacleList[f].Obstacles)
                        {
                            g.FillRectangle(BrushObstacle, TransformX(SingleObstacle.PositionX), TransformY(SingleObstacle.PositionY), 1, 1);
                        }
                    }
                }
            }
            else
            {
                if (ObstacleList[FrameNo].Obstacles != null)
                {
                    foreach (Obstacle SingleObstacle in ObstacleList[FrameNo].Obstacles)
                    {
                        g.FillRectangle(BrushObstacle, TransformX(SingleObstacle.PositionX), TransformY(SingleObstacle.PositionY), 1, 1);
                    }
                }
            }
            // Paint Path
            if (ShowRobotTrack)
            {
                if (PathTimeLine[FrameNo].Paths != null)
                {
                    foreach (RobotPath SinglePath in PathTimeLine[FrameNo].Paths)
                    {
                        g.DrawLine(PenRobotTrack, TransformX(SinglePath.Position1.PositionX), TransformY(SinglePath.Position1.PositionY), TransformX(SinglePath.Position2.PositionX), TransformY(SinglePath.Position2.PositionY));
                    }
                }
            }            
            // Paint Robot
            double RobotRadius = (double)RobotDiameter / 2;
            int RobotTopX = TransformX(TimelineItems[FrameNo].PositionX - RobotRadius);
            int RobotTopY = TransformY(TimelineItems[FrameNo].PositionY + RobotRadius);
            int robotSize = (int)((double)RobotDiameter / TransformMultiplier);
            g.DrawEllipse(PenRobot, RobotTopX, RobotTopY, robotSize, robotSize);
            g.FillPie(BrushRobot, RobotTopX, RobotTopY, robotSize, robotSize, (int)-TimelineItems[FrameNo].Phi + 15, -30);
        }

        /// <summary>
        /// Reset drawing bounds - this should be executed usually at start and at robot size change
        /// </summary>
        private void SetDrawingBounds()
        {
            // Get Min and Max obstacle position
            double ObstacleMinX = Math.Floor(ObstacleList.Min(OList => (OList.Obstacles != null) ? OList.Obstacles.Min(OArray => OArray.PositionX) : 0));
            double ObstacleMaxX = Math.Ceiling(ObstacleList.Max(OList => (OList.Obstacles != null) ? OList.Obstacles.Max(OArray => OArray.PositionX) : 0));
            double ObstacleMinY = Math.Floor(ObstacleList.Min(OList => (OList.Obstacles != null) ? OList.Obstacles.Min(OArray => OArray.PositionY) : 0));
            double ObstacleMaxY = Math.Ceiling(ObstacleList.Max(OList => (OList.Obstacles != null) ? OList.Obstacles.Max(OArray => OArray.PositionY) : 0));

            // Get totals track length for obstacles on X and Y axis
            double ObstacleTrackXLength = Math.Ceiling(Math.Abs(ObstacleMaxX - ObstacleMinX));
            double ObstacleTrackYLength = Math.Ceiling(Math.Abs(ObstacleMaxY - ObstacleMinY));

            // Extend axis bounds with robot diameter and 5% of track length from each side
            ObstacleMinX = Math.Floor(ObstacleMinX - (double)RobotDiameter - 0.05 * ObstacleTrackXLength);
            ObstacleMaxX = Math.Ceiling(ObstacleMaxX + (double)RobotDiameter + 0.05 * ObstacleTrackXLength);
            ObstacleMinY = Math.Floor(ObstacleMinY - (double)RobotDiameter - 0.05 * ObstacleTrackYLength);
            ObstacleMaxY = Math.Ceiling(ObstacleMaxY + (double)RobotDiameter + 0.05 * ObstacleTrackYLength);

            // Get Max coords for each axis
            MaxCoordX = 0;
            MaxCoordY = 0;
            if (Math.Abs(ObstacleMinX) > MaxCoordX) MaxCoordX = Math.Abs(ObstacleMinX);
            if (Math.Abs(ObstacleMaxX) > MaxCoordX) MaxCoordX = Math.Abs(ObstacleMaxX);
            if (Math.Abs(ObstacleMinY) > MaxCoordY) MaxCoordY = Math.Abs(ObstacleMinY);
            if (Math.Abs(ObstacleMaxY) > MaxCoordY) MaxCoordY = Math.Abs(ObstacleMaxY);
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
