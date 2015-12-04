using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;
using System;
using Windy.Business.Converters;
using Windy.CrossCutting.TestHelpers;
using Windy.Domain.Entities.Samples;

namespace Windy.Business.Tests.Converters
{
    [TestClass]
    public class ByteArrayConverterTests : TestsFor<ByteArrayConverter<TemperatureSample>>
    {
        [TestMethod]
        public void ConvertToBytes_SampleIsNull_ResultIsEmptyArray()
        {
            // Arrange
            TemperatureSample nullSample = null;

            // Act
            var result = Instance.ConvertToBytes(nullSample);

            // Assert            
            result.ShouldBeEmpty();
        }

        [TestMethod]
        public void ConvertToBytes_SampleIsValid_ResultIsByteArrayOfDoubleLength()
        {
            // Act
            var result = Instance.ConvertToBytes(ValidTemperatureSample);

            // Assert            
            result.Length.ShouldBeGreaterThanOrEqualTo(sizeof(double));
        }

        [TestMethod]
        public void ConvertFromBytes_ObjectIsNull_ResultIsNull()
        {
            // Arrange
            var emptyArray = new byte[0];

            // Act
            var result = Instance.ConvertFromBytes(emptyArray);

            // Assert            
            result.ShouldBeNull();
        }

        [TestMethod]
        public void ConvertFromBytes_ObjectIsValid_ResultIsTemperatureSample()
        {
            // Arrange
            var bytes = Instance.ConvertToBytes(ValidTemperatureSample);

            // Act
            var result = Instance.ConvertFromBytes(bytes);

            // Assert            
            result.ShouldBeType<TemperatureSample>();
        }

        [TestMethod]
        public void ConvertFromBytes_ObjectIsValid_SampleContainsOriginalValues()
        {
            // Arrange
            var bytes = Instance.ConvertToBytes(ValidTemperatureSample);

            // Act
            var result = Instance.ConvertFromBytes(bytes);

            // Assert            
            result.WindmillId.ShouldEqual(ValidTemperatureSample.WindmillId);
            result.Temperature.ShouldEqual(ValidTemperatureSample.Temperature);

            //TODO: Compare dates too
        }


        private TemperatureSample ValidTemperatureSample
        {
            get
            {
                return new TemperatureSample
                {
                    WindmillId = 1,
                    SampleTime = DateTime.Now,
                    Temperature = 25.0
                };
            }
        }
    }
}
