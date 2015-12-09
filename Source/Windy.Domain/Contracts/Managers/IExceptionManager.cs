using System;

namespace Windy.Domain.Contracts.Managers
{
    public interface IExceptionManager
    {
        void Execute(Action unsafeAtion, string callingMethodDescription, bool rethrowException = false);

        TReturnValue Execute<TReturnValue>(Func<TReturnValue> unsafeFunction, string callingMethodDescription, bool rethrowException = false);
    }
}
