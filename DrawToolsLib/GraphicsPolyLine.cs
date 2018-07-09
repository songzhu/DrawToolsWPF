using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace DrawToolsLib
{
    /// <summary>
    ///  PolyLine graphics object.
    /// </summary>
    public class GraphicsPolyLine : GraphicsBase
    {
        #region Class Members

        // This class member contains all required geometry.
        // It is ready for drawing and hit testing.
        protected PathGeometry pathGeometry;

        // Points from pathGeometry, including StartPoint
        protected Point[] points;
        //points数组的一份拷贝
        protected Point[] pointsOfCopy;
        //为解决旋转失真，而保存的一分点信息
        protected Point[] pointsForRorate;
        //该pathGeometry是否为手势的点，为手势的点则不需要画出来
        private bool canDraw;

        static Cursor handleCursor;

        #endregion Class Members

        #region Constructors

        public GraphicsPolyLine(Point[] points, double lineWidth, Color objectColor, double actualScale)
        {
            Fill(points, lineWidth, objectColor, actualScale);

            //RefreshDrawng();
        }


        public GraphicsPolyLine()
            :
            this(new Point[] { new Point(0.0, 0.0), new Point(100.0, 100.0) }, 1.0, Colors.Black, 1.0)
        {
        }

        static GraphicsPolyLine()
        {
            MemoryStream stream = new MemoryStream(Properties.Resources.PolyHandle);

            handleCursor = new Cursor(stream);
        }

        #endregion Constructors

        #region Other Functions

        /// <summary>
        /// Convert geometry to array of points.
        /// </summary>
        void MakePoints()
        {
            points = new Point[pathGeometry.Figures[0].Segments.Count + 1];
            

            points[0] = pathGeometry.Figures[0].StartPoint;
            

            for (int i = 0; i < pathGeometry.Figures[0].Segments.Count; i++)
            {
                points[i + 1] = ((LineSegment)(pathGeometry.Figures[0].Segments[i])).Point;
            }
        }

        /// <summary>
        /// Return array of points.
        /// </summary>
        public Point[] GetPoints()
        {
            return points;
        }
        
        /// <summary>
        /// Return array of copy points
        /// </summary>
        /// <returns></returns>
        public Point[] getPointsOfCopy() {
            return pointsOfCopy;
        }

        /// <summary>
        /// Convert array of points to geometry.
        /// </summary>
        void MakeGeometryFromPoints(ref Point[] points)
        {
            if (points == null)
            {
                // This really sucks, XML file contains Points object,
                // but list of points is empty. Do something to prevent program crush.

                points = new Point[2];
            }

            PathFigure figure = new PathFigure();

            if (points.Length >= 1)
            {
                figure.StartPoint = points[0];
            }

            for (int i = 1; i < points.Length; i++)
            {
                LineSegment segment = new LineSegment(points[i], true);
                segment.IsSmoothJoin = true;

                figure.Segments.Add(segment);
            }

            pathGeometry = new PathGeometry();

            pathGeometry.Figures.Add(figure);

            MakePoints();   // keep points array up to date
        }

        // Called from constructors
        void Fill(Point[] points, double lineWidth, Color objectColor, double actualScale)
        {
            MakeGeometryFromPoints(ref points);

            this.graphicsLineWidth = lineWidth;
            this.graphicsObjectColor = objectColor;
            this.graphicsActualScale = actualScale;
            this.CanDraw = true;
        }


        /// <summary>
        /// Add new point (line segment)
        /// </summary>
        public void AddPoint(Point point)
        {
            LineSegment segment = new LineSegment(point, true);
            segment.IsSmoothJoin = true;

            pathGeometry.Figures[0].Segments.Add(segment);

            MakePoints();   // keep points array up to date
        }

        #endregion Other Functions

        #region Overrides

        /// <summary>
        /// Draw object
        /// </summary>
        public override void Draw(DrawingContext drawingContext)
        {
            if (drawingContext == null)
            {
                throw new ArgumentNullException("drawingContext");
            }
            if (!GestureData.IsTouch)
            {
                if (CanDraw)
                {
                    drawingContext.DrawGeometry(
                    null,
                    new Pen(new SolidColorBrush(ObjectColor), ActualLineWidth),
                    pathGeometry);
                }
            }
            else
            {
                if (CanDraw)
                {
                    drawingContext.DrawGeometry(
                    null,
                    new Pen(new SolidColorBrush(ObjectColor), ActualLineWidth),
                    pathGeometry);
                }
            }

            base.Draw(drawingContext);
        }

        /// <summary>
        /// Test whether object contains point
        /// </summary>
        public override bool Contains(Point point)
        {
            return pathGeometry.FillContains(point) ||
                pathGeometry.StrokeContains(new Pen(Brushes.Black, LineHitTestWidth), point);
        }

        /// <summary>
        /// XML serialization support
        /// </summary>
        public override PropertiesGraphicsBase CreateSerializedObject()
        {
            return new PropertiesGraphicsPolyLine(this);
        }


        /// <summary>
        /// Get number of handles
        /// </summary>
        public override int HandleCount
        {
            get
            {
                return pathGeometry.Figures[0].Segments.Count + 1;
            }
        }


        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        public override Point GetHandle(int handleNumber)
        {
            if (handleNumber < 1)
                handleNumber = 1;

            if (handleNumber > points.Length)
                handleNumber = points.Length;

            return points[handleNumber - 1];
        }

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            return handleCursor;
        }

        /// <summary>
        /// Move handle to new point (resizing).
        /// handleNumber is 1-based.
        /// </summary>
        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if (handleNumber == 1)
            {
                pathGeometry.Figures[0].StartPoint = point;
            }
            else
            {
                ((LineSegment)(pathGeometry.Figures[0].Segments[handleNumber - 2])).Point = point;
            }

            MakePoints();

            RefreshDrawing();
        }


        /// <summary>
        /// Move object
        /// </summary>
        public override void Move(double deltaX, double deltaY)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X += deltaX;
                points[i].Y += deltaY;
            }
            if (pointsForRorate != null)
            {
                int length = Math.Min(points.Length, pointsForRorate.Length);
                assignPoints(length, ref pointsForRorate);
            }
            MakeGeometryFromPoints(ref points);

            RefreshDrawing();
        }


        /// <summary>
        /// Hit test.
        /// Return value: -1 - no hit
        ///                0 - hit anywhere
        ///                > 1 - handle number
        /// </summary>
        public override int MakeHitTest(Point point)
        {
            if (IsSelected)
            {
                for (int i = 1; i <= HandleCount; i++)
                {
                    if (GetHandleRectangle(i).Contains(point))
                        return i;
                }
            }

            if (Contains(point))
                return 0;

            return -1;
        }


        #endregion Overrides

        /// <summary>
        /// Test whether object intersects with rectangle
        /// </summary>
        public override bool IntersectsWith(Rect rectangle)
        {
            RectangleGeometry rg = new RectangleGeometry(rectangle);

            PathGeometry p = Geometry.Combine(rg, pathGeometry, GeometryCombineMode.Intersect, null);

            return (!p.IsEmpty());
        }

        public override void Zoom(double scale, Point center)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X = points[i].X * scale + (1 - scale)*center.X;
                points[i].Y = points[i].Y * scale + (1 - scale)*center.Y;
            }
            if(pointsForRorate != null)
            {
                int length = Math.Min(points.Length, pointsForRorate.Length);
                assignPoints(length, ref pointsForRorate);
            }
            

            MakeGeometryFromPoints(ref points);

            RefreshDrawing();
        }

        public void assignPoints(int length, ref Point[] ps)
        {
            for (int i = 0; i < length; i++)
            {
                ps[i] = points[i];
            }
        }

        /// <summary>
        /// 将初始点拷贝一份，在moveUp或者touchUp执行的时候进行调用
        /// </summary>
        public override void CopyPoints()
        {
            if (!IsSigned)
            {
                pointsOfCopy = new Point[pathGeometry.Figures[0].Segments.Count + 1];

                pointsForRorate = new Point[pathGeometry.Figures[0].Segments.Count + 1];

                pointsOfCopy[0] = pathGeometry.Figures[0].StartPoint;

                pointsForRorate[0] = pathGeometry.Figures[0].StartPoint;

                assignPoints(pathGeometry.Figures[0].Segments.Count, ref pointsOfCopy);

                assignPoints(pathGeometry.Figures[0].Segments.Count, ref pointsForRorate);

                IsSigned = true;
            }
        }

        public override void Reset(double scale, Point center)
        {
            if (pointsOfCopy == null)
            {
                return;
            }
            for (int i = 0; i < pointsOfCopy.Length; i++)
            {
                points[i].X = pointsOfCopy[i].X * scale + (1 - scale) * center.X;
                points[i].Y = pointsOfCopy[i].Y * scale + (1 - scale) * center.Y;
            }

            MakeGeometryFromPoints(ref points);

            RefreshDrawing();
        }

        public override void Rotate(double angle, Point center)
        {

            if (pointsForRorate == null)
            {
                return;
            }
            double dCos = Math.Cos(angle);
            double dSin = Math.Sin(angle);
            
            for (int i = 0; i < pointsForRorate.Length; i++)
            {
                //points[i].X =(points[i].X - center.X) * dCos - (points[i].Y - center.Y) * dSin + center.X;
                //points[i].Y =(points[i].X - center.X) * dSin + (points[i].Y - center.Y) * dCos + center.Y;
                points[i].X = (pointsForRorate[i].X - center.X) * dCos - (pointsForRorate[i].Y - center.Y) * dSin + center.X;
                points[i].Y = (pointsForRorate[i].X - center.X) * dSin + (pointsForRorate[i].Y - center.Y) * dCos + center.Y;
            }

            MakeGeometryFromPoints(ref points);

            RefreshDrawing();
        }

        /// <summary>
        /// 触摸设备Id，用来区分不同的手指
        /// </summary>
        public int DeviceID { get; set; }
        public bool CanDraw { get => canDraw; set => canDraw = value; }
    }
}
