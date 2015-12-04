using System.Collections.Generic;
using Windy.Domain.Entities;

namespace Windy.Domain.Contracts.Queries
{
    public interface IWindmillFarmsQuery 
    {
        IEnumerable<WindmillFarm> GetAll();
    }
}
