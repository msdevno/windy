using System.Collections.Generic;
using Windy.Domain.Entities;

namespace Windy.Domain.Contracts
{
    public interface IClientRepository 
    {
        List<WindmillFarm> GetAllClients();
    }
}
