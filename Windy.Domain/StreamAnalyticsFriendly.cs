using System;
using System.Runtime.Serialization;

namespace Windy.Domain
{
    [DataContract(Name="streamanalyticsfriendly")]
    public class StreamAnalyticsFriendly
    {
        [DataMember(Name="name", Order = 10)]
        public string Name { get; set; }

        [DataMember(Name = "millid", Order = 20)]
        public int MillId { get; set; }

        [DataMember(Name = "milllocation", Order = 40)]
        public string MillLocation { get; set; }

        [DataMember(Name = "sampletime", Order = 50)]
        public DateTime SampleTime { get; set; }

        [DataMember(Name = "megawatt", Order = 60)]
        public double Megawatt { get; set; }
    }
}