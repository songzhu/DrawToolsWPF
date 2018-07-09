using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;



namespace DrawToolsLib
{
    /// <summary>
    ///  Line graphics object.
    /// </summary>
    public class GraphicsLine : GraphicsBase
    {
        #region Class Members

        protected Point lineStart;
        protected Point lineEnd;

        protected Point lineStartOfCopy;
        protected Point lineEndOfCopy;
        #endregion Class Members

        #region Constructors

        public GraphicsLine(Point start, Point end, double lineWidth, Color objectColor, double actualScale)
        {
            this.lineStart = start;
            this.lineEnd = end;
            this.graphicsLineWidth = lineWidth;
            this.graphicsObjectColor = objectColor;
            this.graphicsActualScale = actualScale;

            //RefreshDrawng();
        }

        public GraphicsLine()
            :
            this(new Point(0.0, 0.0), new Point(100.0, 100.0), 1.0, Colors.Black, 1.0)
        {
        }

        #endregion Constructors

        #region Properties

        public Point StartOfCopy
        {
            get { return lineStartOfCopy; }
            set { lineStartOfCopy = value; }
        }

        public Point EndOfCopy
        {
            get { return lineEndOfCopy; }
            set { lineEndOfCopy = value; }
        }

        public Point Start
        {
            get { return lineStart; }
            set { lineStart = value; }
        }

        public Point End
        {
            get { return lineEnd; }
            set { lineEnd = value; }
        }

        #endregion Properties

        #region Overrides

        /// <summary>
        /// Draw object
        /// </summary>
        public override void Draw(DrawingContext drawingContext)
        {
            if ( drawingContext == null )
            {
                throw new ArgumentNullException("drawingContext");
            }
            //Color.FromArgb(0, 0, 0, 0)，ObjectColor
            drawingContext.DrawLine(
                //Color.FromArgb(255, 0, 0, 0)
                new Pen(new SolidColorBrush(ObjectColor), ActualLineWidth),
                lineStart,
                lineEnd);
            
            base.Draw(drawingContext);
        }

        /// <summary>
        /// Test whether object contains point
        /// </summary>
        public override bool Contains(Point point)
        {
            LineGeometry g = new LineGeometry(lineStart, lineEnd);

            return g.StrokeContains(new Pen(Brushes.Black, LineHitTestWidth), point);
        }

        /// <summary>
        /// XML serialization support
        /// </summary>
        /// <returns></returns>
        public override PropertiesGraphicsBase CreateSerializedObject()
        {
            return new PropertiesGraphicsLine(this);
        }

        /// <summary>
        /// Get number of handles
        /// </summary>
        public override int HandleCount
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        public override Point GetHandle(int handleNumber)
        {
            if (handleNumber == 1)
                return lineStart;
            else
                return lineEnd;
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


        /// <summary>
        /// Test whether object intersects with rectangle
        /// </summary>
        public override bool IntersectsWith(Rect rectangle)
        {
            RectangleGeometry rg = new RectangleGeometry(rectangle);

            LineGeometry lg = new LineGeometry(lineStart, lineEnd);
            PathGeometry widen = lg.GetWidenedPathGeometry(new Pen(Brushes.Black, LineHitTestWidth));

            PathGeometry p = Geometry.Combine(rg, widen, GeometryCombineMode.Intersect, null);

            return (!p.IsEmpty());
        }

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch (handleNumber)
            {
                case 1:
                case 2:
                    return Cursors.SizeAll;
                default:
                    return HelperFunctions.DefaultCursor;
            }
        }

        /// <summary>
        /// Move handle to new point (resizing)
        /// </summary>
        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if (handleNumber == 1)
            {
                lineStart = point;
                lineStartOfCopy = point;
            }
            else
            {
                lineEnd = point;
                lineEndOfCopy = point;
            }
            RefreshDrawing();
        }

        /// <summary>
        /// Move object
        /// </summary>
        public override void Move(double deltaX, double deltaY)
        {
            lineStart.X += deltaX;
            lineStart.Y += deltaY;

            lineEnd.X += deltaX;
            lineEnd.Y += deltaY;

            RefreshDrawing();
        }

        public override void Zoom(double scale, Point center)
        {
            lineStart.X = lineStart.X * scale + (1 - scale) * center.X;
            lineStart.Y = lineStart.Y * scale + (1 - scale) * center.Y;
            lineEnd.X = lineEnd.X * scale + (1 - scale) * center.X;
            lineEnd.Y = lineEnd.Y * scale + (1 - scale) * center.Y;
            RefreshDrawing();

        }
        /// <summary>
        /// 将初始点拷贝一份，在moveUp或者touchUp执行的时候进行调用
        /// </summary>
        public override void CopyPoints()
        {
            //如果未被赋值，才赋值
            if (!IsSigned)
            {
                this.lineStartOfCopy = this.lineStart;
                this.lineEndOfCopy = this.lineEnd;
                IsSigned = true;
            }
        }

        public override void Reset(double scale, Point center)
        {      
            lineStart.X = lineStartOfCopy.X * scale + (1 - scale)*center.X;
            lineStart.Y = lineStartOfCopy.Y * scale + (1 - scale)*center.Y;
            lineEnd.X = lineEndOfCopy.X * scale + (1 - scale)*center.X;
            lineEnd.Y = lineEndOfCopy.Y * scale + (1 - scale)*center.Y;
        }

        public override void Rotate(double angle, Point center)
        {
            double dCos = Math.Cos(angle);
            double dSin = Math.Sin(angle);
            double tx = lineStart.X;
            double ty = lineStart.Y;
            lineStart.X = (tx - center.X) * dCos - (ty - center.Y) * dSin + center.X;
            lineStart.Y = (tx - center.X) * dSin + (ty - center.Y) * dCos + center.Y;
            tx = lineEnd.X;
            ty = lineEnd.Y;
            lineEnd.X = (tx - center.X) * dCos - (ty - center.Y) * dSin + center.X;
            lineEnd.Y = (tx - center.X) * dSin + (ty - center.Y) * dCos + center.Y;

            RefreshDrawing();
        }

        #endregion Overrides
    }
}
