using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    public class FloodMap
    {
        private ushort[] MapData;

        private short minX;
        private short maxX;
        private short minY;
        private short maxY;


        public static readonly ushort PIXEL_FREE = 0;
        public static readonly ushort PIXEL_OBSTACLE = 1;


        public short MinX
        {
            get
            {
                return minX;
            }
        }


        public short MaxX
        {
            get
            {
                return maxX;
            }
        }


        public short MinY
        {
            get
            {
                return minY;
            }
        }


        public short MaxY
        {
            get
            {
                return maxY;
            }
        }


        public ushort this[int x, int y]
        {
            get
            {
                return MapData[GetIndex(x, y)];
            }
            set
            {
                MapData[GetIndex(x, y)] = value;
            }
        }


        


        public FloodMap(short MinX, short MaxX, short MinY, short MaxY)
        {
            minX = MinX;
            maxX = MaxX;
            minY = MinY;
            maxY = MaxY;
            ushort ItemsX = (ushort)(MaxX - MinX);
            ushort ItemsY = (ushort)(MaxY - MinY);
            if (MinX == 0 || MaxX == 0 || (MinX < 0 && MaxX > 0)) ItemsX++;
            if (MinY == 0 || MaxY == 0 || (MinY < 0 && MaxY > 0)) ItemsY++;
            int TotalItems = ItemsX * ItemsY;
            MapData = new ushort[TotalItems];
            for (int i = 0; i < TotalItems; i++)
            {
                MapData[i] = PIXEL_FREE;
            }
        }


        public FloodMap(FloodMap ExistingMap)
        {
            minX = ExistingMap.MinX;
            maxX = ExistingMap.MaxX;
            minY = ExistingMap.MinY;
            maxY = ExistingMap.MaxY;
            MapData = new ushort[ExistingMap.MapData.Length];
            for (int i = 0; i < ExistingMap.MapData.Length; i++)
            {
                MapData[i] = ExistingMap.MapData[i];
            }
        }


        public FloodMapPoint GetDrawPoint(int x, int y)
        {
            ushort MapPoint = MapData[GetIndex(x, y)];
            if (MapPoint == PIXEL_FREE)
            {
                return FloodMapPoint.FreeSpace;
            }
            else if (MapPoint == PIXEL_OBSTACLE)
            {
                return FloodMapPoint.Obstacle;
            }
            else
            {
                return FloodMapPoint.NavigationTrack;
            }
        }


        private int GetIndex(int x, int y)
        {
            ushort ItemsX = (ushort)(MaxX - MinX);
            ushort ItemsY = (ushort)(MaxY - MinY);
            if (MinX == 0 || MaxX == 0 || (MinX < 0 && MaxX > 0)) ItemsX++;
            if (MinY == 0 || MaxY == 0 || (MinY < 0 && MaxY > 0)) ItemsY++;
            int index = ((x - MinX) * ItemsY) + (y - MinY);
            //System.Diagnostics.Debug.WriteLine("x = " + x + "; y = " + y + "; idx = " + index);
            return index;
        }


    }
}
