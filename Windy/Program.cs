using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windy.Domain;
using Windy.Domain.Entities;
using Windy.Domain.Entities.Yr;
using Windy.Domain.Managers;

namespace Windy
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new WindyConfiguration();
            var eventHubClient = EventHubClient.CreateFromConnectionString(configuration["EventHubSenderConnectionString"], "windyeventhub");
            var consumerGroup = eventHubClient.GetDefaultConsumerGroup();


            var clients = CreateAndPopulateClientsList();
            var allEventData = new List<EventData>();
            Console.WriteLine("");
            foreach(var client in clients)
            {
                var samples = client.AsStreamAnalyticsFriendly();
                foreach (var sample in samples)
                {
                    var json = JsonConvert.SerializeObject(sample);
                    var utf8EncodedSample = Encoding.UTF8.GetBytes(json);

                    allEventData.Add(new EventData(utf8EncodedSample));
                    Console.WriteLine($"[{client.Name}] Location: {sample.LocationName} WindSpeed: {sample.WindSpeeed_MS} m/s Produced {sample.Megawatt} MW");
                }
            }
            eventHubClient.SendBatch(allEventData);
            Console.WriteLine($"Data transmitted {DateTime.Now.ToString("dd MMM yyyy HH:mm")}");            
        }

        private static List<Client> CreateAndPopulateClientsList()
        {
            var clients = ConstructClientsWithWindMills();


            foreach (var client in clients)
            {
                foreach (var windmill in client.Windmills)
                {
                    Console.WriteLine($"[{client.Name}] Retrieving weather for: {windmill.Generator.Name} position: (lat={windmill.Location.Latitude}, lon={windmill.Location.Longitude}");
                    var weatherData = WeatherProxy.GetWeatherDataForLocation(windmill.Location.Latitude, windmill.Location.Longitude);
                    var locationData = weatherData.product.time[0].location;
                    var windSpeed = locationData.windSpeed.mps;
                    var temperature = locationData.temperature.value;

                    windmill.LastSample = new WindmillData
                    {
                        WindmillId = windmill.Id,
                        WindSpeed = (double)windSpeed,
                        Temperature = (double)temperature,
                        SampleTime = DateTime.Now,
                        MegaWatt = CalculateMegawattForGenerator(windmill.Generator, (double)windSpeed)
                    };
                }
            }

            return clients;
        }

        private static double CalculateMegawattForGenerator(Generator generator, double windSpeed)
        {
            var megawatt = 0.0;

            if (windSpeed < generator.CutInSpeed)
                return 0.0;

            if (windSpeed >= generator.MinOptimalWindspeed)
                megawatt = generator.MaxOutputMw;

            if (windSpeed < generator.MinOptimalWindspeed)
            {
                megawatt = (generator.TangentMin * (windSpeed - generator.CutInSpeed)) + generator.MinOuputMw;
            }

            return megawatt;
        }

        private static List<Client> ConstructClientsWithWindMills()
        {
            var millId = 1;
            var generators = Generator.Generators.ToArray();
            return new List<Client> {
                new Client { Id = 1, Name="Egersund Kommune", Windmills= new List<Windmill> {
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Egersund", Longitude= 58.430409, Latitude =5.863582  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Egersund", Longitude= 58.430294, Latitude =5.863649  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Egersund", Longitude= 58.430213, Latitude =5.863776  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Egersund", Longitude= 58.430119, Latitude =5.863983  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Egersund", Longitude= 58.430063, Latitude =5.864110  } },
                    }
                },
                new Client { Id = 2, Name="Karmøy Kommune", Windmills= new List<Windmill> {
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Karmøy", Longitude= 59.204889, Latitude =5.166571  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Karmøy", Longitude= 59.204776, Latitude =5.166563  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Karmøy", Longitude= 59.204663, Latitude =5.166554  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Karmøy", Longitude= 59.204528, Latitude =5.166545  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Karmøy", Longitude= 59.204402, Latitude =5.166554  } },
                    }
                },
                new Client { Id = 3, Name="Måløy Kommune", Windmills= new List<Windmill> {
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Måløy", Longitude= 61.204528, Latitude =5.166545  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Måløy", Longitude= 61.204889, Latitude =5.166571  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Måløy", Longitude= 61.204776, Latitude =5.166563  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Måløy", Longitude= 61.204663, Latitude =5.166554  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Måløy", Longitude= 61.204402, Latitude =5.166554  } },
                    }
                },
                new Client { Id = 4, Name="Havøya Kommune", Windmills= new List<Windmill> {
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Havøygavlen", Longitude= 71.021194, Latitude =24.545339  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Havøygavlen", Longitude= 71.019698, Latitude =24.545270  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Havøygavlen", Longitude= 71.090022, Latitude =24.545409  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Havøygavlen", Longitude= 71.018413, Latitude =24.545825  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Havøygavlen", Longitude= 71.017782, Latitude =24.545894  } },
                    }
                },
                new Client { Id = 5, Name="Røst Kommune", Windmills= new List<Windmill> {
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Røst", Longitude= 67.524194, Latitude =12.118339  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Røst", Longitude= 67.524698, Latitude =12.118270  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Røst", Longitude= 67.524022, Latitude =12.118409  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Røst", Longitude= 67.524413, Latitude =12.118825  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Røst", Longitude= 67.524782, Latitude =12.118894  } },
                    }
                },
                new Client { Id = 6, Name="Løvund Kommune", Windmills= new List<Windmill> {
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Løvund", Longitude= 66.366194, Latitude =12.359339  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Løvund", Longitude= 66.366698, Latitude =12.359270  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Løvund", Longitude= 66.366022, Latitude =12.359409  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Løvund", Longitude= 66.366413, Latitude =12.359825  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Løvund", Longitude= 66.366782, Latitude =12.359894  } },
                    }
                },


            };
        }
    }
}



