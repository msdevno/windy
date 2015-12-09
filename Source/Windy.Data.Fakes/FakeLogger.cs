using System;
using Windy.Domain.Contracts;

namespace Windy.Data.Fakes
{
    public class FakeLogger : ILogger
    {
        public void LogException(string methodDescription, Exception ex)
        {
            if (string.IsNullOrEmpty(methodDescription))
                return;

            Console.WriteLine($"EXCEPTION: {methodDescription}\nStackTrace:\n{ex?.StackTrace}" );
        }


        public void LogInformation(string information)
        {
            if (string.IsNullOrEmpty(information))
                return;

            Console.WriteLine(information);
        }
    }
}
