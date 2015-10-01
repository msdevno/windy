using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windy.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace WindySubscriber
{
    public class WindyEventProcessor : IEventProcessor
    {
        private PartitionContext _context;
        private CloudTable _ourTable;

        public WindyEventProcessor()
        {
            var connectionstring = "DefaultEndpointsProtocol=https;AccountName=windystorage;AccountKey=dVO07yiZaIDnh5tcE1OVjPm/vxwC9cTi5bF3JzuxlsbjWV8jObSF5qej9pvPredYveYPrBDw7gbkcIZ7NlvHEg==";
            var cloudStorageAccount = CloudStorageAccount.Parse(connectionstring);
            var tableClient = cloudStorageAccount.CreateCloudTableClient();

            _ourTable = tableClient.GetTableReference("windytable");

            _ourTable.CreateIfNotExists();
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {

            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
                context = null;
            }
            _context = null;
        }

        public async Task OpenAsync(PartitionContext context)
        {
            _context = context;
            Console.WriteLine($"[{context.EventHubPath}] Consumer Group: {context.ConsumerGroupName} open on partition {context.Lease.PartitionId}");
            await Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {

            foreach (var message in messages)
            {
                var rawString  = Encoding.UTF8.GetString(message.GetBytes());
                var sampleData = JsonConvert.DeserializeObject<WindmillData>(rawString);
                var time       = string.Format("{0:D19}", (DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks));

                if (sampleData.IsValid())
                {
                    var dynamicTableEntity = new DynamicTableEntity(sampleData.Client, time);
                    dynamicTableEntity.Properties.Add("Location", new EntityProperty(sampleData.Location));
                    dynamicTableEntity.Properties.Add("MillId", new EntityProperty(sampleData.MillId));
                    dynamicTableEntity.Properties.Add("Megawatt", new EntityProperty(sampleData.MegaWatt));

                    var insertOperation = TableOperation.InsertOrReplace(dynamicTableEntity);
                    try
                    {
                        _ourTable.Execute(insertOperation);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Console.WriteLine($"[{context.Lease.PartitionId}]  {sampleData.Client} - {sampleData.Location }: [{sampleData.MillId}] =  {sampleData.MegaWatt.ToString("0.00")} MW");
                }
            }
            await context.CheckpointAsync();
        }
    }
}
