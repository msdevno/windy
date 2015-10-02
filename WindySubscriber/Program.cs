using Microsoft.ServiceBus.Messaging;
using System;
using Windy.Domain.Managers;

namespace WindySubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new WindyConfiguration();
            var storageConnectionString  = configuration["StorageConnectionString"];
            var eventHubConnectionString = configuration["EventHubReaderConnectionString"];
            var eventHubPath             = "windyeventhub";
            var consumerGroupName        = "windygroup";
            var hostname                 = "host_" + Guid.NewGuid().ToString();

            var host = new EventProcessorHost(hostname, eventHubPath, consumerGroupName, eventHubConnectionString, storageConnectionString);

            host.RegisterEventProcessorAsync<WindyEventProcessor>().Wait();

            Console.WriteLine("Press ANY key to exit");
            Console.ReadKey();
        }
    }
}
