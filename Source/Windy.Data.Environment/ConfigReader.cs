using Windy.Domain.Contracts.Queries;
using System.Configuration;
using System.IO;
using System;

namespace Windy.Data.Environment
{
    public class ConfigReader : IConfigReader
    {
        private bool _isInAzure;
        private Configuration _config;

        public ConfigReader()
        {
            var userProfilePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            var configurationFile = Path.Combine(userProfilePath, "Windy.config");

            if (!File.Exists(configurationFile))
            {
                var azureRegionName = System.Environment.GetEnvironmentVariable("REGION_NAME");
                if (!string.IsNullOrEmpty(azureRegionName))
                {
                    _isInAzure = true;
                }
                else
                {
                    throw new FileNotFoundException("Did not find the file 'Windy.config' in your user profile folder!");
                }
            }
            else
            {
                var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = configurationFile };
                try
                {
                    _config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                }
                catch
                {
                    throw;
                }
            }
        }

        public string this[string settingName]
        {
            get
            {
                if (_isInAzure)
                {
                    var azureSetting = string.Format($"APPSETTING_{settingName}");
                    return System.Environment.GetEnvironmentVariable(azureSetting);
                }

                return _config.AppSettings.Settings[settingName].Value;
            }
        }
    }
}
