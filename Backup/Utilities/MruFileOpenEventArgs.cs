using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// Event arguments for FileSelected event raised from MruManager.
    /// </summary>
    public class MruFileOpenEventArgs : System.EventArgs
    {
        private string fileName;

        public MruFileOpenEventArgs(string fileName)
        {
            this.fileName = fileName;
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
        }
    }
}
