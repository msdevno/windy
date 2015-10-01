using FizzWare.NBuilder;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Linq;
using Windy.Entitities;

namespace Windy
{
    class Program
    {
        static void Main(string[] args)
        {
            var randomGenerator = new RandomGenerator((int)DateTime.Now.Ticks);
            var eventHubClient = EventHubClient.CreateFromConnectionString("Endpoint=sb://windypedro.servicebus.windows.net/;SharedAccessKeyName=PedrosTransmissionPolicy;SharedAccessKey=lAhSvgXwQFDGKqfMTcs9ptoMwwP2XnCAAbW5cjnBfP0=", "windmillseventhub");

            while (true)
            {
                // Generate tons of Random data
                var newWindmillSamples = Builder<WindmillData>
                    .CreateListOfSize(32)
                    .All()
                        .With(o => o.MillId = randomGenerator.Next(1, 800).ToString())
                        .With(o => o.Client = "Bærum")
                        .With(o => o.MegaWatt = randomGenerator.Next(0.01, 3.0))
                    .Build().ToList();


                // post random data to Event Hub
                var byteArrays = newWindmillSamples.Select(t => new EventData(WindmillData.AsByteArray(t)));
                eventHubClient.SendBatch(byteArrays);

                var first = newWindmillSamples.First();

                Console.WriteLine($"Just sent data for Client: {first.Client}, Mill: {first.MillId}, Producing {first.MegaWatt} MW");
            }
        

        }
    }
}
