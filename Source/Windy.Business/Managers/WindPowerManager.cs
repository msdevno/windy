using System;
using System.Collections.Generic;
using System.Linq;
using Windy.Domain.Contracts;
using Windy.Domain.Contracts.Calculators;
using Windy.Domain.Contracts.Factories;
using Windy.Domain.Contracts.Managers;
using Windy.Domain.Contracts.Queries;
using Windy.Domain.Contracts.Yr;
using Windy.Domain.Entities.Samples;

namespace Windy.Business.Managers
{
    public class WindPowerManager : IWindPowerManager
    {
        private readonly ILogger _logger;
        private readonly IMegaWattCalculator _megawattCalculator;
        private readonly ISamplesTransmitterFactory _transmitterFactory;
        private readonly IWeatherProxy       _weatherProxy;
        private readonly IWindmillFarmsQuery _windmillFarmsQuery;

        public WindPowerManager(IWindmillFarmsQuery windmillFarmsQuery, IWeatherProxy weatherProxy, IMegaWattCalculator megawattCalculator, ISamplesTransmitterFactory transmitterFactory, ILogger logger)
        {
            _windmillFarmsQuery = windmillFarmsQuery;
            _weatherProxy       = weatherProxy;
            _megawattCalculator = megawattCalculator;
            _transmitterFactory = transmitterFactory;
            _logger             = logger;
        }

        public void Start()
        {
            var allFarms = _windmillFarmsQuery.GetAll();
            if (allFarms == null || allFarms.Any() == false)
                return;

            var samples = GatherSampleData(allFarms);

            foreach(var sample in samples)
            {
                _transmitterFactory.Transmit(sample);
            }
        }

        private IEnumerable<WindmillSample> GatherSampleData(List<Domain.Entities.WindmillFarm> allFarms)
        {
            var samples = new List< WindmillSample >();

            foreach (var farm in allFarms)
            {
                foreach (var mill in farm.Windmills)
                {
                    var weatherData = _weatherProxy.GetWeatherDataForLocation(mill.Location.Latitude, mill.Location.Longitude);
                    var locationData = weatherData.product.time[0].location;

                    var windSpeed = (double)locationData.windSpeed.mps;
                    samples.Add(new WindSpeedSample { WindFarmId = farm.Id, WindmillId = mill.Id, SampleTime = DateTime.Now, WindSpeed = windSpeed });
                    samples.Add(new TemperatureSample { WindFarmId = farm.Id, WindmillId = mill.Id, SampleTime = DateTime.Now, Temperature = (double)locationData.temperature.value });

                    var megawatt = _megawattCalculator.CalculateForGeneratorBasedOnWindSpeed(mill.Generator, windSpeed);
                    samples.Add(new MegawattSample { WindFarmId = farm.Id, WindmillId = mill.Id, SampleTime = DateTime.Now, MegaWatt = megawatt });
                }
                _logger.LogInformation($"Wind Farm: '{farm.Name}' {farm.Windmills.Count()} windmill samples gathered.");
            }
            return samples;
        }
    }
}
