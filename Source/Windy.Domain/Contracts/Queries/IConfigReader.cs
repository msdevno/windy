namespace Windy.Domain.Contracts.Queries
{
    public interface IConfigReader
    {
        string this[string settingName] { get; }
    }
}
