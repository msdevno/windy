using System;
using System.Runtime.Serialization;

namespace Windy.Domain.Entities.Samples
{
    [Serializable, DataContract(Name="windspeedsample")]
    public class WindSpeedSample : WindmillSample
    {
        public WindSpeedSample()
        {
        }

        public WindSpeedSample(DateTime sampleTime, int windmillId)
            :base(sampleTime, windmillId)
        {
        }

        [DataMember(Name = "windspeed_mps", Order = 10)]
        public double WindSpeed { get; set; }
    }
}
