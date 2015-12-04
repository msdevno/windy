using System;
using System.Runtime.Serialization;

namespace Windy.Domain.Entities.Samples
{
    [Serializable, DataContract(Name="sample")]
    public class WindmillSample
    {
        [DataMember(Name = "sampletime", Order = 10)]
        public DateTime SampleTime { get; set; }

        [DataMember(Name = "windfarmid", Order = 20)]
        public int WindFarmId { get; set; }

        [DataMember(Name = "windmillid", Order = 30)]
        public int WindmillId { get; set; }
    }
}
