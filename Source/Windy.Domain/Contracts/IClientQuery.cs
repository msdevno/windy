using System.Collections.Generic;
using Windy.Domain.Entities;

namespace Windy.Domain.Contracts
{
    public interface IClientQuery 
    {
        List<WindmillFarm> GetAllClients();
    }
}
