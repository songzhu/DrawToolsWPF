using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System;

namespace DrawToolsLib
{
    class ToolEraser : ToolRectangleBase
    {
        private GraphicsRectangleBase g = null;
        private int r = 20;


        public ToolEraser()
        {
            // MemoryStream stream = new MemoryStream(Properties.Resources.Pencil);
            ToolCursor = Cursors.Arrow;
            
        }

        public int R { get => r; set => r = value; }

        /// <summary>
        /// Create new rectangle
        /// </summary>
        public override void OnMouseDown(DrawingCanvas drawingCanvas, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(drawingCanvas);
            
            AddNewObject(drawingCanvas,
                new GraphicsEraser(
                p.X - R,
                p.Y - R,
                p.X + R,
                p.Y + R,
                drawingCanvas.LineWidth,
                Colors.Yellow,//drawingCanvas.ObjectColor
                drawingCanvas.ActualScale));


            if (drawingCanvas.Count > 0)
            {
                for ( int i = 0; i< drawingCanvas.GraphicsList.Count;i++)
                {
                    GraphicsBase b = drawingCanvas[i];
                    if (b.IntersectsWith(new Rect(p.X - R, p.Y - R, p.X + r, p.Y + R)))
                    {
                        //Rect rect = new Rect(0,0, drawingCanvas.MaxWidth, drawingCanvas.MaxHeight);
                        Rect rect = new Rect(0, 0, 600, 400);
                        CombinedGeometry cb = new CombinedGeometry(GeometryCombineMode.Xor,
                                 new RectangleGeometry(rect),
                                 new EllipseGeometry(p, r, r)//drawingCanvas[drawingCanvas.Count - 1].Clip//
                               );

                        b.Clip = cb;
                        /*Type type = b.GetType();
                        GraphicsBase clearedVisual = null;
                        if (b is GraphicsPolyLine)
                        {
                            Point[] points = ((GraphicsPolyLine)b).GetPoints();
                            clearedVisual = new GraphicsPolyLine(points,2,Colors.Black,2);
                        }
                        if (clearedVisual == null) return;
                        DrawingContext dc = clearedVisual.RenderOpen();
                        dc.PushClip(cb);
                        dc.DrawDrawing(clearedVisual.Drawing);
                        int index = drawingCanvas.GraphicsList.IndexOf(b);
                        if (index >= 0)
                        {
                            drawingCanvas.GraphicsList.RemoveAt(index);
                            drawingCanvas.GraphicsList.Insert(index, clearedVisual);
                        }
                        else
                        {
                            drawingCanvas.GraphicsList.Add(clearedVisual);
                        }*/
                    }
                }

            }
        }

        /// <summary>
        /// Set cursor and resize new object.
        /// </summary>
        public override void OnMouseMove(DrawingCanvas drawingCanvas, MouseEventArgs e)
        {
            drawingCanvas.Cursor = ToolCursor;
            Point p = e.GetPosition(drawingCanvas);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (drawingCanvas.Count > 0 && drawingCanvas.GraphicsList[drawingCanvas.Count - 1] is GraphicsEraser)
                {
                    g = (GraphicsRectangleBase)drawingCanvas.GraphicsList[drawingCanvas.Count - 1];
                }
                if (g != null)
                {
                    //得到圆心坐标
                    double x1 = g.Left + R;
                    double y1 = g.Top + R;
                    double x2 = p.X;
                    double y2 = p.Y;
                    double deltax = x2 - x1;
                    double deltay = y2 - y1;
           
                    if (drawingCanvas.Count > 0)
                    {
                        drawingCanvas[drawingCanvas.Count - 1].Move(deltax, deltay);
                        for (int i = 0; i < drawingCanvas.GraphicsList.Count; i++)
                        {
                            GraphicsBase b = drawingCanvas[i];
                            if (b.IntersectsWith(new Rect(p.X - R, p.Y - R, p.X + r, p.Y + R)))
                            {
                                //Rect rect = new Rect(0,0, drawingCanvas.MaxWidth, drawingCanvas.MaxHeight);
                                Rect rect = new Rect(0, 0, 600, 400);
                                CombinedGeometry cb = new CombinedGeometry(GeometryCombineMode.Xor,
                                         new RectangleGeometry(rect),
                                         new EllipseGeometry(p, r, r)//drawingCanvas[drawingCanvas.Count - 1].Clip//
                                       );

                                b.Clip = cb;
                               
                            }
                        }
                        /*DrawingContext dc = drawingCanvas[drawingCanvas.Count - 1].RenderOpen();
                        dc.DrawGeometry(Brushes.Blue, null, cb);
                        drawingCanvas[drawingCanvas.Count - 1].Clip = cb;*/

                        //获取两个点之间的剪辑区域四个端点
                        /*double asin = R * Math.Sin(Math.Atan((y2 - y1) / (x2 - x1)));
                        double acos = R * Math.Cos(Math.Atan((y2 - y1) / (x2 - x1)));
                        double x3 = x1 + asin;
                        double y3 = y1 - acos;
                        double x4 = x1 - asin;
                        double y4 = y1 + acos;
                        double x5 = x2 + asin;
                        double y5 = y2 - acos;
                        double x6 = x2 - asin;
                        double y6 = y2 + acos;
                        drawingCanvas[drawingCanvas.Count - 1].MoveHandleTo();*/
                    }
                }
                
            }
        }


        public override void OnMouseUp(DrawingCanvas drawingCanvas, MouseButtonEventArgs e)
        {
            if (drawingCanvas.Count > 0 &&  drawingCanvas.GraphicsList[drawingCanvas.Count - 1] is GraphicsEraser)
            {
                drawingCanvas.GraphicsList.RemoveAt(drawingCanvas.Count - 1);
            }
            //drawingCanvas.RefreshClip();
            drawingCanvas.ReleaseMouseCapture();
        }
    }
}
