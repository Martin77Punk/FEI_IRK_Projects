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

        #region Private variables

        private Color ColorBackground = Color.AliceBlue;
        private Pen PenRobotTrack = new Pen(Color.OrangeRed, 1);
        private Pen PenRobot = new Pen(Color.DarkSlateBlue, 1);
        private Brush BrushRobot = Brushes.DarkSlateBlue;
        
        private int ZeroX = 0;
        private int ZeroY = 0;
        private double TransformMultiplier = 0;
        private double MaxRobotCoordX = 0;
        private double MaxRobotCoordY = 0;

        private RobotPathTimeLine PathTimeLine;

        #endregion


        #region Constructors

        /// <summary>
        /// Disabled
        /// </summary>
        private LocalizationTimeline() { }

        /// <summary>
        /// Initialize Localization Timeline with supplied Sensor and Laser scanner data
        /// </summary>
        /// <param name="SensorData">Robot sensor data</param>
        /// <param name="ScanData">RPLidar Laser scanner data</param>
        public LocalizationTimeline(RobotSensorDataList SensorData, RPLidarMeasurementList ScanData) : base(SensorData, ScanData)
        {
            // Generate Robot Path Timeline
            PathTimeLine = new RobotPathTimeLine(TimelineItems);
            // Update drawing bounds
            SetDrawingBounds();
        }

        #endregion


        #region Private Paint functions

        /// <summary>
        /// Function indicating that the contents of the image was changed and whether to invalidate PictureBox
        /// </summary>
        /// <returns>TRUE if PictureBox should be invalidated and repainted</returns>
        protected override Boolean ShouldInvalidatePictureBoxInNewFrame(int PreviousFrameNo, int NewFrameNo)
        {
            // Invalidate in case of different PosX, PosY, Phi
            double PrevX = TimelineItems[PreviousFrameNo].PositionX;
            double PrevY = TimelineItems[PreviousFrameNo].PositionY;
            double PrevPhi = TimelineItems[PreviousFrameNo].Phi;
            double NewX = TimelineItems[NewFrameNo].PositionX;
            double NewY = TimelineItems[NewFrameNo].PositionY;
            double NewPhi = TimelineItems[NewFrameNo].Phi;
            if (PrevX != NewX || PrevY != NewY || PrevPhi != NewPhi)
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
            // Paint Path
            if (PathTimeLine[FrameNo].Paths != null)
            {
                foreach (RobotPath SinglePath in PathTimeLine[FrameNo].Paths)
                {
                    g.DrawLine(PenRobotTrack, TransformX(SinglePath.Position1.PositionX), TransformY(SinglePath.Position1.PositionY), TransformX(SinglePath.Position2.PositionX), TransformY(SinglePath.Position2.PositionY));
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
        /// Reset transform multiplier used for drawing
        /// </summary>
        /// <param name="PictureBoxMaxX">Maximum width of PictureBox area</param>
        /// <param name="PictureBoxMaxY">Maximum height of PictureBox area</param>
        private void TransformSetPicureBoxSizes(int PictureBoxMaxX, int PictureBoxMaxY)
        {
            ZeroX = (PictureBoxMaxX / 2);
            ZeroY = (PictureBoxMaxY / 2);
            if ( (MaxRobotCoordX / ZeroX) < (MaxRobotCoordY / ZeroY) )
            {
                TransformMultiplier = MaxRobotCoordY / ZeroY; // MaxRobotCoordX / ZeroX;
            }
            else
            {
                TransformMultiplier = MaxRobotCoordX / ZeroX; //MaxRobotCoordY / ZeroY;
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

        /// <summary>
        /// Reset drawing bounds - this should be executed usually at start and at robot size change
        /// </summary>
        private void SetDrawingBounds()
        {
            // Get Min and Max robot positions
            double RobotMinX = Math.Floor(TimelineItems.Min(TLItem => TLItem.PositionX));
            double RobotMaxX = Math.Ceiling(TimelineItems.Max(TLItem => TLItem.PositionX));
            double RobotMinY = Math.Floor(TimelineItems.Min(TLItem => TLItem.PositionY));
            double RobotMaxY = Math.Ceiling(TimelineItems.Max(TLItem => TLItem.PositionY));

            // Get totals for X and Y axis of robot's travel
            double RobotTrackXLength = Math.Ceiling(Math.Abs(RobotMaxX - RobotMinX));
            double RobotTrackYLength = Math.Ceiling(Math.Abs(RobotMaxY - RobotMinY));

            // Extend each robot bounds by 5% of the axis track
            RobotMinX = Math.Floor(RobotMinX - (double)RobotDiameter - 0.05 * RobotTrackXLength);
            RobotMaxX = Math.Ceiling(RobotMaxX + (double)RobotDiameter + 0.05 * RobotTrackXLength);
            RobotMinY = Math.Floor(RobotMinY - (double)RobotDiameter - 0.05 * RobotTrackYLength);
            RobotMaxY = Math.Ceiling(RobotMaxY + (double)RobotDiameter + 0.05 * RobotTrackYLength);

            // Get Max coords for each axis
            MaxRobotCoordX = 0;
            MaxRobotCoordY = 0;
            if (Math.Abs(RobotMinX) > MaxRobotCoordX) MaxRobotCoordX = Math.Abs(RobotMinX);
            if (Math.Abs(RobotMaxX) > MaxRobotCoordX) MaxRobotCoordX = Math.Abs(RobotMaxX);
            if (Math.Abs(RobotMinY) > MaxRobotCoordY) MaxRobotCoordY = Math.Abs(RobotMinY);
            if (Math.Abs(RobotMaxY) > MaxRobotCoordY) MaxRobotCoordY = Math.Abs(RobotMaxY);
        }

        #endregion
        
    }
}
