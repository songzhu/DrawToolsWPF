using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace Utilities
{
    /// <summary>
    /// Class is responsible to manage MRU files list.
    /// 
    /// MruInfo passed to this instance should be read by client
    /// from some persistent storage (like XML file).
    /// 
    /// Using: create this class and pass to its constructor
    /// MruInfo class instance and Recent Files menu item.
    /// Call class methods Add and Delete when necessary
    /// (see notes in these functions).
    /// Subscribe to FileSelected event and open file in
    /// this event handler.
    /// </summary>
    public class MruManager
    {
        #region Class Members

        MruInfo infoMru;
        MenuItem menuItemMru;
        int maxLength = 10;

        public event EventHandler<MruFileOpenEventArgs> FileSelected;

        #endregion Class Members

        #region Constructor

        public MruManager(MruInfo mruInfo, MenuItem mruMenuItem)
        {
            if (mruInfo == null)
            {
                throw new ArgumentNullException("mruInfo");
            }

            if (mruMenuItem == null)
            {
                throw new ArgumentNullException("mruMenuItem");
            }

            this.infoMru = mruInfo;
            this.menuItemMru = mruMenuItem;

            UpdateMenu();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Maximal number of entries in MRU list
        /// </summary>
        public int MaxLength
        {
            get 
            { 
                return maxLength; 
            }
            set 
            { 
                if ( value > 0 )
                {
                    maxLength = value; 
                }
            }
        }

        #endregion Properties

        #region Public Functions

        /// <summary>
        /// Add file name to MRU list.
        /// Call this function after every successful Open operation
        /// (including file selected from MRU list).
        /// </summary>
        public void Add(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }


            // If file exists, move it to the beginning.
            // If not, add it to the beginning.

            List<string> list = new List<string>();

            list.Add(fileName);

            if (infoMru.RecentFiles != null)
            {
                foreach (string s in infoMru.RecentFiles)
                {
                    if (s != fileName)
                    {
                        list.Add(s);
                    }
                }
            }

            MakeArrayFromList(list);

            UpdateMenu();
        }

        /// <summary>
        /// Delete file name from MRU list.
        /// Call this function after every unsuccessful Open operation
        /// (including file selected from MRU list).
        /// </summary>
        public void Delete(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            List<string> list = new List<string>();

            if (infoMru.RecentFiles != null)
            {
                foreach (string s in infoMru.RecentFiles)
                {
                    if (s != fileName)
                    {
                        list.Add(s);
                    }
                }
            }

            MakeArrayFromList(list);

            UpdateMenu();
        }

        #endregion Public Functions

        #region Other Functions

        /// <summary>
        /// Fill MRU menu
        /// </summary>
        void UpdateMenu()
        {
            menuItemMru.Items.Clear();

            if (infoMru.RecentFiles != null )
            {
                foreach (string fullName in infoMru.RecentFiles)
                {
                    MenuItem item = new MenuItem();
                    string shortName = System.IO.Path.GetFileName(fullName);
                    ToolTip toolTip = new ToolTip();
                    toolTip.Content = fullName;

                    item.Header = shortName;
                    item.ToolTip = toolTip;
                    item.Tag = fullName;

                    item.Click += new RoutedEventHandler(item_Click);

                    menuItemMru.Items.Add(item);
                }
            }

            menuItemMru.IsEnabled = (infoMru.RecentFiles.Length > 0);
        }


        /// <summary>
        /// MRU item is selected - report to client.
        /// </summary>
        void item_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string fileName = (string)item.Tag;

            if ( FileSelected != null )
            {
                FileSelected(this, new MruFileOpenEventArgs(fileName));
            }
        }

        /// <summary>
        /// Make string array from list and set it in MriInfo
        /// </summary>
        void MakeArrayFromList(List<string> list)
        {
            string[] files = new string[Math.Min(list.Count, maxLength)];

            int index = 0;

            foreach(string s in list)
            {
                files[index++] = s;

                if ( index >= maxLength )
                {
                    break;
                }
            }

            infoMru.RecentFiles = files;
        }

        #endregion Other Functions
    }
}
