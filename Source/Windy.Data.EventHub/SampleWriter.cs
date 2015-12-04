using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;
using Windy.Domain.Contracts;
using Windy.Domain.Contracts.Converters;
using Windy.Domain.Contracts.Queries;

namespace Windy.Data.EventHub
{
    public class SampleWriter<TEntity> : ISampleWriter<TEntity> where TEntity : class
    {
        private readonly IConfigReader                _configReader;
        private readonly IByteArrayConverter<TEntity> _converter;
        private readonly EventHubClient               _eventHubClient;

        public SampleWriter(IByteArrayConverter<TEntity> converter, IConfigReader configReader)
        {
            _converter      = converter;
            _configReader   = configReader;
            _eventHubClient = EventHubClient.CreateFromConnectionString(_configReader["EventHubSenderConnectionString"], _configReader["eventHubName"]);
        }


        public async Task Write(TEntity entity)
        {
            var byteArray = _converter.ConvertToBytes(entity);
            var eventData = new EventData(byteArray);

            await _eventHubClient.SendAsync(eventData);
        }
    }
}
