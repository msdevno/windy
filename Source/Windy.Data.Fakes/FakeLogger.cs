using System;
using Windy.Domain.Contracts;

namespace Windy.Data.Fakes
{
    public class FakeLogger : ILogger
    {
        public void LogInformation(string information)
        {
            if (string.IsNullOrEmpty(information))
                return;

            Console.WriteLine(information);
        }
    }
}
