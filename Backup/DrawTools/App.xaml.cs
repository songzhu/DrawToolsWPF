using System;
using System.Windows;
using System.Data;
using System.Xml;
using System.Configuration;

namespace DrawTools
{
    /// <summary>
    /// Application class
    /// </summary>
    public partial class App : System.Windows.Application
    {
        #region Overrides

        protected override void OnStartup(StartupEventArgs e)
        {
            SettingsManager.OnStartup();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            SettingsManager.OnExit();

            base.OnExit(e);
        }

        #endregion Overrides
    }
}