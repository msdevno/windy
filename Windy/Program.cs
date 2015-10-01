using FizzWare.NBuilder;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using Windy.Domain;

namespace Windy
{
    class Program
    {
        static void Main(string[] args)
        {
            var randomGenerator = new RandomGenerator((int)DateTime.Now.Ticks);
            var eventHubClient = EventHubClient.CreateFromConnectionString("Endpoint=sb://windypedro.servicebus.windows.net/;SharedAccessKeyName=sender;SharedAccessKey=sKzgqs3GDHd9kVForG4PFsMLHBUsK6Cc1Y9h1x+vIlg=", "windmillseventhub");
            var consumerGroup  = eventHubClient.GetDefaultConsumerGroup();
            while (true)
            {
                // Generate tons of Random data
                var sample = Builder<WindmillData>
                    .CreateNew()
                        .With(o => o.MillId = randomGenerator.Next(1, 800).ToString())
                        .With(o => o.Client = "Asker")
                        .With(o => o.MegaWatt = randomGenerator.Next(0.01, 3.0))
                    .Build();


                var jsonObject = JsonConvert.SerializeObject(sample);

                // post random data to Event Hub
                var eventData = new EventData(Encoding.UTF8.GetBytes(jsonObject));                
                eventHubClient.Send(eventData);
                Thread.Sleep(1000);

                Console.WriteLine($"Just sent data for Client: {sample.Client}, Mill: {sample.MillId}, Producing {sample.MegaWatt} MW");
            }
        }
    }
}
