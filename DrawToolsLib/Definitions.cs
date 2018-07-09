using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace DrawToolsLib
{
    /// <summary>
    /// Defines drawing tool
    /// </summary>
    public enum ToolType
    {
        None,
        Pointer,
        Rectangle,
        Ellipse,
        Line,
        PolyLine,
        Eraser,
        Text,
        Max
    };
    public enum GestureId
    {
        None,
        ZOOM,
        PAN,
        RORATE,
        ERASE
    };
    public static class GestureData
    {
        //手指触碰个数
        static int fingers = 0;
        //第一个手指的Id
        static int firstDeviceId = -1;
        //第二个手指的Id
        static int secondDeviceId = -1;
        //第一个手指触碰的起始点
        static Point firstFingerBeginPoint = new Point(0,0);
        //第一个手指当前的位置
        static Point firstFingerCurPoint = firstFingerBeginPoint;
        //是否是手势
        static bool isGesture = false;
        //默认为鼠标事件
        static bool isTouch = false;
        //是否给第一个手指初始赋值（有些参数只初始化一次）
        static bool isSignFoFingerOne = false;

        //是否给第二个手指初始赋值（有些参数只初始化一次）
        static bool isSignForFingerTwo = false;

        /// <summary>
        /// 判断具体手势的一些基本信息
        /// </summary>
        //两指最开始的距离
        static int startDistInTwoFingers = 0;
        //第二个手指的触碰的起始点
        static Point secFingerBeginPoint = new Point(0,0);
        //第二个手指的当前位置
        static Point secFingerCurPoint = secFingerBeginPoint;
        //两指起始地中心位置
        static Point startCenterPointInTwoFingers = new Point(0, 0);
        //第一个手指之前的点信息
        static Point preFirstFingerPoint;
        //第二个手指之前的点信息
        static Point preSecFingerPoint;
        //两指之前的距离
        static int preDistInTwoFingers = 0;
        
        //给preFirstFingerPoint，preSecFingerPoint赋值的标志量
        static bool isSign = true;
        //是否是放大,立即将其置为true，否则它和平移不太容易区分开
        static bool isZoom = false;
        //缩小尺寸
        static double zoomOutScale = 1.0;
        //放大尺寸
        static double zoomInScale = 1.0;
        //实际尺寸
        static double zoomScale = 1.0;
        //平移x距离
        static double panx = 0.0;
        //平移y距离
        static double pany = 0.0;
        //旋转角度
        static double rotateAngle = 0;
        //控制放缩次数，间接控制放缩速度
        static int zoomCnt = 0;
        //控制旋转次数，间接控制放缩速度
        static int rorateCnt = 0;

        //判断当前第一指在x小的位置，还是第二指在x小的位置,默认第二指的x位置小
        static bool maxPositionX = false;

        public static int Fingers { get => fingers; set => fingers = value; }
        public static Point FirstFingerBeginPoint { get => firstFingerBeginPoint; set => firstFingerBeginPoint = value; }
        public static Point FirstFingerCurPoint { get => firstFingerCurPoint; set => firstFingerCurPoint = value; }
        public static bool IsGesture { get => isGesture; set => isGesture = value; }
        public static bool IsTouch { get => isTouch; set => isTouch = value; }
        public static int FirstDeviceId { get => firstDeviceId; set => firstDeviceId = value; }
        public static int SecondDeviceId { get => secondDeviceId; set => secondDeviceId = value; }
        public static int StartDistInTwoFingers { get => startDistInTwoFingers; set => startDistInTwoFingers = value; }
        public static Point SecFingerBeginPoint { get => secFingerBeginPoint; set => secFingerBeginPoint = value; }
        public static Point SecFingerCurPoint { get => secFingerCurPoint; set => secFingerCurPoint = value; }
        public static Point StartCenterPointInTwoFingers { get => startCenterPointInTwoFingers; set => startCenterPointInTwoFingers = value; }
        public static Point PreFirstFingerPoint { get => preFirstFingerPoint; set => preFirstFingerPoint = value; }
        public static Point PreSecFingerPoint { get => preSecFingerPoint; set => preSecFingerPoint = value; }
        public static bool IsSign { get => isSign; set => isSign = value; }
        public static int PreDistInTwoFingers { get => preDistInTwoFingers; set => preDistInTwoFingers = value; }
        public static bool IsZoom { get => isZoom; set => isZoom = value; }
        public static double ZoomOutScale { get => zoomOutScale; set => zoomOutScale = value; }
        public static double ZoomInScale { get => zoomInScale; set => zoomInScale = value; }
        public static double ZoomScale { get => zoomScale; set => zoomScale = value; }
        public static double Panx { get => panx; set => panx = value; }
        public static double Pany { get => pany; set => pany = value; }
        public static double RotateAngle { get => rotateAngle; set => rotateAngle = value; }
        public static int ZoomCnt { get => zoomCnt; set => zoomCnt = value; }
        public static int RorateCnt { get => rorateCnt; set => rorateCnt = value; }
        public static bool IsSignFoFingerOne { get => isSignFoFingerOne; set => isSignFoFingerOne = value; }
        public static bool IsSignForFingerTwo { get => isSignForFingerTwo; set => isSignForFingerTwo = value; }
        public static bool MaxPositionX { get => maxPositionX; set => maxPositionX = value; }
    }; 
    public static class Threshold
    {
        const int curToBeginMaxDistInFirstFinger = 500;
        const int firstFingerToSecFingerMaxDist = 75000;
        const int panMaxDist = 5000;
        const int scaleThreshold = 2;
        const double esp = 1E-5;
        const double inf = 1E+14;
        public static int CurToBeginMaxDistInFirstFinger => curToBeginMaxDistInFirstFinger;

        public static int FirstFingerToSecFingerMaxDist => firstFingerToSecFingerMaxDist;

        public static int ScaleThreshold => scaleThreshold;

        public static double Esp => esp;

        public static double Inf => inf;

        public static int PanMaxDist => panMaxDist;
    }
    /// <summary>
    /// Context menu command types
    /// </summary>
    internal enum ContextMenuCommand
    {
        SelectAll,
        UnselectAll,
        Delete, 
        DeleteAll,
        MoveToFront,
        MoveToBack,
        Undo,
        Redo,
        SerProperties
    };
}
