using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Windy.Domain
{
    [DataContract(Name = "mill")]
    public class Mill
    {
        private List<WindmillData> _samples;

        public Mill()
        {
            _samples = new List<WindmillData>();
        }

        [DataMember(Name = "id", Order = 10)]
        public int Id { get; set; }


        [DataMember(Name = "location", Order = 20)]
        public Location Location { get; set; }

        [DataMember(Name = "samples", Order = 30)]
        public IEnumerable<WindmillData> Samples
        {
            get { return _samples; }
            set
            {
                _samples = new List<WindmillData>(value);
            }
        }
    }
}