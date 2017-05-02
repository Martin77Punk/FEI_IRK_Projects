using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    public class FloodMapper
    {
        private ObstacleItemList RawObstacles;
        private Obstacle[] Obstacles;
        private decimal PixelLength;
        private FloodMap BaseMap;

        double MaxCoordX = 0;
        double MaxCoordY = 0;


        public FloodMap Map
        {
            get
            {
                return BaseMap;
            }
        }


        public FloodMapper(TimelineItemList TimelineData, decimal PixelLength)
        {
            this.PixelLength = PixelLength;
            RawObstacles = new ObstacleItemList(TimelineData, true);
            UpdateDrawingBounds();
            GenerateObstacleList();
            GenerateObstacleMap();
        }


        public Tuple<double, double> UpdateDrawingBounds()
        {
            // Get Min and Max obstacle position
            double ObstacleMinX = Math.Floor(RawObstacles.Min(OList => (OList.Obstacles != null) ? OList.Obstacles.Min(OArray => OArray.PositionX) : 0));
            double ObstacleMaxX = Math.Ceiling(RawObstacles.Max(OList => (OList.Obstacles != null) ? OList.Obstacles.Max(OArray => OArray.PositionX) : 0));
            double ObstacleMinY = Math.Floor(RawObstacles.Min(OList => (OList.Obstacles != null) ? OList.Obstacles.Min(OArray => OArray.PositionY) : 0));
            double ObstacleMaxY = Math.Ceiling(RawObstacles.Max(OList => (OList.Obstacles != null) ? OList.Obstacles.Max(OArray => OArray.PositionY) : 0));

            // Get totals track length for obstacles on X and Y axis
            double ObstacleTrackXLength = Math.Ceiling(Math.Abs(ObstacleMaxX - ObstacleMinX));
            double ObstacleTrackYLength = Math.Ceiling(Math.Abs(ObstacleMaxY - ObstacleMinY));

            // Extend axis bounds with robot diameter and 5% of track length from each side
            ObstacleMinX = Math.Floor(ObstacleMinX - 0.05 * ObstacleTrackXLength);
            ObstacleMaxX = Math.Ceiling(ObstacleMaxX + 0.05 * ObstacleTrackXLength);
            ObstacleMinY = Math.Floor(ObstacleMinY - 0.05 * ObstacleTrackYLength);
            ObstacleMaxY = Math.Ceiling(ObstacleMaxY + 0.05 * ObstacleTrackYLength);

            // Get Max coords for each axis
            MaxCoordX = 0;
            MaxCoordY = 0;
            if (Math.Abs(ObstacleMinX) > MaxCoordX) MaxCoordX = Math.Abs(ObstacleMinX);
            if (Math.Abs(ObstacleMaxX) > MaxCoordX) MaxCoordX = Math.Abs(ObstacleMaxX);
            if (Math.Abs(ObstacleMinY) > MaxCoordY) MaxCoordY = Math.Abs(ObstacleMinY);
            if (Math.Abs(ObstacleMaxY) > MaxCoordY) MaxCoordY = Math.Abs(ObstacleMaxY);

            if (MaxCoordX % (double)PixelLength != 0) { MaxCoordX += (double)PixelLength - (MaxCoordX % (double)PixelLength); MaxCoordX = Math.Round(MaxCoordX); }
            if (MaxCoordY % (double)PixelLength != 0) { MaxCoordY += (double)PixelLength - (MaxCoordY % (double)PixelLength); MaxCoordY = Math.Round(MaxCoordY); }

            return new Tuple<double, double>(MaxCoordX, MaxCoordY);
        }


        public void CheckMapRecreate(decimal PixelLength)
        {
            if (this.PixelLength != PixelLength)
            {
                this.PixelLength = PixelLength;
                GenerateObstacleMap();
            }            
        }


        private void GenerateObstacleList()
        {
            //RawObstacles
            List<Obstacle> ObstacleList = new List<Obstacle>();
            foreach (ObstacleItem RawObstacleItemList in RawObstacles.Where(o => o.HasScanData == true))
            {
                if (RawObstacleItemList.Obstacles != null)
                {
                    foreach (Obstacle RawObstacle in RawObstacleItemList.Obstacles)
                    {
                        ObstacleList.Add(RawObstacle);
                    }
                }
            }
            Obstacles = ObstacleList.ToArray();
        }


        private void GenerateObstacleMap()
        {
            short PixX = (short)(MaxCoordX / (double)PixelLength);
            short PixY = (short)(MaxCoordY / (double)PixelLength);
            BaseMap = new FloodMap((short)-PixX, PixX, (short)-PixY, PixY);

            for (int x = -PixX; x <= PixX; x++)
            {
                for (int y = -PixY; y <= PixY; y++)
                {
                    double x1 = x * (double)PixelLength;
                    double x2 = x1 + (double)PixelLength;
                    double y1 = y * (double)PixelLength;
                    double y2 = y1 + (double)PixelLength;
                    int ObstacleCount = Obstacles.Count(o => o.PositionX > x1 && o.PositionX < x2 && o.PositionY > y1 && o.PositionY < y2);
                    BaseMap[x, y] = (ObstacleCount == 0) ? FloodMap.PIXEL_FREE : FloodMap.PIXEL_OBSTACLE;
                }
            }

        }


    }
}
