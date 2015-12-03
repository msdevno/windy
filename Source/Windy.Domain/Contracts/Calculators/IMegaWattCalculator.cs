using Windy.Domain.Entities;

namespace Windy.Domain.Contracts.Calculators
{
    public interface IMegaWattCalculator
    {
        double CalculateForGeneratorBasedOnWindSpeed(PowerGenerator powerGenerator, double windSpeed);
    }
}
