using System.Runtime.Serialization;

namespace Windy.Domain
{
    [DataContract(Name ="location")]
    public class Location
    {
        [DataMember(Name="name")]
        public string Name { get; set; }
    }
}
