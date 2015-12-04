using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windy.CrossCutting.TestHelpers;
using Windy.Business.Calculators;
using Windy.Domain.Entities;
using Should;
using System.Linq;

namespace Windy.Business.Tests.Calculators
{
    [TestClass]
    public class MegaWattCalculatorTests : TestsFor<MegaWattCalculator>
    {
        [TestMethod]
        public void CalculateForGeneratorBasedOnWindSpeed_GeneratorIsNull_ResultIs0()
        {
            // Arrange
            PowerGenerator nullGenerator = null;

            // Act
            var result = Instance.CalculateForGeneratorBasedOnWindSpeed(nullGenerator, ValidWindSpeed);

            // Assert            
            result.ShouldEqual(0.0);
        }


        [TestMethod]
        public void CalculateForGeneratorBasedOnWindSpeed_WindSpeedIsNegative_ResultIs0()
        {
            // Arrange
            var negativeWindSpeed = -1.0;


            // Act
            var result = Instance.CalculateForGeneratorBasedOnWindSpeed(ValidPowerGenerator, negativeWindSpeed);

            // Assert            
            result.ShouldEqual(0.0);
        }

        [TestMethod]
        public void CalculateForGeneratorBasedOnWindSpeed_WindspeedIsOptimal_ReturnsMaximumMwForThatGenerator()
        {
            // Arrange
            var optimalWindSpeed = ValidPowerGenerator.MinOptimalWindspeed + ((ValidPowerGenerator.MaxOptimalWindspeed - ValidPowerGenerator.MinOptimalWindspeed) / 2);

            // Act
            var result = Instance.CalculateForGeneratorBasedOnWindSpeed(ValidPowerGenerator, optimalWindSpeed);

            // Assert   
            result.ShouldEqual(ValidPowerGenerator.MaxOutputMw);
        }


        private PowerGenerator ValidPowerGenerator { get { return PowerGenerator.Generators.First(); } }

        private double ValidWindSpeed { get { return 7.2; } }
    }
}
