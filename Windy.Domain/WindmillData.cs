using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Windy.Domain
{
    [DataContract(Name ="windmilldata")]
    public class WindmillData
    {
        [DataMember(Name ="client", Order = 1)]
        public string Client { get; set; }

        [DataMember(Name = "millId", Order = 2)]
        public string MillId { get; set; }

        [DataMember(Name = "megawatt", Order = 3)]
        public double MegaWatt { get; set; }       
    }
}
