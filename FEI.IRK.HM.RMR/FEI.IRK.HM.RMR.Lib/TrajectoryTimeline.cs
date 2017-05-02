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
        private Brush BrushObstacle = Brushes.DarkOliveGreen;

        private int ZeroX = 0;
        private int ZeroY = 0;
        private double TransformMultiplier = 0;
        private double MaxCoordX = 0;
        private double MaxCoordY = 0;

        private FloodMapper FloodHelper;

        private Boolean Navigate = false;
        private Boolean NavigateClickedStart = false;
        private Boolean NavigateClickedEnd = false;
        private int NavigateFromX = 0;
        private int NavigateFromY = 0;
        private int NavigateToX = 0;
        private int NavigateToY = 0;

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
        public TrajectoryTimeline(RobotSensorDataList SensorData, RPLidarMeasurementList ScanData, decimal PixelLength) : base(SensorData, ScanData)
        {
            FloodHelper = new FloodMapper(TimelineItems, PixelLength);
            SetDrawingBounds();
        }

        #endregion


        #region Public functions

        public void TrajectoryStart()
        {
            Navigate = true;
            NavigateClickedStart = false;
            NavigateClickedEnd = false;
            ClearTrajectory();
            RefreshImageBox();
            NavigationHint = "Vyber začiatok cesty kliknutím na mapu...";
        }


        public void TrajectoryEnd()
        {
            Navigate = false;
            NavigateClickedStart = false;
            NavigateClickedEnd = false;
            ClearTrajectory();
            RefreshImageBox();
            NavigationHint = String.Empty;
        }


        public void TrajectoryClick(int PictureBoxX, int PictureBoxY)
        {
            int X = UnTransformX(PictureBoxX);
            int Y = UnTransformY(PictureBoxY);

            if (Navigate)
            {
                if (!NavigateClickedStart)
                {
                    // Select navigation start
                    if (X < FloodHelper.Map.MinX || X > FloodHelper.Map.MaxX || Y < FloodHelper.Map.MinY || Y > FloodHelper.Map.MaxY) return;
                    if (FloodHelper.Map.GetDrawPoint(X, Y) == FloodMapPoint.Obstacle) return;
                    NavigateFromX = X;
                    NavigateFromY = Y;
                    Navigate = true;
                    NavigateClickedStart = true;
                    NavigateClickedEnd = false;
                    NavigationHint = String.Format("Začiatok:\t[\t{0},\t{1}]\r\nVyber koniec cesty kliknutím na mapu...", NavigateFromX, NavigateFromY);
                    return;
                }

                if (!NavigateClickedEnd)
                {
                    // Select navigation end
                    if (X < FloodHelper.Map.MinX || X > FloodHelper.Map.MaxX || Y < FloodHelper.Map.MinY || Y > FloodHelper.Map.MaxY) return;
                    if (FloodHelper.Map.GetDrawPoint(X, Y) == FloodMapPoint.Obstacle) return;
                    if (X == NavigateFromX && Y == NavigateFromY) return;
                    NavigateToX = X;
                    NavigateToY = Y;
                    Navigate = true;
                    NavigateClickedStart = true;
                    NavigateClickedEnd = true;
                    NavigationHint = String.Format("Začiatok:\t[\t{0},\t{1}]\r\nKoniec:\t[\t{2},\t{3}]\r\n---------------------------------------------------------\r\n", NavigateFromX, NavigateFromY, NavigateToX, NavigateToY);
                    BuildTrajectory();
                    RefreshImageBox();
                    return;
                }
            }
        }


        #endregion


        #region Private Trajectory functions


        private void ClearTrajectory()
        {
            for (int x = FloodHelper.Map.MinX; x <= FloodHelper.Map.MaxX; x++)
            {
                for (int y = FloodHelper.Map.MinY; y <= FloodHelper.Map.MaxY; y++)
                {
                    if (FloodHelper.Map[x, y] != FloodMap.PIXEL_OBSTACLE)
                    {
                        FloodHelper.Map[x, y] = FloodMap.PIXEL_FREE;
                    }
                }
            }
        }


        private void BuildTrajectory()
        {
            Boolean TrajectoryFound = false;
            FloodMap TrajectoryMap = new FloodMap(FloodHelper.Map);
            Queue<TrajectoryPath> RobotPath = new Queue<TrajectoryPath>();
            RobotPath.Enqueue(new TrajectoryPath { X = NavigateToX, Y = NavigateToY, No = 2});

            while (RobotPath.Count > 0)
            {
                TrajectoryPath TItem = RobotPath.Dequeue();
                if (TrajectoryMap[TItem.X, TItem.Y] != FloodMap.PIXEL_FREE)
                    continue;
                TrajectoryMap[TItem.X, TItem.Y] = TItem.No;
                if (TItem.X == NavigateFromX && TItem.Y == NavigateFromY)
                {
                    TrajectoryFound = true;
                    break;
                }
                ushort NextNo = (ushort)(TItem.No + 1);
                // UP     -> +y
                if (TItem.X >= TrajectoryMap.MinX && TItem.X <= TrajectoryMap.MaxX && TItem.Y + 1 >= TrajectoryMap.MinY && TItem.Y + 1 <= TrajectoryMap.MaxY)
                    RobotPath.Enqueue(new TrajectoryPath { X = TItem.X, Y = TItem.Y + 1, No = NextNo});

                // RIGHT  -> +x
                if (TItem.X +1 >= TrajectoryMap.MinX && TItem.X + 1 <= TrajectoryMap.MaxX && TItem.Y >= TrajectoryMap.MinY && TItem.Y <= TrajectoryMap.MaxY)
                    RobotPath.Enqueue(new TrajectoryPath { X = TItem.X + 1, Y = TItem.Y, No = NextNo });

                // DOWN   -> -y
                if (TItem.X >= TrajectoryMap.MinX && TItem.X <= TrajectoryMap.MaxX && TItem.Y - 1>= TrajectoryMap.MinY && TItem.Y - 1 <= TrajectoryMap.MaxY)
                    RobotPath.Enqueue(new TrajectoryPath { X = TItem.X, Y = TItem.Y - 1, No = NextNo });

                // LEFT   -> -x
                if (TItem.X - 1 >= TrajectoryMap.MinX && TItem.X - 1 <= TrajectoryMap.MaxX && TItem.Y >= TrajectoryMap.MinY && TItem.Y <= TrajectoryMap.MaxY)
                    RobotPath.Enqueue(new TrajectoryPath { X = TItem.X - 1, Y = TItem.Y, No = NextNo });

            }

            if (TrajectoryFound)
            {
                int PosX = NavigateFromX;
                int PosY = NavigateFromY;
                for (ushort i = TrajectoryMap[NavigateFromX, NavigateFromY]; i >= 2; i--)
                {
                    FloodHelper.Map[PosX, PosY] = i;
                    if (i > 2)
                    {
                        // UP     -> +y
                        if (PosX >= FloodHelper.Map.MinX && PosX <= FloodHelper.Map.MaxX && PosY + 1 >= FloodHelper.Map.MinY && PosY + 1 <= FloodHelper.Map.MaxY)
                        {
                            if (TrajectoryMap[PosX, PosY + 1] == i - 1)
                            {
                                NavigationHint += String.Format("Hore:\t[\t{0},\t{1}]\r\n", PosX, PosY + 1);
                                PosY = PosY + 1;
                                continue;
                            }
                        }

                        // RIGHT  -> +x
                        if (PosX + 1 >= FloodHelper.Map.MinX && PosX + 1 <= FloodHelper.Map.MaxX && PosY >= FloodHelper.Map.MinY && PosY <= FloodHelper.Map.MaxY)
                        {
                            if (TrajectoryMap[PosX + 1, PosY] == i - 1)
                            {
                                NavigationHint += String.Format("Vpravo:\t[\t{0},\t{1}]\r\n", PosX + 1, PosY);
                                PosX = PosX + 1;
                                continue;
                            }
                        }

                        // DOWN   -> -y
                        if (PosX >= FloodHelper.Map.MinX && PosX <= FloodHelper.Map.MaxX && PosY - 1 >= FloodHelper.Map.MinY && PosY - 1 <= FloodHelper.Map.MaxY)
                        {
                            if (TrajectoryMap[PosX, PosY - 1] == i - 1)
                            {
                                NavigationHint += String.Format("Dole:\t[\t{0},\t{1}]\r\n", PosX, PosY - 1);
                                PosY = PosY - 1;
                                continue;
                            }
                        }

                        // LEFT   -> -x
                        if (PosX - 1 >= FloodHelper.Map.MinX && PosX - 1 <= FloodHelper.Map.MaxX && PosY >= FloodHelper.Map.MinY && PosY <= FloodHelper.Map.MaxY)
                        {
                            if (TrajectoryMap[PosX - 1, PosY] == i - 1)
                            {
                                NavigationHint += String.Format("Vľavo:\t[\t{0},\t{1}]\r\n", PosX - 1, PosY);
                                PosX = PosX - 1;
                                continue;
                            }
                        }
                    }
                }
            }
            else
            {
                NavigationHint = "Nemožno nájsť cestu!";
            }

        }


        /// <summary>
        /// Transform the X position from picture box to axis X position
        /// </summary>
        /// <param name="ValueX">Picture box X position</param>
        /// <returns>Axis X position</returns>
        private int UnTransformX(int ValueX)
        {
            if (ZeroX - ValueX == 0)
            {
                return 0;
            }
            else
            {
                return (int)Math.Ceiling(-(ZeroX - ValueX) * TransformMultiplier / (double)MapQuantisationRate)-1;
            }
        }

        /// <summary>
        /// Transform the Y position from picture box to axis Y position
        /// </summary>
        /// <param name="ValueY">Picture box Y position</param>
        /// <returns>Axis Y position</returns>
        private int UnTransformY(int ValueY)
        {
            if (ZeroY - ValueY == 0)
            {
                return 0;
            }
            else
            {
                return (int)Math.Ceiling((ZeroY - ValueY) * TransformMultiplier / (double)MapQuantisationRate);
            }
        }



        #endregion


        #region Private paint functions

        /// <summary>
        /// Function indicating that the contents of the image was changed and whether to invalidate PictureBox
        /// </summary>
        /// <returns>TRUE if PictureBox should be invalidated and repainted</returns>
        protected override Boolean ShouldInvalidatePictureBoxInNewFrame(int PreviousFrameNo, int NewFrameNo)
        {
            return false;
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
            // Check whether recreate map
            FloodHelper.CheckMapRecreate(MapQuantisationRate);
            TransformSetPicureBoxSizes(PictureBoxMaxX, PictureBoxMaxY);
            g.Clear(ColorBackground);
            // Paint Map
            for (int x = FloodHelper.Map.MinX; x <= FloodHelper.Map.MaxX; x++)
            {
                for (int y = FloodHelper.Map.MinY; y <= FloodHelper.Map.MaxY; y++)
                {
                    if (FloodHelper.Map.GetDrawPoint(x, y) != FloodMapPoint.FreeSpace)
                    {
                        double PosX = x * (double)MapQuantisationRate;
                        double PosY = y * (double)MapQuantisationRate;
                        Brush PixelBrush = (FloodHelper.Map.GetDrawPoint(x, y) == FloodMapPoint.NavigationTrack) ? BrushTrajectory : BrushObstacle;
                        g.FillRectangle(PixelBrush, TransformX(PosX), TransformY(PosY), (float)MapQuantisationRate / (float)TransformMultiplier, (float)MapQuantisationRate / (float)TransformMultiplier);
                    }


                    //if (x == 0 && y == 0)
                    //{
                    //    double PosX = x * (double)MapQuantisationRate;
                    //    double PosY = y * (double)MapQuantisationRate;
                    //    g.FillRectangle(BrushTrajectory, TransformX(PosX), TransformY(PosY), (float)MapQuantisationRate / (float)TransformMultiplier, (float)MapQuantisationRate / (float)TransformMultiplier);
                    //}

                }
            }
            
        }

        /// <summary>
        /// Reset drawing bounds - this should be executed usually at start and at robot size change
        /// </summary>
        private void SetDrawingBounds()
        {
            // Update Max coords for X and Y
            Tuple<double, double> Coords = FloodHelper.UpdateDrawingBounds();
            MaxCoordX = Coords.Item1;
            MaxCoordY = Coords.Item2;
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
