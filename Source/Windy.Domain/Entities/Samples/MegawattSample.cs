using System;
using System.Runtime.Serialization;

namespace Windy.Domain.Entities.Samples
{
    [Serializable, DataContract(Name="megawattsample")]
    public class MegawattSample : WindmillSample
    {
        [DataMember(Name = "megawatt", Order = 10)]
        public double MegaWatt { get; set; }
    }
}
