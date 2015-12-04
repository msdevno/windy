using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Windy.Domain.Entities
{
    [DataContract(Name ="windmillfarm")]
    public class WindmillFarm
    {
        [DataMember(Name ="id", Order =10)]
        public int Id { get; set; }

        [DataMember(Name ="name", Order = 20)]
        public string Name { get; set; }

        [DataMember(Name = "windmills", Order = 30)]
        public List<Windmill> Windmills { get; set; }
    }
}
