using FizzWare.NBuilder;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windy.Domain;
using Windy.Domain.Managers;

namespace Windy
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new WindyConfiguration();
            var eventHubClient = EventHubClient.CreateFromConnectionString(configuration["EventHubSenderConnectionString"], "windyeventhub");
            var consumerGroup  = eventHubClient.GetDefaultConsumerGroup();
            var randomGenerator = new RandomGenerator((int)DateTime.Now.Ticks);

            var clients = new[] { "Alfa", "Beta", "Gamma", "Delta", "Etta" };
            var cities  = new[] { "Arendal", "Bergen","Bodø","Drammen","Egersund","Farsund","Flekkefjord ","Florø","Fredrikstad ","Gjøvik","Grimstad","Halden","Hamar","Hammerfest","Harstad","Haugesund","Holmestrand ","Horten","Hønefoss","Kongsberg","Kongsvinger ","Kristiansand","Kristiansund","Larvik","Lillehammer ","Mandal","Molde","Moss","Namsos","Narvik","Notodden","Oslo","Porsgrunn","Risør","Sandefjord  ","Sandnes","Sarpsborg","Skien","Stavanger","Steinkjer","Søgne","Tromsø","Trondheim","Tønsberg","Vadsø","Vardø","Vennesla","Ålesund"};


            int messagesSent = 0;
            while (true)
            {
                var allSamples = new List<WindmillData>();

                // Generate tons of Random data
                for(int millId = 0; millId < 100; millId++)
                {
                    allSamples.Add(Builder<WindmillData>
                        .CreateNew()
                            .With(o => o.MillId = millId.ToString())
                            .With(o => o.Client = clients[randomGenerator.Next(0,4)] )
                            .With(o => o.Location = cities[randomGenerator.Next(0, cities.Length)].Trim())
                            .With(o => o.MegaWatt = randomGenerator.Next(0.01, 3.0))
                        .Build());
                }

                var allEventData = allSamples.Select(sample =>
                {
                    var jsonObject = JsonConvert.SerializeObject(sample);
                   
                    return new EventData(Encoding.UTF8.GetBytes(jsonObject));                
                });

                eventHubClient.SendBatch(allEventData);
                Console.WriteLine($"{messagesSent} Samples...");
                messagesSent += 100;              
            }
        }
    }
}



