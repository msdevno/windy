using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;
using System;
using Windy.CrossCutting.TestHelpers;
using Windy.Domain.Entities.Samples.Samples;

namespace Windy.Data.EventHub.Tests
{
    [TestClass]
    public class DataWriterTests : TestsFor<DataWriter<TemperatureSample>>
    {
       
    }
}
