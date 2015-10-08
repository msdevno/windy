using System;
using System.Runtime.Serialization;

namespace Windy.Domain
{
    [DataContract(Name ="windmilldata")]
    public class WindmillData
    {
        [DataMember(Name ="sampletime", Order =10)]
        public DateTime SampleTime { get; set; }

        [DataMember(Name ="windmillid", Order = 20)]
        public int  WindmillId { get; set; }

        [DataMember(Name = "temperature_C", Order = 30)]
        public double Temperature { get; set; }

        [DataMember(Name = "windspeed_mps", Order = 40)]
        public double WindSpeed{ get; set; }

        [DataMember(Name = "megawatt", Order = 50)]
        public double MegaWatt { get; set; }
    }
}
