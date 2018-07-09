using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;

namespace DrawToolsLib
{
    /// <summary>
    /// Polyline tool
    /// </summary>
    class ToolPolyLine : ToolObject
    {
        private double lastX;
        private double lastY;
        private GraphicsPolyLine newPolyLine;
        private const double minDistance = 15;
        private readonly Dictionary<int, GraphicsPolyLine> _activeStrokes = new Dictionary<int, GraphicsPolyLine>();

        public ToolPolyLine()
        {
            MemoryStream stream = new MemoryStream(Properties.Resources.Pencil);
            ToolCursor = new Cursor(stream);
        }

        /// <summary>
        /// Create new object
        /// </summary>
        public override void OnMouseDown(DrawingCanvas drawingCanvas, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(drawingCanvas);
            GestureData.IsTouch = false;
            newPolyLine = new GraphicsPolyLine(
                new Point[]
                {
                    p,
                    new Point(p.X + 1, p.Y + 1)
                },
                drawingCanvas.LineWidth,
                drawingCanvas.ObjectColor,
                drawingCanvas.ActualScale);

            AddNewObject(drawingCanvas, newPolyLine);

            lastX = p.X;
            lastY = p.Y;
            System.Diagnostics.Debug.WriteLine("Mousedown");
        }

        /// <summary>
        /// Set cursor and resize new polyline
        /// </summary>
        public override void OnMouseMove(DrawingCanvas drawingCanvas, MouseEventArgs e)
        {
            drawingCanvas.Cursor = ToolCursor;

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            if ( ! drawingCanvas.IsMouseCaptured )
            {
                return;
            }

            if ( newPolyLine == null )
            {
                return;         // precaution
            }

            Point p = e.GetPosition(drawingCanvas);

            double distance = (p.X - lastX) * (p.X - lastX) + (p.Y - lastY) * (p.Y - lastY);

            double d = drawingCanvas.ActualScale <= 0 ? 
                minDistance * minDistance :
                minDistance * minDistance / drawingCanvas.ActualScale;

            if ( distance < d)
            {
                // Distance between last two points is less than minimum -
                // move last point
                newPolyLine.MoveHandleTo(p, newPolyLine.HandleCount);
            }
            else
            {
                // Add new segment
                newPolyLine.AddPoint(p);

                lastX = p.X;
                lastY = p.Y;
            }
            System.Diagnostics.Debug.WriteLine("Mousemove");
        }

        public override void OnMouseUp(DrawingCanvas drawingCanvas, MouseButtonEventArgs e)
        {
            newPolyLine = null;
            base.OnMouseUp(drawingCanvas, e);
            System.Diagnostics.Debug.WriteLine("Mouseup");
        }

        #region Touch Handler
        // Touch down event handler.
        /// <summary>
        /// 输入一个点，判定是否为手势，是则不需要在继续判断，手指离开时，将手势标识设置为false
        /// 如果不是则继续判断是否为手势.
        /// </summary>
        /// <param name="drawingCanvas"></param>
        /// <param name="e"></param>
        public void TouchDownHandler(DrawingCanvas drawingCanvas, TouchEventArgs e)
        {
            GestureData.Fingers++;//记录手指数
            GraphicsPolyLine polyLine;
            TouchPoint touchPoint = e.GetTouchPoint(drawingCanvas);
            Point  p = touchPoint.Position;
            if (GestureData.Fingers == 1)//记录第一个手指的基本信息，后后续手势判断使用
            {
                if (!GestureData.IsSignFoFingerOne)
                {
                    GestureData.IsTouch = true;
                    GestureData.FirstDeviceId = e.TouchDevice.Id;
                    GestureData.FirstFingerBeginPoint = p;
                    GestureData.FirstFingerCurPoint = p;
                    GestureData.IsSignFoFingerOne = true;
                }
            }

            HelperFunctions.IsGesture(drawingCanvas, e);//判断是否为手势
                
            //如果是手势，则将第一个手指画的几何型的Candraw属性置为false,使得后续不在画出来
            if (GestureData.IsGesture && GestureData.Fingers == 2 && _activeStrokes.TryGetValue(GestureData.FirstDeviceId, out polyLine))
            {
                polyLine.CanDraw = false;
            }

            if (_activeStrokes.TryGetValue(e.TouchDevice.Id, out polyLine))
            {
                FinishStroke(polyLine);
                return;
            }

            //当手指的个数大于2时，将第一个手指与第二个手指的canDraw属性置回true
            if (!GestureData.IsGesture && GestureData.Fingers > 2)
            {
                if (_activeStrokes.TryGetValue(GestureData.FirstDeviceId, out polyLine))
                {
                    polyLine.CanDraw = true;
                }
                if (_activeStrokes.TryGetValue(GestureData.SecondDeviceId, out polyLine))
                {
                    polyLine.CanDraw = true;
                }
            }

            // Create new stroke, add point and assign a color to it.
            GraphicsPolyLine newPolyLine = new GraphicsPolyLine(
                new Point[]
                {
                    p,
                    new Point(p.X + 1, p.Y + 1)
                },
                drawingCanvas.LineWidth,
                drawingCanvas.ObjectColor,
                drawingCanvas.ActualScale);
            newPolyLine.DeviceID = e.TouchDevice.Id;
            //是手势并且是第二个手指，将其画的几何的CanDraw置为false，不用画
            if (GestureData.IsGesture && GestureData.Fingers == 2)
            {
                if (!GestureData.IsSignForFingerTwo)
                {
                    GestureData.SecondDeviceId = e.TouchDevice.Id;
                    GestureData.SecFingerBeginPoint = p;
                    GestureData.StartDistInTwoFingers = HelperFunctions.CalcDistanceSquare(GestureData.FirstFingerBeginPoint, GestureData.SecFingerBeginPoint);
                    GestureData.StartCenterPointInTwoFingers = HelperFunctions.getCenterPoint(GestureData.FirstFingerBeginPoint, GestureData.SecFingerBeginPoint);
                    GestureData.SecFingerCurPoint = p;
                    newPolyLine.CanDraw = false;
                    GestureData.IsSignForFingerTwo = true;
                    if (GestureData.FirstFingerBeginPoint.X < GestureData.SecFingerBeginPoint.X)
                    {
                        GestureData.MaxPositionX = true;
                    }
                }
            }
            //防止已经是手势了，但因为用户不小心使得其他手指不小心碰到了触控，需要使得该误碰的手写内容不画出来
            if (GestureData.IsGesture && GestureData.Fingers > 2)
            {
                newPolyLine.CanDraw = false;
            }
            AddNewObject(drawingCanvas, newPolyLine);

            // Add new stroke to the collection of strokes in drawing.
            _activeStrokes[newPolyLine.DeviceID] = newPolyLine;

            System.Diagnostics.Debug.WriteLine("Touchdown:{0}", GestureData.IsGesture);
        }

        private void FinishStroke(GraphicsPolyLine polyLine)
        {
            //Seal stroke for better performance
            //polyLine.Freeze();
            if (polyLine == null) return;
            // Remove finished stroke from the collection of strokes in drawing.
            _activeStrokes.Remove(polyLine.DeviceID);
        }

        // Touch up event handler.
        public void TouchUpHandler(DrawingCanvas drawingCanvas, TouchEventArgs e)
        {
            // Find the stroke in the collection of the strokes in drawing.
            GraphicsPolyLine polyLine;
            if (_activeStrokes.TryGetValue(e.TouchDevice.Id, out polyLine))
            {
                FinishStroke(polyLine);
            }
            /*
             针对同时有多个手指
             */
            for (int i =0; i< drawingCanvas.Count;i++)
            {
                if (!drawingCanvas[i].IsSigned)
                {
                    drawingCanvas[i].CopyPoints();
                    drawingCanvas[i].IsSigned = true;
                }
            }
                
            if(GestureData.Fingers > 0)
                GestureData.Fingers--;
            HelperFunctions.clear(drawingCanvas);
            
            System.Diagnostics.Debug.WriteLine("Touchup");
        }

        // Touch move event handler.
        /// <summary>
        /// 在move过程判断具体的手势（zoom，pan,rotate）
        /// </summary>
        /// <param name="drawingCanvas"></param>
        /// <param name="e"></param>
        public void TouchMoveHandler(DrawingCanvas drawingCanvas, TouchEventArgs e)
        {
            GraphicsPolyLine polyLine;
            if (_activeStrokes.TryGetValue(e.TouchDevice.Id, out polyLine))
            {
                TouchPoint touchPoint = e.GetTouchPoint(drawingCanvas);
                Point p = touchPoint.Position;
                polyLine.AddPoint(p);
                if (e.TouchDevice.Id == GestureData.FirstDeviceId)
                {
                    GestureData.FirstFingerCurPoint = p;
                }
                if (e.TouchDevice.Id == GestureData.SecondDeviceId)
                {
                    GestureData.SecFingerCurPoint = p;
                }
                //polyLine.AddToCanvas(drawingCanvas);
                //判断出具体手势，然后根据手势，在case中按照相应的比例绘画图像。
                if (GestureData.IsGesture)
                {
                    GestureId gestureId = HelperFunctions.getGestureId(drawingCanvas, e);
                    bool hasSelected = HelperFunctions.hasSelected(drawingCanvas);
                    System.Diagnostics.Debug.WriteLine("hasSelected:{0}", hasSelected);
                    switch (gestureId)
                    {
                        case GestureId.ZOOM:
                            if (hasSelected)
                            {
                                foreach (GraphicsBase b in drawingCanvas.GraphicsList)
                                {
                                    if (b.IsOption)
                                    {
                                        b.Zoom(GestureData.ZoomScale, GestureData.StartCenterPointInTwoFingers);
                                    }
                                    
                                }
                            }
                            else
                            {
                                foreach (GraphicsBase b in drawingCanvas.GraphicsList)
                                {
                                    b.Zoom(GestureData.ZoomScale, GestureData.StartCenterPointInTwoFingers);
                                }
                            }
                            break;
                        case GestureId.PAN:
                            if (hasSelected)
                            {
                                foreach (GraphicsBase b in drawingCanvas.GraphicsList)
                                {
                                    if (b.IsOption)
                                    {
                                        b.Move(GestureData.Panx, GestureData.Pany);
                                    }

                                }
                            }
                            else
                            {
                                foreach (GraphicsBase b in drawingCanvas.GraphicsList)
                                {
                                    b.Move(GestureData.Panx, GestureData.Pany);
                                }
                            }
                            break;
                        case GestureId.RORATE:
                            if (hasSelected)
                            {
                                foreach (GraphicsBase b in drawingCanvas.GraphicsList)
                                {
                                    if (b.IsOption)
                                    {
                                        b.Rotate(GestureData.RotateAngle, GestureData.StartCenterPointInTwoFingers);
                                    }

                                }
                            }
                            else
                            {
                                foreach (GraphicsBase b in drawingCanvas.GraphicsList)
                                {
                                    b.Rotate(GestureData.RotateAngle, GestureData.StartCenterPointInTwoFingers);
                                }
                            }
                            break;
                        case GestureId.ERASE:
                            break;
                        default:
                            break;
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine("Touchmove");
        }
        #endregion Touch Handler

        #region manipulation
       
        public void ManipulationStarting(DrawingCanvas drawingCanvas, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = drawingCanvas;
            e.Handled = true;

            System.Diagnostics.Debug.WriteLine("ManipulationStarting");
        }
        public void ManipulationDelta(DrawingCanvas drawingCanvas, ManipulationDeltaEventArgs e)
        {
            // Get the Rectangle and its RenderTransform matrix.
            Rectangle rectToMove = e.OriginalSource as Rectangle;
            Matrix rectsMatrix = ((MatrixTransform)rectToMove.RenderTransform).Matrix;

            // Rotate the Rectangle.
            rectsMatrix.RotateAt(e.DeltaManipulation.Rotation,
                                 e.ManipulationOrigin.X,
                                 e.ManipulationOrigin.Y);

            // Resize the Rectangle.  Keep it square 
            // so use only the X value of Scale.
            rectsMatrix.ScaleAt(e.DeltaManipulation.Scale.X,
                                e.DeltaManipulation.Scale.X,
                                e.ManipulationOrigin.X,
                                e.ManipulationOrigin.Y);

            // Move the Rectangle.
            rectsMatrix.Translate(e.DeltaManipulation.Translation.X,
                                  e.DeltaManipulation.Translation.Y);

            // Apply the changes to the Rectangle.
            rectToMove.RenderTransform = new MatrixTransform(rectsMatrix);

            Rect containingRect =
                new Rect(((FrameworkElement)e.ManipulationContainer).RenderSize);

            Rect shapeBounds =
                rectToMove.RenderTransform.TransformBounds(
                    new Rect(rectToMove.RenderSize));

            // Check if the rectangle is completely in the window.
            // If it is not and intertia is occuring, stop the manipulation.
            if (e.IsInertial && !containingRect.Contains(shapeBounds))
            {
                e.Complete();
                e.ReportBoundaryFeedback(e.DeltaManipulation);
            }


            e.Handled = true;
            System.Diagnostics.Debug.WriteLine("ManipulationDelta");
        }
        public void InertiaStarting(DrawingCanvas drawingCanvas, ManipulationInertiaStartingEventArgs e)
        {

            // Decrease the velocity of the Rectangle's movement by 
            // 10 inches per second every second.
            // (10 inches * 96 pixels per inch / 1000ms^2)
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's resizing by 
            // 0.1 inches per second every second.
            // (0.1 inches * 96 pixels per inch / (1000ms^2)
            e.ExpansionBehavior.DesiredDeceleration = 0.1 * 96 / 1000.0 * 1000.0;

            // Decrease the velocity of the Rectangle's rotation rate by 
            // 2 rotations per second every second.
            // (2 * 360 degrees / (1000ms^2)
            e.RotationBehavior.DesiredDeceleration = 720 / (1000.0 * 1000.0);

            e.Handled = true;
            System.Diagnostics.Debug.WriteLine("InertiaStarting");
        }
        #endregion manipulation

    }
}
