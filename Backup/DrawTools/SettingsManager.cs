using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;
using System.IO;


namespace DrawTools
{
    /// <summary>
    /// Class is responsible for loading and saving persistent application settings.
    /// Call OnStartup method from App.OnStartup.
    /// Call OnExit method from App.OnExit.
    /// 
    /// Class contains Settings instance. Settings contains all persistent
    /// application settings and can be accessed from every place in the program
    /// using SettingsManager.ApplicationSettings property.
    /// 
    /// OnStartup creates Settings instance in any case, even if file with application
    /// settings cannot be loaded.
    /// 
    /// Application settings are kept in XML file inside of Application Data directory.
    /// For every program where this class is used, change applicationDirectory
    /// and optionally settingsFileName values in this class.
    /// 
    /// For example, if applicationDirectory = "MyProgram", and 
    /// settingsFileName = "Settings.xml", settings file name is:
    /// (System Drive):\Documents and Settings\(User Name)\Local Settings\Application Data\MyProgram\Settings.xml
    /// 
    /// See also: Settings class.
    /// </summary>
    static class SettingsManager
    {
        #region Class Members

        // Persistent application settings
        static Settings settings = new Settings();  // default Settings instance
        // in the case settings cannot be loaded from file.

        // Subdirectory in Application Data where settings file is kept.
        // Change this value for every program where SettingsManager class is used.
        const string applicationDirectory = "Alex F\\DrawToolsWPF";

        // Name of settings file
        const string settingsFileName = "Settings.xml";

        #endregion Class Members

        #region Constructor

        static SettingsManager()
        {
            EnsureDirectoryExists();
        }

        #endregion Constructor

        #region Properties

        public static Settings ApplicationSettings
        {
            get { return settings; }
        }

        #endregion Properties

        #region Startup, Exit

        /// <summary>
        /// Call this function from App.OnStartup function
        /// </summary>
        public static void OnStartup()
        {
            LoadSettings();
        }

        /// <summary>
        /// Call this function from App.OnExit function
        /// </summary>
        public static void OnExit()
        {
            SaveSettings();
        }

        #endregion Overrides

        #region Other Functions

        /// <summary>
        /// Returns application settings file name
        /// </summary>
        static string SettingsFileName
        {
            get
            {
                // File is kept in Application Data directory,
                // program subdirectory.
                // See also: EnsureDirectoryExists function.
                return Path.Combine(
                    Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    applicationDirectory),
                    settingsFileName);
            }
        }


        /// <summary>
        /// Load application settings from xml file
        /// </summary>
        static void LoadSettings()
        {
            Settings tmp;

            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(Settings));

                using (Stream stream = new FileStream(SettingsFileName,
                    FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    tmp = (Settings)xml.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                return;
            }

            // If everything is OK, replace default Settings instance
            // with instance loaded from file
            settings = tmp;
        }

        /// <summary>
        /// Save application settings to xml file
        /// </summary>
        static void SaveSettings()
        {
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(Settings));

                using (Stream stream = new FileStream(SettingsFileName,
                       FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    xml.Serialize(stream, settings);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        // SuppressMessage doesn't work, to ask in MSDN forum
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        static void EnsureDirectoryExists()
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    applicationDirectory));

                if (!info.Exists)
                {
                    info.Create();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        #endregion Other Functions

    }
}
