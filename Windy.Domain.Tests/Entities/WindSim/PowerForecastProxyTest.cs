using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;
using System.Linq;
using Windy.Domain.Entities.WindSim;
using Windy.Domain.Managers;

namespace Windy.Domain.Test.Entities.WindSim
{
    [TestClass]
    public class PowerForecastProxyTest
    {
        WindyConfiguration _configuration;

        [TestInitialize]
        public void Before_Eeach_UnitTest()
        {
            _configuration = new WindyConfiguration();
        }

        [TestMethod, TestCategory("SLOW")]
        public void GetWindFarmData_WhenCalled_ProducesActualResults()
        {
            var windSimFarmKey       = _configuration["WindSim_WindFarmKey01"];
            var windFarmData = PowerForecastingProxy.GetWindFarmData(windSimFarmKey);

            windFarmData.ShouldNotBeNull("WindFarmData should returned null");
            windFarmData.PowerForecast.ShouldNotBeNull("The PowerForecast of windfarmdata is null");
            windFarmData.PowerForecast.Keys.Count.ShouldEqual(45, "The number of keys in the powerforecast was not 45");
            

            var totalWindFarmPowerForecastTimeSeries = windFarmData.PowerForecast.FirstOrDefault(x => x.Value.Any(c => c.Value.Type == PowerForecastingProxy.ForecastElementType.WindFarm)).Value;
            var totalFirstTuebinePowerForecastTimeSeries = windFarmData.PowerForecast.FirstOrDefault(x => x.Value.Any(c => c.Value.Type == PowerForecastingProxy.ForecastElementType.Turbine)).Value;


            Assert.AreEqual(totalWindFarmPowerForecastTimeSeries.Count(), totalFirstTuebinePowerForecastTimeSeries.Count, "The time series of WindFarm should be equal to First Turbine ");
        }
    }
}
