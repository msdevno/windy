using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Windy.Business.Managers;
using Windy.Data.Fakes;
using Windy.Data.Yr;
using Windy.Domain.Contracts;
using Windy.Domain.Contracts.Yr;
using Windy.Domain.Entities;

namespace Windy
{
    class Program
    {
        private static WindyConfiguration _configuration;
        private static IClientRepository _clientRepository;
        private static IWeatherProxy _weatherProxy;

        static void Main(string[] args)
        {
            _configuration = new WindyConfiguration();

            _clientRepository = new FakeClientRepository();
            _weatherProxy = new WeatherProxy();

            var clients = CreateAndPopulateClientsList();

            TransmitDataToEventHub(clients);
            StoreDataInTableStorage(clients);

            Console.WriteLine($"Data transmitted and stored {DateTime.Now.ToString("dd MMM yyyy HH:mm")}");
        }


        private static void StoreDataInTableStorage(List<WindmillFarm> clients)
        {
            var storageConnectionString = _configuration["StorageConnectionString"];
            var storageClient = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = storageClient.CreateCloudTableClient();
            var tableReference = tableClient.GetTableReference("windytable");

            var entityCount = 0;
            var batchOperation = new TableBatchOperation();

            foreach (var client in clients)
            {
                foreach (var windmill in client.Windmills)
                {
                    var time = string.Format("{0:D19}", DateTime.MaxValue.Ticks - windmill.LastSample.SampleTime.Ticks);
                    var partitionKey = string.Format($"client.{client.Id}");
                    var rowKey = string.Format($"windmill.{windmill.Id}.{time}");
                    var dynamicTableEntity = new DynamicTableEntity(partitionKey, rowKey);

                    dynamicTableEntity.Properties.Add("client_id", new EntityProperty(client.Id));
                    dynamicTableEntity.Properties.Add("client_name", new EntityProperty(client.Name));

                    dynamicTableEntity.Properties.Add("windmill_id", new EntityProperty(windmill.Id));

                    dynamicTableEntity.Properties.Add("location_name", new EntityProperty(windmill.Location.Name));
                    dynamicTableEntity.Properties.Add("location_latitude", new EntityProperty(windmill.Location.Latitude));
                    dynamicTableEntity.Properties.Add("location_longitude", new EntityProperty(windmill.Location.Longitude));

                    dynamicTableEntity.Properties.Add("generator_id", new EntityProperty(windmill.Generator.Id));
                    dynamicTableEntity.Properties.Add("generator_name", new EntityProperty(windmill.Generator.Name));
                    dynamicTableEntity.Properties.Add("generator_minoutput", new EntityProperty(windmill.Generator.MinOuputMw));
                    dynamicTableEntity.Properties.Add("generator_maxoutput", new EntityProperty(windmill.Generator.MaxOutputMw));
                    dynamicTableEntity.Properties.Add("generator_cutinspeed", new EntityProperty(windmill.Generator.CutInSpeed));
                    dynamicTableEntity.Properties.Add("generator_minoptimalwindspeed", new EntityProperty(windmill.Generator.MinOptimalWindspeed));


                    dynamicTableEntity.Properties.Add("sample_time", new EntityProperty(windmill.LastSample.SampleTime));
                    dynamicTableEntity.Properties.Add("sample_temperature", new EntityProperty(windmill.LastSample.Temperature));
                    dynamicTableEntity.Properties.Add("sample_windspeed", new EntityProperty(windmill.LastSample.WindSpeed));
                    dynamicTableEntity.Properties.Add("sample_megawatt", new EntityProperty(windmill.LastSample.MegaWatt));

                    batchOperation.Add(TableOperation.Insert(dynamicTableEntity));
                    entityCount++;
                }
                tableReference.ExecuteBatch(batchOperation);
                Console.WriteLine($"{client.Name}: Samples from {entityCount} windmills added.");
                batchOperation = new TableBatchOperation();
                entityCount = 0;
            }

            if (entityCount > 0)
            {
                Console.WriteLine(string.Format($"Wrote {entityCount} rows to Azure Table Storage."));
                tableReference.ExecuteBatch(batchOperation);
            }
        }

        private static bool TransmitDataToEventHub(List<WindmillFarm> clients)
        {
            var eventhubConnectionString = _configuration["EventHubSenderConnectionString"];
            if (string.IsNullOrEmpty(eventhubConnectionString))
            {
                Console.WriteLine("Can't grab the connection string. Need to exit");
                return false;
            }


            var eventHubClient = EventHubClient.CreateFromConnectionString(eventhubConnectionString, "windyeventhub");
            var consumerGroup = eventHubClient.GetDefaultConsumerGroup();

            var allEventData = new List<EventData>();
            Console.WriteLine("");
            foreach (var client in clients)
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
            return true;
        }

        private static List<WindmillFarm> CreateAndPopulateClientsList()
        {
            var clients = _clientRepository.GetAllClients();


            foreach (var client in clients)
            {
                foreach (var windmill in client.Windmills)
                {
                    Console.WriteLine($"[{client.Name}] Retrieving weather: {windmill.Generator.Name} position: (lat={windmill.Location.Latitude.ToString("0.0000")}, lon={windmill.Location.Longitude.ToString("0.0000")})");
                    var weatherData = _weatherProxy.GetWeatherDataForLocation(windmill.Location.Latitude, windmill.Location.Longitude);
                    var locationData = weatherData.product.time[0].location;
                    var windSpeed = locationData.windSpeed.mps;
                    var temperature = locationData.temperature.value;

                    windmill.LastSample = new TemperatureSample
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

        private static double CalculateMegawattForGenerator(PowerGenerator generator, double windSpeed)
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
    }
}



