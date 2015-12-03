using System;
using System.Runtime.Serialization;

namespace Windy.Domain.Entities.Samples
{
    [Serializable, DataContract(Name="sample")]
    public class WindmillSample
    {
        public WindmillSample()
        {
        }

        public WindmillSample(DateTime sampleTime, int windmillId)
        {
            SampleTime = sampleTime;
            WindmillId = windmillId;
        }

        [DataMember(Name = "sampletime", Order = 10)]
        public DateTime SampleTime { get; set; }

        [DataMember(Name = "windmillid", Order = 20)]
        public int WindmillId { get; set; }
    }
}
