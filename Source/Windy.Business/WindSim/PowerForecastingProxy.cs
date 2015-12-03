using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Windy.Domain.Contracts.WindSim;
using Windy.Domain.Entities.WindSim;

namespace Windy.Business.WindSim
{
    public class PowerForecastingProxy : IPowerForecastingProxy
    {
        /// <summary>
        /// Retrive Power Forecast Data fro WindSim API
        /// </summary>
        /// <param name="key">WindSim WindFarm Key</param>
        /// <returns>Power Forecast data for a specific WindFarm</returns>
        public PowerForecastData GetWindFarmData(string key)
        {
            string requestUrl = $"https://windsimapi.azurewebsites.net/api/V1/Stream/{key}";
            using (var client = new WebClient())
            {
                ///TODO: Check Web Errors
                return ParseForecastingData(client.DownloadString(new Uri(requestUrl)));
            }
        }


        /// <summary>
        /// Parse an WindSim Power Forecast result into TimeSeries
        /// </summary>
        /// <param name="data"> String all the data </param>
        /// <returns>TimeSeries of Power Forecast</returns>
        private static PowerForecastData ParseForecastingData(string data)
        {
            var result = new PowerForecastData();
            var tmpColumnIndex = new Dictionary<int, string>();
            foreach (var line in data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.StartsWith("Date/Time	ALL"))//FirstLine
                {
                    int column = 1;
                    Array.ForEach(line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray(), t =>
                    {
                        result.PowerForecast.Add(t.Trim(), new Dictionary<DateTime, ForecastElementInfo>());
                        tmpColumnIndex.Add(column++, t.Trim());
                    });
                    continue;
                }
                var powerLineValues = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                DateTime date = DateTime.ParseExact(powerLineValues[0], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                for (int i = 1; i < powerLineValues.Length; ++i)
                {
                    result.PowerForecast[tmpColumnIndex[i]].Add(date, new ForecastElementInfo()
                    {
                        Name = tmpColumnIndex[i],
                        PowerKW = double.Parse(powerLineValues[i], new CultureInfo("en-US"))
                    });
                }
            }
            return result;
        }

    }
}
