using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windy.Domain;

namespace WindySubscriber
{
    public class WindyEventProcessor : IEventProcessor
    {
        private PartitionContext _context;

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            if(reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
                context = null;
            }
            _context = null;
        }

        public async Task OpenAsync(PartitionContext context)
        {
            _context = context;
            Console.WriteLine($"[{context.EventHubPath}] Consumer Group: {context.ConsumerGroupName} open on partition {context.Lease.PartitionId}" );
            await Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach(var message in messages)
            {
                var rawString = Encoding.UTF8.GetString(message.GetBytes());

                var sampleData = JsonConvert.DeserializeObject<WindmillData>(rawString);

                Console.WriteLine($"Partition {context.Lease.PartitionId}: {sampleData.Client} mill {sampleData.MillId} producing {sampleData.MegaWatt}MW");
                
                await context.CheckpointAsync();
            }
        }
    }
}
