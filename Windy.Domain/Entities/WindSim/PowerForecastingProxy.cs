using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Windy.Domain.Entities.WindSim
{
    public class PowerForecastingProxy
    {
        /// <summary>
        /// Retrive Power Forecast Data fro WindSim API
        /// </summary>
        /// <param name="key">WindSim WindFarm Key</param>
        /// <returns>Power Forecast data for a specific WindFarm</returns>
        public static PowerForecastData GetWindFarmData(string key)
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


        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class PowerForecastData
        {
            private Dictionary<string, Dictionary<DateTime, ForecastElementInfo>> _powerForecast = new Dictionary<string, Dictionary<DateTime, ForecastElementInfo>>();
            public Dictionary<string, Dictionary<DateTime, ForecastElementInfo>> PowerForecast
            {
                get { return _powerForecast; }
                set { _powerForecast = value; }
            }
        }

        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class ForecastElementInfo
        {
            public ForecastElementType Type { get; private set; }

            private string _name;
            public string Name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                    Type = value == "ALL" ? ForecastElementType.WindFarm : ForecastElementType.Turbine;
                }
            }

            public double PowerKW { get; set; }
        }


        /// <summary>
        /// Represents the ElementType of Forecast
        /// </summary>
        public enum ForecastElementType
        {
            WindFarm = 0,
            Turbine
        }
    }
}
