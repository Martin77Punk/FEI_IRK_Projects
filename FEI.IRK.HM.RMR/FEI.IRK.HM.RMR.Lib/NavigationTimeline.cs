using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEI.IRK.HM.RMR.Lib
{
    public class NavigationTimeline : TimelineBase
    {

        #region Private variables

        private Color ColorBackground = Color.AliceBlue;
        private Pen PenRobotNavigation = new Pen(Color.OrangeRed, 1);
        private Pen PenRobot = new Pen(Color.DarkSlateBlue, 1);
        private Brush BrushRobot = Brushes.DarkSlateBlue;
        private Brush BrushObstacle = Brushes.DarkOliveGreen;

        private int ZeroX = 0;
        private int ZeroY = 0;
        private double TransformMultiplier = 0;
        private double MaxObstacleCoordX = 0;
        private double MaxObstacleCoordY = 0;

        private ObstacleItemList ObstacleList;

        private Boolean Navigate = false;
        private Boolean NavigateClicked = false;
        private double NavigateToX = 0;
        private double NavigateToY = 0;
        private double NavigationDetourDistance = 100;
        private List<NavigationPath> NavigationList;

        #endregion


        #region Constructors

        /// <summary>
        /// Disabled
        /// </summary>
        private NavigationTimeline() { }

        /// <summary>
        /// Initialize Navigation Timeline with supplied Sensor and Laser scanner data
        /// </summary>
        /// <param name="SensorData">Robot sensor data</param>
        /// <param name="ScanData">RPLidar Laser scanner data</param>
        public NavigationTimeline(RobotSensorDataList SensorData, RPLidarMeasurementList ScanData) : base(SensorData, ScanData)
        {
            ObstacleList = new ObstacleItemList(TimelineItems, false);
            SetDrawingBounds();
        }

        #endregion


        #region Public functions

        /// <summary>
        /// Enables navigation from Picture Box clicks
        /// </summary>
        public void NavigationStart()
        {
            Navigate = true;
            NavigateClicked = false;
            RefreshImageBox();
            NavigationHint = "Vyber cieľ kliknutím na mapu...";
        }

        /// <summary>
        /// Quits navigation. Disables PictureBox clicks and empty navigation text box
        /// </summary>
        public void NavigationEnd()
        {
            Navigate = false;
            NavigateClicked = false;
            RefreshImageBox();
            NavigationHint = String.Empty;
        }

        /// <summary>
        /// Displays to navigation to the selected point by using Tangent bug algoritm
        /// </summary>
        /// <param name="PictureBoxX">Target X position clicked on picture box</param>
        /// <param name="PictureBoxY">Target Y position clicked on picture box</param>
        public void NavigateTo(int PictureBoxX, int PictureBoxY)
        {
            if (Navigate)
            {
                NavigateToX = UnTransformX(PictureBoxX);
                NavigateToY = UnTransformY(PictureBoxY);
                TangentBugNavigation();
                int i = 0;
                NavigationHint = "Vyber cieľ kliknutím na mapu...";
                foreach (NavigationPath Path in NavigationList)
                {
                    if (i > 0)
                        NavigationHint += String.Format("\r\nX = {0}, Y = {1}, Phi = {2}, D = {3}", Path.PositionX.ToString(DoubleDistanceFormat), Path.PositionY.ToString(DoubleDistanceFormat), ((int)Path.Angle).ToString(), Path.Distance.ToString(DoubleDistanceFormat));
                    i++;
                }
                RefreshImageBox();
                Navigate = true;
                NavigateClicked = true;
            }
        }

        #endregion


        #region Private Navigation functions

        /// <summary>
        /// Transform the X position from picture box to axis X position
        /// </summary>
        /// <param name="ValueX">Picture box X position</param>
        /// <returns>Axis X position</returns>
        private double UnTransformX(int ValueX)
        {
            if (ZeroX - ValueX == 0)
            {
                return 0;
            }
            else
            {
                return -(ZeroX - ValueX) * TransformMultiplier;
            }
        }

        /// <summary>
        /// Transform the Y position from picture box to axis Y position
        /// </summary>
        /// <param name="ValueY">Picture box Y position</param>
        /// <returns>Axis Y position</returns>
        private double UnTransformY(int ValueY)
        {
            if (ZeroY - ValueY == 0)
            {
                return 0;
            }
            else
            {
                return (ZeroY - ValueY) * TransformMultiplier;
            }
        }

        /// <summary>
        /// Main function for Tangent Bug navigation
        /// </summary>
        private void TangentBugNavigation()
        {
            double RemainingDistance = 0;
            double DistanceFromDetour = 0;
            Boolean WallCheckMode = false;
            NavigationPath StartPosition = new NavigationPath() { PositionX = 0, PositionY = 0, Distance = 0, Angle = 0 };
            NavigationPath DestinationPosition = new NavigationPath() { PositionX = NavigateToX, PositionY = NavigateToY};
            // Prepare Navigation List
            if (NavigationList == null)
                NavigationList = new List<NavigationPath>();
            if (NavigationList.Count > 0)
                NavigationList.Clear();
            // Add initial item
            NavigationList.Add(StartPosition);
            // Add first tangent
            NavigationPath NextPath = GetBestTangetToDestination(NavigationList.Last(), DestinationPosition);
            RemainingDistance = GetRemainingDistance(NextPath);
            NavigationList.Add(NextPath);
            while (RemainingDistance != 0)
            {
                if (WallCheckMode)
                {
                    // Detour wall
                    break;
                } 
                else
                {
                    // Detour obstacle
                    NavigationPath ObstacleDetour = FindObstacleDetour(NavigationList.Last(), DestinationPosition);
                    if (ObstacleDetour == null) break;
                    NextPath = GetBestTangetToDestination(ObstacleDetour, DestinationPosition);
                    DistanceFromDetour = GetRemainingDistance(NextPath);
                    if ((RemainingDistance - DistanceFromDetour) < 1)
                    {
                        WallCheckMode = true;
                    }
                    else
                    {
                        NavigationList.Add(ObstacleDetour);
                        NavigationList.Add(NextPath);
                        RemainingDistance = GetRemainingDistance(NavigationList.Last());
                    }
                }
            }
        }

        /// <summary>
        /// Function will draw tangent line from staring to destination position until any obstacles occur and returns the navigation path
        /// </summary>
        /// <param name="MyPosition">Starting position</param>
        /// <param name="Destination">Destination position</param>
        /// <returns>Best possible navigation using tangent line to destination</returns>
        private NavigationPath GetBestTangetToDestination(NavigationPath MyPosition, NavigationPath Destination)
        {
            Obstacle[] CurrentObstacles = ObstacleList[CurrentFrameNumber].Obstacles;
            double RobotRadius = ((double)RobotDiameter) / 2;
            double RobotRadius2 = RobotRadius * RobotRadius;
            double DifferenceX = Destination.PositionX - MyPosition.PositionX; 
            double DifferenceY = Destination.PositionY - MyPosition.PositionY;
            double Distance = Math.Sqrt(DifferenceX * DifferenceX + DifferenceY * DifferenceY);
            double Angle = Math.Atan2(DifferenceY, DifferenceX) * 180 / Math.PI;
            Boolean foundObstacle = false;
            NavigationPath GoodPosition = MyPosition;
            if (CurrentObstacles != null && CurrentObstacles.Length > 0)
            {
                for (double mm = 1; mm < Distance; mm++)
                {
                    double CheckXPos = MyPosition.PositionX + mm * Math.Cos(Angle / 180 * Math.PI);
                    double CheckYPos = MyPosition.PositionY + mm * Math.Sin(Angle / 180 * Math.PI);
                    double CheckDifferenceX = CheckXPos - MyPosition.PositionX;
                    double CheckDifferenceY = CheckYPos - MyPosition.PositionY;
                    double CheckDistance = Math.Sqrt(CheckDifferenceX * CheckDifferenceX + CheckDifferenceY * CheckDifferenceY);
                    foreach (Obstacle OneObstacle in CurrentObstacles)
                    {
                        double ObstacleDifferenceX = OneObstacle.PositionX - CheckXPos;
                        double ObstacleDifferenceY = OneObstacle.PositionY - CheckYPos;
                        if ((ObstacleDifferenceX * ObstacleDifferenceX + ObstacleDifferenceY * ObstacleDifferenceY) < RobotRadius2)
                        {
                            foundObstacle = true;
                            goto FinishCycle;
                        }
                    }
                    GoodPosition = new NavigationPath() { PositionX = CheckXPos, PositionY = CheckYPos, Distance = CheckDistance, Angle = Angle };
                }
                if (!foundObstacle)
                {
                    foreach (Obstacle OneObstacle in CurrentObstacles)
                    {
                        double ObstacleDifferenceX = OneObstacle.PositionX - Destination.PositionX;
                        double ObstacleDifferenceY = OneObstacle.PositionY - Destination.PositionY;
                        if ((ObstacleDifferenceX * ObstacleDifferenceX + ObstacleDifferenceY * ObstacleDifferenceY) < RobotRadius2)
                        {
                            foundObstacle = true;
                            break;
                        }
                    }
                    GoodPosition = new NavigationPath() { PositionX = Destination.PositionX, PositionY = Destination.PositionY, Distance = Distance, Angle = Angle };
                }
            }
            else
            {
                GoodPosition = new NavigationPath() { PositionX = Destination.PositionX, PositionY = Destination.PositionY, Distance = Distance, Angle = Angle };
            }
            FinishCycle:
            return GoodPosition;
        }

        /// <summary>
        /// Function will draw tangent line from starting position to point described by angle and maximum distance and returns nevigation to it or to any obstacle before it
        /// </summary>
        /// <param name="MyPosition">Starting position</param>
        /// <param name="Angle">Angle in which direction to draw tangent line</param>
        /// <param name="MaxDistance">Maximum distance of tangent line</param>
        /// <returns>Best possible navigation using tangent line to point described by angle and maximum distance</returns>
        private NavigationPath GetMaxTangetByAngleAndDistance(NavigationPath MyPosition, double Angle, double MaxDistance)
        {
            Obstacle[] CurrentObstacles = ObstacleList[CurrentFrameNumber].Obstacles;
            double RobotRadius = ((double)RobotDiameter) / 2;
            double RobotRadius2 = RobotRadius * RobotRadius;
            double Distance = MaxDistance;
            NavigationPath GoodPosition = MyPosition;
            for (double mm = 1; mm < Distance; mm++)
            {
                double CheckXPos = MyPosition.PositionX + mm * Math.Cos(Angle / 180 * Math.PI);
                double CheckYPos = MyPosition.PositionY + mm * Math.Sin(Angle / 180 * Math.PI);
                if (Math.Abs(CheckXPos) > MaxObstacleCoordX || Math.Abs(CheckYPos) > MaxObstacleCoordY)
                    break;
                double CheckDifferenceX = CheckXPos - MyPosition.PositionX;
                double CheckDifferenceY = CheckYPos - MyPosition.PositionY;
                double CheckDistance = Math.Sqrt(CheckDifferenceX * CheckDifferenceX + CheckDifferenceY * CheckDifferenceY);
                if (CurrentObstacles != null && CurrentObstacles.Length > 0)
                {
                    foreach (Obstacle OneObstacle in CurrentObstacles)
                    {
                        double ObstacleDifferenceX = OneObstacle.PositionX - CheckXPos;
                        double ObstacleDifferenceY = OneObstacle.PositionY - CheckYPos;
                        if ((ObstacleDifferenceX * ObstacleDifferenceX + ObstacleDifferenceY * ObstacleDifferenceY) < RobotRadius2)
                        {
                            goto FinishCycle;
                        }
                    }
                }
                GoodPosition = new NavigationPath() { PositionX = CheckXPos, PositionY = CheckYPos, Distance = CheckDistance, Angle = Angle };
            }
            FinishCycle:
            return GoodPosition;
        }

        /// <summary>
        /// Finds the best possible detour of obstacle in all possible angles and returns it
        /// </summary>
        /// <param name="MyPosition">Starting position</param>
        /// <param name="Destination">Destination position</param>
        /// <returns>Navigation to best possible detour</returns>
        private NavigationPath FindObstacleDetour(NavigationPath MyPosition, NavigationPath Destination)
        {
            double CurrentDistance = GetRemainingDistance(MyPosition);
            NavigationPath[] Detours = new NavigationPath[360];
            double[] DetourDistances = new double[360];
            NavigationPath[] DetouredTangents = new NavigationPath[360];
            double[] DetouredDistances = new double[360];
            double BestDetourDistance = CurrentDistance;
            NavigationPath BestDetour = null;
            for (int Angle = 0; Angle < 360; Angle++)
            {
                Detours[Angle] = GetMaxTangetByAngleAndDistance(MyPosition, Angle, NavigationDetourDistance);
                DetourDistances[Angle] = GetRemainingDistance(Detours[Angle]);
                DetouredTangents[Angle] = GetBestTangetToDestination(Detours[Angle], Destination);
                DetouredDistances[Angle] = GetRemainingDistance(DetouredTangents[Angle]);
                if (DetouredDistances[Angle] < BestDetourDistance)
                {
                    BestDetourDistance = DetouredDistances[Angle];
                    BestDetour = Detours[Angle];
                }
            }
            return BestDetour;
        }
        
        /// <summary>
        /// Counts the remaining Euclid distance from current position to destination
        /// </summary>
        /// <param name="MyPosition">Current position</param>
        /// <returns>Euclid distance from current position to destination</returns>
        private double GetRemainingDistance(NavigationPath MyPosition)
        {
            double DifferenceX = NavigateToX - MyPosition.PositionX;
            double DifferenceY = NavigateToY - MyPosition.PositionY;
            return Math.Sqrt(DifferenceX * DifferenceX + DifferenceY * DifferenceY);
        }


        #endregion


        #region Private paint functions

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
            if (ObstacleList[FrameNo].Obstacles != null)
            {
                foreach (Obstacle SingleObstacle in ObstacleList[FrameNo].Obstacles)
                {
                    g.FillRectangle(BrushObstacle, TransformX(SingleObstacle.PositionX), TransformY(SingleObstacle.PositionY), 1, 1);
                }
            }
            if (NavigateClicked)
            {
                for (int i = 1; i < NavigationList.Count; i++)
                {
                    g.DrawLine(PenRobotNavigation, TransformX(NavigationList[i - 1].PositionX), TransformY(NavigationList[i - 1].PositionY), TransformX(NavigationList[i].PositionX), TransformY(NavigationList[i].PositionY));
                }
            }
            // Paint Robot
            double RobotRadius = (double)RobotDiameter / 2;
            int RobotTopX = TransformX(-RobotRadius);
            int RobotTopY = TransformY(RobotRadius);
            int robotSize = (int)((double)RobotDiameter / TransformMultiplier);
            g.DrawEllipse(PenRobot, RobotTopX, RobotTopY, robotSize, robotSize);
            g.FillPie(BrushRobot, RobotTopX, RobotTopY, robotSize, robotSize, 15, -30);
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
            MaxObstacleCoordX = 0;
            MaxObstacleCoordY = 0;
            if (Math.Abs(ObstacleMinX) > MaxObstacleCoordX) MaxObstacleCoordX = Math.Abs(ObstacleMinX);
            if (Math.Abs(ObstacleMaxX) > MaxObstacleCoordX) MaxObstacleCoordX = Math.Abs(ObstacleMaxX);
            if (Math.Abs(ObstacleMinY) > MaxObstacleCoordY) MaxObstacleCoordY = Math.Abs(ObstacleMinY);
            if (Math.Abs(ObstacleMaxY) > MaxObstacleCoordY) MaxObstacleCoordY = Math.Abs(ObstacleMaxY);
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
            if ((MaxObstacleCoordX / ZeroX) < (MaxObstacleCoordY / ZeroY))
            {
                TransformMultiplier = MaxObstacleCoordY / ZeroY; // MaxRobotCoordX / ZeroX;
            }
            else
            {
                TransformMultiplier = MaxObstacleCoordX / ZeroX; //MaxRobotCoordY / ZeroY;
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
