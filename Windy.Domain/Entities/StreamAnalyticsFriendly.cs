using System;
using System.Runtime.Serialization;

namespace Windy.Domain.Entities
{
    [DataContract(Name="streamanalyticsfriendly")]
    public class StreamAnalyticsFriendly
    {        
        [DataMember(Name = "clientname", Order = 10)]
        public string ClientName { get; set; }

        [DataMember(Name = "sampletime", Order = 20)]
        public DateTime SampleTime { get; set; }

        [DataMember(Name = "location", Order = 30)]
        public string LocationName { get; set; }

        [DataMember(Name = "longitude", Order = 40)]
        public double Longitude { get; set; }

        [DataMember(Name = "latitude", Order = 50)]
        public double  Latitude { get; set; }

        [DataMember(Name = "temperature_celcius", Order = 60)]
        public double Temperature_C { get; set; }

        [DataMember(Name = "windspeed_meterspersecond", Order = 70)]
        public double WindSpeeed_MS{ get; set; }

        [DataMember(Name = "megawatt", Order = 80)]
        public double Megawatt { get; set; }
    }
}