using System;
using System.Runtime.Serialization;

namespace Windy.Domain
{
    [DataContract(Name ="windmilldata")]
    public class WindmillData
    {
        [DataMember(Name ="sampletime", Order =10)]
        public DateTime SampleTime { get; set; }

        [DataMember(Name = "megawatt", Order = 30)]
        public double MegaWatt { get; set; }

        public bool IsValid()
        {
            if (MegaWatt < 0) return false;

            return true;
        }
    }
}
