using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windy.CrossCutting.TestHelpers;
using Windy.Business.Managers;
using Windy.Domain.Contracts.Queries;
using Moq;

namespace Windy.Business.Tests.Managers
{
    [TestClass]
    public class WindPowerManagerTests : TestsFor<WindPowerManager>
    {
        [TestMethod]
        public void Start_WhenCalled_LoadsWindmillFarmsUsingTheRightQuery()
        {
            // Act
            Instance.Start();

            // Assert            
            GetMockFor<IWindmillFarmsQuery>().Verify(o => o.GetAll(), Times.Once());
        }
    }
}
