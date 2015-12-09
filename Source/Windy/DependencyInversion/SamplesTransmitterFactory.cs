using System.Collections.Generic;
using Windy.Business.Converters;
using Windy.Data.EventHub;
using Windy.Domain.Contracts.Factories;
using Windy.Domain.Contracts.Queries;

namespace Windy.DependencyInversion
{
    public class SamplesTransmitterFactory : ISamplesTransmitterFactory
    {
        private readonly IConfigReader _configReader;
        private Dictionary<string, object> _writers;


        public SamplesTransmitterFactory(IConfigReader configReader)
        {
            _writers      = new Dictionary<string, object>();
            _configReader = configReader;
        }


        public async void Transmit<TSampleType>(TSampleType sample) where TSampleType : class
        {
            var writer = GetOrCreateWriter(sample);

            await writer.Write(sample);            
        }


        private SampleWriter<TSampleType> GetOrCreateWriter<TSampleType>(TSampleType sample) where TSampleType : class
        {
            var sampleType = sample.GetType().Name;

            if (!_writers.ContainsKey(sampleType))
                _writers[sampleType] = new SampleWriter<TSampleType>(new ByteArrayConverter<TSampleType>(), _configReader);

            return _writers[sampleType] as SampleWriter<TSampleType>;
        }
    }
}
