﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;
using System;
using Windy.CrossCutting.TestHelpers;
using Windy.Domain.Entities.Samples;

namespace Windy.Data.EventHub.Tests
{
    [TestClass]
    public class SampleWriterTests : TestsFor<SampleWriter<TemperatureSample>>
    {
    }
}
