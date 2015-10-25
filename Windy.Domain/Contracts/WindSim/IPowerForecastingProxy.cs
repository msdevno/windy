using Windy.Domain.Entities.WindSim;

namespace Windy.Domain.Contracts.WindSim
{
    public interface IPowerForecastingProxy
    {
        PowerForecastData GetWindFarmData(string key);
    }
}
