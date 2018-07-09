using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace Utilities
{
    /// <summary>
    /// This class is responsible to set initial window state
    /// and track window state changes. 
    /// 
    /// Class instance must be created before owner window is initialized.
    /// WindowStateInfo passed to this instance should be read by client
    /// from some persistent storage (like XML file).
    /// </summary>
    public class WindowStateManager
    {
        WindowStateInfo windowStateInfo;
        Window ownerWindow;

        public WindowStateInfo WindowStateInfo
        {
            get { return windowStateInfo; }
            set { windowStateInfo = value; }
        }

        public Window OwnerForm
        {
            get { return ownerWindow; }
            set { ownerWindow = value; }
        }


        public WindowStateManager(WindowStateInfo windowStateInfo, Window ownerWindow)
        {
            if ( windowStateInfo == null )
            {
                throw new ArgumentNullException("windowStateInfo");
            }

            if (ownerWindow == null)
            {
                throw new ArgumentNullException("ownerWindow");
            }

            this.windowStateInfo = windowStateInfo;
            this.ownerWindow = ownerWindow;

            // Subscribe to owner window's events to track window state changes
            ownerWindow.LocationChanged += new EventHandler(ownerWindow_LocationChanged);
            ownerWindow.SizeChanged += new SizeChangedEventHandler(ownerWindow_SizeChanged);

            SetInitialWindowState();
        }

        /// <summary>
        /// Set initial window state
        /// </summary>
        private void SetInitialWindowState()
        {
            if (windowStateInfo.Width == 0.0 && windowStateInfo.Height == 0.0)
            {
                // Initial state is not supplied by client, let window to decide itself
                return;
            }

            double minSize = 50.0;

            // Validate windowStateInfo values 
            if (windowStateInfo.Width < ownerWindow.MinWidth)
                windowStateInfo.Width = ownerWindow.MinWidth;

            if (windowStateInfo.Width > ownerWindow.MaxWidth)
                windowStateInfo.Width = ownerWindow.MaxWidth;


            if (windowStateInfo.Height < ownerWindow.MinHeight)
                windowStateInfo.Height = ownerWindow.MinHeight;

            if (windowStateInfo.Height > ownerWindow.MaxHeight)
                windowStateInfo.Height = ownerWindow.MaxHeight;


            if (windowStateInfo.Left < 0.0)
                windowStateInfo.Left = 0.0;

            if (windowStateInfo.Left > SystemParameters.WorkArea.Width - minSize)
                windowStateInfo.Left = (SystemParameters.WorkArea.Width - minSize);

            if (windowStateInfo.Top < 0.0)
                windowStateInfo.Top = 0.0;

            if (windowStateInfo.Top > SystemParameters.WorkArea.Height - minSize)
                windowStateInfo.Top = (SystemParameters.WorkArea.Height - minSize);

            // Set window parameters

            ownerWindow.Left = windowStateInfo.Left;
            ownerWindow.Top = windowStateInfo.Top;
            ownerWindow.Width = windowStateInfo.Width;
            ownerWindow.Height = windowStateInfo.Height;

            // Set window state only if it has correct value.
            // Don't create initially minimized window.
            if (windowStateInfo.State == WindowState.Normal || windowStateInfo.State == WindowState.Maximized)
            {
                ownerWindow.WindowState = windowStateInfo.State;
            }
        }

        /// <summary>
        /// Track window location
        /// </summary>
        void ownerWindow_LocationChanged(object sender, EventArgs e)
        {
            // save position
            if (ownerWindow.WindowState == WindowState.Normal)
            {
                windowStateInfo.Left = ownerWindow.Left;
                windowStateInfo.Top = ownerWindow.Top;
            }
        }

        /// <summary>
        /// Track window size
        /// </summary>
        void ownerWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // save width and height
            if (ownerWindow.WindowState == WindowState.Normal)
            {
                windowStateInfo.Width = ownerWindow.Width;
                windowStateInfo.Height = ownerWindow.Height;
            }

            windowStateInfo.State = ownerWindow.WindowState;
        }
    };
}
