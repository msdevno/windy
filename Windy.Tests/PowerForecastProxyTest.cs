using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windy.Domain.Entities.WindSim;
using System.Linq;

namespace Windy.Tests
{
    [TestClass]
    public class PowerForecastProxyTest
    {
        [TestMethod]
        public void RetriveData()
        {

            var powerForecastingData=PowerForecastingProxy.GetWindFarmData("62c3ecac-5131-421e-b24d-4419ccd59264/22a0edef-ef56-4f10-960c-8d6540ef07a7");

            Assert.IsNotNull(powerForecastingData,"The result is Null");
            Assert.IsNotNull(powerForecastingData.PowerForecast, "The parsed values is Null");
            Assert.AreEqual(powerForecastingData.PowerForecast.Keys.Count, 45);

            var totalWindFarmPowerForecastTimeSeries = powerForecastingData.PowerForecast.FirstOrDefault(x => x.Value.Any(c => c.Value.Type == PowerForecastingProxy.ForecastElementType.WindFarm)).Value;

            var totalFirstTuebinePowerForecastTimeSeries = powerForecastingData.PowerForecast.FirstOrDefault(x => x.Value.Any(c => c.Value.Type == PowerForecastingProxy.ForecastElementType.Turbine)).Value;

            Assert.AreEqual(totalWindFarmPowerForecastTimeSeries.Count(), totalFirstTuebinePowerForecastTimeSeries.Count, "The time series of WindFarm should be equal to First Turbine ");
        }
    }
}
