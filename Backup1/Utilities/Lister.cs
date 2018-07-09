//---------------------------------------
// Lister.cs (c) 2006 by Charles Petzold
//---------------------------------------
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;


namespace Petzold.ChooseFont
{
    class Lister : ContentControl
    {
        ScrollViewer scroll;
        StackPanel stack;
        ArrayList list = new ArrayList();
        int indexSelected = -1;

        // Public event.
        public event EventHandler SelectionChanged;

        // Constructor.
        public Lister()
        {
            Focusable = false;

            // Make Border the content of the ContentControl.
            Border bord = new Border();
            bord.BorderThickness = new Thickness(1);
            bord.BorderBrush = SystemColors.ActiveBorderBrush;
            bord.Background = SystemColors.WindowBrush;
            Content = bord;

            // Make ScrollViewer the child of the border.
            scroll = new ScrollViewer();
            scroll.Focusable = false;
            scroll.Padding = new Thickness(2, 0, 0, 0);
            bord.Child = scroll;

            // Make StackPanel the content of the ScrollViewer.
            stack = new StackPanel();
            scroll.Content = stack;

            // Install a handler for the mouse left button down.
            AddHandler(TextBlock.MouseLeftButtonDownEvent,
                new MouseButtonEventHandler(TextBlockOnMouseLeftButtonDown));

            Loaded += OnLoaded;
        }
        void OnLoaded(object sender, RoutedEventArgs args)
        {
            // Scroll the selected item into view when Lister is first displayed.
            ScrollIntoView();
        }

        // Public methods to add, insert, etc, items in Lister.
        public void Add(object obj)
        {
            list.Add(obj);
            TextBlock txtblk = new TextBlock();
            txtblk.Text = obj.ToString();
            stack.Children.Add(txtblk);
        }
        public void Insert(int index, object obj)
        {
            list.Insert(index, obj);
            TextBlock txtblk = new TextBlock();
            txtblk.Text = obj.ToString();
            stack.Children.Insert(index, txtblk);
        }
        public void Clear()
        {
            SelectedIndex = -1;
            stack.Children.Clear();
            list.Clear();
        }
        public bool Contains(object obj)
        {
            return list.Contains(obj);
        }
        public int Count
        {
            get { return list.Count; }
        }

        // This method is called to select an item based on a typed letter.
        public void GoToLetter(char ch)
        {
            int offset = SelectedIndex + 1;

            for (int i = 0; i < Count; i++)
            {
                int index = (i + offset) % Count;

                if (Char.ToUpper(ch, CultureInfo.InvariantCulture) == Char.ToUpper(list[index].ToString()[0],
                    CultureInfo.InvariantCulture))
                {
                    SelectedIndex = index;
                    break;
                }
            }
        }

        // SelectedIndex property is responsible for displaying selection bar.
        public int SelectedIndex
        {
            set
            {
                if (value < -1 || value >= Count)
                    throw new ArgumentOutOfRangeException("SelectedIndex");

                if (value == indexSelected)
                    return;

                if (indexSelected != -1)
                {
                    TextBlock txtblk = stack.Children[indexSelected] as TextBlock;
                    txtblk.Background = SystemColors.WindowBrush;
                    txtblk.Foreground = SystemColors.WindowTextBrush;
                }

                indexSelected = value;

                if (indexSelected > -1)
                {
                    TextBlock txtblk = stack.Children[indexSelected] as TextBlock;
                    txtblk.Background = SystemColors.HighlightBrush;
                    txtblk.Foreground = SystemColors.HighlightTextBrush;
                }
                ScrollIntoView();

                // Trigger SelectionChanged event.
                OnSelectionChanged(EventArgs.Empty);
            }
            get
            {
                return indexSelected;
            }
        }

        // SelectedItem property makes use of SelectedIndex.
        public object SelectedItem
        {
            set
            {
                SelectedIndex = list.IndexOf(value);
            }
            get
            {
                if (SelectedIndex > -1)
                    return list[SelectedIndex];

                return null;
            }
        }

        // Public methods to page up and down through the list.
        public void PageUp()
        {
            if (SelectedIndex == -1 || Count == 0)
                return;

            int index = SelectedIndex - (int)(Count *
                                scroll.ViewportHeight / scroll.ExtentHeight);
            if (index < 0)
                index = 0;

            SelectedIndex = index;
        }
        public void PageDown()
        {
            if (SelectedIndex == -1 || Count == 0)
                return;

            int index = SelectedIndex + (int)(Count *
                                scroll.ViewportHeight / scroll.ExtentHeight);
            if (index > Count - 1)
                index = Count - 1;

            SelectedIndex = index;
        }

        // Private method to scroll selected item into view.
        void ScrollIntoView()
        {
            if (Count == 0 || SelectedIndex == -1 ||
                                scroll.ViewportHeight > scroll.ExtentHeight)
                return;

            double heightPerItem = scroll.ExtentHeight / Count;
            double offsetItemTop = SelectedIndex * heightPerItem;
            double offsetItemBot = (SelectedIndex + 1) * heightPerItem;

            if (offsetItemTop < scroll.VerticalOffset)
                scroll.ScrollToVerticalOffset(offsetItemTop);

            else if (offsetItemBot > scroll.VerticalOffset + scroll.ViewportHeight)
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset +
                    offsetItemBot - scroll.VerticalOffset - scroll.ViewportHeight);
        }

        // Event handler and trigger.
        void TextBlockOnMouseLeftButtonDown(object sender,
                                            MouseButtonEventArgs args)
        {
            if (args.Source is TextBlock)
                SelectedIndex = stack.Children.IndexOf(args.Source as TextBlock);
        }

        protected virtual void OnSelectionChanged(EventArgs args)
        {
            if (SelectionChanged != null)
                SelectionChanged(this, args);
        }
    }
}
