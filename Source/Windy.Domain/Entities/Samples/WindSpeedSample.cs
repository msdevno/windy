using System;
using System.Runtime.Serialization;

namespace Windy.Domain.Entities.Samples
{
    [Serializable, DataContract(Name="windspeedsample")]
    public class WindSpeedSample : WindmillSample
    {
        [DataMember(Name = "windspeed_mps", Order = 10)]
        public double WindSpeed { get; set; }
    }
}
