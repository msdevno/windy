using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap.AutoMocking.Moq;

namespace Windy.CrossCutting.TestHelpers
{
    [TestClass]
    public class TestsFor<TInstance> where TInstance : class
    {
        public TInstance Instance { get; set; }
        public MoqAutoMocker<TInstance> AutoMocker { get; set; }

        [TestInitialize]
        public void Init()
        {
            AutoMocker = new MoqAutoMocker<TInstance>();

            Instance = AutoMocker.ClassUnderTest;
        }

        public Mock<TContract> GetMockFor<TContract>() where TContract : class
        {
            return Mock.Get(AutoMocker.Get<TContract>());
        }
    }
}
