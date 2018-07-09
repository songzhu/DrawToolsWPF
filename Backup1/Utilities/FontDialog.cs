//-------------------------------------------
// FontDialog.cs (c) 2006 by Charles Petzold
//-------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;

namespace Petzold.ChooseFont
{
    public class FontDialog : Window
    {
        TextBoxWithLister boxFamily, boxStyle, boxWeight, boxStretch, boxSize;
        Label lblDisplay;
        bool isUpdateSuppressed = true;

        // Public properties.
        public Typeface Typeface
        {
            set
            {
                if (boxFamily.Contains(value.FontFamily))
                    boxFamily.SelectedItem = value.FontFamily;
                else
                    boxFamily.SelectedIndex = 0;

                if (boxStyle.Contains(value.Style))
                    boxStyle.SelectedItem = value.Style;
                else
                    boxStyle.SelectedIndex = 0;

                if (boxWeight.Contains(value.Weight))
                    boxWeight.SelectedItem = value.Weight;
                else
                    boxWeight.SelectedIndex = 0;

                if (boxStretch.Contains(value.Stretch))
                    boxStretch.SelectedItem = value.Stretch;
                else
                    boxStretch.SelectedIndex = 0;
            }
            get
            {
                return new Typeface((FontFamily)boxFamily.SelectedItem,
                                    (FontStyle)boxStyle.SelectedItem,
                                    (FontWeight)boxWeight.SelectedItem,
                                    (FontStretch)boxStretch.SelectedItem);
            }
        }
        public double FaceSize
        {
            set
            {
                double size = 0.75 * value;
                boxSize.Text = size.ToString(CultureInfo.InvariantCulture);

                if (!boxSize.Contains(size))
                    boxSize.Insert(0, size);

                boxSize.SelectedItem = size;
            }
            get
            {
                double size;

                if (!Double.TryParse(boxSize.Text, out size))
                    size = 8.25;

                return size / 0.75;
            }
        }

        // Constructor.
        public FontDialog()
        {
            Title = "Font";
            ShowInTaskbar = false;
            WindowStyle = WindowStyle.ToolWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            SizeToContent = SizeToContent.WidthAndHeight;
            ResizeMode = ResizeMode.NoResize;

            // Create three-row Grid as content of window.
            Grid gridMain = new Grid();
            Content = gridMain;

            // This row is for the TextBoxWithLister controls.
            RowDefinition rowdef = new RowDefinition();
            rowdef.Height = new GridLength(200, GridUnitType.Pixel);
            gridMain.RowDefinitions.Add(rowdef);

            // This row is for the sample text.
            rowdef = new RowDefinition();
            rowdef.Height = new GridLength(150, GridUnitType.Pixel);
            gridMain.RowDefinitions.Add(rowdef);

            // This row is for the buttons.
            rowdef = new RowDefinition();
            rowdef.Height = GridLength.Auto;
            gridMain.RowDefinitions.Add(rowdef);

            // One column in main Grid.
            ColumnDefinition coldef = new ColumnDefinition();
            coldef.Width = new GridLength(650, GridUnitType.Pixel);
            gridMain.ColumnDefinitions.Add(coldef);

            // Create two-row, five-column Grid for TextBoxWithLister controls.
            Grid gridBoxes = new Grid();
            gridMain.Children.Add(gridBoxes);

            // This row is for the labels.
            rowdef = new RowDefinition();
            rowdef.Height = GridLength.Auto;
            gridBoxes.RowDefinitions.Add(rowdef);

            // This row is for the EditBoxWithLister controls.
            rowdef = new RowDefinition();
            rowdef.Height = new GridLength(100, GridUnitType.Star);
            gridBoxes.RowDefinitions.Add(rowdef);

            // First column is FontFamily.
            coldef = new ColumnDefinition();
            coldef.Width = new GridLength(175, GridUnitType.Star);
            gridBoxes.ColumnDefinitions.Add(coldef);

            // Second column is FontStyle.
            coldef = new ColumnDefinition();
            coldef.Width = new GridLength(100, GridUnitType.Star);
            gridBoxes.ColumnDefinitions.Add(coldef);

            // Third column is FontWeight.
            coldef = new ColumnDefinition();
            coldef.Width = new GridLength(100, GridUnitType.Star);
            gridBoxes.ColumnDefinitions.Add(coldef);

            // Fourth column is FontStretch.
            coldef = new ColumnDefinition();
            coldef.Width = new GridLength(100, GridUnitType.Star);
            gridBoxes.ColumnDefinitions.Add(coldef);

            // Fifth column is Size.
            coldef = new ColumnDefinition();
            coldef.Width = new GridLength(75, GridUnitType.Star);
            gridBoxes.ColumnDefinitions.Add(coldef);

            // Create FontFamily labels and TextBoxWithLister controls.
            Label lbl = new Label();
            lbl.Content = "Font Family";
            lbl.Margin = new Thickness(12, 12, 12, 0);
            gridBoxes.Children.Add(lbl);
            Grid.SetRow(lbl, 0);
            Grid.SetColumn(lbl, 0);

            boxFamily = new TextBoxWithLister();
            boxFamily.IsReadOnly = true;
            boxFamily.Margin = new Thickness(12, 0, 12, 12);
            gridBoxes.Children.Add(boxFamily);
            Grid.SetRow(boxFamily, 1);
            Grid.SetColumn(boxFamily, 0);

            // Create FontStyle labels and TextBoxWithLister controls.
            lbl = new Label();
            lbl.Content = "Style";
            lbl.Margin = new Thickness(12, 12, 12, 0);
            gridBoxes.Children.Add(lbl);
            Grid.SetRow(lbl, 0);
            Grid.SetColumn(lbl, 1);

            boxStyle = new TextBoxWithLister();
            boxStyle.IsReadOnly = true;
            boxStyle.Margin = new Thickness(12, 0, 12, 12);
            gridBoxes.Children.Add(boxStyle);
            Grid.SetRow(boxStyle, 1);
            Grid.SetColumn(boxStyle, 1);

            // Create FontWeight labels and TextBoxWithLister controls.
            lbl = new Label();
            lbl.Content = "Weight";
            lbl.Margin = new Thickness(12, 12, 12, 0);
            gridBoxes.Children.Add(lbl);
            Grid.SetRow(lbl, 0);
            Grid.SetColumn(lbl, 2);

            boxWeight = new TextBoxWithLister();
            boxWeight.IsReadOnly = true;
            boxWeight.Margin = new Thickness(12, 0, 12, 12);
            gridBoxes.Children.Add(boxWeight);
            Grid.SetRow(boxWeight, 1);
            Grid.SetColumn(boxWeight, 2);

            // Create FontStretch labels and TextBoxWithLister controls.
            lbl = new Label();
            lbl.Content = "Stretch";
            lbl.Margin = new Thickness(12, 12, 12, 0);
            gridBoxes.Children.Add(lbl);
            Grid.SetRow(lbl, 0);
            Grid.SetColumn(lbl, 3);

            boxStretch = new TextBoxWithLister();
            boxStretch.IsReadOnly = true;
            boxStretch.Margin = new Thickness(12, 0, 12, 12);
            gridBoxes.Children.Add(boxStretch);
            Grid.SetRow(boxStretch, 1);
            Grid.SetColumn(boxStretch, 3);

            // Create Size labels and TextBoxWithLister controls.
            lbl = new Label();
            lbl.Content = "Size";
            lbl.Margin = new Thickness(12, 12, 12, 0);
            gridBoxes.Children.Add(lbl);
            Grid.SetRow(lbl, 0);
            Grid.SetColumn(lbl, 4);

            boxSize = new TextBoxWithLister();
            boxSize.Margin = new Thickness(12, 0, 12, 12);
            gridBoxes.Children.Add(boxSize);
            Grid.SetRow(boxSize, 1);
            Grid.SetColumn(boxSize, 4);

            // Create Label to display sample text.
            lblDisplay = new Label();
            lblDisplay.Content = "AaBbCc XxYzZz 012345";
            lblDisplay.HorizontalContentAlignment = HorizontalAlignment.Center;
            lblDisplay.VerticalContentAlignment = VerticalAlignment.Center;
            gridMain.Children.Add(lblDisplay);
            Grid.SetRow(lblDisplay, 1);

            // Create five-column Grid for Buttons.
            Grid gridButtons = new Grid();
            gridMain.Children.Add(gridButtons);
            Grid.SetRow(gridButtons, 2);

            for (int i = 0; i < 5; i++)
                gridButtons.ColumnDefinitions.Add(new ColumnDefinition());

            // OK button.
            Button btn = new Button();
            btn.Content = "OK";
            btn.IsDefault = true;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.MinWidth = 60;
            btn.Margin = new Thickness(12);
            btn.Click += OkOnClick;
            gridButtons.Children.Add(btn);
            Grid.SetColumn(btn, 1);

            // Cancel button.
            btn = new Button();
            btn.Content = "Cancel";
            btn.IsCancel = true;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.MinWidth = 60;
            btn.Margin = new Thickness(12);
            gridButtons.Children.Add(btn);
            Grid.SetColumn(btn, 3);

            // Initialize FontFamily box with system font families.
            foreach (FontFamily fam in Fonts.SystemFontFamilies)
                boxFamily.Add(fam);

            // Initialize FontSize box.
            double[] ptsizes = new double[] { 8, 9, 10, 11, 12, 14, 16, 18, 
                                              20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (double ptsize in ptsizes)
                boxSize.Add(ptsize);

            // Set event handlers.
            boxFamily.SelectionChanged += FamilyOnSelectionChanged;
            boxStyle.SelectionChanged += StyleOnSelectionChanged;
            boxWeight.SelectionChanged += StyleOnSelectionChanged;
            boxStretch.SelectionChanged += StyleOnSelectionChanged;
            boxSize.TextChanged += SizeOnTextChanged;

            // Initialize selected values based on Window properties.
            // (These will probably be overridden when properties are set.)
            Typeface = new Typeface(FontFamily, FontStyle,
                                    FontWeight, FontStretch);
            FaceSize = FontSize;


            // Set keyboard focus.
            boxFamily.Focus();

            // Allow updates to the sample text.
            isUpdateSuppressed = false;
            UpdateSample();
        }

        // Event handler for SelectionChanged in FontFamily box.
        void FamilyOnSelectionChanged(object sender, EventArgs args)
        {
            // Get selected FontFamily.
            FontFamily fntfam = (FontFamily)boxFamily.SelectedItem;

            // Save previous Style, Weight, Stretch.
            // These should only be null when this method is called for the
            //  first time.
            FontStyle? fntstyPrevious = (FontStyle?)boxStyle.SelectedItem;
            FontWeight? fntwtPrevious = (FontWeight?)boxWeight.SelectedItem;
            FontStretch? fntstrPrevious = (FontStretch?)boxStretch.SelectedItem;

            // Turn off Sample display.
            isUpdateSuppressed = true;

            // Clear Style, Weight, and Stretch boxes.         
            boxStyle.Clear();
            boxWeight.Clear();
            boxStretch.Clear();

            // Loop through typefaces in selected FontFamily.
            foreach (FamilyTypeface ftf in fntfam.FamilyTypefaces)
            {
                // Put Style in boxStyle (Normal always at top).
                if (!boxStyle.Contains(ftf.Style))
                {
                    if (ftf.Style == FontStyles.Normal)
                        boxStyle.Insert(0, ftf.Style);
                    else
                        boxStyle.Add(ftf.Style);
                }
                // Put Weight in boxWeight (Normal always at top).
                if (!boxWeight.Contains(ftf.Weight))
                {
                    if (ftf.Weight == FontWeights.Normal)
                        boxWeight.Insert(0, ftf.Weight);
                    else
                        boxWeight.Add(ftf.Weight);
                }
                // Put Stretch in boxStretch (Normal always at top).
                if (!boxStretch.Contains(ftf.Stretch))
                {
                    if (ftf.Stretch == FontStretches.Normal)
                        boxStretch.Insert(0, ftf.Stretch);
                    else
                        boxStretch.Add(ftf.Stretch);
                }
            }

            // Set selected item in boxStyle.
            if (boxStyle.Contains(fntstyPrevious))
                boxStyle.SelectedItem = fntstyPrevious;
            else
                boxStyle.SelectedIndex = 0;

            // Set selected item in boxWeight.
            if (boxWeight.Contains(fntwtPrevious))
                boxWeight.SelectedItem = fntwtPrevious;
            else
                boxWeight.SelectedIndex = 0;

            // Set selected item in boxStretch.
            if (boxStretch.Contains(fntstrPrevious))
                boxStretch.SelectedItem = fntstrPrevious;
            else
                boxStretch.SelectedIndex = 0;

            // Resume Sample update and update the Sample.
            isUpdateSuppressed = false;
            UpdateSample();
        }

        // Event handler for SelectionChanged in Style, Weight, Stretch boxes.
        void StyleOnSelectionChanged(object sender, EventArgs args)
        {
            UpdateSample();
        }

        // Event handler for TextChanged in Size box.
        void SizeOnTextChanged(object sender, TextChangedEventArgs args)
        {
            UpdateSample();
        }

        // Update the Sample text.
        void UpdateSample()
        {
            if (isUpdateSuppressed)
                return;

            lblDisplay.FontFamily = (FontFamily)boxFamily.SelectedItem;
            lblDisplay.FontStyle = (FontStyle)boxStyle.SelectedItem;
            lblDisplay.FontWeight = (FontWeight)boxWeight.SelectedItem;
            lblDisplay.FontStretch = (FontStretch)boxStretch.SelectedItem;

            double size;

            if (!Double.TryParse(boxSize.Text, out size))
                size = 8.25;

            lblDisplay.FontSize = size / 0.75;
        }

        // OK button terminates dialog box.
        void OkOnClick(object sender, RoutedEventArgs args)
        {
            DialogResult = true;
        }
    }
}
