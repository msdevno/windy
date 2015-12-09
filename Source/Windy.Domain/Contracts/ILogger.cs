using System;

namespace Windy.Domain.Contracts
{
    public interface ILogger
    {
        void LogInformation(string information);


        void LogException(string methodDescription, Exception ex);
    }
}
