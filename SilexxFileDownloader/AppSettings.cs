using System.Configuration;

namespace SilexxFileDownloader
{
    public class AppSettings
    {
        public AppSettings() 
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            SourcePath = config.AppSettings.Settings["SourcePath"].Value;
            DestinationPath = config.AppSettings.Settings["DestinationPath"].Value;
            LoggingPath = config.AppSettings.Settings["LoggingPath"].Value;
        }

        public string SourcePath { get; private set; }
        public string DestinationPath { get; private set; }
        public string LoggingPath { get; private set; }
    }
}
