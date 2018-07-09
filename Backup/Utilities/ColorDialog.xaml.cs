using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Utilities
{
    /// <summary>
    /// Color selection dialog.
    /// </summary>
    public partial class ColorDialog : System.Windows.Window
    {
        Color color = Colors.Black;     // [in, out]

        Color currentColor;             // changed when slider is moved

        /// <summary>
        /// Set this property before call to ShowDialog.
        /// If ShowDialog returns true, read this property as result
        /// of user selection.
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public ColorDialog()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(ColorDialog_Loaded);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        void ColorDialog_Loaded(object sender, RoutedEventArgs e)
        {
            buttonOK.Click += new RoutedEventHandler(buttonOK_Click);

            sliderA.Value = color.A;
            sliderB.Value = color.B;
            sliderG.Value = color.G;
            sliderR.Value = color.R;

            currentColor = color;

            borderSample.Background = new SolidColorBrush(currentColor);

            sliderA.ValueChanged += slider_ValueChanged;
            sliderR.ValueChanged += slider_ValueChanged;
            sliderG.ValueChanged += slider_ValueChanged;
            sliderB.ValueChanged += slider_ValueChanged;
        }

        /// <summary>
        /// Set output color and close dialog
        /// </summary>
        void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            color = currentColor;

            this.DialogResult = true;
            this.Close();
        }

        static byte DoubleToByte(double value)
        {
            int n = (int)(value + 0.5);

            if (n < 0)
            {
                n = 0;
            }

            if (n > 255)
            {
                n = 255;
            }

            return (byte)n;
        }

        /// <summary>
        /// Set current color and show it on the sample control
        /// </summary>
        void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            currentColor = Color.FromArgb(
                DoubleToByte(sliderA.Value),
                DoubleToByte(sliderR.Value),
                DoubleToByte(sliderG.Value),
                DoubleToByte(sliderB.Value));

            borderSample.Background = new SolidColorBrush(currentColor);
        }
    }
}