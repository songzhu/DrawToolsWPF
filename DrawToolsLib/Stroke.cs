//-----------------------------------------------------------------------------
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;

namespace DrawToolsLib
{
    // Stroke object represents a single stroke, trajectory of the finger from
    // touch-down to touch-up. Object has two properties: color of the stroke,
    // and ID used to distinguish strokes coming from different fingers.
    public class Stroke
    {
        private PathFigure _pathFigure;  // the list of stroke points
        private bool _isOnCanvas;
        private Path _path = new Path();
        private Color _color = Colors.Black;


        private const float _penWidth = 3.0f;    // pen width for drawing the stroke

        /// <summary>
        /// Seal the object.
        /// </summary>
        /// <remarks>
        /// To improve performance, Seal replaces the list with fixed array.
        /// </remarks>
        public void Freeze()
        {
            if (_pathFigure != null)
                _pathFigure.Freeze();
        }

        /// <summary>
        /// Indicate if we can add nore points to the stroke
        /// </summary>
        public bool IsFrozen
        {
            get
            {
                return _pathFigure.IsFrozen;
            }
        }

        /// <summary>
        ///  Add the complete stroke
        /// </summary>
        /// <param name="canvas">the canvas that hold all figures</param>
        public void AddToCanvas(DrawingCanvas canvas)
        {
            if (_isOnCanvas)
                return;
            _isOnCanvas = true;

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = new PathFigureCollection() {_pathFigure};
            _path.StrokeThickness = _penWidth;
            _path.Data = pathGeometry;
            canvas.Children.Add(_path); 
        }

        
        /// <summary>
        /// Access to the property stroke color
        /// </summary>
        public Color Color 
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                _path.Stroke = new SolidColorBrush(_color);
            }
        }


        /// <summary>
        /// Access to the property stroke ID 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Adds a point to the stroke.
        /// </summary>
        /// <param name="pt">point to be added to the stroke</param>
        public void Add(Point pt)
        {
            if (_pathFigure != null && IsFrozen)
                throw new InvalidOperationException("This object is frozen");

            if (_pathFigure == null)
            {
                _pathFigure = new PathFigure();
                _pathFigure.IsClosed = false;
                _pathFigure.StartPoint = pt;
                _pathFigure.Segments = new PathSegmentCollection();
            }
            else
            {
                _pathFigure.Segments.Add(new LineSegment(pt, true));
            }
        }
    }
}