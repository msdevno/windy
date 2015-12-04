using System;
using System.Runtime.Serialization;

namespace Windy.Domain.Entities.Samples
{
    [Serializable, DataContract(Name ="windmilldata")]
    public class TemperatureSample : WindmillSample
    {
        [DataMember(Name = "temperature_C", Order = 30)]
        public double Temperature { get; set; }
    }
}
