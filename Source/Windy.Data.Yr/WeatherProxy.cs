using System;
using System.Net;
using System.Xml.Serialization;
using Windy.Domain.Contracts.Yr;
using Windy.Domain.Entities.Yr;

namespace Windy.Data.Yr
{
    public class WeatherProxy : IWeatherProxy
    {
        public weatherdata GetWeatherDataForLocation(double latitude, double longitude)
        {
            var client = new WebClient();
            client.Headers.Add("content-type", "application/json");
            try
            {
                var uri = string.Format($"http://api.yr.no/weatherapi/locationforecast/1.9/?lat={latitude};lon={longitude}");
                var stream = client.OpenRead(new Uri(uri));
                var serializer = new XmlSerializer(typeof(weatherdata));

                return serializer.Deserialize(stream) as weatherdata;
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong: " + e.Message);
            }
            return null;
        }
    }
}

