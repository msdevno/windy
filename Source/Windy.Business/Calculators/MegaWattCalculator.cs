using Windy.Domain.Contracts.Calculators;
using Windy.Domain.Entities;

namespace Windy.Business.Calculators
{
    public class MegaWattCalculator : IMegaWattCalculator
    {
        public double CalculateForGeneratorBasedOnWindSpeed(PowerGenerator generator, double windSpeed)
        {
            var megawatt = 0.0;

            if (generator == null || windSpeed < generator.CutInSpeed)
                return megawatt; 


            if (windSpeed >= generator.MinOptimalWindspeed)
                megawatt = generator.MaxOutputMw;

            if (windSpeed < generator.MinOptimalWindspeed)
            {
                var tangent = GetTangentForPowerGenerator(generator);
                megawatt = (tangent * (windSpeed - generator.CutInSpeed)) + generator.MinOuputMw;
            }

            return megawatt;
        }

        private double GetTangentForPowerGenerator(PowerGenerator generator)
        {
            return (generator.MaxOutputMw - generator.MinOuputMw) / (generator.MinOptimalWindspeed - generator.CutInSpeed);
        }
    }
}
