﻿using StructureMap;
using Windy.Data.Fakes;
using Windy.Domain.Contracts;
using Windy.Domain.Contracts.Factories;
using Windy.Domain.Contracts.Queries;

namespace Windy.DependencyInversion
{
    public class RuntimeRegistry : Registry
    {
        public RuntimeRegistry()
        {
            Scan(x =>
            {
                // Data Layer
                x.Assembly("Windy.Data.Environment");
                x.Assembly("Windy.Data.EventHub");
                x.Assembly("Windy.Data.Fakes");
                x.Assembly("Windy.Data.Yr");

                // Domain Layer
                x.Assembly("Windy.Domain");

                // Business Layer
                x.Assembly("Windy.Business");               

                x.WithDefaultConventions();
            });

            // Fakes
            For<ISampleGatherer>(    ).Use<FakeSampleGatherer>();
            For<IWindmillFarmsQuery>().Use<FakeWindmillFarmsQuery>();
            For<ILogger>(            ).Use<FakeLogger>();

            // Singletons
            For<ISamplesTransmitterFactory>().Singleton().Use<SamplesTransmitterFactory>();
        }
    }
}
