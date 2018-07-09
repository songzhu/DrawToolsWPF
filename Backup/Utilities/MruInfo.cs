using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace Utilities
{
    /// <summary>
    /// Class contains information about MRU files list.
    /// Client's responsibility is to create instance of this class,
    /// load it from some persistent storage (like XML file)
    /// when program starts, and save it when program ends.
    /// 
    /// This class itself doesn't make any actions, it only keeps information
    /// about recently used files.
    /// Used together with MruManager class.
    /// </summary>
    public class MruInfo
    {
        string[] recentFiles;

        public MruInfo()
        {
            recentFiles = new string[0];
        }

        /// <summary>
        /// List of file names.
        /// This property should be serialized.
        /// </summary>
        public string[] RecentFiles
        {
            get { return recentFiles; }
            set { recentFiles = value; }
        }
    }
}
