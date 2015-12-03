using Windy.Domain.Entities.Yr;

namespace Windy.Domain.Contracts.Yr
{
    public interface IWeatherProxy
    {
        weatherdata GetWeatherDataForLocation(double latitude, double longitude);
    }
}
