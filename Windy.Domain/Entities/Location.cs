using System.Runtime.Serialization;

namespace Windy.Domain.Entities
{
    [DataContract(Name ="location")]
    public class Location
    {
        [DataMember(Name="name")]
        public string Name { get; set; }

        [DataMember(Name = "longitude")]
        public double Longitude { get; set; }

        [DataMember(Name = "latitude")]
        public double Latitude { get; set; }
    }
}
