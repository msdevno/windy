using Microsoft.ServiceBus.Messaging;
using System;

namespace WindySubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var storageConnectionString  = "DefaultEndpointsProtocol=https;AccountName=windystorage;AccountKey=anEPDluAymphAK0moL8lTljz3qtIHjKs68OpluD2Tzyt3vscWDgHBIFlVCCDxMVyzIsnwIMq0mIkwb5gAPW24w==";
            var eventHubConnectionString = "Endpoint=sb://windypedro.servicebus.windows.net/;SharedAccessKeyName=reader;SharedAccessKey=TBp/IdCnmIF+YVia0WhDKRIVFvRYmaRNxF8WiYHFW5g=";
            var eventHubPath             = "windmillseventhub";
            var consumerGroupName        = "windygroup";
            var hostname                 = "host_" + Guid.NewGuid().ToString();


            var host = new EventProcessorHost(hostname, eventHubPath, consumerGroupName, eventHubConnectionString, storageConnectionString);

            host.RegisterEventProcessorAsync<WindyEventProcessor>().Wait();

            Console.WriteLine("Press ANY key to exit");
            Console.ReadKey();
        }
    }
}
