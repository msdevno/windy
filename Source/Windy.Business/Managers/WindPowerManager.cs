using System.Linq;
using Windy.Domain.Contracts.Factories;
using Windy.Domain.Contracts.Managers;
using Windy.Domain.Contracts.Queries;

namespace Windy.Business.Managers
{
    public class WindPowerManager : IWindPowerManager
    {
        private readonly IExceptionManager          _exceptionManager;
        private readonly ISampleGatherer            _sampleGatherer;
        private readonly ISamplesTransmitterFactory _transmitterFactory;
        private readonly IWindmillFarmsQuery        _windmillFarmsQuery;


        public WindPowerManager(IWindmillFarmsQuery windmillFarmsQuery, ISamplesTransmitterFactory transmitterFactory, ISampleGatherer sampleGatherer, IExceptionManager exceptionManager)
        {
            _windmillFarmsQuery = windmillFarmsQuery;
            _transmitterFactory = transmitterFactory;
            _sampleGatherer     = sampleGatherer;
            _exceptionManager   = exceptionManager;
        }


        public void Start()
        {
            var allFarms = _exceptionManager.Execute(() => _windmillFarmsQuery.GetAll(), "Retrieving all Windmill Farms");
            if (allFarms == null || allFarms.Any() == false)
                return;

            var samples = _sampleGatherer.GetSamplesFrom(allFarms); 

            foreach(var sample in samples)
            {
                _exceptionManager.Execute(() => 
                    _transmitterFactory.Transmit(sample), 
                    $"Transmitting sample for Wind farm: {sample.WindFarmId}, Windmill: {sample.WindmillId}");
            }
        }
    }
}
