using FizzWare.NBuilder;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Windy.Domain;
using Windy.Domain.Managers;

namespace Windy
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration  = new WindyConfiguration();
            var eventHubClient = EventHubClient.CreateFromConnectionString(configuration["EventHubSenderConnectionString"], "windyeventhub");
            var consumerGroup  = eventHubClient.GetDefaultConsumerGroup();
            var clients        = ConstructClientData().ToList();

            while (true)
            {
                var allEventData = new List<EventData>();
                foreach(var client in clients)
                {
                    var samples = client.AsStreamAnalyticsFriendly();
                    foreach (var sample in samples)
                    {
                        var clientAsJson = JsonConvert.SerializeObject(sample);
                        var utf8EncodedClient = Encoding.UTF8.GetBytes(clientAsJson);

                        allEventData.Add(new EventData(utf8EncodedClient));
                        
                        Console.WriteLine($"{sample.Name}, {sample.MillLocation}: {sample.Megawatt.ToString("0.000")} mW");
                    }
                }
                eventHubClient.SendBatch(allEventData);
                Thread.Sleep(TimeSpan.FromSeconds(1));                
            }
        }

        private static IEnumerable<Client> ConstructClientData()
        {
            yield return new Client { Name = "Alfa"    , ProductionFactor = 0.8,  Mills = GetMills(0 , 0.8 )}; 
            yield return new Client { Name = "Beta"    , ProductionFactor = 0.7,  Mills = GetMills(8 , 0.7 )}; 
            yield return new Client { Name = "Gamma"   , ProductionFactor = 1.0,  Mills = GetMills(16, 1.0 )}; 
            yield return new Client { Name = "Delta"   , ProductionFactor = 0.65, Mills = GetMills(24, 0.65)}; 
            yield return new Client { Name = "Epsilon" , ProductionFactor = 0.2,  Mills = GetMills(32, 0.3 )}; 
            yield return new Client { Name = "Zeta"    , ProductionFactor = 0.6,  Mills = GetMills(40, 0.6 )};  
        }

        private static IEnumerable<Mill> GetMills(int startPos, double discriminator)
        {
            var randomGenerator = new RandomGenerator((int)DateTime.Now.Ticks);

            var LocationNames = new[] { "Arendal", "Bergen", "Bodø", "Drammen", "Egersund", "Farsund", "Flekkefjord ", "Florø", "Fredrikstad ", "Gjøvik", "Grimstad", "Halden", "Hamar", "Hammerfest", "Harstad", "Haugesund", "Holmestrand ", "Horten", "Hønefoss", "Kongsberg", "Kongsvinger ", "Kristiansand", "Kristiansund", "Larvik", "Lillehammer ", "Mandal", "Molde", "Moss", "Namsos", "Narvik", "Notodden", "Oslo", "Porsgrunn", "Risør", "Sandefjord  ", "Sandnes", "Sarpsborg", "Skien", "Stavanger", "Steinkjer", "Søgne", "Tromsø", "Trondheim", "Tønsberg", "Vadsø", "Vardø", "Vennesla", "Ålesund" };
            string[] result = new string[8];
            Array.Copy(LocationNames, startPos, result, 0, 8);

            for(int i = 1; i <= 8; i++)
            {
                yield return new Mill {
                    Id = i,
                    Location = new Location {
                        Name = result[i - 1] },
                    Samples = new List<WindmillData> {
                        new WindmillData {
                            SampleTime = DateTime.Now,
                            MegaWatt = randomGenerator.Next(0.1, 3.0) * discriminator
                        }
                    }
                };
            }
        }
    }
}



