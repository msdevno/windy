using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Windy.Domain.Entities.WindSim;
using Windy.Domain.Managers;

namespace Windy.Domain.Test.Entities.WindSim
{
    [TestClass]
    public class PowerForecastProxyTest
    {
        WindyConfiguration _configuration = new WindyConfiguration();

        [TestMethod]
        public void RetriveData()
        {

            var powerForecastingData=PowerForecastingProxy.GetWindFarmData(_configuration["WindSim_WindFarmKey01"]);

            Assert.IsNotNull(powerForecastingData,"The result is Null");
            Assert.IsNotNull(powerForecastingData.PowerForecast, "The parsed values is Null");
            Assert.AreEqual(powerForecastingData.PowerForecast.Keys.Count, 45);

            var totalWindFarmPowerForecastTimeSeries = powerForecastingData.PowerForecast.FirstOrDefault(x => x.Value.Any(c => c.Value.Type == PowerForecastingProxy.ForecastElementType.WindFarm)).Value;

            var totalFirstTuebinePowerForecastTimeSeries = powerForecastingData.PowerForecast.FirstOrDefault(x => x.Value.Any(c => c.Value.Type == PowerForecastingProxy.ForecastElementType.Turbine)).Value;

            Assert.AreEqual(totalWindFarmPowerForecastTimeSeries.Count(), totalFirstTuebinePowerForecastTimeSeries.Count, "The time series of WindFarm should be equal to First Turbine ");
        }
    }
}
