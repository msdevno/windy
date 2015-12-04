using System.Collections.Generic;
using Windy.Domain.Entities;
using Windy.Domain.Entities.Samples;

namespace Windy.Domain.Contracts.Queries
{
    public interface ISampleGatherer
    {
        IEnumerable<WindmillSample> GetSamplesFrom(IEnumerable<WindmillFarm> allFarms);
    }
}
