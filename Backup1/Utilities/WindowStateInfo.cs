using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace Utilities
{
    /// <summary>
    /// Class contains information about WPF Window state.
    /// Client's responsibility is to create instance of this class,
    /// load it from some persistent storage (like XML file)
    /// when program starts, and save it when program ends.
    /// 
    /// Instance of this class contains information of one WPF window state.
    /// It is connected to specific window using class WindowStateManager.
    /// 
    /// This class itself doesn't make any actions, it only keeps information
    /// about window state.
    /// </summary>
    public class WindowStateInfo
    {
        // Window parameters in normal state
        double left;
        double top;
        double width;
        double height;

        // Window state
        WindowState state;

        public WindowStateInfo()
        {
            state = WindowState.Normal;
        }

        public double Left
        {
            get { return left; }
            set { left = value; }
        }

        public double Top
        {
            get { return top; }
            set { top = value; }
        }

        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        public double Height
        {
            get { return height; }
            set { height = value; }
        }


        public WindowState State
        {
            get { return state; }
            set { state = value; }
        }
    }
}
