namespace Windy.Domain.Contracts.Factories
{
    public interface ISamplesTransmitterFactory
    {
        void Transmit<TSample>(TSample sample) where TSample : class;
    }
}
