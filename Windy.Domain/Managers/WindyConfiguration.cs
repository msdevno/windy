using System;
using System.Configuration;
using System.IO;

namespace Windy.Domain.Managers
{
    public class WindyConfiguration
    {
        private Configuration _config;

        public WindyConfiguration()
        {
            var userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var configurationFile = Path.Combine(userProfilePath, "Windy.config");

            if (!File.Exists(configurationFile))
                throw new FileNotFoundException("Did not find the file 'Windy.config' in your user profile folder!");

            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = configurationFile };
            _config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }


        public string this[string index]
        {
            get
            {
                return _config.AppSettings.Settings[index].Value;
            }
        }
    }
}
