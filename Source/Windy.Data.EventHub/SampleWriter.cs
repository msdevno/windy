using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;
using Windy.Domain.Contracts;
using Windy.Domain.Contracts.Converters;

namespace Windy.Data.EventHub
{
    public class SampleWriter<TEntity> : ISampleWriter<TEntity> where TEntity : class
    {
        private readonly IByteArrayConverter<TEntity> _converter;
        private readonly EventHubClient _eventHubClient;


        public SampleWriter(IByteArrayConverter<TEntity> converter, string connectionString)
        {
            _converter = converter;
            _eventHubClient = EventHubClient.CreateFromConnectionString(connectionString);
        }


        public async Task Write(TEntity entity)
        {
            var byteArray = _converter.ConvertToBytes(entity);
            var eventData = new EventData(byteArray);
            await _eventHubClient.SendAsync(eventData);
        }
    }
}
