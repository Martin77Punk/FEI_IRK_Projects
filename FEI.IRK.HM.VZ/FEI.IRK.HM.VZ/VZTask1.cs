using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
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


        public void PauseCapture()
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


        public void StopCapture()
        {
            PauseCapture();
            if (CaptureDevice != null)
            {
                CaptureDevice.Dispose();
                CaptureDevice = null;
            }
            SetEmptyScreens();
        }


        public void Dispose()
        {
            PauseCapture();
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
                CircleF[] RedCircles = CvInvoke.HoughCircles(RedFrame, HoughType.Gradient, 1, 100, 100, 30, 20, 0);
                CircleF[] GreenCircles = CvInvoke.HoughCircles(GreenFrame, HoughType.Gradient, 1, 100, 100, 30, 20, 0);
                CircleF[] BlueCircles = CvInvoke.HoughCircles(BlueFrame, HoughType.Gradient, 1, 100, 100, 30, 20, 0);
                CircleF[] CyanCircles = CvInvoke.HoughCircles(CyanFrame, HoughType.Gradient, 1, 100, 100, 30, 20, 0);
                CircleF[] MagentaCircles = CvInvoke.HoughCircles(MagentaFrame, HoughType.Gradient, 1, 100, 100, 30, 20, 0);
                CircleF[] YellowCircles = CvInvoke.HoughCircles(YellowFrame, HoughType.Gradient, 1, 100, 100, 30, 20, 0);

                // Triangles and squares
                List<Triangle2DF> RedTriangles = new List<Triangle2DF>();
                List<Triangle2DF> GreenTriangles = new List<Triangle2DF>();
                List<Triangle2DF> BlueTriangles = new List<Triangle2DF>();
                List<Triangle2DF> CyanTriangles = new List<Triangle2DF>();
                List<Triangle2DF> MagentaTriangles = new List<Triangle2DF>();
                List<Triangle2DF> YellowTriangles = new List<Triangle2DF>();
                List<RotatedRect> RedSquares = new List<RotatedRect>();
                List<RotatedRect> GreenSquares = new List<RotatedRect>();
                List<RotatedRect> BlueSquares = new List<RotatedRect>();
                List<RotatedRect> CyanSquares = new List<RotatedRect>();
                List<RotatedRect> MagentaSquares = new List<RotatedRect>();
                List<RotatedRect> YellowSquares = new List<RotatedRect>();
                for (int t = 1; t <= 6; t++)
                {
                    Mat Canny;
                    List<Triangle2DF> Triangles;
                    List<RotatedRect> Squares;
                    if (t == 1)
                    {
                        Canny = RedCannyFrame;
                        Triangles = RedTriangles;
                        Squares = RedSquares;
                    }
                    else if (t == 2)
                    {
                        Canny = GreenCannyFrame;
                        Triangles = GreenTriangles;
                        Squares = GreenSquares;
                    }
                    else if (t == 3)
                    {
                        Canny = BlueCannyFrame;
                        Triangles = BlueTriangles;
                        Squares = BlueSquares;
                    }
                    else if (t == 4)
                    {
                        Canny = CyanCannyFrame;
                        Triangles = CyanTriangles;
                        Squares = CyanSquares;
                    }
                    else if (t == 5)
                    {
                        Canny = MagentaFrame;
                        Triangles = MagentaTriangles;
                        Squares = MagentaSquares;
                    }
                    else
                    {
                        Canny = YellowCannyFrame;
                        Triangles = YellowTriangles;
                        Squares = YellowSquares;
                    }
                    // Find objects
                    using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
                    {
                        CvInvoke.FindContours(Canny, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                        for (int c = 0; c < contours.Size; c++)
                        {
                            using (VectorOfPoint contour = contours[c])
                            using (VectorOfPoint approxContour = new VectorOfPoint())
                            {
                                CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                                if (CvInvoke.ContourArea(approxContour, false) > 250) //only consider contours with area greater than 250
                                {
                                    if (approxContour.Size == 3) //The contour has 3 vertices, it is a triangle
                                    {
                                        Point[] pts = approxContour.ToArray();
                                        Triangles.Add(new Triangle2DF(
                                           pts[0],
                                           pts[1],
                                           pts[2]
                                           ));
                                    }
                                    else if (approxContour.Size == 4) //The contour has 4 vertices.
                                    {
                                        bool isRectangle = true;
                                        Point[] pts = approxContour.ToArray();
                                        LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                                        for (int j = 0; j < edges.Length; j++)
                                        {
                                            double angle = Math.Abs(
                                               edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                                            if (angle < 80 || angle > 100)
                                            {
                                                isRectangle = false;
                                                break;
                                            }
                                        }

                                        if (isRectangle) Squares.Add(CvInvoke.MinAreaRect(approxContour));
                                    }
                                }
                            }
                        }
                    }
                }

                _FoundObjects.Clear();
                int i = 0;

                // ---------------------------------------------------------------------------------------------------------------------

                if (RedCircles != null && RedCircles.Length > 0)
                {
                    foreach(CircleF Circle in RedCircles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Červený kruh - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Circle.Radius / 2))));
                        CvInvoke.Circle(CaptureFrame, new Point((int)Circle.Center.X, (int)Circle.Center.Y), (int)Circle.Radius, new MCvScalar(255, 255, 255));
                    }
                }

                if (GreenCircles != null && GreenCircles.Length > 0)
                {
                    foreach (CircleF Circle in GreenCircles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Zelený kruh - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Circle.Radius / 2))));
                        CvInvoke.Circle(CaptureFrame, new Point((int)Circle.Center.X, (int)Circle.Center.Y), (int)Circle.Radius, new MCvScalar(255, 255, 255));
                    }
                }

                if (BlueCircles != null && BlueCircles.Length > 0)
                {
                    foreach (CircleF Circle in BlueCircles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Modrý kruh - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Circle.Radius / 2))));
                        CvInvoke.Circle(CaptureFrame, new Point((int)Circle.Center.X, (int)Circle.Center.Y), (int)Circle.Radius, new MCvScalar(255, 255, 255));
                    }
                }

                if (CyanCircles != null && CyanCircles.Length > 0)
                {
                    foreach (CircleF Circle in CyanCircles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Tyrkysový kruh - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Circle.Radius / 2))));
                        CvInvoke.Circle(CaptureFrame, new Point((int)Circle.Center.X, (int)Circle.Center.Y), (int)Circle.Radius, new MCvScalar(255, 255, 255));
                    }
                }

                if (MagentaCircles != null && MagentaCircles.Length > 0)
                {
                    foreach (CircleF Circle in MagentaCircles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Purpurový kruh - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Circle.Radius / 2))));
                        CvInvoke.Circle(CaptureFrame, new Point((int)Circle.Center.X, (int)Circle.Center.Y), (int)Circle.Radius, new MCvScalar(255, 255, 255));
                    }
                }

                if (YellowCircles != null && YellowCircles.Length > 0)
                {
                    foreach (CircleF Circle in YellowCircles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Žltý kruh - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Circle.Radius / 2))));
                        CvInvoke.Circle(CaptureFrame, new Point((int)Circle.Center.X, (int)Circle.Center.Y), (int)Circle.Radius, new MCvScalar(255, 255, 255));
                    }
                }

                // ---------------------------------------------------------------------------------------------------------------------

                if (RedTriangles != null && RedTriangles.Count > 0)
                {
                    foreach(Triangle2DF Triangle in RedTriangles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Červený trojuholník - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Math.Sqrt(Math.Abs(Triangle.Centeroid.X - Triangle.V0.X) * Math.Abs(Triangle.Centeroid.X - Triangle.V0.X) + Math.Abs(Triangle.Centeroid.Y - Triangle.V0.Y) * Math.Abs(Triangle.Centeroid.Y - Triangle.V0.Y)) / 3))));
                        CvInvoke.Polylines(CaptureFrame, Array.ConvertAll(Triangle.GetVertices(), Point.Round), true, new MCvScalar(255, 255, 255));
                    }
                }

                if (GreenTriangles != null && GreenTriangles.Count > 0)
                {
                    foreach (Triangle2DF Triangle in GreenTriangles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Zelený trojuholník - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Math.Sqrt(Math.Abs(Triangle.Centeroid.X - Triangle.V0.X) * Math.Abs(Triangle.Centeroid.X - Triangle.V0.X) + Math.Abs(Triangle.Centeroid.Y - Triangle.V0.Y) * Math.Abs(Triangle.Centeroid.Y - Triangle.V0.Y)) / 3))));
                        CvInvoke.Polylines(CaptureFrame, Array.ConvertAll(Triangle.GetVertices(), Point.Round), true, new MCvScalar(255, 255, 255));
                    }
                }

                if (BlueTriangles != null && BlueTriangles.Count > 0)
                {
                    foreach (Triangle2DF Triangle in BlueTriangles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Modrý trojuholník - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Math.Sqrt(Math.Abs(Triangle.Centeroid.X - Triangle.V0.X) * Math.Abs(Triangle.Centeroid.X - Triangle.V0.X) + Math.Abs(Triangle.Centeroid.Y - Triangle.V0.Y) * Math.Abs(Triangle.Centeroid.Y - Triangle.V0.Y)) / 3))));
                        CvInvoke.Polylines(CaptureFrame, Array.ConvertAll(Triangle.GetVertices(), Point.Round), true, new MCvScalar(255, 255, 255));
                    }
                }

                if (CyanTriangles != null && CyanTriangles.Count > 0)
                {
                    foreach (Triangle2DF Triangle in CyanTriangles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Tyrkysový trojuholník - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Math.Sqrt(Math.Abs(Triangle.Centeroid.X - Triangle.V0.X) * Math.Abs(Triangle.Centeroid.X - Triangle.V0.X) + Math.Abs(Triangle.Centeroid.Y - Triangle.V0.Y) * Math.Abs(Triangle.Centeroid.Y - Triangle.V0.Y)) / 3))));
                        CvInvoke.Polylines(CaptureFrame, Array.ConvertAll(Triangle.GetVertices(), Point.Round), true, new MCvScalar(255, 255, 255));
                    }
                }

                if (MagentaTriangles != null && MagentaTriangles.Count > 0)
                {
                    foreach (Triangle2DF Triangle in MagentaTriangles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Purpurový trojuholník - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Math.Sqrt(Math.Abs(Triangle.Centeroid.X - Triangle.V0.X) * Math.Abs(Triangle.Centeroid.X - Triangle.V0.X) + Math.Abs(Triangle.Centeroid.Y - Triangle.V0.Y) * Math.Abs(Triangle.Centeroid.Y - Triangle.V0.Y)) / 3))));
                        CvInvoke.Polylines(CaptureFrame, Array.ConvertAll(Triangle.GetVertices(), Point.Round), true, new MCvScalar(255, 255, 255));
                    }
                }

                if (YellowTriangles != null && YellowTriangles.Count > 0)
                {
                    foreach (Triangle2DF Triangle in YellowTriangles)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Žltý trojuholník - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Math.Sqrt(Math.Abs(Triangle.Centeroid.X - Triangle.V0.X) * Math.Abs(Triangle.Centeroid.X - Triangle.V0.X) + Math.Abs(Triangle.Centeroid.Y - Triangle.V0.Y) * Math.Abs(Triangle.Centeroid.Y - Triangle.V0.Y)) / 3))));
                        CvInvoke.Polylines(CaptureFrame, Array.ConvertAll(Triangle.GetVertices(), Point.Round), true, new MCvScalar(255, 255, 255));
                    }
                }

                // ---------------------------------------------------------------------------------------------------------------------

                if (RedSquares != null && RedSquares.Count > 0)
                {
                    foreach(RotatedRect Square in RedSquares)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Červený štvoruholník - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Square.Size.Width / 4))));
                        CvInvoke.Polylines(CaptureFrame, Array.ConvertAll(Square.GetVertices(), Point.Round), true, new MCvScalar(255, 255, 255));
                    }
                }

                if (GreenSquares != null && GreenSquares.Count > 0)
                {
                    foreach (RotatedRect Square in GreenSquares)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Zelený štvoruholník - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Square.Size.Width / 4))));
                        CvInvoke.Polylines(CaptureFrame, Array.ConvertAll(Square.GetVertices(), Point.Round), true, new MCvScalar(255, 255, 255));
                    }
                }

                if (BlueSquares != null && BlueSquares.Count > 0)
                {
                    foreach (RotatedRect Square in BlueSquares)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Modrý štvoruholník - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Square.Size.Width / 4))));
                        CvInvoke.Polylines(CaptureFrame, Array.ConvertAll(Square.GetVertices(), Point.Round), true, new MCvScalar(255, 255, 255));
                    }
                }

                if (CyanSquares != null && CyanSquares.Count > 0)
                {
                    foreach (RotatedRect Square in CyanSquares)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Tyrkysový štvoruholník - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Square.Size.Width / 4))));
                        CvInvoke.Polylines(CaptureFrame, Array.ConvertAll(Square.GetVertices(), Point.Round), true, new MCvScalar(255, 255, 255));
                    }
                }

                if (MagentaSquares != null && MagentaSquares.Count > 0)
                {
                    foreach (RotatedRect Square in MagentaSquares)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Purpurový štvoruholník - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Square.Size.Width / 4))));
                        CvInvoke.Polylines(CaptureFrame, Array.ConvertAll(Square.GetVertices(), Point.Round), true, new MCvScalar(255, 255, 255));
                    }
                }

                if (YellowSquares != null && YellowSquares.Count > 0)
                {
                    foreach (RotatedRect Square in YellowSquares)
                    {
                        _FoundObjects.Add(String.Format("[{0}] Žltý štvoruholník - Vzdialenosť = {1} cm", ++i, (50 * 40 / (Square.Size.Width / 4))));
                        CvInvoke.Polylines(CaptureFrame, Array.ConvertAll(Square.GetVertices(), Point.Round), true, new MCvScalar(255, 255, 255));
                    }
                }

                // ---------------------------------------------------------------------------------------------------------------------


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
