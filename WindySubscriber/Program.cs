using Microsoft.ServiceBus.Messaging;
using System;

namespace WindySubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var storageConnectionString  = "DefaultEndpointsProtocol=https;AccountName=windystorage;AccountKey=dVO07yiZaIDnh5tcE1OVjPm/vxwC9cTi5bF3JzuxlsbjWV8jObSF5qej9pvPredYveYPrBDw7gbkcIZ7NlvHEg==";
            var eventHubConnectionString = "Endpoint=sb://windybus.servicebus.windows.net/;SharedAccessKeyName=listener;SharedAccessKey=IzCnCSJ7y5fYt1ZKdGKLFizWb/6wF1CszP8HdP9RPnE=";
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
