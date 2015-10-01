using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Windy.Domain
{
    [DataContract(Name ="windmilldata")]
    public class WindmillData
    {
        [DataMember(Name ="client", Order = 10)]
        public string Client { get; set; }

        [DataMember(Name = "location", Order = 15)]
        public string Location { get; set; }

        [DataMember(Name = "millId", Order = 20)]
        public string MillId { get; set; }

        [DataMember(Name = "megawatt", Order = 30)]
        public double MegaWatt { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Client)) return false;
            if (string.IsNullOrEmpty(Location)) return false;
            if (string.IsNullOrEmpty(MillId)) return false;
            if (MegaWatt < 0) return false;

            return true;
        }
    }
}
