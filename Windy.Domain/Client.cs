using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Windy.Domain
{
    [DataContract(Name ="client")]
    public class Client
    {
        [DataMember(Name ="name", Order = 10)]
        public string Name { get; set; }

        [DataMember(Name = "mills", Order = 20)]
        public IEnumerable<Mill> Mills { get; set; }

        public double ProductionFactor { get; set; }

        public IEnumerable<StreamAnalyticsFriendly> AsStreamAnalyticsFriendly()
        {
            foreach(var mill in Mills)
            {
                foreach(var sample in mill.Samples)
                {
                    yield return new StreamAnalyticsFriendly
                    {
                        Name         = Name, 
                        MillId       = mill.Id, 
                        MillLocation = mill.Location.Name,
                        SampleTime   = sample.SampleTime,
                        Megawatt     = sample.MegaWatt
                    };
                }
            }
        }
    }
}
