using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windy.CrossCutting.TestHelpers;
using Windy.Business.Managers;
using Windy.Domain.Contracts.Queries;
using Moq;
using Windy.Domain.Entities;
using System.Collections.Generic;
using Windy.Domain.Contracts.Factories;
using Windy.Domain.Entities.Samples;
using System.Linq;
using StructureMap.AutoMocking.Moq;
using Windy.Domain.Contracts.Managers;

namespace Windy.Business.Tests.Managers
{
    [TestClass]
    public class WindPowerManagerTests : TestsFor<WindPowerManager>
    {
        public override void Before_Each_UnitTest()
        {
            AutoMocker.Container.Configure(o => o.For<IExceptionManager>().Use<ExceptionManager>());
        }

        [TestMethod]
        public void Start_WhenCalled_LoadsWindmillFarmsUsingTheRightQuery()
        {
            // Act
            Instance.Start();

            // Assert            
            GetMockFor<IWindmillFarmsQuery>().Verify(o => o.GetAll(), Times.Once());
        }


        [TestMethod]
        public void Start_WhenNoWindFarmsExist_DoesNotAttemptToGatherSamples()
        {
            // Act
            Instance.Start();

            // Assert
            
            GetMockFor<ISampleGatherer>().Verify(o => o.GetSamplesFrom(It.IsAny<IEnumerable<WindmillFarm>>()), Times.Never());
        }


        [TestMethod]
        public void Start_WindFarmsAndWindmillsAreObtained_TransmitsAllSamples()
        {
            // Arrange
            var allFarms = new List<WindmillFarm> { ValidFarm };
            GetMockFor<IWindmillFarmsQuery>().Setup(o => o.GetAll()).Returns(allFarms);
            GetMockFor<ISampleGatherer>().Setup(o => o.GetSamplesFrom(allFarms)).Returns(ValidSamples);

            // Act
            Instance.Start();

            // Assert
            GetMockFor<ISamplesTransmitterFactory>()
                .Verify(o => o.Transmit(It.IsAny<WindmillSample>()), Times.AtLeastOnce());
        }


        private WindmillFarm ValidFarm
        {
            get
            {
                return new WindmillFarm
                {
                    Id = 1,
                    Name = "ValidWindMill",
                    Windmills = new List<Windmill>{ ValidWindMill }
                };
            }
        }


        private Windmill ValidWindMill
        {
            get
            {
                return new Windmill
                {
                    Id = 1,
                    Generator = PowerGenerator.Generators.First(),
                    Location = new Location { Name = "Somewhere", Latitude = 13.3, Longitude = 70.1 }
                };
            }
        }


        private IEnumerable<WindmillSample> ValidSamples
        {
            get
            {
                return new List<WindmillSample>
                {
                    new TemperatureSample { WindFarmId = 1, WindmillId = 1, SampleTime = DateTime.Now, Temperature = 25.7 }
                };
            }
        }

    }
}
