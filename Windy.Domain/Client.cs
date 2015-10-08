using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Windy.Domain.Entities;

namespace Windy.Domain
{
    [DataContract(Name ="client")]
    public class Client
    {
        [DataMember(Name ="id", Order = 5)]
        public int Id { get; set; }

        [DataMember(Name ="name", Order = 10)]
        public string Name { get; set; }

        [DataMember(Name = "windmills", Order = 20)]
        public List<Windmill> Windmills { get; set; }

        public IEnumerable<StreamAnalyticsFriendly> AsStreamAnalyticsFriendly()
        {
            foreach(var windmill in Windmills)
            {
                yield return new StreamAnalyticsFriendly
                {
                    ClientName    = Name, 
                    LocationName  = windmill.Location.Name, 
                    Latitude      = windmill.Location.Latitude,
                    Longitude     = windmill.Location.Longitude,
                    SampleTime    = windmill.LastSample.SampleTime,
                    Temperature_C = windmill.LastSample.Temperature,
                    WindSpeeed_MS = windmill.LastSample.WindSpeed,
                    Megawatt      = windmill.LastSample.MegaWatt
                };
            }            
        }
    }
}
