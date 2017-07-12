using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace FEI.IRK.HM.VZ
{
    public class VZTask2 : IDisposable
    {

        private bool _ImgLoaded;
        private bool _ImgProcessed;
        private bool _InProgress;
        private string _InputFile;

        private Thread ProcessingThread;
        private Thread[] HoughThreads;

        private Bitmap BaseImage;
        private Bitmap SobelImage;
        private Bitmap HoughImage;
        private Bitmap FullImage;

        private int _MinRadius;
        private int _MaxRadius;
        private int _MinThreshold;
        private int _MinDistance;

        public delegate void BitmapChangedFunction(Bitmap bmp);
        public event BitmapChangedFunction BitmapChanged;
        public delegate void Task2StatusChangedFunction(bool Loaded, bool Processed);
        public event Task2StatusChangedFunction Task2StatusChanged;

        private int[,,] Accumulator;
        private bool[,] EdgeMap;
        private object AccumulatorLock;

        public string InputFile
        {
            get
            {
                return _InputFile;
            }
            set
            {
                if (_InProgress)
                    return;
                _InputFile = value;
                LoadImageFile();
            }
        }


        public int MinRadius
        {
            get
            {
                return _MinRadius;
            }
            set
            {
                _MinRadius = value;
            }
        }


        public int MaxRadius
        {
            get
            {
                return _MaxRadius;
            }
            set
            {
                _MaxRadius = value;
            }
        }


        public int MinThreshold
        {
            get
            {
                return _MinThreshold;
            }
            set
            {
                _MinThreshold = value;
            }
        }


        public int MinDistance
        {
            get
            {
                return _MinDistance;
            }
            set
            {
                _MinDistance = value;
            }
        }


        public VZTask2()
        {
            _ImgLoaded = false;
            _ImgProcessed = false;
            _InProgress = false;
            _InputFile = null;
            ProcessingThread = null;
            _MinRadius = 60;
            _MaxRadius = 130;
            _MinThreshold = 100;
            _MinDistance = 20;
            BaseImage = new Bitmap(800, 600);
            SobelImage = new Bitmap(800, 600);
            HoughImage = new Bitmap(800, 600);
            FullImage = new Bitmap(800, 600);
            HoughThreads = new Thread[16];
            AccumulatorLock = new object();
        }


        public void ShowImage(int ImageIndex)
        {
            if (_InProgress)
                return;
            if (ImageIndex < 1 || ImageIndex > 4)
                return;
            if (ImageIndex == 1)
            {
                if (!_ImgLoaded)
                    return;
                if (BitmapChanged != null)
                {
                    BitmapChanged(BaseImage);
                }
            }
            else if (ImageIndex == 2)
            {
                if (!_ImgProcessed)
                    return;
                if (BitmapChanged != null)
                {
                    BitmapChanged(SobelImage);
                }
            }
            else if (ImageIndex == 3)
            {
                if (!_ImgProcessed)
                    return;
                if (BitmapChanged != null)
                {
                    BitmapChanged(HoughImage);
                }
            }
            else
            {
                if (!_ImgProcessed)
                    return;
                if (BitmapChanged != null)
                {
                    BitmapChanged(FullImage);
                }
            }
        }



        public void StartProcessing()
        {
            // Check ci je Image nahraty + ci uz neprebieha Processing alebo ci uz neprebehol...
            if (!_ImgLoaded)
                return;
            if (_ImgProcessed)
            {
                if (Task2StatusChanged != null)
                {
                    Task2StatusChanged(_ImgLoaded, _ImgProcessed);
                }
                return;
            }
            if (_InProgress)
                return;

            // MaxRadius musi byt vacsi ako MinRadius
            if (_MaxRadius < _MinRadius)
                return;

            // Nastavime ze procesujeme image
            _InProgress = true;

            // Spustime procesny Thread
            ProcessingThread = new Thread(new ThreadStart(DoProcessing));
            ProcessingThread.Name = "Task 2 processing";
            ProcessingThread.Start();

        }


        private void DoProcessing()
        {

            // Pripravime si akumulator
            Accumulator = new int[BaseImage.Width, BaseImage.Height, _MaxRadius - _MinRadius + 1];
            for (int a = 0; a < BaseImage.Width; a++)
            {
                for (int b = 0; b < BaseImage.Height; b++)
                {
                    for (int r = 0; r <= (_MaxRadius - _MinRadius); r++)
                    {
                        Accumulator[a, b, r] = 0;
                    }
                }
            }

            // Sobelov operator na detekciu hran
            SobelEdgeDetect();

            // Vytvorime si pomocnu premennu pole boolov - pre obrazok indikujuce ci je  v danom bode hrana
            EdgeMap = new bool[SobelImage.Width, SobelImage.Height];
            // Citanie zo SobelImage pomocou LockBits
            BitmapData bmpDataSobelImage = SobelImage.LockBits(new Rectangle(0, 0, SobelImage.Width, SobelImage.Height), ImageLockMode.ReadOnly, BaseImage.PixelFormat);
            IntPtr SobelImagePtr = bmpDataSobelImage.Scan0;
            int bytesCountSobelImage = Math.Abs(bmpDataSobelImage.Stride) * bmpDataSobelImage.Height;
            byte[] SobelImageData = new byte[bytesCountSobelImage];
            Marshal.Copy(SobelImagePtr, SobelImageData, 0, bytesCountSobelImage);
            int nBytesPerPixelSobelImage = Image.GetPixelFormatSize(SobelImage.PixelFormat) / 8;
            for (int x = 0; x < SobelImage.Width; x++)
            {
                for (int y = 0; y < SobelImage.Height; y++)
                {
                    if (SobelImageData[(y * bmpDataSobelImage.Stride) + (x * nBytesPerPixelSobelImage)] == 0)
                    {
                        // cierna - tu je hrana
                        EdgeMap[x, y] = true;
                    }
                    else
                    {
                        // biela - tu je prazdne miesto
                        EdgeMap[x, y] = false;
                    }                    
                }
            }
            SobelImage.UnlockBits(bmpDataSobelImage);

            // Hladanie kruznic - v samostatnych 16 vlaknach
            int XperThread = SobelImage.Width / HoughThreads.Length;
            for (int tn = 0; tn < HoughThreads.Length; tn++)
            {
                int MinX = tn * XperThread;
                int MaxX = MinX + XperThread - 1;
                if (tn == HoughThreads.Length - 1)
                {
                    MaxX = SobelImage.Width - 1;
                }
                HoughThreads[tn] = new Thread(new ParameterizedThreadStart(DoHoughDetection));
                HoughArgs args = new HoughArgs();
                args.SobelImageWidth = SobelImage.Width;
                args.SobelImageHeight = SobelImage.Height;
                args.MinX = MinX;
                args.MaxX = MaxX;
                HoughThreads[tn].Start(args);
            }
            // Pockame kym vyhladavanie skonci
            for (int tn = 0; tn < HoughThreads.Length; tn++)
            {
                HoughThreads[tn].Join();
            }
            
            // Najdenie lokalnych maxim + ulozenie kruznic
            List<HoughCircle> HoughCircles = new List<HoughCircle>();
            //for (int r = 0; r <= (_MaxRadius - _MinRadius); r++)
            for (int r = 1; r < (_MaxRadius - _MinRadius); r++)
            {
                for (int x = 1; x < SobelImage.Width - 1; x++)
                {
                    for (int y = 1; y < SobelImage.Height - 1; y++)
                    {
                        //if (Accumulator[x, y, r] > _MinThreshold && Accumulator[x, y, r] > Accumulator[x - 1, y, r] && Accumulator[x, y, r] > Accumulator[x + 1, y, r] && Accumulator[x, y, r] > Accumulator[x, y - 1, r] && Accumulator[x, y, r] > Accumulator[x, y + 1, r])
                        //{
                        //    HoughCircles.Add(new HoughCircle() { X = x, Y = y, Radius = r});
                        //}
                        if (Accumulator[x, y, r] > _MinThreshold &&
                            Accumulator[x, y, r] > Accumulator[x, y, r - 1] &&
                            Accumulator[x, y, r] > Accumulator[x - 1, y, r - 1] &&
                            Accumulator[x, y, r] > Accumulator[x - 1, y + 1, r - 1] &&
                            Accumulator[x, y, r] > Accumulator[x, y + 1, r - 1] &&
                            Accumulator[x, y, r] > Accumulator[x + 1, y + 1, r - 1] &&
                            Accumulator[x, y, r] > Accumulator[x + 1, y, r - 1] &&
                            Accumulator[x, y, r] > Accumulator[x + 1, y + 1, r - 1] &&
                            Accumulator[x, y, r] > Accumulator[x, y - 1, r - 1] &&
                            Accumulator[x, y, r] > Accumulator[x - 1, y - 1, r - 1] &&
                            Accumulator[x, y, r] > Accumulator[x - 1, y, r] &&
                            Accumulator[x, y, r] > Accumulator[x - 1, y + 1, r] &&
                            Accumulator[x, y, r] > Accumulator[x, y + 1, r] &&
                            Accumulator[x, y, r] > Accumulator[x + 1, y + 1, r] &&
                            Accumulator[x, y, r] > Accumulator[x + 1, y, r] &&
                            Accumulator[x, y, r] > Accumulator[x + 1, y + 1, r] &&
                            Accumulator[x, y, r] > Accumulator[x, y - 1, r] &&
                            Accumulator[x, y, r] > Accumulator[x - 1, y - 1, r] &&
                            Accumulator[x, y, r] > Accumulator[x, y, r + 1] &&
                            Accumulator[x, y, r] > Accumulator[x - 1, y, r + 1] &&
                            Accumulator[x, y, r] > Accumulator[x - 1, y + 1, r + 1] &&
                            Accumulator[x, y, r] > Accumulator[x, y + 1, r + 1] &&
                            Accumulator[x, y, r] > Accumulator[x + 1, y + 1, r + 1] &&
                            Accumulator[x, y, r] > Accumulator[x + 1, y, r + 1] &&
                            Accumulator[x, y, r] > Accumulator[x + 1, y + 1, r + 1] &&
                            Accumulator[x, y, r] > Accumulator[x, y - 1, r + 1] &&
                            Accumulator[x, y, r] > Accumulator[x - 1, y - 1, r + 1]
                            )
                        {
                            bool OkToInsert = true;
                            if (_MinDistance > 0)
                            {
                                foreach (HoughCircle Circle in HoughCircles)
                                {
                                    if ( (((x - Circle.X) * (x - Circle.X)) + ((y - Circle.Y) * (y - Circle.Y))) < (_MinDistance * _MinDistance)  )
                                    {
                                        if (Circle.Threshold < Accumulator[x, y, r])
                                        {
                                            HoughCircles.Remove(Circle);
                                        }
                                        else
                                        {
                                            OkToInsert = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (OkToInsert)
                            {
                                HoughCircles.Add(new HoughCircle() { X = x, Y = y, Radius = r, Threshold = Accumulator[x, y, r] });
                            }                            
                        }
                    }
                }
            }

            // Urobime novu bitmapu s najdenymi kruznicami
            HoughImage = new Bitmap(SobelImage.Width, SobelImage.Height);
            using (Graphics g = Graphics.FromImage(HoughImage))
            {
                Pen BlackPen = new Pen(Color.Black);
                foreach(HoughCircle Circle in HoughCircles)
                {
                    int x = Circle.X - (Circle.Radius + _MinRadius);
                    int y = Circle.Y - (Circle.Radius + _MinRadius);
                    int size = (Circle.Radius + _MinRadius) * 2;
                    if (x > 0 && x < HoughImage.Width && x + size < HoughImage.Width && y > 0 && y < HoughImage.Height && y + size < HoughImage.Height)
                    {
                        g.DrawEllipse(BlackPen, x, y, size, size);
                    }                        
                }
            }

            // Finalna bitmapa, vychadzajuca z povodnej + zaznacene kruznice
            FullImage = new Bitmap(BaseImage);
            using (Graphics g = Graphics.FromImage(FullImage))
            {
                Pen BlackPen = new Pen(Color.Black);
                foreach (HoughCircle Circle in HoughCircles)
                {
                    int x = Circle.X - (Circle.Radius + _MinRadius);
                    int y = Circle.Y - (Circle.Radius + _MinRadius);
                    int size = (Circle.Radius + _MinRadius) * 2;
                    if (x > 0 && x < FullImage.Width && x + size < FullImage.Width && y > 0 && y < FullImage.Height && y + size < FullImage.Height)
                    {
                        g.DrawEllipse(BlackPen, x, y, size, size);
                    }
                }
            }

            // Uvolnit miesto
            Accumulator = null;
            EdgeMap = null;
            GC.Collect();

            _InProgress = false;
            _ImgLoaded = true;
            _ImgProcessed = true;
            if (Task2StatusChanged != null)
            {
                Task2StatusChanged(_ImgLoaded, _ImgProcessed);
            }
        }


        private void DoHoughDetection(object args)
        {
            // Priprava argumentov
            HoughArgs houghArgs = (HoughArgs)args;

            // Hladanie kruznic - v samostatnych 8 vlaknach
            for (int x = houghArgs.MinX; x < houghArgs.MaxX; x++)
            {
                //System.Diagnostics.Debug.WriteLine("X = " + x);
                for (int y = 0; y < houghArgs.SobelImageHeight; y++)
                {
                    if (EdgeMap[x, y] == false)
                        continue;
                    for (int r = 0; r <= (_MaxRadius - _MinRadius); r++)
                    {
                        int radius = r + _MinRadius;
                        for (int t = 0; t < 360; t++)
                        {
                            int a = (int)((double)x - (double)radius * Math.Cos(t * Math.PI / 180));
                            int b = (int)((double)y - (double)radius * Math.Sin(t * Math.PI / 180));
                            if (a < houghArgs.SobelImageWidth && a > 0 && b < houghArgs.SobelImageHeight && b > 0)
                            {
                                lock(AccumulatorLock)
                                {
                                    Accumulator[a, b, r] += 1;
                                }
                            }
                        }
                    }
                }
            }
        }


        private void LoadImageFile()
        {
            try
            {
                BaseImage = new Bitmap(_InputFile);
                _ImgLoaded = true;
                _ImgProcessed = false;
                if (Task2StatusChanged != null)
                {
                    Task2StatusChanged(_ImgLoaded, _ImgProcessed);
                }
            }
            catch
            {

            }
        }


        private void SobelEdgeDetect()
        {
            // vytvorime novu bitmapu z originalnej nahratej
            SobelImage = new Bitmap(BaseImage);

            // dlzka sirka
            int width = BaseImage.Width;
            int height = BaseImage.Height;

            // GX, GY - Sobelove Gradient operatory
            int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gy = new int[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

            // Inicializujeme
            int[,] BmpChannelsR = new int[width, height];
            int[,] BmpChannelsG = new int[width, height];
            int[,] BmpChannelsB = new int[width, height];

            // Gradient limit
            int limit = 128 * 128;

            // Lock bits BaseImage - rychlejsie citanie BMP dat
            BitmapData bmpDataBaseImage = BaseImage.LockBits(new Rectangle(0, 0, BaseImage.Width, BaseImage.Height), ImageLockMode.ReadOnly, BaseImage.PixelFormat);
            IntPtr BaseImagePtr = bmpDataBaseImage.Scan0;
            int bytesCountBaseImage = Math.Abs(bmpDataBaseImage.Stride) * bmpDataBaseImage.Height;
            byte[] BaseImageData = new byte[bytesCountBaseImage];
            Marshal.Copy(BaseImagePtr, BaseImageData, 0, bytesCountBaseImage);
            int nBytesPerPixelBaseImage = Image.GetPixelFormatSize(BaseImage.PixelFormat) / 8;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //BmpChannelsR[i, j] = BaseImage.GetPixel(i, j).R;
                    //BmpChannelsG[i, j] = BaseImage.GetPixel(i, j).G;
                    //BmpChannelsB[i, j] = BaseImage.GetPixel(i, j).B;
                    BmpChannelsR[i, j] = BaseImageData[(j * bmpDataBaseImage.Stride) + (i * nBytesPerPixelBaseImage)];
                    BmpChannelsG[i, j] = BaseImageData[(j * bmpDataBaseImage.Stride) + (i * nBytesPerPixelBaseImage) + 1];
                    BmpChannelsB[i, j] = BaseImageData[(j * bmpDataBaseImage.Stride) + (i * nBytesPerPixelBaseImage) + 2];
                }
            }
            BaseImage.UnlockBits(bmpDataBaseImage);


            // Lock bits SobelImage - rychlejsi zapis BMP dat 
            BitmapData bmpDataSobelImage = SobelImage.LockBits(new Rectangle(0, 0, SobelImage.Width, SobelImage.Height), ImageLockMode.ReadWrite, BaseImage.PixelFormat);
            IntPtr SobelImagePtr = bmpDataSobelImage.Scan0;
            int bytesCountSobelImage = Math.Abs(bmpDataSobelImage.Stride) * bmpDataSobelImage.Height;
            byte[] SobelImageData = new byte[bytesCountSobelImage];
            Marshal.Copy(SobelImagePtr, SobelImageData, 0, bytesCountSobelImage);
            int nBytesPerPixelSobelImage = Image.GetPixelFormatSize(SobelImage.PixelFormat) / 8;

            int Rx = 0, Ry = 0;
            int Gx = 0, Gy = 0;
            int Bx = 0, By = 0;
            int Rc, Gc, Bc;
            for (int i = 1; i < BaseImage.Width - 1; i++)
            {
                for (int j = 1; j < BaseImage.Height - 1; j++)
                {

                    Rx = 0;
                    Ry = 0;
                    Gx = 0;
                    Gy = 0;
                    Bx = 0;
                    By = 0;
                    Rc = 0;
                    Gc = 0;
                    Bc = 0;

                    for (int wi = -1; wi < 2; wi++)
                    {
                        for (int hw = -1; hw < 2; hw++)
                        {
                            Rc = BmpChannelsR[i + hw, j + wi];
                            Rx += gx[wi + 1, hw + 1] * Rc;
                            Ry += gy[wi + 1, hw + 1] * Rc;

                            Gc = BmpChannelsG[i + hw, j + wi];
                            Gx += gx[wi + 1, hw + 1] * Gc;
                            Gy += gy[wi + 1, hw + 1] * Gc;

                            Bc = BmpChannelsB[i + hw, j + wi];
                            Bx += gx[wi + 1, hw + 1] * Bc;
                            By += gy[wi + 1, hw + 1] * Bc;
                        }
                    }
                    if (Rx * Rx + Ry * Ry > limit || Gx * Gx + Gy * Gy > limit || Bx * Bx + By * By > limit)
                    {
                        //SobelImage.SetPixel(i, j, Color.Black);
                        SobelImageData[(j * bmpDataSobelImage.Stride) + (i * nBytesPerPixelSobelImage)] = 0;
                        SobelImageData[(j * bmpDataSobelImage.Stride) + (i * nBytesPerPixelSobelImage) + 1] = 0;
                        SobelImageData[(j * bmpDataSobelImage.Stride) + (i * nBytesPerPixelSobelImage) + 2] = 0;
                    }                        
                    else
                    {
                        //SobelImage.SetPixel(i, j, Color.White);
                        SobelImageData[(j * bmpDataSobelImage.Stride) + (i * nBytesPerPixelSobelImage)] = 255;
                        SobelImageData[(j * bmpDataSobelImage.Stride) + (i * nBytesPerPixelSobelImage) + 1] = 255;
                        SobelImageData[(j * bmpDataSobelImage.Stride) + (i * nBytesPerPixelSobelImage) + 2] = 255;
                    }                        
                }
            }

            // Copy back + Unlock bits
            Marshal.Copy(SobelImageData, 0, SobelImagePtr, bytesCountSobelImage);
            SobelImage.UnlockBits(bmpDataSobelImage);

        }


        public void Dispose()
        {
            if (ProcessingThread != null)
            {
                if (ProcessingThread.IsAlive)
                {
                    ProcessingThread.Abort();
                }
                ProcessingThread = null;
            }
            if (HoughThreads != null && HoughThreads.Length > 0)
            {
                for (int i = 0; i < HoughThreads.Length; i++)
                {
                    if (HoughThreads[i] != null)
                    {
                        if (HoughThreads[i].IsAlive)
                        {
                            HoughThreads[i].Abort();
                        }
                        HoughThreads[i] = null;
                    }
                }
                HoughThreads = null;
            }
        }

    }


    class HoughArgs
    {
        public int SobelImageWidth;
        public int SobelImageHeight;
        public int MinX;
        public int MaxX;
    }

    class HoughCircle
    {
        public int X;
        public int Y;
        public int Radius;
        public int Threshold;
    }

}
