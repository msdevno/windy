using Microsoft.WindowsAzure;
using System;
using System.Configuration;
using System.IO;

namespace Windy.Domain.Managers
{
    public class WindyConfiguration
    {
        private bool _isInAzure;
        private Configuration _config;

        public WindyConfiguration()
        {
            var userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var configurationFile = Path.Combine(userProfilePath, "Windy.config");

            if (!File.Exists(configurationFile))
            {
                var azureRegionName = Environment.GetEnvironmentVariable("REGION_NAME");
                if(!string.IsNullOrEmpty(azureRegionName))
                {
                    Console.WriteLine($"App is running in Azure region: {azureRegionName}");
                    _isInAzure = true;
                }
                else
                {
                    throw new FileNotFoundException("Did not find the file 'Windy.config' in your user profile folder!");
                }
            }
            else
            {
                Console.WriteLine("App is running on local developer machine");
                var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = configurationFile };
                _config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            }
        }


        public string this[string settingName]
        {
            get
            {
                if(_isInAzure)
                {
                    var azureSetting = string.Format($"APPSETTING_{settingName}");
                    return Environment.GetEnvironmentVariable(azureSetting);
                }

                return _config.AppSettings.Settings[settingName].Value;
            }
        }
    }
}
