﻿using System.Runtime.Serialization;

namespace Windy.Domain.Entities
{
    [DataContract(Name = "windmill")]
    public class Windmill
    {
        [DataMember(Name = "id", Order = 10)]
        public int Id { get; set; }

        [DataMember(Name = "generator", Order = 20)]
        public PowerGenerator Generator { get; set; }

        [DataMember(Name = "location", Order = 30)]
        public Location Location { get; set; }
    }
}