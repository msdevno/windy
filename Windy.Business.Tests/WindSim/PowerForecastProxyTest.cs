using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;
using System.Linq;
using Windy.Business.WindSim;
using Windy.Domain.Entities.WindSim;
using Windy.Domain.Managers;

namespace Windy.Business.Tests.WindSim
{
    [TestClass]
    public class PowerForecastProxyTest
    {
        PowerForecastingProxy Instance { get; set; }
        WindyConfiguration _configuration;
        private string _windSimFarmKey;

        [TestInitialize]
        public void Before_Eeach_UnitTest()
        {
            _configuration  = new WindyConfiguration();
            _windSimFarmKey = _configuration["WindSim_WindFarmKey01"];
            Instance        = new PowerForecastingProxy();
        }

        [TestMethod, TestCategory("SLOW")]
        public void GetWindFarmData_WhenCalled_ProducesActualResults()
        {         
             
            // Act  
            var windFarmData   = Instance.GetWindFarmData(_windSimFarmKey);

            // Assert
            windFarmData.ShouldNotBeNull("WindFarmData should returned null");
            windFarmData.PowerForecast.ShouldNotBeNull("The PowerForecast of windfarmdata is null");
            windFarmData.PowerForecast.Keys.Count.ShouldEqual(45, "The number of keys in the powerforecast was not 45");            

            var totalWindFarmPowerForecastTimeSeries = windFarmData.PowerForecast.FirstOrDefault(x => x.Value.Any(c => c.Value.Type == ForecastElementType.WindFarm)).Value;
            var totalFirstTuebinePowerForecastTimeSeries = windFarmData.PowerForecast.FirstOrDefault(x => x.Value.Any(c => c.Value.Type == ForecastElementType.Turbine)).Value;

            totalWindFarmPowerForecastTimeSeries.Count()
                .ShouldEqual(totalFirstTuebinePowerForecastTimeSeries.Count, 
                "The time series of WindFarm should be equal to First Turbine ");
        }
    }
}
