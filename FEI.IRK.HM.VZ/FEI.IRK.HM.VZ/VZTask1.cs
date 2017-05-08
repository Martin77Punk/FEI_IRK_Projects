using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Diagnostics;

namespace FEI.IRK.HM.VZ
{
    public class VZTask1 : IDisposable
    {
        // 1280x720, 800x600
        public static readonly int FRAME_FPS = 25;
        public static readonly int FRAME_WIDTH = 800;
        public static readonly int FRAME_HEIGHT = 600;

        public delegate void FrameUpdatedFunction();
        public event FrameUpdatedFunction FrameUpdated;
        public delegate void FoundObjectsFunction(string[] FoundObjects);
        public event FoundObjectsFunction FoundObjectsUpdated;

        private object syncLock;

        private Thread CaptureThread;
        private VideoCapture CaptureDevice;
        private bool _FlipHorizontal;
        private bool _FlipVertical;
        private double _Contrast;
        private List<string> _FoundObjects;
        
        private Mat _MainFrame;
        private Mat _HsvFrame;

        private Mat _RedFrame;
        private Mat _RedCannyFrame;
        private Mat _GreenFrame;
        private Mat _GreenCannyFrame;
        private Mat _BlueFrame;
        private Mat _BlueCannyFrame;

        private Mat _CyanFrame;
        private Mat _CyanCannyFrame;
        private Mat _MagentaFrame;
        private Mat _MagentaCannyFrame;
        private Mat _YellowFrame;
        private Mat _YellowCannyFrame;


        public bool FlipHorizontal
        {
            get
            {
                return _FlipHorizontal;
            }
            set
            {
                _FlipHorizontal = value;
                if (CaptureDevice != null && CaptureDevice.Ptr != IntPtr.Zero)
                {
                    CaptureDevice.FlipHorizontal = _FlipHorizontal;
                }
            }
        }

        public bool FlipVertical
        {
            get
            {
                return _FlipVertical;
            }
            set
            {
                _FlipVertical = value;
                if (CaptureDevice != null && CaptureDevice.Ptr != IntPtr.Zero)
                {
                    CaptureDevice.FlipVertical = _FlipVertical;
                }
            }
        }


        public double Contrast
        {
            get
            {
                return _Contrast;
            }
            set
            {
                _Contrast = value;
            }
        }


        public string[] FoundObjects
        {
            get
            {
                return _FoundObjects.ToArray();
            }
        }


        public Mat MainFrame
        {
            get
            {
                lock(syncLock)
                {
                    return _MainFrame;
                }
            }
        }

        public Mat HsvFrame
        {
            get
            {
                lock(syncLock)
                {
                    return _HsvFrame;
                }
            }
        }

        public Mat RedFrame
        {
            get
            {
                lock(syncLock)
                {
                    return _RedFrame;
                }
            }
        }


        public Mat RedCannyFrame
        {
            get
            {
                lock(syncLock)
                {
                    return _RedCannyFrame;
                }
            }
        }

        public Mat GreenFrame
        {
            get
            {
                lock (syncLock)
                {
                    return _GreenFrame;
                }
            }
        }


        public Mat GreenCannyFrame
        {
            get
            {
                lock (syncLock)
                {
                    return _GreenCannyFrame;
                }
            }
        }

        public Mat BlueFrame
        {
            get
            {
                lock (syncLock)
                {
                    return _BlueFrame;
                }
            }
        }


        public Mat BlueCannyFrame
        {
            get
            {
                lock (syncLock)
                {
                    return _BlueCannyFrame;
                }
            }
        }

        public Mat CyanFrame
        {
            get
            {
                lock (syncLock)
                {
                    return _CyanFrame;
                }
            }
        }


        public Mat CyanCannyFrame
        {
            get
            {
                lock (syncLock)
                {
                    return _CyanCannyFrame;
                }
            }
        }



        public Mat MagentaFrame
        {
            get
            {
                lock (syncLock)
                {
                    return _MagentaFrame;
                }
            }
        }


        public Mat MagentaCannyFrame
        {
            get
            {
                lock (syncLock)
                {
                    return _MagentaCannyFrame;
                }
            }
        }



        public Mat YellowFrame
        {
            get
            {
                lock (syncLock)
                {
                    return _YellowFrame;
                }
            }
        }


        public Mat YellowCannyFrame
        {
            get
            {
                lock (syncLock)
                {
                    return _YellowCannyFrame;
                }
            }
        }

        

        public VZTask1()
        {
            CvInvoke.UseOpenCL = true;
            syncLock = new object();
            _FlipHorizontal = false;
            _FlipVertical = false;
            _Contrast = 1.0;
            _FoundObjects = new List<string>();
            SetEmptyScreens();
        }


        public void StartCapture()
        {
            if (CaptureThread == null)
            {
                CaptureThread = new Thread(new ThreadStart(CaptureProcess));
                CaptureThread.Name = "Image Processing Thread";
            }
            if (!CaptureThread.IsAlive)
            {
                CaptureThread.Start();
            }
        }


        public void StopCapture()
        {
            if (CaptureThread != null)
            {
                if (CaptureThread.IsAlive)
                {
                    CaptureThread.Abort();
                }
                CaptureThread = null;
            }
        }


        public void Dispose()
        {
            StopCapture();
            if (CaptureDevice != null)
            {
                CaptureDevice.Dispose();
                CaptureDevice = null;
            }
        }


        private void CaptureProcess()
        {
            if (CaptureDevice == null)
            {
                SetProcessingScreens();
                try
                {
                    CaptureDevice = new VideoCapture();
                    CaptureDevice.SetCaptureProperty(CapProp.Fps, FRAME_FPS);
                    CaptureDevice.SetCaptureProperty(CapProp.FrameWidth, FRAME_WIDTH);
                    CaptureDevice.SetCaptureProperty(CapProp.FrameHeight, FRAME_HEIGHT);
                    CaptureDevice.FlipHorizontal = _FlipHorizontal;
                    CaptureDevice.FlipVertical = _FlipVertical;
                    CaptureDevice.Start();
                }
                catch
                {
                    return;
                }
            }
            else
            {
                CaptureDevice.Start();
            }


            Stopwatch watch = Stopwatch.StartNew();
            long FpsWatch = (long)(1000 / FRAME_FPS);

            Mat CaptureFrame = new Mat();
            Mat HsvFrame = new Mat();
            Mat RedLowerFrame = new Mat();
            Mat RedUpperFrame = new Mat();
            Mat RedFrame = new Mat();
            Mat RedCannyFrame = new Mat();
            Mat GreenFrame = new Mat();
            Mat GreenCannyFrame = new Mat();
            Mat BlueFrame = new Mat();
            Mat BlueCannyFrame = new Mat();
            Mat CyanFrame = new Mat();
            Mat CyanCannyFrame = new Mat();
            Mat MagentaFrame = new Mat();
            Mat MagentaCannyFrame = new Mat();
            Mat YellowFrame = new Mat();
            Mat YellowCannyFrame = new Mat();

            while (CaptureDevice.IsOpened)
            {
                
                CaptureFrame = CaptureDevice.QueryFrame();
                if (CaptureFrame == null)
                    break;

                CaptureFrame.ConvertTo(CaptureFrame, DepthType.Default, _Contrast, 0);

                CvInvoke.CvtColor(CaptureFrame, HsvFrame, ColorConversion.Bgr2Hsv);

                //Red levels HUE = 0 +/ -15
                CvInvoke.InRange(HsvFrame, new ScalarArray(new MCvScalar(0, 100, 50)), new ScalarArray(new MCvScalar(15, 255, 255)), RedLowerFrame);
                CvInvoke.InRange(HsvFrame, new ScalarArray(new MCvScalar(165, 100, 50)), new ScalarArray(new MCvScalar(180, 255, 255)), RedUpperFrame);
                CvInvoke.AddWeighted(RedLowerFrame, 1.0, RedUpperFrame, 1.0, 0.0, RedFrame);
                //CvInvoke.InRange(HsvFrame, new ScalarArray(new MCvScalar(0, 100, 50)), new ScalarArray(new MCvScalar(20, 255, 255)), RedLowerFrame);
                //CvInvoke.InRange(HsvFrame, new ScalarArray(new MCvScalar(160, 100, 50)), new ScalarArray(new MCvScalar(180, 255, 255)), RedUpperFrame);
                //CvInvoke.AddWeighted(RedLowerFrame, 1.0, RedUpperFrame, 1.0, 0.0, RedFrame);

                // Green levels HUE = 60 +/- 15
                CvInvoke.InRange(HsvFrame, new ScalarArray(new MCvScalar(45, 50, 80)), new ScalarArray(new MCvScalar(75, 255, 255)), GreenFrame);
                //CvInvoke.InRange(HsvFrame, new ScalarArray(new MCvScalar(40, 50, 80)), new ScalarArray(new MCvScalar(80, 255, 255)), GreenFrame);

                // Blue levels HUE = 120 +/- 15
                CvInvoke.InRange(HsvFrame, new ScalarArray(new MCvScalar(105, 50, 80)), new ScalarArray(new MCvScalar(135, 255, 255)), BlueFrame);
                //CvInvoke.InRange(HsvFrame, new ScalarArray(new MCvScalar(100, 50, 80)), new ScalarArray(new MCvScalar(140, 255, 255)), BlueFrame);

                // Cyan levels HUE = 90 +/- 15
                CvInvoke.InRange(HsvFrame, new ScalarArray(new MCvScalar(75, 50, 80)), new ScalarArray(new MCvScalar(105, 255, 255)), CyanFrame);

                // Magenta levels HUE = 150 +/- 15
                CvInvoke.InRange(HsvFrame, new ScalarArray(new MCvScalar(135, 50, 80)), new ScalarArray(new MCvScalar(165, 255, 255)), MagentaFrame);

                // Yellow levels HUE = 30 +/- 15
                CvInvoke.InRange(HsvFrame, new ScalarArray(new MCvScalar(15, 50, 80)), new ScalarArray(new MCvScalar(45, 255, 255)), YellowFrame);


                // Erode + Dilate
                CvInvoke.Erode(RedFrame, RedFrame, CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(10, 10), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Dilate(RedFrame, RedFrame, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Dilate(RedFrame, RedFrame, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Erode(RedFrame, RedFrame, CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(10, 10), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Erode(GreenFrame, GreenFrame, CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(10, 10), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Dilate(GreenFrame, GreenFrame, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Dilate(GreenFrame, GreenFrame, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Erode(GreenFrame, GreenFrame, CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(10, 10), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Erode(BlueFrame, BlueFrame, CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(10, 10), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Dilate(BlueFrame, BlueFrame, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Dilate(BlueFrame, BlueFrame, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Erode(BlueFrame, BlueFrame, CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(10, 10), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Erode(CyanFrame, CyanFrame, CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(10, 10), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Dilate(CyanFrame, CyanFrame, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Dilate(CyanFrame, CyanFrame, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Erode(CyanFrame, CyanFrame, CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(10, 10), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Erode(MagentaFrame, MagentaFrame, CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(10, 10), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Dilate(MagentaFrame, MagentaFrame, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Dilate(MagentaFrame, MagentaFrame, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Erode(MagentaFrame, MagentaFrame, CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(10, 10), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Erode(YellowFrame, YellowFrame, CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(10, 10), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Dilate(YellowFrame, YellowFrame, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Dilate(YellowFrame, YellowFrame, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.Erode(YellowFrame, YellowFrame, CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(10, 10), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar());

                // Gaussian Blur
                CvInvoke.GaussianBlur(RedFrame, RedFrame, new Size(9, 9), 2, 2);
                CvInvoke.GaussianBlur(GreenFrame, GreenFrame, new Size(9, 9), 2, 2);
                CvInvoke.GaussianBlur(BlueFrame, BlueFrame, new Size(9, 9), 2, 2);
                CvInvoke.GaussianBlur(CyanFrame, CyanFrame, new Size(9, 9), 2, 2);
                CvInvoke.GaussianBlur(MagentaFrame, MagentaFrame, new Size(9, 9), 2, 2);
                CvInvoke.GaussianBlur(YellowFrame, YellowFrame, new Size(9, 9), 2, 2);

                // Canny
                CvInvoke.Canny(RedFrame, RedCannyFrame, 10, 20);
                CvInvoke.Canny(GreenFrame, GreenCannyFrame, 10, 20);
                CvInvoke.Canny(BlueFrame, BlueCannyFrame, 10, 20);
                CvInvoke.Canny(CyanFrame, CyanCannyFrame, 10, 20);
                CvInvoke.Canny(MagentaFrame, MagentaCannyFrame, 10, 20);
                CvInvoke.Canny(YellowFrame, YellowCannyFrame, 10, 20);

                // Hough Circles
                CircleF[] RedCircles = CvInvoke.HoughCircles(RedFrame, HoughType.Gradient, 1, 100, 100, 20, 50, 0);
                CircleF[] GreenCircles = CvInvoke.HoughCircles(GreenFrame, HoughType.Gradient, 1, 100, 100, 20, 50, 0);
                CircleF[] BlueCircles = CvInvoke.HoughCircles(BlueFrame, HoughType.Gradient, 1, 100, 100, 20, 50, 0);
                CircleF[] CyanCircles = CvInvoke.HoughCircles(CyanFrame, HoughType.Gradient, 1, 100, 100, 20, 50, 0);
                CircleF[] MagentaCircles = CvInvoke.HoughCircles(MagentaFrame, HoughType.Gradient, 1, 100, 100, 20, 50, 0);
                CircleF[] YellowCircles = CvInvoke.HoughCircles(YellowFrame, HoughType.Gradient, 1, 100, 100, 20, 50, 0);

                _FoundObjects.Clear();
                int i = 0;

                if (RedCircles != null && RedCircles.Length > 0)
                {
                    foreach(CircleF Circle in RedCircles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Červený kruh - X = {1}; Y = {2}; R = {3}", ++i, Circle.Center.X.ToString("C3"), Circle.Center.Y.ToString("C3"), Circle.Radius.ToString("C3")));
                        CvInvoke.Circle(CaptureFrame, new Point((int)Circle.Center.X, (int)Circle.Center.Y), (int)Circle.Radius, new MCvScalar(255, 255, 255));
                    }
                }

                if (GreenCircles != null && GreenCircles.Length > 0)
                {
                    foreach (CircleF Circle in GreenCircles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Zelený kruh - X = {1}; Y = {2}; R = {3}", ++i, Circle.Center.X.ToString("C3"), Circle.Center.Y.ToString("C3"), Circle.Radius.ToString("C3")));
                        CvInvoke.Circle(CaptureFrame, new Point((int)Circle.Center.X, (int)Circle.Center.Y), (int)Circle.Radius, new MCvScalar(255, 255, 255));
                    }
                }

                if (BlueCircles != null && BlueCircles.Length > 0)
                {
                    foreach (CircleF Circle in BlueCircles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Modrý kruh - X = {1}; Y = {2}; R = {3}", ++i, Circle.Center.X.ToString("C3"), Circle.Center.Y.ToString("C3"), Circle.Radius.ToString("C3")));
                        CvInvoke.Circle(CaptureFrame, new Point((int)Circle.Center.X, (int)Circle.Center.Y), (int)Circle.Radius, new MCvScalar(255, 255, 255));
                    }
                }

                if (CyanCircles != null && CyanCircles.Length > 0)
                {
                    foreach (CircleF Circle in CyanCircles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Tyrkysový kruh - X = {1}; Y = {2}; R = {3}", ++i, Circle.Center.X.ToString("C3"), Circle.Center.Y.ToString("C3"), Circle.Radius.ToString("C3")));
                        CvInvoke.Circle(CaptureFrame, new Point((int)Circle.Center.X, (int)Circle.Center.Y), (int)Circle.Radius, new MCvScalar(255, 255, 255));
                    }
                }

                if (MagentaCircles != null && MagentaCircles.Length > 0)
                {
                    foreach (CircleF Circle in MagentaCircles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Purpurový kruh - X = {1}; Y = {2}; R = {3}", ++i, Circle.Center.X.ToString("C3"), Circle.Center.Y.ToString("C3"), Circle.Radius.ToString("C3")));
                        CvInvoke.Circle(CaptureFrame, new Point((int)Circle.Center.X, (int)Circle.Center.Y), (int)Circle.Radius, new MCvScalar(255, 255, 255));
                    }
                }

                if (YellowCircles != null && YellowCircles.Length > 0)
                {
                    foreach (CircleF Circle in YellowCircles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Žltý kruh - X = {1}; Y = {2}; R = {3}", ++i, Circle.Center.X.ToString("C3"), Circle.Center.Y.ToString("C3"), Circle.Radius.ToString("C3")));
                        CvInvoke.Circle(CaptureFrame, new Point((int)Circle.Center.X, (int)Circle.Center.Y), (int)Circle.Radius, new MCvScalar(255, 255, 255));
                    }
                }


                if (FoundObjectsUpdated != null)
                {
                    FoundObjectsUpdated(_FoundObjects.ToArray());
                }

                lock (syncLock)
                {
                    _MainFrame = CaptureFrame;
                    _HsvFrame = HsvFrame;
                    _RedFrame = RedFrame;
                    _RedCannyFrame = RedCannyFrame;
                    _GreenFrame = GreenFrame;
                    _GreenCannyFrame = GreenCannyFrame;
                    _BlueFrame = BlueFrame;
                    _BlueCannyFrame = BlueCannyFrame;
                    _CyanFrame = CyanFrame;
                    _CyanCannyFrame = CyanCannyFrame;
                    _MagentaFrame = MagentaFrame;
                    _MagentaCannyFrame = MagentaCannyFrame;
                    _YellowFrame = YellowFrame;
                    _YellowCannyFrame = YellowCannyFrame;
                }

                // Fire event to inform frame was updated
                if (FrameUpdated != null) FrameUpdated();


                // Wait for next frame
                watch.Stop();
                if (watch.ElapsedMilliseconds < FpsWatch)
                {
                    Thread.Sleep((int)(FpsWatch - watch.ElapsedMilliseconds));
                }
                watch.Reset();
                watch.Start();
            }

            

        }


        private void SetEmptyScreens()
        {
            // Create initial Frame
            Mat InitialFrame = new Mat(new Size(FRAME_WIDTH, FRAME_HEIGHT), DepthType.Cv8U, 3);
            CvInvoke.Rectangle(InitialFrame, new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT), new Bgr(Color.AliceBlue).MCvScalar, -1);
            CvInvoke.PutText(InitialFrame, "Spusti webkameru", new Point(250, 300), FontFace.HersheySimplex, 1, new Bgr(Color.Black).MCvScalar, 3);
            // Set frame to all Image boxes
            lock (syncLock)
            {
                _MainFrame = InitialFrame;
                _HsvFrame = InitialFrame;
                _RedFrame = InitialFrame;
                _RedCannyFrame = InitialFrame;
                _GreenFrame = InitialFrame;
                _GreenCannyFrame = InitialFrame;
                _BlueFrame = InitialFrame;
                _BlueCannyFrame = InitialFrame;
                _CyanFrame = InitialFrame;
                _CyanCannyFrame = InitialFrame;
                _MagentaFrame = InitialFrame;
                _MagentaCannyFrame = InitialFrame;
                _YellowFrame = InitialFrame;
                _YellowCannyFrame = InitialFrame;
            }
            if (FrameUpdated != null) FrameUpdated();
        }


        private void SetProcessingScreens()
        {
            // Create processing Frame
            Mat ProcessingFrame = new Mat(new Size(FRAME_WIDTH, FRAME_HEIGHT), DepthType.Cv8U, 3);
            CvInvoke.Rectangle(ProcessingFrame, new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT), new Bgr(Color.AliceBlue).MCvScalar, -1);
            CvInvoke.PutText(ProcessingFrame, "Inicializujem webkameru", new Point(200, 300), FontFace.HersheySimplex, 1, new Bgr(Color.Black).MCvScalar, 3);
            // Set frame to all Image boxes
            lock (syncLock)
            {
                _MainFrame = ProcessingFrame;
                _HsvFrame = ProcessingFrame;
                _RedFrame = ProcessingFrame;
                _RedCannyFrame = ProcessingFrame;
                _GreenFrame = ProcessingFrame;
                _GreenCannyFrame = ProcessingFrame;
                _BlueFrame = ProcessingFrame;
                _BlueCannyFrame = ProcessingFrame;
                _CyanFrame = ProcessingFrame;
                _CyanCannyFrame = ProcessingFrame;
                _MagentaFrame = ProcessingFrame;
                _MagentaCannyFrame = ProcessingFrame;
                _YellowFrame = ProcessingFrame;
                _YellowCannyFrame = ProcessingFrame;
            }
            if (FrameUpdated != null) FrameUpdated();
        }

        
    }
}
