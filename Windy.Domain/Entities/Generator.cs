using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Windy.Domain.Entities
{
    [DataContract(Name = "generator")]
    public class Generator
    {
        [DataMember(Name = "id", Order = 10)]
        public int Id { get; set; }

        [DataMember(Name = "name", Order = 20)]
        public string Name { get; set; }

        [DataMember(Name="maxoutputmw", Order =30)]
        public double MaxOutputMw { get; set; }

        [DataMember(Name = "minoutputmw", Order = 40)]
        public double MinOuputMw { get; set; }

        [DataMember(Name = "minoptimalwindspeed", Order = 50)]
        public double MinOptimalWindspeed { get; set; }

        [DataMember(Name = "maxoptimalwindspeed", Order = 60)]
        public double MaxOptimalWindspeed { get; set; }

        [DataMember(Name = "cutinspeed", Order = 70)]
        public double CutInSpeed { get; set; }

        public double TangentMin
        {
            get
            {
                return (MaxOutputMw - MinOuputMw) / (MinOptimalWindspeed - CutInSpeed);
            }
        }       

        public static IEnumerable<Generator> Generators
        {
            get
            {
                yield return new Generator { Id = 4, Name = "Hitachi X800",  MaxOutputMw = 3.5, MinOuputMw = 0.15, MinOptimalWindspeed = 6.8, MaxOptimalWindspeed = 14.0, CutInSpeed = 2.0, };
                yield return new Generator { Id = 1, Name = "Vestas V90",    MaxOutputMw = 2.0, MinOuputMw = 0.1,  MinOptimalWindspeed = 6.2, MaxOptimalWindspeed = 18.0, CutInSpeed = 3.5, };
                yield return new Generator { Id = 2, Name = "Siemens SWT-6", MaxOutputMw = 5.0, MinOuputMw = 0.25, MinOptimalWindspeed = 7.0, MaxOptimalWindspeed = 14.0, CutInSpeed = 6.0, };
                yield return new Generator { Id = 3, Name = "Enercon E126",  MaxOutputMw = 7.0, MinOuputMw = 0.3,  MinOptimalWindspeed = 8.2, MaxOptimalWindspeed = 18.0, CutInSpeed = 7.0, };
            }
        }
    }
}
