using System;
using System.Collections.Generic;
using Windy.Domain.Contracts.Calculators;
using Windy.Domain.Contracts.Managers;
using Windy.Domain.Contracts.Queries;
using Windy.Domain.Contracts.Yr;
using Windy.Domain.Entities.Samples;

namespace Windy.Business.Managers
{
    public class WindPowerManager : IWindPowerManager
    {
        private readonly IMegaWattCalculator _megawattCalculator;
        private readonly IWeatherProxy       _weatherProxy;
        private readonly IWindmillFarmsQuery _windmillFarmsQuery;

        public WindPowerManager(IWindmillFarmsQuery windmillFarmsQuery, IWeatherProxy weatherProxy, IMegaWattCalculator megawattCalculator)
        {
            _windmillFarmsQuery = windmillFarmsQuery;
            _weatherProxy       = weatherProxy;
            _megawattCalculator = megawattCalculator;
        }

        public void Start()
        {
            var allFarms = _windmillFarmsQuery.GetAll();

            var samples = GatherSampleData(allFarms);



        }

        private IEnumerable<WindmillSample> GatherSampleData(List<Domain.Entities.WindmillFarm> allFarms)
        {
            var samples = new List< WindmillSample >();
            foreach (var farm in allFarms)
                foreach (var mill in farm.Windmills)
                {
                    var weatherData = _weatherProxy.GetWeatherDataForLocation(mill.Location.Latitude, mill.Location.Longitude);
                    var locationData = weatherData.product.time[0].location;

                    var windSpeed = (double)locationData.windSpeed.mps;
                    samples.Add(new WindSpeedSample { WindmillId = mill.Id, SampleTime = DateTime.Now, WindSpeed = windSpeed });
                    samples.Add(new TemperatureSample { WindmillId = mill.Id, SampleTime = DateTime.Now, Temperature = (double)locationData.temperature.value });

                    var megawatt = _megawattCalculator.CalculateForGeneratorBasedOnWindSpeed(mill.Generator, windSpeed);
                    samples.Add(new MegawattSample { WindmillId = mill.Id, SampleTime = DateTime.Now, MegaWatt = megawatt });
                }

            return samples;
        }
    }
}
