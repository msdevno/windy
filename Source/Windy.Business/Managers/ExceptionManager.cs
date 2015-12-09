using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windy.Domain.Contracts;
using Windy.Domain.Contracts.Managers;

namespace Windy.Business.Managers
{
    public class ExceptionManager : IExceptionManager
    {
        private readonly ILogger _logger;


        public ExceptionManager(ILogger logger)
        {
            _logger = logger;
        }


        public void Execute(Action unsafeAction, string callingMethodDescription, bool rethrowException = false)
        {
            try
            {
                unsafeAction.Invoke();
            }
            catch(Exception ex)
            {
                _logger.LogException(callingMethodDescription, ex);
                if (rethrowException)
                    throw;
            }
        }


        public TReturnValue Execute<TReturnValue>(Func<TReturnValue> unsafeFunction, string callingMethodDescription, bool rethrowException = false)
        {
            try
            {
                return unsafeFunction.Invoke();
            }
            catch(Exception ex)
            {
                _logger.LogException(callingMethodDescription, ex);
                if (rethrowException)
                    throw;
            }
            return default(TReturnValue);
        }
    }
}
