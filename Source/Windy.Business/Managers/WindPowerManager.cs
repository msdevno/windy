using System;
using System.Collections.Generic;
using System.Linq;
using Windy.Domain.Contracts;
using Windy.Domain.Contracts.Calculators;
using Windy.Domain.Contracts.Factories;
using Windy.Domain.Contracts.Managers;
using Windy.Domain.Contracts.Queries;
using Windy.Domain.Contracts.Yr;
using Windy.Domain.Entities;
using Windy.Domain.Entities.Samples;

namespace Windy.Business.Managers
{
    public class WindPowerManager : IWindPowerManager
    {
        private readonly ISampleGatherer            _sampleGatherer;
        private readonly ISamplesTransmitterFactory _transmitterFactory;
        private readonly IWindmillFarmsQuery        _windmillFarmsQuery;


        public WindPowerManager(IWindmillFarmsQuery windmillFarmsQuery, ISamplesTransmitterFactory transmitterFactory, ISampleGatherer sampleGatherer)
        {
            _windmillFarmsQuery = windmillFarmsQuery;
            _transmitterFactory = transmitterFactory;
            _sampleGatherer     = sampleGatherer;
        }


        public void Start()
        {
            var allFarms = _windmillFarmsQuery.GetAll();
            if (allFarms == null || allFarms.Any() == false)
                return;

            var samples = _sampleGatherer.GetSamplesFrom(allFarms); 

            foreach(var sample in samples)
            {
                _transmitterFactory.Transmit(sample);
            }
        }
    }
}
