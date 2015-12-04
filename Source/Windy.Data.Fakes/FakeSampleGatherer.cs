using System;
using System.Collections.Generic;
using System.Linq;
using Windy.Domain.Contracts;
using Windy.Domain.Contracts.Calculators;
using Windy.Domain.Contracts.Queries;
using Windy.Domain.Contracts.Yr;
using Windy.Domain.Entities;
using Windy.Domain.Entities.Samples;
using Windy.Domain.Entities.Yr;

namespace Windy.Data.Fakes
{
    public class FakeSampleGatherer : ISampleGatherer
    {
        private readonly ILogger             _logger;
        private readonly IMegaWattCalculator _megawattCalculator;
        private readonly IWeatherProxy       _weatherProxy;

        public FakeSampleGatherer(IWeatherProxy weatherProxy, ILogger logger, IMegaWattCalculator megawattCalculator)
        {
            _weatherProxy       = weatherProxy;
            _logger             = logger;
            _megawattCalculator = megawattCalculator;
        }

        public IEnumerable<WindmillSample> GetSamplesFrom(IEnumerable<WindmillFarm> farms)
        {
            var samples = new List<WindmillSample>();

            foreach (var farm in farms)
            {
                foreach (var mill in farm.Windmills)
                {
                    var locationData = GetLocalWeatherForMill(mill);

                    // Sample Windspeed
                    var windSpeed = (double)locationData.windSpeed.mps;
                    samples.Add(new WindSpeedSample { WindFarmId = farm.Id, WindmillId = mill.Id, SampleTime = DateTime.Now, WindSpeed = windSpeed });

                    // Sample Temperature
                    samples.Add(new TemperatureSample { WindFarmId = farm.Id, WindmillId = mill.Id, SampleTime = DateTime.Now, Temperature = (double)locationData.temperature.value });

                    // Sample Megawatt
                    var megawatt = _megawattCalculator.CalculateForGeneratorBasedOnWindSpeed(mill.Generator, windSpeed);
                    samples.Add(new MegawattSample { WindFarmId = farm.Id, WindmillId = mill.Id, SampleTime = DateTime.Now, MegaWatt = megawatt });
                }
                _logger.LogInformation($"Wind Farm: '{farm.Name}' {farm.Windmills.Count()} windmill samples gathered.");
            }
            return samples;
        }

        private weatherdataProductTimeLocation GetLocalWeatherForMill(Windmill mill)
        {
            var weatherData = _weatherProxy.GetWeatherDataForLocation(mill.Location.Latitude, mill.Location.Longitude);
            var locationData = weatherData.product.time[0].location;
            return locationData;
        }
    }
}
